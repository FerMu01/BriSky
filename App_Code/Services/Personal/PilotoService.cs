using System;
using BriSky.Models.Personal;
using BriSky.Data.Personal;

namespace BriSky.Services.Personal
{
    public class PilotoService
    {
        private PilotoDAO _dao;

        public PilotoService()
        {
            _dao = new PilotoDAO();
        }

        public void Insertar(Piloto p)
        {
            ValidarCamposBase(p);
            
            if (string.IsNullOrWhiteSpace(p.Licencia))
                throw new ArgumentException("La licencia es obligatoria para los pilotos.");
                
            if (string.IsNullOrWhiteSpace(p.RangoPiloto))
                throw new ArgumentException("El rango es obligatorio para los pilotos.");

            _dao.Insertar(p);
        }

        private void ValidarCamposBase(Empleado e)
        {
            if (string.IsNullOrWhiteSpace(e.CodEmpleado)) throw new ArgumentException("El código de empleado es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.Nombre)) throw new ArgumentException("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.Apellido)) throw new ArgumentException("El apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.Documento)) throw new ArgumentException("El documento es obligatorio.");
            if (string.IsNullOrWhiteSpace(e.CodArea)) throw new ArgumentException("Debe seleccionar un área.");
        }
    }
}
