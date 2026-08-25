using System;

namespace BriSky.Models.Ubicaciones
{
    public class Ciudad
    {
        public string CodCiudad { get; set; }
        public string Nombre { get; set; }
        public string Departamento { get; set; }

        public Ciudad() { }

        public Ciudad(string codCiudad, string nombre, string departamento)
        {
            CodCiudad = codCiudad;
            Nombre = nombre;
            Departamento = departamento;
        }
    }
}
