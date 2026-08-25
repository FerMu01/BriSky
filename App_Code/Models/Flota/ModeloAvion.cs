using System;

namespace BriSky.Models.Flota
{
    public class ModeloAvion
    {
        public string CodModelo { get; set; }
        public string Fabricante { get; set; }
        public string Nombre { get; set; }
        public string Tipo { get; set; }
        public int CapacidadPasajeros { get; set; }
        public decimal CapacidadEquipaje { get; set; }
        public string Categoria { get; set; }

        // Propiedad de Apoyo UI
        public string NombreCompleto 
        {
            get { return Fabricante + " - " + Nombre; }
        }
    }
}
