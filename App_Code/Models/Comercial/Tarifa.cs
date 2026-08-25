using System;

namespace BriSky.Models.Comercial
{
    public class Tarifa
    {
        public string CodTarifa { get; set; }
        public string Nombre { get; set; }
        public decimal PrecioBase { get; set; }
        public string Condiciones { get; set; } // Opcional
        public decimal EquipajeIncluido { get; set; }
    }
}
