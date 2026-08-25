using System;

namespace BriSky.Models.Ubicaciones
{
    public class Oficina
    {
        public string CodOficina { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Correo { get; set; }
        public string CodCiudad { get; set; }

        // Propiedad auxiliar UI
        public string NombreCiudad { get; set; }

        public Oficina() { }

        public Oficina(string codOficina, string nombre, string direccion, string telefono, string correo, string codCiudad)
        {
            CodOficina = codOficina;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
            Correo = correo;
            CodCiudad = codCiudad;
        }
    }
}
