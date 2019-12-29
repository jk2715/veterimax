using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Veterimax.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Web;
using System.Data;

namespace Veterimax.Controllers
{
    public class CompraController : Controller
    {
        // GET: Compra
        public ActionResult Index()
        {
            return View("RegistroCompra");
        }

        // GET: Compra/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult RegistroCompra()
        {
            Get_Productos();
            ViewData["Message"] = "Registrar una Orden de Compra";
            return View();
        }

        public IActionResult VistaCompra()
        {
            ViewData["Message"] = "Ver Ordenes de Compras";
            return View();
        }

        public IActionResult RecepcionCompra()
        {
            ViewData["Message"] = "Recibir Ordenes de Compras";
            return View();
        }

        public IActionResult LotesProducto()
        {
            ExpirarLotes();
            ViewData["Message"] = "Ver Lotes de Productos";
            return View();
        }

        // GET: Compra/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Compra/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroCompra(Compra compra, string itbisH, string totalH, string productosHidden, string cantidadHidden, string precioHidden, string itbisHidden, string totalHidden)
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    string[] listaProductos = productosHidden.Split(",");
                    string[] listaCantidad = cantidadHidden.Split(",");
                    string[] listaPrecio = precioHidden.Split(",");
                    string[] listaITBIS = itbisHidden.Split(",");
                    string[] listaTotal = totalHidden.Split(",");
                    for(int i = 0; i<listaProductos.Length; i++)
                    {
                        var command = con.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Verificar_Cantidad";
                        command.Parameters.AddWithValue("@Cantidad", listaCantidad[i]);
                        command.Parameters.AddWithValue("@IdProducto", listaProductos[i]);
                        if(Convert.ToInt32(command.ExecuteScalar()) == -1)
                        {
                            ViewBag.Message = "Maximo";
                            return View("RegistroCompra");
                        }
                    }
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Registrar_Compra";
                    itbisH = itbisH.Remove(0, 2);
                    totalH = totalH.Remove(0, 1);
                    compra.Total = double.Parse(totalH);
                    compra.TotalITBIS = double.Parse(itbisH);
                    string numeroComp = "B" + compra.NumeroComprobante + get_Comprobante();
                    cmd.Parameters.AddWithValue("@Total", compra.Total);
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IdSuplidor", compra.IdSuplidor);
                    cmd.Parameters.AddWithValue("@ModoPago", compra.ModoPago);
                    cmd.Parameters.AddWithValue("@TotalITBIS", compra.TotalITBIS);
                    cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                    cmd.Parameters.AddWithValue("@NumeroComprobante", numeroComp);
                    cmd.ExecuteNonQuery();
                    for(int i = 0; i < listaProductos.Length; i++)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Registrar_CompraDetalle";
                        com.Parameters.AddWithValue("@IdCompra", Compra.tempCompraId);
                        com.Parameters.AddWithValue("@IdProducto", listaProductos[i]);
                        com.Parameters.AddWithValue("@IdSuplidor", compra.IdSuplidor);
                        com.Parameters.AddWithValue("@Valor", decimal.Parse(listaPrecio[i]));
                        com.Parameters.AddWithValue("@Cantidad", decimal.Parse(listaCantidad[i]));
                        com.Parameters.AddWithValue("@ITBIS", decimal.Parse(listaITBIS[i]));
                        com.Parameters.AddWithValue("@Total", decimal.Parse(listaTotal[i]));
                        ViewBag.Message = "Exito";
                        com.ExecuteNonQuery();
                    }

