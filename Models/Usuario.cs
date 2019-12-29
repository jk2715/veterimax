using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Veterimax.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public int IdEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Clave { get; set; }
        public string Permisos { get; set; }
        public static string p1 = "Cual es su color preferido?";
        public static string p2 = "Cual fue el nombre de su primera mascota?";
        public static string p3 = "Cual es el nombre del lugar donde crecio?";
        public static string p4 = "Cual es su pasatiempo preferido?";
        public static string p5 = "Cual es el nombre de la escuela primaria en la que estudio?";
        public static string p6 = "Cual es el nombre de su primer(a) novio(a)?";
        public static string p7 = "Cul es su tipo de musica preferida?";
        public static string p8 = "Cual es el nombre de su mejor amigo(a)?";
        public static string p9 = "A que lugar del mundo desearia visitar?";
    }
}
