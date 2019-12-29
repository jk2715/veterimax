using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterimax.Models;
using System.Data.SqlClient;
using System.Data;

namespace Veterimax.Controllers
{
    public class ProductoController : Controller
    {
        public static DataTable dt = new DataTable();
        // GET: Producto
        public ActionResult Index()
        {
            return View("RegistroProducto");
        }

        public IActionResult RegistroProducto()
        {
            ViewData["Message"] = "Registrar un Producto";
            return View("RegistroProducto");
        }

        public IActionResult ConsultaProducto()
        {
            ConsultarProducto();
            ViewData["Message"] = "Ver Productos";
            return View("ConsultaProducto");
        }
        // GET: Producto/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Producto/Create
        public ActionResult ConsultarProducto()
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS; Database = Veterimax; Trusted_Connection = True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdProducto, Productos.Descripcion, a.Categoria, Cantidad, [Precio de Venta], PuntoReorden from Productos left join[Tipo Producto] a on Productos.IdTipoProducto = a.IdTipoProducto";
                var preader = cmd.ExecuteReader();
                dt.Load(preader);
                con.Close();
            }
            return View();
        }

        // POST: Producto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroProducto(Producto producto, string evalHidden, string precioHidden)
        {
            //try
           // {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS; Database = Veterimax; Trusted_Connection = True;"))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    producto.PrecioVenta = double.Parse(precioHidden);
                    if(evalHidden == "false")
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Registrar_Producto";
                        cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                        cmd.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        cmd.Parameters.AddWithValue("@Unidades", producto.Unidades);
                        cmd.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                        cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                        cmd.Parameters.AddWithValue("@Servicio", producto.Servicio);
                        cmd.Parameters.AddWithValue("@Estado", "En Existencia");
                        cmd.Parameters.AddWithValue("@PuntoReorden", producto.PuntoReorden);
                        cmd.Parameters.AddWithValue("@Costo", producto.Costo);
                        cmd.Parameters.AddWithValue("@Ganancia", producto.Ganancia);
                        cmd.Parameters.AddWithValue("@Maximo", producto.Maximo);
                        if(Convert.ToInt32(cmd.ExecuteScalar()) == -1)
                        {
                            ViewBag.Message = "Entrada ya existe";
                            return View("RegistroProducto");
                        }
                        else
                        {
                            ViewBag.Message = "Exito";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Editar_Producto";
                        cmd.Parameters.AddWithValue("@IdProducto", producto.IdProducto);
                        cmd.Parameters.AddWithValue("@IdTipoProducto", producto.IdTipoProducto);
                        cmd.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
                        cmd.Parameters.AddWithValue("@Unidades", producto.Unidades);
                        cmd.Parameters.AddWithValue("@PrecioVenta", producto.PrecioVenta);
                        cmd.Parameters.AddWithValue("@FechaMod", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Servicio", producto.Servicio);
                        cmd.Parameters.AddWithValue("@PuntoReorden", producto.PuntoReorden);
                        cmd.Parameters.AddWithValue("@Costo", producto.Costo);
                        cmd.Parameters.AddWithValue("@Ganancia", producto.Ganancia);
                        cmd.Parameters.AddWithValue("@Maximo", producto.Maximo);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
           // }
          //  catch
           // {
           //     return View();
           // }
        }

        public DataTable GetProductos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdProducto, tp.Categoria, Productos.Descripcion, Cantidad, Unidades, Costo, MargenGanancia, [Precio de venta], Productos.FechaRegistro, Servicio, Productos.Estado, PuntoReorden, Maximo from Productos left join[Tipo Producto] tp on Productos.IdTipoProducto = tp.IdTipoProducto where Productos.Estado != 'Eliminado'";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable GetProductosOrdenar()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdProducto, tp.Categoria, Productos.Descripcion, Cantidad, Unidades, Costo, MargenGanancia, [Precio de venta], Productos.FechaRegistro, Servicio, Productos.Estado, PuntoReorden, Maximo from Productos left join[Tipo Producto] tp on Productos.IdTipoProducto = tp.IdTipoProducto where Productos.Estado = 'En existencia' order by Cantidad asc";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable GetTiposProductos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdTipoProducto, Categoria from [Tipo Producto]";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable GetSuplidores()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdSuplidor, Empresa from Suplidores where Estado is null";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        // GET: Producto/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Producto/Edit/5
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

        // GET: Producto/Delete/5
        public ActionResult EliminarProducto(string listaProductos)
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                if (listaProductos.Contains(','))
                {
                    string[] lista = listaProductos.Split(',');
                    foreach (string x in lista)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Eliminar_Producto";
                        com.Parameters.AddWithValue("@IdProducto", x);
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Eliminar_Producto";
                    cmd.Parameters.AddWithValue("@IdProducto", listaProductos);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return View("RegistroProducto");
        }

        // POST: Producto/Delete/5
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
    }
}