                    con.Close();
                }
                return View("RegistroCompra");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RecepcionCompra(Compra compra, string loteHidden, string cantidadHidden, string expiracionHidden, string fechaHidden, string productosHidden, string compraIdHidden, string ubicacionHidden)
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    if(compra.Comentarios == null) {
                        compra.Comentarios = "N/A";
                    }
                    var cmd = con.CreateCommand();
                    string[] listaProductos = productosHidden.Split(",");
                    string[] listaCantidad = cantidadHidden.Split(",");
                    string[] listaLotes = loteHidden.Split(",");
                    string[] listaExpiracion = expiracionHidden.Split(",");
                    string[] listaUbicacion = ubicacionHidden.Split(",");
                    for (int i = 0; i < listaProductos.Length; i++)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Recibir_Compra";
                        com.Parameters.AddWithValue("@IdLote", int.Parse(listaLotes[i]));
                        com.Parameters.AddWithValue("@IdCompra", compraIdHidden);
                        com.Parameters.AddWithValue("@IdProducto", listaProductos[i]);
                        com.Parameters.AddWithValue("@FechaEntrada", DateTime.Now);
                        com.Parameters.AddWithValue("@Comentarios", compra.Comentarios);
                        com.Parameters.AddWithValue("@FechaRegistro", DateTime.Parse(fechaHidden));
                        com.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                        com.Parameters.AddWithValue("@CantidadRecibida", decimal.Parse(listaCantidad[i]));
                        com.Parameters.AddWithValue("@FechaExpiracion", DateTime.Parse(listaExpiracion[i]));
                        com.Parameters.AddWithValue("@Ubicacion", listaUbicacion[i]);
                        com.ExecuteNonQuery();
                    }

                    con.Close();
                }
                return View("RecepcionCompra");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarLote(Compra compra, string loteHidden, string comentarioHidden, string cantidadHidden, string expiracionHidden, string ubicacionHidden)
        {
           // try
           // {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    var com = con.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "Editar_Lote";
                    com.Parameters.AddWithValue("@IdLote", int.Parse(loteHidden));
                    com.Parameters.AddWithValue("@Comentarios", comentarioHidden);
                    com.Parameters.AddWithValue("@Cantidad", decimal.Parse(cantidadHidden));
                    com.Parameters.AddWithValue("@FechaExpiracion", DateTime.Parse(expiracionHidden));
                    com.Parameters.AddWithValue("@Ubicacion", ubicacionHidden);
                    com.ExecuteNonQuery();
                    con.Close();
                }
                return View("LotesProducto");
          //  }
          //  catch
           // {
           //     return View("LotesProducto");
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EliminarLote(Compra compra, string listaLotes)
        {
            // try
            // {
            // TODO: Add insert logic here
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                string[] lista = listaLotes.Split(",");
                foreach(string x in lista)
                {
                    var com = con.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "Eliminar_Lote";
                    com.Parameters.AddWithValue("@IdLote", int.Parse(x));
                    com.ExecuteNonQuery();
                }
                con.Close();
            }
            return View("LotesProducto");
            //  }
            //  catch
            // {
            //     return View("LotesProducto");
            //}
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CancelarCompra(Compra compra, string listaCompras)
        {
            // try
            // {
            // TODO: Add insert logic here
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                string[] lista = listaCompras.Split(",");
                foreach (string x in lista)
                {
                    var com = con.CreateCommand();
                    com.CommandType = System.Data.CommandType.StoredProcedure;
                    com.CommandText = "Cancelar_Compra";
                    com.Parameters.AddWithValue("@IdCompra", int.Parse(x));
                    com.ExecuteNonQuery();
                }
                con.Close();
            }
            return View("RecepcionCompra");
            //  }
            //  catch
            // {
            //     return View("LotesProducto");
            //}
        }

        public string get_Comprobante()
        {
            string secuencia = "";
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select COUNT([Numero de Comprobante]) from Compras";
                var num = Convert.ToInt32(comm.ExecuteScalar());
                num++;
                for(int i = 0; i<8-num.ToString().Length; i++)
                {
                    secuencia = secuencia + "0";
                }
                secuencia = secuencia + num.ToString();
                con.Close();
            }
            return secuencia;
        }

        public void ExpirarLotes()
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    var com = con.CreateCommand();
                    com.CommandType = CommandType.StoredProcedure;
                    com.CommandText = "Expirar_Lote";
                    com.ExecuteNonQuery();
                    con.Close();
                }
              
            }
            catch
            {
                
            }
        }

        public DataTable Get_TipoComprobante()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdTipoComprobante,Nombre, Secuencia from [Tipo Comprobante]";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        // GET: Compra/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Compra/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Compra/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Compra/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public DataTable GetCompras()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdCompra, s.Empresa, TotalITBIS, Total, Compras.Estado, e.Nombre + ' ' + e.Apellido as Solicitante, Compras.FechaRegistro from Compras left join Suplidores s on Compras.IdSuplidor = s.IdSuplidor left join Usuarios u on Compras.IdUsuario = u.IdUsuario left join Empleados e on u.IdEmpleado = e.IdEmpleado";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable GetComprasPendientes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdCompra, s.Empresa, TotalITBIS, Total, Compras.Estado, e.Nombre + ' ' + e.Apellido as Solicitante, Compras.FechaRegistro from Compras left join Suplidores s on Compras.IdSuplidor = s.IdSuplidor left join Usuarios u on Compras.IdUsuario = u.IdUsuario left join Empleados e on u.IdEmpleado = e.IdEmpleado where Compras.Estado = 'Pendiente'";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable GetDetalles()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdCompra, p.IdProducto, p.Descripcion, [Compras Detalles].Cantidad, [Compras Detalles].Costo, ITBIS, Total from [Compras Detalles] left join Productos p on [Compras Detalles].IdProducto = p.IdProducto";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public void GetCompraId()
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select MAX(IdCompra) from Compras";
                try
                {
                    Compra.tempCompraId = Convert.ToInt32(comm.ExecuteScalar()) + 1;
                }
                catch
                {
                    Compra.tempCompraId = 1;
                }
            }
        }
        public DataTable Get_Productos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdProducto, Descripcion, Unidades from Productos where Estado != 'Eliminado'";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public List<string> Get_Precio()
        {
            List<string> listaPrecio = new List<string>();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select Valor from ITBIS";
                Compra.ITBIS = Convert.ToDouble(cmd.ExecuteScalar());
                comm.CommandText = "select Descripcion, Costo from Productos";
                var reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    var id = reader.GetString(0);
                    var precio = reader.GetDecimal(1);
                    listaPrecio.Add(id);
                    listaPrecio.Add(precio.ToString());
                }
                con.Close();
            }
            return listaPrecio;
        }

        public DataTable Get_Lotes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdLote, IdCompra, Lotes.IdProducto, p.Descripcion, FechaEntrada, Comentarios, Lotes.FechaRegistro, Lotes.FechaMod, Lotes.IdUsuario, e.Nombre + ' ' + e.Apellido as Empleado, CantidadRecibida, FechaExpiracion, Lotes.Estado, Ubicacion from Lotes left join Usuarios u on Lotes.IdUsuario = u.IdUsuario left join Empleados e on u.IdEmpleado = e.IdEmpleado left join Productos p on Lotes.IdProducto = p.IdProducto where Lotes.Estado != 'Eliminado'";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

    }
}