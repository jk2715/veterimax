using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using Veterimax.Models;
using System.Data;
using Microsoft.Extensions.Caching.Memory;

namespace Veterimax.Controllers
{
    public class MascotaController : Controller
    {
        public IMemoryCache _cache;
        public static int IdCita;
        
        // GET: Mascota
        public ActionResult Index()
        {
            return View("RegistroMascota");
        }

        public IActionResult RegistroMascota()
        {
            GetClientes();
            ViewData["Message"] = "Registrar una Mascota";
            return View();
        }

        public IActionResult RegistroCita()
        {
            CancelarCitas();
            ViewData["Message"] = "Registrar una Mascota";
            return View();
        }

        public IActionResult RegistroProcedimiento()
        {
/*            if(_cache.Get("cita") != null)
            {
                IdCita = Convert.ToInt32(_cache.Get("cita"));
            }*/
            ViewData["Message"] = "Registrar un Procedimiento";
            return View();
        }

        // GET: Mascota/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Mascota/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Mascota/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroMascota(Mascota mascota, string dia, string mes, string ano, string hidden, string evalHidden, string mascotaId)
        {
          //  try
           // {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    if(mascota.IdCliente == 0)
                    {
                        mascota.IdCliente = int.Parse(hidden);
                    }
                    if(mascota.Observaciones == null) {
                        mascota.Observaciones = "";
                    }
                    con.Open();
                    var cmd = con.CreateCommand();
                    mascota.FechaNacimiento = DateTime.ParseExact(mes + "/" + dia + "/" + ano, "d", null);
                    if(evalHidden == "false")
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Registrar_Mascota";
                        cmd.Parameters.AddWithValue("@IdCliente", mascota.IdCliente);
                        cmd.Parameters.AddWithValue("@Descripcion", mascota.Descripcion);
                        cmd.Parameters.AddWithValue("@Nombre", mascota.Nombre);
                        cmd.Parameters.AddWithValue("@Raza", mascota.Raza);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", mascota.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@Sexo", mascota.Sexo);
                        cmd.Parameters.AddWithValue("@Especie", mascota.Especie);
                        cmd.Parameters.AddWithValue("@Observaciones", mascota.Observaciones);
                        cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                        ViewBag.Message = "Exito";
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        mascota.IdMascota = int.Parse(mascotaId);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Editar_Mascota";
                        cmd.Parameters.AddWithValue("@IdMascota", mascota.IdMascota);
                        cmd.Parameters.AddWithValue("@Descripcion", mascota.Descripcion);
                        cmd.Parameters.AddWithValue("@Nombre", mascota.Nombre);
                        cmd.Parameters.AddWithValue("@Raza", mascota.Raza);
                        cmd.Parameters.AddWithValue("@FechaNacimiento", mascota.FechaNacimiento);
                        cmd.Parameters.AddWithValue("@Sexo", mascota.Sexo);
                        cmd.Parameters.AddWithValue("@Especie", mascota.Especie);
                        cmd.Parameters.AddWithValue("@Observaciones", mascota.Observaciones);
                        cmd.Parameters.AddWithValue("@FechaMod", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return RedirectToAction(nameof(Index));
            }
           // catch
            //{
            //    return View();
            //}
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroCita(Cita cita, string dia, string mes, string ano, string hora, string minuto, string tiempo, string evalHidden)
        {
            try
            {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    var cmd = con.CreateCommand();
                    string fecha = mes + "/" + dia + "/" + ano + ' ' + hora + ':' + minuto + ' ' + tiempo;
                    cita.IdProcedimiento = Get_IDProcedimiento();
                    if (evalHidden == "false")
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "Registrar_Cita";
                        cmd.Parameters.AddWithValue("@IdMascota", cita.IdMascota);
                        cmd.Parameters.AddWithValue("@Fecha", fecha);
                        cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                        cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                        if (Convert.ToInt32(cmd.ExecuteScalar()) == -1)
                        {
                            ViewBag.Message = "Entrada ya existe";
                            return View("RegistroCita");
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
                        cmd.CommandText = "Editar_Cita";
                        cmd.Parameters.AddWithValue("@IdCita", cita.IdCita);
                        cmd.Parameters.AddWithValue("@IdMascota", cita.IdMascota);
                        cmd.Parameters.AddWithValue("@Fecha", cita.Fecha);
                        cmd.Parameters.AddWithValue("@FechaMod", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return View("RegistroCita");
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegistroProcedimiento(Procedimiento procedimiento, string cita, string itbisH, string subTotalHidden, string totalH, string tiposHidden, string procedimientoHidden, string precioHidden, string itbisHidden, string totalHidden, string mascotaHidden, string tipoComp)
        {
        //   try
          // {
                // TODO: Add insert logic here
                using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
                {
                    con.Open();
                    string[] listaTipos = tiposHidden.Split(",");
                    string[] listaProcedimiento = procedimientoHidden.Split(",");
                    string[] listaPrecio = precioHidden.Split(",");
                    string[] listaITBIS = itbisHidden.Split(",");
                    string[] listaTotal = totalHidden.Split(",");
                    procedimiento.IdMascota = int.Parse(mascotaHidden);
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Registrar_Procedimiento";
                    itbisH = itbisH.Remove(0, 2);
                    totalH = totalH.Remove(0, 1);
                    procedimiento.Total = decimal.Parse(totalH);
                    procedimiento.TotalITBIS = decimal.Parse(itbisH);
                    string numeroComp = "B" + procedimiento.NumeroComprobante + get_Comprobante();
                    cmd.Parameters.AddWithValue("@Total", procedimiento.Total);
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                    cmd.Parameters.AddWithValue("@IdMascota", procedimiento.IdMascota);
                    cmd.Parameters.AddWithValue("@ModoPago", procedimiento.ModoPago);
                    cmd.Parameters.AddWithValue("@TotalITBIS", procedimiento.TotalITBIS);
                    cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                    cmd.Parameters.AddWithValue("@NumeroComprobante", numeroComp);
                    cmd.Parameters.AddWithValue("@IdTipoComprobante", tipoComp);
                    cmd.Parameters.AddWithValue("@SubTotal", decimal.Parse(subTotalHidden));
                    cmd.ExecuteNonQuery();
                    for (int i = 0; i < listaTipos.Length; i++)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Registrar_ProcedimientoDetalle";
                        com.Parameters.AddWithValue("@IdTipoProcedimiento", listaTipos[i]);
                        com.Parameters.AddWithValue("@Descripcion", listaProcedimiento[i]);
                        com.Parameters.AddWithValue("@ITBIS", decimal.Parse(listaITBIS[i]));
                        com.Parameters.AddWithValue("@Valor", decimal.Parse(listaTotal[i]));
                        ViewBag.Message = "Exito";
                        com.ExecuteNonQuery();
                    }
                    if(cita != "0")
                    {
                        var command = con.CreateCommand();
                        command.CommandType = CommandType.StoredProcedure;
                        command.CommandText = "Actualizar_Cita";
                        command.Parameters.AddWithValue("@IdCita", int.Parse(cita));
                        command.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return View("RegistroProcedimiento");
           // }
           // catch
           // {
           //    return View();
           // }
        }

        public string get_Comprobante()
        {
            string secuencia = "";
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var comm = con.CreateCommand();
                comm.CommandText = "select ISNULL(COUNT(NumeroComprobante), 0) from Procedimientos";
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

        public int Get_IDProcedimiento()
        {
            int x = 0;
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select max(IdProcedimiento) from Procedimientos";
                try
                {
                    x = Convert.ToInt32(cmd.ExecuteScalar()) + 1;
                }
                catch
                {
                    x++;
                }
                con.Close();
            }
            return x;
        }

        public DataTable GetMascotas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select Mascotas.IdCliente, c.Nombre + ' ' + c.Apellido as Duenio, IdMascota, Mascotas.Nombre, Especie, Raza, Mascotas.Sexo, Descripcion, FechaNacimiento, Observaciones, Mascotas.FechaRegistro from Mascotas left join Clientes c on Mascotas.IdCliente = c.IdCliente where Mascotas.Estado is null";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public void CancelarCitas()
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Cancelar_Citas";
                var reader = cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        // GET: Mascota/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Mascota/Edit/5
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

        // GET: Mascota/Delete/5
        public ActionResult EliminarMascota(string listaMascotas)
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                if (listaMascotas.Contains(','))
                {
                    string[] lista = listaMascotas.Split(',');
                    foreach (string x in lista)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Eliminar_Mascota";
                        com.Parameters.AddWithValue("@IdMascota", x);
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Eliminar_Mascota";
                    cmd.Parameters.AddWithValue("@IdMascota", listaMascotas);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return View("RegistroMascota");
        }

        public ActionResult EliminarCita(string listaCitas)
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                if (listaCitas.Contains(','))
                {
                    string[] lista = listaCitas.Split(',');
                    foreach (string x in lista)
                    {
                        var com = con.CreateCommand();
                        com.CommandType = System.Data.CommandType.StoredProcedure;
                        com.CommandText = "Cancelar_Cita";
                        com.Parameters.AddWithValue("@IdCita", x);
                        com.ExecuteNonQuery();
                    }
                }
                else
                {
                    var cmd = con.CreateCommand();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Cancelar_Cita";
                    cmd.Parameters.AddWithValue("@IdCita", listaCitas);
                    cmd.ExecuteNonQuery();
                }
                con.Close();
            }
            return View("RegistroCita");
        }
        // POST: Mascota/Delete/5
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

        public DataTable GetDuenios()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select Clientes.IdCliente, Clientes.Cedula, Clientes.Nombre + ' ' + Apellido as NombreCompleto, m.IdMascota, m.Nombre from Clientes left join Mascotas m on Clientes.IdCliente = m.IdCliente where Clientes.IdCliente in (select IdCliente from Mascotas) and Clientes.Estado is null and Clientes.IdCliente != 11";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public void GetClientes()
        {
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdCliente, Nombre + ' ' + Apellido as NombreCompleto from Clientes where Estado is null";
                var reader = cmd.ExecuteReader();
                Cliente.listaClientes.Clear();
                while (reader.Read())
                {
                    Cliente.listaClientes.Add(reader.GetInt32(0), reader.GetString(1));
                }
                con.Close();
            }
        }

        public DataTable GetCitas()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "Filtrar_Citas";
                cmd.Parameters.AddWithValue("@IdUsuario", UsuarioController.idus);
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }

        public DataTable GetTipoProcedimiento()
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection("Server = DESKTOP-PQRUVP8\\SQLEXPRESS;Database=Veterimax;Trusted_Connection=True;"))
            {
                con.Open();
                var cmd = con.CreateCommand();
                cmd.CommandText = "select IdTipoProcedimiento, Descripcion from [Tipo Procedimiento]";
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
                con.Close();
            }
            return dt;
        }
    }
}