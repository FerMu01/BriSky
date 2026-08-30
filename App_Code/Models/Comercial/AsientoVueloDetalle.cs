using System;

namespace BriSky.Models.Comercial
{
    public class AsientoVueloDetalle
    {
        public int IdVuelo { get; set; }
        public string NumAsiento { get; set; }
        public string Clase { get; set; }
        public bool Disponible { get; set; }
    }
}
