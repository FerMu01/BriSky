using System;

namespace BriSky.Models.Operaciones
{
    public class Ruta
    {
        public string CodRuta { get; set; }
        public string CodAeropuertoOrigen { get; set; }
        public string CodAeropuertoDestino { get; set; }

        // Propiedades UI (Solo Lectura)
        public string NombreOrigen { get; set; }
        public string NombreDestino { get; set; }
    }
}
