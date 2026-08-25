using System;

namespace BriSky.Models.Ubicaciones
{
    public class Aeropuerto
    {
        public string CodAeropuerto { get; set; }
        public string Nombre { get; set; }
        public string Pais { get; set; }
        public string Caracteristicas { get; set; }
        public string CodCiudad { get; set; }

        // Propiedad auxiliar UI
        public string NombreCiudad { get; set; }

        public Aeropuerto() { }

        public Aeropuerto(string codAeropuerto, string nombre, string pais, string caracteristicas, string codCiudad)
        {
            CodAeropuerto = codAeropuerto;
            Nombre = nombre;
            Pais = pais;
            Caracteristicas = caracteristicas;
            CodCiudad = codCiudad;
        }
    }
}
