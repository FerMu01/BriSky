using System;

namespace BriSky.Models.Operaciones
{
    public class VueloDetalle
    {
        public int IdVuelo { get; set; }
        public string NumVuelo { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraLlegada { get; set; }
        public string CodigoOrigen { get; set; } 
        public string AeropuertoOrigen { get; set; }
        public string CodigoDestino { get; set; }
        public string AeropuertoDestino { get; set; }
        public string ModeloAvion { get; set; }
        public int AsientosDisponibles { get; set; }
        public decimal PrecioBase { get; set; }
    }
}
