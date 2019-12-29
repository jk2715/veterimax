using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class Compra
    {
        public int IdCompra { get; set; }
        public int IdSuplidor { get; set; }
        public double Total { get; set; }
        public double TotalITBIS { get; set; }
        public DateTime FechaRegistro { get; set; }
        public string ModoPago { get; set; }
        public string NumeroComprobante { get; set; }
        public string Comentarios { get; set; }
        public static List<string> productoArray = new List<string>();
        public static int tempCompraId;
        public static int count = 0;
        public static double ITBIS = 0.00;
    }
}
