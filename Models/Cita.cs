using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class Cita
    {
        public int IdCita { get; set; }
        public int IdCliente { get; set; }
        public int IdMascota { get; set; }
        public DateTime Fecha { get; set; }
        public string Estado { get; set; }
        public int IdProcedimiento { get; set; }

    }
}
