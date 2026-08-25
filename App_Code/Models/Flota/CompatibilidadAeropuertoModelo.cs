using System;

namespace BriSky.Models.Flota
{
    public class CompatibilidadAeropuertoModelo
    {
        public string CodAeropuerto { get; set; }
        public string CodModelo { get; set; }
        public string Restricciones { get; set; }

        // Propiedades Auxiliares UI
        public string NombreAeropuerto { get; set; }
        public string NombreModelo { get; set; }
    }
}
