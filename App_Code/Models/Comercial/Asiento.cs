using System;

namespace BriSky.Models.Comercial
{
    public class Asiento
    {
        public string NumAsiento { get; set; }
        public int IdVuelo { get; set; }
        public bool Disponible { get; set; }
        public string Clase { get; set; }
    }
}
