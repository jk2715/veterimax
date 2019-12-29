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
    public class ClienteController : Controller
    {
        // GET: Cliente
        public ActionResult Index()
        {
            return View("RegistroCliente");
        }

        public IActionResult RegistroCliente()
        {
            ViewData["Message"] = "Registrar un Cliente";
            return View();
        }

        // GET: Cliente/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroCliente(Cliente cliente, string evalHidden)
        {
            try
            {
                 //TODO: Add insert logic here
                using(SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    if(evalHidden == "false")
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Registrar_Cliente";
                        cmd.Parameters.AddWithValue("@IdTipoCliente", 1);
                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                        cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                        cmd.Parameters.AddWithValue("@Cedula", cliente.Cedula);
                        cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                        cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                        cmd.Parameters.AddWithValue("@Correo", cliente.CorreoElectronico);
                        cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Credito", cliente.Credito);
                        cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                        cmd.Parameters.AddWithValue("@Balance", 0.00);
                        if(Convert.ToInt32(cmd.ExecuteScalar()) == -1)
                        {
                            ViewBag.Message = "Entrada ya existe";
                            return View("RegistroCliente");
                        }
                        else
                        {
                            ViewBag.Message = "Exito";
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                    else
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Editar_Cliente";
                        cmd.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", cliente.Apellido);
                        cmd.Parameters.AddWithValue("@Sexo", cliente.Sexo);
                        cmd.Parameters.AddWithValue("@Cedula", cliente.Cedula);
                        cmd.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                        cmd.Parameters.AddWithValue("@Direccion", cliente.Direccion);
                        cmd.Parameters.AddWithValue("@Correo", cliente.CorreoElectronico);
                        cmd.Parameters.AddWithValue("@FechaMod", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Credito", cliente.Credito);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cliente/Edit/5
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

        // GET: Cliente/Delete/5
        public ActionResult EliminarCliente(string listaClientes)
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                if (listaClientes.Contains(','))
                {
                    string[] lista = listaClientes.Split(',');
                    foreach (string x in lista)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Eliminar_Cliente";
                        com.Parameters.AddWithValue("@Cedula", x);
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Eliminar_Cliente";
                    cmd.Parameters.AddWithValue("@Cedula", listaClientes);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return View("RegistroCliente");
        }

        // POST: Cliente/Delete/5
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

        public DataTable GetClientes()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select Nombre, Apellido, Sexo, Cedula, Telefono, Direccion, CorreoElectronico, Credito, Balance, FechaRegistro from Clientes where Estado is null";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }
    }
}