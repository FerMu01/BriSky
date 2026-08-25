using System;

namespace BriSky.Models.Flota
{
    public class Avion
    {
        public string CodInterno { get; set; }
        public string Matricula { get; set; }
        public DateTime FechaIncorporacion { get; set; }
        public string Estado { get; set; }
        
        // Propiedades Nullable según base de datos
        public DateTime? UltimoMantenimiento { get; set; }
        public DateTime? ProximoMantenimiento { get; set; }
        
        public string CodModelo { get; set; }

        // Propiedad de Apoyo UI
        public string NombreModelo { get; set; }
    }
}
