using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class Producto
    {
        public string IdProducto { get; set; }
        public int IdSuplidor { get; set; }
        public int IdTipoProducto { get; set; }
        public string Descripcion { get; set; }
        public double Cantidad { get; set; }
        public double PrecioVenta { get; set; }
        public bool Servicio { get; set; }
        public string Estado { get; set; }
        public double PuntoReorden { get; set; }
        public double Costo { get; set; }
        public double Ganancia { get; set; }
        public string Unidades { get; set; }
        public double Maximo { get; set; }
        public static Dictionary<string, string> listaProductos = new Dictionary<string, string>();
    }
}
