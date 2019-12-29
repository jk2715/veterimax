using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class Procedimiento
    {
        public int IdProcedimiento { get; set; }
        public int IdTipoProcedimiento { get; set; }
        public int IdMascota { get; set; }
        public string Comentarios { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalITBIS { get; set; }
        public string NumeroComprobante { get; set; }
        public string ModoPago { get; set; }
    }
}
