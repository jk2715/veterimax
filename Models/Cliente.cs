using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class Cliente
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public char Sexo { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public bool Credito { get; set; }
        public string CorreoElectronico { get; set; }
        public double Balance { get; set; }
        public static Dictionary<int, string> listaClientes = new Dictionary<int, string>();
    }
}
