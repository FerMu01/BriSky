using System;

namespace BriSky.Models.Personal
{
    public abstract class Tripulante : Empleado
    {
        public string Licencia { get; set; }
        public decimal HorasVuelo { get; set; }
    }
}
