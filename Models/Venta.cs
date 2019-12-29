using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdCliente { get; set; }
        public double SubTotal { get; set; }
        public double TotalITBIS { get; set; }
        public double TotalDescuento { get; set; }
        public double Total { get; set; }
        public string ModoPago { get; set; }
        public string NumeroComprobante { get; set; }
        public int TipoComprobante { get; set; }
    }
}
