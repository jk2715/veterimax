using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterimax.Models;
using System.Data;
using System.Data.SqlClient;

namespace Veterimax.Controllers
{
    public class VentaController : Controller
    {
        // GET: Venta
        public ActionResult Index()
        {
            return View();
        }

        // GET: Venta/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult RegistroVenta()
        {
            ViewData["Message"] = "Reigstro de Ventas";
            return View();
        }


        // GET: Venta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Venta/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroVenta(Venta venta, string itbisH, string totalH, string descuentoH, string subTotalH, string productosHidden, string cantidadHidden, string precioHidden, string descuentoHidden, string itbisHidden, string totalHidden)
        {
            //try
           // {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    string[] listaProductos = productosHidden.Split(",");
                    string[] listaCantidad = cantidadHidden.Split(",");
                    string[] listaPrecio = precioHidden.Split(",");
                    string[] listaDescuento = descuentoHidden.Split(",");
                    string[] listaITBIS = itbisHidden.Split(",");
                    string[] listaTotal = totalHidden.Split(",");
                    for(int i = 0; i<listaProductos.Length; i++)
                    {
                        var command = con.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Verificar_Cantidad";
                        command.Parameters.AddWithValue("@Cantidad", listaCantidad[i]);
                        command.Parameters.AddWithValue("@IdProducto", listaProductos[i]);
                        if (Convert.ToInt32(command.ExecuteScalar()) == -2)
                        {
                            ViewBag.Message = "No hay existencias";
                            return View("RegistroVenta");
                        }
                    }
                    int ventaId = GetVentaId();
                    string numComp = "B" + venta.NumeroComprobante + get_Comprobante();
                    var cmd = con.CreateCommand();
                    itbisH = itbisH.Remove(0,2);
                    totalH = totalH.Remove(0,1);
                    venta.TotalITBIS = double.Parse(itbisH);
                    venta.Total = double.Parse(totalH);
                    venta.SubTotal = double.Parse(subTotalH);
                    venta.TotalDescuento = double.Parse(descuentoH);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Registrar_Venta";
                    cmd.Parameters.AddWithValue("@IdCliente", venta.IdCliente);
                    cmd.Parameters.AddWithValue("@SubTotal", venta.SubTotal);
                    cmd.Parameters.AddWithValue("@TotalDescuento", venta.TotalDescuento);
                    cmd.Parameters.AddWithValue("@Total", venta.Total);
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IdTipoComp", 1);
                    cmd.Parameters.AddWithValue("@ModoPago", venta.ModoPago);
                    cmd.Parameters.AddWithValue("@TotalITBIS", venta.TotalITBIS);
                    cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                    cmd.Parameters.AddWithValue("@NumeroComp", numComp);
                    cmd.ExecuteNonQuery();
                    for (int i = 0; i < listaProductos.Length; i++)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Registrar_VentaDetalle";
                        com.Parameters.AddWithValue("@IdFactura", ventaId);
                        com.Parameters.AddWithValue("@IdProducto", listaProductos[i]);
                        com.Parameters.AddWithValue("@Valor", decimal.Parse(listaTotal[i]));
                        com.Parameters.AddWithValue("@Cantidad", decimal.Parse(listaCantidad[i]));
                        com.Parameters.AddWithValue("@ITBIS", decimal.Parse(listaITBIS[i]));
                        com.Parameters.AddWithValue("@PrecioVenta", decimal.Parse(listaPrecio[i]));
                        com.Parameters.AddWithValue("@Descuento", decimal.Parse(listaDescuento[i]));
                        ViewBag.Message = "Exito";
                        com.ExecuteNonQuery();        
                    }
                    con.Close();
                }
                return View("RegistroVenta");
            //}
           // catch
           // {
           //     return View("RegistroVenta");
           // }
        }

        public string get_Comprobante()
        {
            string secuencia = "";
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select COUNT([Numero de Comprobante]) from Ventas";
                var num = Convert.ToInt32(comm.ExecuteScalar());
                num++;
                for (int i = 0; i < 8 - num.ToString().Length; i++)
                {
                    secuencia = secuencia + "0";
                }
                secuencia = secuencia + num.ToString();
                con.Close();
            }
            return secuencia;
        }

        // GET: Venta/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Venta/Edit/5
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

        // GET: Venta/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Venta/Delete/5
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

        public DataTable Get_Productos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdProducto, Descripcion, Unidades from Productos where Estado = 'En existencia'";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public int GetVentaId()
        {
            int ventaId = 0;
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select MAX(IdVenta) from Ventas";
                try
                {
                    ventaId = Convert.ToInt32(comm.ExecuteScalar()) + 1;
                }
                catch
                {
                    ventaId = 1;
                }
            }
            return ventaId;
        }

        public DataTable GetClientes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdCliente, Nombre + ' ' + Apellido as NombreCompleto, Cedula from Clientes where Estado is null and IdCliente != 11";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public List<string> Get_Precio()
        {
            List<string> dt = new List<string>();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select Valor from ITBIS";
                Compra.ITBIS = Convert.ToDouble(cmd.ExecuteScalar());
                comm.CommandText = "select Descripcion, [Precio de venta] from Productos";
                var reader = comm.ExecuteReader();
                while (reader.Read())
                {
                    dt.Add(reader.GetString(0));
                    dt.Add(reader.GetDecimal(1).ToString());
                }
                con.Close();
            }
            return dt;
        }
    }
}