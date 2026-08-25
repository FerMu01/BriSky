using System;
using BriSky.Models.Personal;
using BriSky.Data.Personal;

namespace BriSky.Services.Personal
{
    public class EmpleadoOficinaService
    {
        private EmpleadoOficinaDAO _dao;

        public EmpleadoOficinaService()
        {
            _dao = new EmpleadoOficinaDAO();
        }

        public void Insertar(EmpleadoOficina e)
        {
            ValidarCamposBase(e);
            
            if (string.IsNullOrWhiteSpace(e.Cargo))
                throw new ArgumentException("El cargo es obligatorio para los empleados de oficina.");
                
            if (string.IsNullOrWhiteSpace(e.CodOficina))
                throw new ArgumentException("Debe seleccionar una oficina para el empleado.");

            _dao.Insertar(e);
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
