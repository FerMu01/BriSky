using System;

namespace BriSky.Models.Operaciones
{
    public class AsignacionTripulacion
    {
        public int IdVuelo { get; set; }
        public string CodEmpleado { get; set; }
        public string Rol { get; set; }

        // Propiedades UI (Solo Lectura)
        public string NumVuelo { get; set; }
        public string NombreEmpleado { get; set; }
    }
}
