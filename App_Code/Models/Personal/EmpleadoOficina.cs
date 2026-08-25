using System;

namespace BriSky.Models.Personal
{
    public class EmpleadoOficina : Empleado
    {
        public string Cargo { get; set; }
        public string CodOficina { get; set; }
        
        // Propiedad de apoyo UI
        public string NombreOficina { get; set; }
    }
}
