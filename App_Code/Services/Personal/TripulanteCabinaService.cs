using System;
using BriSky.Models.Personal;
using BriSky.Data.Personal;

namespace BriSky.Services.Personal
{
    public class TripulanteCabinaService
    {
        private TripulanteCabinaDAO _dao;

        public TripulanteCabinaService()
        {
            _dao = new TripulanteCabinaDAO();
        }

        public void Insertar(TripulanteCabina t)
        {
            ValidarCamposBase(t);
            
            if (string.IsNullOrWhiteSpace(t.Licencia))
                throw new ArgumentException("La licencia es obligatoria para los tripulantes de cabina.");

            _dao.Insertar(t);
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
