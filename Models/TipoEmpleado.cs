using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class TipoEmpleado
    {
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string FechaRegistro { get; set; }
        public static Dictionary<int, string> listaTiposEmp = new Dictionary<int, string>();
    }
}
