using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Veterimax.Models;
using System.Data;

namespace Veterimax.Controllers
{
    public class EmpleadoController : Controller
    {
        public static DataTable dt = new DataTable();
        // GET: Empleado
        public ActionResult Index()
        {
            return View("RegistroEmpleado");
        }

        public IActionResult RegistroEmpleado()
        {
            GetTiposEmpleados();
            ViewData["Message"] = "Registrar un Empleado";
            return View();
        }

        public IActionResult ConsultaEmpleado()
        {
            ConsultarEmpleados();
            ViewData["Message"] = "Ver informacion de Empleados";
            return View();
        }

        // GET: Empleado/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Empleado/Create
        public ActionResult ConsultarEmpleados()
        {
            using(SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select Nombre + ' ' + Apellido as NombreCompleto, Sexo, Cedula, Direccion, Telefono, CorreoElectronico, Empleados.FechaRegistro, a.Titulo, Salario from Empleados left join[Tipos de Empleados] a on Empleados.IdTIpoEmpleado = a.IdTipoEmpleado";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return View();
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroEmpleado(Empleado empleado, string dia, string mes, string ano, string evalHidden)
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    if(evalHidden == "false")
                    {
                        empleado.FechaNacimiento = DateTime.ParseExact(mes + "/" + dia + "/" + ano, "d", null);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Registrar_Empleado";
                        cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                        cmd.Parameters.AddWithValue("@Sexo", empleado.Sexo);
                        cmd.Parameters.AddWithValue("@Cedula", empleado.Cedula);
                        cmd.Parameters.AddWithValue("@FechaNac", empleado.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@Direccion", empleado.Direccion);
                        cmd.Parameters.AddWithValue("@Telefono", empleado.Telefono);
                        cmd.Parameters.AddWithValue("@Correo", empleado.Correo);
                        cmd.Parameters.AddWithValue("@IdTipoEmpleado", Convert.ToInt32(empleado.IdTipo));
                        cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                        cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                        if(Convert.ToInt32(cmd.ExecuteScalar()) == -1)
                        {
                            ViewBag.Message = "Entrada ya existe";
                            return View("RegistroEmpleado");
                        }
                        else
                        {
                            ViewBag.Message = "Exito";
                            cmd.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        empleado.FechaNacimiento = DateTime.ParseExact(mes + "/" + dia + "/" + ano, "d", null);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Editar_Empleado";
                        cmd.Parameters.AddWithValue("@Nombre", empleado.Nombre);
                        cmd.Parameters.AddWithValue("@Apellido", empleado.Apellido);
                        cmd.Parameters.AddWithValue("@Sexo", empleado.Sexo);
                        cmd.Parameters.AddWithValue("@Cedula", empleado.Cedula);
                        cmd.Parameters.AddWithValue("@FechaNac", empleado.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@Direccion", empleado.Direccion);
                        cmd.Parameters.AddWithValue("@Telefono", empleado.Telefono);
                        cmd.Parameters.AddWithValue("@Correo", empleado.Correo);
                        cmd.Parameters.AddWithValue("@IdTipoEmpleado", Convert.ToInt32(empleado.IdTipo));
                        cmd.Parameters.AddWithValue("@FechaMod", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Salario", empleado.Salario);
                    }
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public DataTable GetEmpleados()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select Nombre, Apellido, Sexo, FechaNacimiento, a.Titulo, Cedula, Telefono, Direccion, CorreoElectronico, Salario, Empleados.FechaRegistro from Empleados left join[Tipos de Empleados] a on Empleados.IdTIpoEmpleado = a.IdTipoEmpleado where Empleados.Estado != 'Eliminado'";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable ListaEmpleados()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdEmpleado, Nombre + ' ' + Apellido as NombreCompleto from Empleados where IdEmpleado not in (select IdEmpleado from Usuarios)";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        // GET: Empleado/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Empleado/Edit/5
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

        // GET: Empleado/Delete/5
        public ActionResult EliminarEmpleado(string listaEmpleado)
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                if (listaEmpleado.Contains(','))
                {
                    string[] lista = listaEmpleado.Split(',');
                    foreach (string x in lista)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Eliminar_Empleado";
                        com.Parameters.AddWithValue("@Cedula", x);
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Eliminar_Empleado";
                    cmd.Parameters.AddWithValue("@Cedula", listaEmpleado);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return View("RegistroEmpleado");
        }

        // POST: Empleado/Delete/5
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

        public void GetTiposEmpleados()
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;")){
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdTipoEmpleado, Titulo from [Tipos de Empleados]";
                var reader = comm.ExecuteReader();
                TipoEmpleado.listaTiposEmp.Clear();
                while (reader.Read())
                {
                    TipoEmpleado.listaTiposEmp.Add(reader.GetInt32(0), reader.GetString(1));
                }
                con.Close();
            }
        }
    }
}