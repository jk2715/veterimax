using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Veterimax.Models;
using Veterimax.Views;
using System.Data;
namespace Veterimax.Controllers
{
    public class UsuarioController : Controller
    {
        static int idUsuario = 0;
        static int intentos = 0;
        public static string bloqueo = "";
        public static int tipoUsuario;
        public static int idus = -2;
        public static string permisos;
        // GET: Usuario
        public ActionResult Index()
        {
            return View("RegistroUsuario");
        }

        public IActionResult Login()
        {
            if(idus !=-2)
            {
                idus = -1;
            }
            ViewData["Message"] = "Inicio de Sesion";
            return View("InicioSesion");
        }

        public IActionResult RecuperacionClave()
        {
            ViewData["Message"] = "Recuperacion de Clave";
            return View("RecuperacionClave");
        }

        public IActionResult ActualizacionClave()
        {
            ViewData["Message"] = "Actualizacion de Clave";
            return View("ActualizacionClave");
        }

        public IActionResult RegistroUsuario()
        {
            ViewData["Message"] = "Registrar un Usuario";
            return View();
        }

        // GET: Usuario/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Usuario/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Usuario/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroUsuario(Usuario usuario, string hidden, string r1Box, string r2Box, string r3Box, string r4Box, string r5Box, string r6Box, string r7Box, string r8Box, string r9Box, string evalHidden)
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS; Database = Veterimax; Trusted_Connection = True; "))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    if(evalHidden == "false")
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Registrar_Usuario";
                        cmd.Parameters.AddWithValue("@idEmpleado", usuario.IdEmpleado);
                        cmd.Parameters.AddWithValue("@Usuario", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                        cmd.Parameters.AddWithValue("@permisos", usuario.Permisos);
                        cmd.Parameters.AddWithValue("@fechaRegistro", DateTime.Now);
                        cmd.Parameters.AddWithValue("@respuesta1", r1Box);
                        cmd.Parameters.AddWithValue("@respuesta2", r2Box);
                        cmd.Parameters.AddWithValue("@respuesta3", r3Box);
                        cmd.Parameters.AddWithValue("@respuesta4", r4Box);
                        cmd.Parameters.AddWithValue("@respuesta5", r5Box);
                        cmd.Parameters.AddWithValue("@respuesta6", r6Box);
                        cmd.Parameters.AddWithValue("@respuesta7", r7Box);
                        cmd.Parameters.AddWithValue("@respuesta8", r8Box);
                        cmd.Parameters.AddWithValue("@respuesta9", r9Box);
                        cmd.ExecuteNonQuery();
                        ViewBag.Message = "Exito";
                    }
                    else
                    {
                        usuario.IdEmpleado = int.Parse(hidden);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Editar_Usuario";
                        cmd.Parameters.AddWithValue("@idEmpleado", usuario.IdEmpleado);
                        cmd.Parameters.AddWithValue("@Usuario", usuario.Nombre);
                        cmd.Parameters.AddWithValue("@permisos", usuario.Permisos);
                        cmd.Parameters.AddWithValue("@fechaMod", DateTime.Now);
                        cmd.Parameters.AddWithValue("@respuesta1", r1Box);
                        cmd.Parameters.AddWithValue("@respuesta2", r2Box);
                        cmd.Parameters.AddWithValue("@respuesta3", r3Box);
                        cmd.Parameters.AddWithValue("@respuesta4", r4Box);
                        cmd.Parameters.AddWithValue("@respuesta5", r5Box);
                        cmd.Parameters.AddWithValue("@respuesta6", r6Box);
                        cmd.Parameters.AddWithValue("@respuesta7", r7Box);
                        cmd.Parameters.AddWithValue("@respuesta8", r8Box);
                        cmd.Parameters.AddWithValue("@respuesta9", r9Box);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return View("RegistroUsuario");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult InicioSesion(Usuario usuario)
        {
            try
            {
                // TODO: Add insert logic here
                using(SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS; Database = Veterimax; Trusted_Connection = True; "))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Validar_Usuario";
                    cmd.Parameters.AddWithValue("@Usuario", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                    usuario.IdUsuario = Convert.ToInt32(cmd.ExecuteScalar());
                    idus = usuario.IdUsuario;
                    var com = con.CreateCommand();
                    com.CommandText = "select u.IdUsuario, u.Permisos, e.IdEmpleado, te.IdTipoEmpleado from Usuarios u left join Empleados e on u.IdEmpleado = e.IdEmpleado inner join [Tipos de Empleados] te on e.IdTIpoEmpleado = te.IdTipoEmpleado";
                    var reader = com.ExecuteReader();
                    var dt = new DataTable();
                    dt.Load(reader);
                    foreach(DataRow x in dt.Rows)
                    {
                        if(idus == Convert.ToInt32(x["IdUsuario"]))
                        {
                            tipoUsuario = Convert.ToInt32(x["IdTipoEmpleado"]);
                            permisos = x["Permisos"].ToString();
                            break;
                        }
                    }
                }
                if(usuario.IdUsuario > 0)
                {
                    return RedirectToAction("Index", "Home", null);
                }
                else
                {
                    
                    return View();
                }
            }
            catch
            {
                if(intentos == 3)
                {
                     bloqueo = "Ha fallado en 3 intentos de inicio de sesion";
                }
                else
                {
                    intentos++;
                    ViewBag.Message = "Combinacion de usuario y clave incorrectas.";
                }
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidarAdmin(Usuario usuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS; Database = Veterimax; Trusted_Connection = True; "))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Validar_Admin";
                    cmd.Parameters.AddWithValue("@usuario", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@clave", usuario.Clave);
                    idUsuario = Convert.ToInt32(cmd.ExecuteScalar());
                }
                if (idUsuario > 0)
                {
                    bloqueo = "";
                    ViewBag.Message = "Exito";
                    return View("InicioSesion");
                }
                else
                {
                    return View("InicioSesion");
                }
            }
            catch
            {
                ViewBag.Message = "Combinacion de usuario y clave incorrectas.";
                return View("InicioSesion");
            }
        }

        public ActionResult RecuperarClave(Usuario usuario, string r1Box, string r2Box, string r3Box)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS; Database = Veterimax; Trusted_Connection = True; "))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Get_Respuestas";
                    cmd.Parameters.AddWithValue("@nombreUsuario", usuario.Nombre);
                    cmd.Parameters.AddWithValue("@respuesta1", r1Box);
                    cmd.Parameters.AddWithValue("@respuesta2", r2Box);
                    cmd.Parameters.AddWithValue("@respuesta3", r3Box);
                    idUsuario = Convert.ToInt32(cmd.ExecuteScalar());
                }
                if (idUsuario > 0)
                {
                    return RedirectToAction("ActualizacionClave", "Usuario", null);
                }
                else
                {
                    return View("RecuperacionClave");
                }
            }
            catch
            {
                ViewBag.Message = "Preguntas Incorrectas";
                return View("RecuperacionClave");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ActualizarClave(Usuario usuario)
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS; Database = Veterimax; Trusted_Connection = True; "))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Actualizar_Clave";
                    cmd.Parameters.AddWithValue("@IdUsuario", idUsuario);
                    cmd.Parameters.AddWithValue("@Clave", usuario.Clave);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction("Login", "Usuario", null);
            }
            catch
            {
                return View();
            }
        }

        public DataTable GetUsuarios()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select Usuarios.IdUsuario, e.IdEmpleado, e.Nombre + ' ' + e.Apellido as NombreCompleto, Usuario, Permisos, Usuarios.FechaRegistro from Usuarios left join Empleados e on Usuarios.IdEmpleado = e.IdEmpleado where Usuarios.Estado is null";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable GetRespuestas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select IdUsuario, Respuesta1, Respuesta2, Respuesta3, Respuesta4, Respuesta5, Respuesta6, Respuesta7, Respuesta8, Respuesta9 from Usuarios";
                var reader = comm.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        // GET: Usuario/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Usuario/Edit/5
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

        // GET: Usuario/Delete/5
        public ActionResult EliminarUsuario(string listaUsuarios)
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                if (listaUsuarios.Contains(','))
                {
                    string[] lista = listaUsuarios.Split(',');
                    foreach (string x in lista)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Eliminar_Usuarii";
                        com.Parameters.AddWithValue("@IdUsuario", x);
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Eliminar_Usuario";
                    cmd.Parameters.AddWithValue("@IdUsuario", listaUsuarios);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return View("RegistroUsuario");
        }

        // POST: Usuario/Delete/5
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