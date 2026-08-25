using System;

namespace BriSky.Models.Operaciones
{
    public class Vuelo
    {
        public int IdVuelo { get; set; }
        public string NumVuelo { get; set; }
        public DateTime Fecha { get; set; }
        public TimeSpan HoraSalida { get; set; }
        public TimeSpan HoraLlegada { get; set; }
        public string Estado { get; set; }
        public string CodRuta { get; set; }
        public string CodInterno { get; set; } // Anulable en la BD

        // Propiedades UI
        public string RutaFormateada { get; set; }
        public string MatriculaAvion { get; set; }
    }
}
