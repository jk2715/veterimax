using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class PagoCliente
    {
        public int IdPago { get; set; }
        public int IdCliente { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaPago { get; set; }

    }
}
