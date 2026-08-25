using System;
using System.Collections.Generic;
using BriSky.Models.Personal;
using BriSky.Data.Personal;

namespace BriSky.Services.Personal
{
    public class EmpleadoService
    {
        private EmpleadoDAO _dao;

        public EmpleadoService()
        {
            _dao = new EmpleadoDAO();
        }

        public List<Empleado> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }
    }
}
