using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.Entity;
using Veterimax.Views;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Veterimax.Models
{
    public class Suplidor
    {
        public int IdSuplidor { get; set; }
        public string Empresa { get; set; }
        public string RepVentas { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string CorreoElectronico { get; set; }
        public int RNC { get; set; }
        public static Dictionary<int, string> listaSuplidores = new Dictionary<int, string>();
    }
}
