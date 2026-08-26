using System;

namespace BriSky.Models.Comercial
{
    public class Pago
    {
        public string CodPago { get; set; }
        public string CodReserva { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Metodo { get; set; }
    }
}
