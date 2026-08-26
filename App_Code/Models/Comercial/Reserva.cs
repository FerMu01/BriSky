using System;

namespace BriSky.Models.Comercial
{
    public abstract class Reserva
    {
        public string CodReserva { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Precio { get; set; }
        public string Estado { get; set; } // PENDIENTE / CONFIRMADA / CANCELADA
        public string CodPasajero { get; set; }
        public int IdVuelo { get; set; }
        public string CodTarifa { get; set; }
        public string TipoReserva { get; set; } // Discriminador: OFICINA / INTERNET

        // Propiedades de apoyo para la interfaz
        public string NombrePasajero { get; set; }
        public string RutaFormateadaVuelo { get; set; }
        public string NombreTarifa { get; set; }
    }
}
