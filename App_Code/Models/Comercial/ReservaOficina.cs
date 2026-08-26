using System;

namespace BriSky.Models.Comercial
{
    public class ReservaOficina : Reserva
    {
        public string CodEmpleado { get; set; } // Empleado que atiende la reserva

        // brisky.registrar_venta crea reserva + boleto + pago en una sola
        // transacción, por eso estos datos viajan junto con la reserva.
        public string NumBoleto { get; set; }
        public string MetodoPago { get; set; }
        public string CodPago { get; set; }

        // Propiedad de apoyo para la interfaz
        public string NombreEmpleado { get; set; }
    }
}
