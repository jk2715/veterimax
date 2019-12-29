using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Veterimax.Models;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace Veterimax.Controllers
{
    public class SuplidorController : Controller
    {
        public static bool error = false;
        // GET: Suplidor
        public ActionResult Index()
        {
            return View("RegistroSuplidor");
        }

        public IActionResult RegistroSuplidor()
        {
            ViewData["Message"] = "Registrar un Suplidor";
            GetSuplidores();
            return View();
        }

        public IActionResult ConsultaSuplidor()
        {
            ViewData["Message"] = "Consultar un Suplidor";
            return View();
        }

        // GET: Suplidor/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Suplidor/Create
        public DataTable GetSuplidores()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select Empresa, RepresentanteDeVenta, Direccion, Telefono, CorreoElectronico, RNC, FechaRegistro from Suplidores where Estado is null";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        // POST: Suplidor/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroSuplidor(Suplidor suplidor, string evalHidden, string listaRNC)
        {
              //  try
              //  {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    if(evalHidden == "false")
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Registrar_Suplidor";
                        cmd.Parameters.AddWithValue("@empresa", suplidor.Empresa);
                        cmd.Parameters.AddWithValue("@representante", suplidor.RepVentas);
                        cmd.Parameters.AddWithValue("@direccion", suplidor.Direccion);
                        cmd.Parameters.AddWithValue("@telefono", suplidor.Telefono);
                        cmd.Parameters.AddWithValue("@correo", suplidor.CorreoElectronico);
                        cmd.Parameters.AddWithValue("@rnc", suplidor.RNC);
                        cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                        if(Convert.ToInt32(cmd.ExecuteScalar()) == -1)
                        {
                            error = true;
                            return View("RegistroSuplidor");
                        }
                        else
                        {
                            ViewBag.Message = "Exito";
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                    else if(evalHidden == "true")
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Editar_Suplidor";
                        cmd.Parameters.AddWithValue("@empresa", suplidor.Empresa);
                        cmd.Parameters.AddWithValue("@representante", suplidor.RepVentas);
                        cmd.Parameters.AddWithValue("@direccion", suplidor.Direccion);
                        cmd.Parameters.AddWithValue("@telefono", suplidor.Telefono);
                        cmd.Parameters.AddWithValue("@correo", suplidor.CorreoElectronico);
                        cmd.Parameters.AddWithValue("@rnc", suplidor.RNC);
                        cmd.Parameters.AddWithValue("@FechaMod", DateTime.Now);
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                    else
                    {
                        if (listaRNC.Contains(','))
                        {
                            string[] lista = listaRNC.Split(',');
                            foreach(string x in lista)
                            {
                                var com = con.CreateCommand();
                                com.CommandType = System.Data.CommandType.StoredProcedure;
                                com.CommandText = "Eliminar_Suplidor";
                                com.Parameters.AddWithValue("@rnc", int.Parse(x));
                                com.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "Eliminar_Suplidor";
                            cmd.Parameters.AddWithValue("@rnc", int.Parse(listaRNC));
                            cmd.ExecuteNonQuery();
                        }
                        con.Close();
                    }
                }
                return RedirectToAction(nameof(Index));
              //  }
             //  catch
              // {
              //  return View("RegistroSuplidor");
               //}

        }

        // GET: Suplidor/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Suplidor/Edit/5
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

        // GET: Suplidor/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Suplidor/Delete/5
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