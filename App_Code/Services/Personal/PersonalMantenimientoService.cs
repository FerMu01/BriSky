using System;
using BriSky.Models.Personal;
using BriSky.Data.Personal;

namespace BriSky.Services.Personal
{
    public class PersonalMantenimientoService
    {
        private PersonalMantenimientoDAO _dao;

        public PersonalMantenimientoService()
        {
            _dao = new PersonalMantenimientoDAO();
        }

        public void Insertar(PersonalMantenimiento m)
        {
            ValidarCamposBase(m);
            
            if (string.IsNullOrWhiteSpace(m.Especialidad))
                throw new ArgumentException("La especialidad es obligatoria para el personal de mantenimiento.");

            _dao.Insertar(m);
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
