using System;

namespace BriSky.Models.Personal
{
    public abstract class Empleado
    {
        public string CodEmpleado { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Documento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string EstadoLaboral { get; set; }
        public string CodArea { get; set; }
        public string TipoEmpleado { get; set; }

        // Propiedad de apoyo para la Interfaz Gráfica
        public string NombreArea { get; set; } 
    }
}
