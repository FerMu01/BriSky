using System;

namespace BriSky.Models.Personal
{
    public class Area
    {
        public string CodArea { get; set; }
        public string Nombre { get; set; }
        public string Funcion { get; set; }

        public Area() { }

        public Area(string codArea, string nombre, string funcion)
        {
            CodArea = codArea;
            Nombre = nombre;
            Funcion = funcion;
        }
    }
}
