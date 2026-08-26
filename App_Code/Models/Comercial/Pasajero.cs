using System;

namespace BriSky.Models.Comercial
{
    public class Pasajero
    {
        public string CodPasajero { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string NumDocumento { get; set; }
        public string Nacionalidad { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }

        // Propiedad de apoyo para la interfaz (no viene de una columna propia)
        public string NombreCompleto => $"{Nombre} {Apellido}";
    }
}
