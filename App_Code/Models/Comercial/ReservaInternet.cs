using System;

namespace BriSky.Models.Comercial
{
    public class ReservaInternet : Reserva
    {
        public string IpOrigen { get; set; }

        // brisky.crear_reserva_internet la asigna internamente (GETDATE());
        // esta propiedad solo se usa para mostrarla al leer, no se envía al crear.
        public DateTime FechaHoraWeb { get; set; }
    }
}
