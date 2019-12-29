using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using Veterimax.Models;

namespace Veterimax.Controllers
{
    public class PagoController : Controller
    {
        // GET: Pago
        public ActionResult Index()
        {
            return View();
        }

        // GET: Pago/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public IActionResult PagoCliente()
        {
            ViewData["Message"] = "Registro de Pago de Cliente";
            return View();
        }

        // GET: Pago/Create
        public ActionResult Create()
        {
            return View();
        }

        public DataTable GetPagos()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Get_PagosClientes";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable Get_Deudas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Buscar_FacturasPendientes";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable Get_Clientes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdCliente, Nombre + ' ' + Apellido as Cliente from Clientes where IdCliente in (select IdCliente from Ventas where Estado = 'Pendiente')";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        // POST: Pago/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Registrar_PagoCliente(PagoCliente pago, string listaMonto, string listaFacturas, string listaEstado)
        {
          //  try
         //   {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    string[] listaPagos = listaMonto.Split(",");
                    string[] listaVentas = listaFacturas.Split(",");
                    string[] listaEst = listaEstado.Split(",");
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Registrar_PagoCliente";
                    cmd.Parameters.AddWithValue("@IdCliente", pago.IdCliente);
                    cmd.Parameters.AddWithValue("@Monto", pago.Monto);
                    cmd.Parameters.AddWithValue("@FechaPago", DateTime.Now);
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                    cmd.ExecuteNonQuery();
                    for (int i = 0; i < listaPagos.Length; i++)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Registrar_PagoClDetalle";
                        com.Parameters.AddWithValue("@IdVenta", int.Parse(listaVentas[i]));
                        com.Parameters.AddWithValue("@Monto", decimal.Parse(listaPagos[i]));
                        com.Parameters.AddWithValue("@Estado", listaEst[i]);
                        ViewBag.Message = "Exito";
                        com.ExecuteNonQuery();
                    }

                    con.Close();
                }
                return View("PagoCliente");
           // }
           // catch
           // {
           //     return View("PagoCliente");
           // }
        }

        // GET: Pago/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Pago/Edit/5
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

        // GET: Pago/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Pago/Delete/5
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