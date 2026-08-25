using System;

namespace BriSky.Models.Operaciones
{
    public class Mantenimiento
    {
        public string CodMantenimiento { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }
        public string Descripcion { get; set; }
        public DateTime? ProximaFecha { get; set; }
        public bool Finalizado { get; set; }
        public string CodInterno { get; set; }

        // Propiedad UI (Solo Lectura)
        public string MatriculaAvion { get; set; }
    }
}
