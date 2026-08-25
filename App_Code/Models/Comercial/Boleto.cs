using System;

namespace BriSky.Models.Comercial
{
    public class Boleto
    {
        public string NumBoleto { get; set; }
        public decimal Precio { get; set; }
        public string CodReserva { get; set; } // Lo dejamos como string y TextBox manual según opción A
        public int IdVuelo { get; set; }
        public string NumAsiento { get; set; }
        public bool Anulado { get; set; }

        // Propiedad UI (Solo Lectura)
        public string RutaFormateadaVuelo { get; set; }
    }
}
