using System;
using System.Collections.Generic;
using BriSky.Models.Personal;
using BriSky.Data.Personal;

namespace BriSky.Services.Personal
{
    public class AreaService
    {
        private AreaDAO _dao;

        public AreaService()
        {
            _dao = new AreaDAO();
        }

        public List<Area> ObtenerTodas()
        {
            return _dao.ObtenerTodos();
        }

        public Area ObtenerPorId(string codArea)
        {
            if (string.IsNullOrWhiteSpace(codArea)) return null;
            return _dao.ObtenerPorId(codArea);
        }

        public void Guardar(Area area, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(area.CodArea))
                throw new ArgumentException("El código de área es obligatorio.");
            
            if (string.IsNullOrWhiteSpace(area.Nombre))
                throw new ArgumentException("El nombre del área es obligatorio.");

            if (esNuevo)
            {
                if (_dao.ObtenerPorId(area.CodArea) != null)
                {
                    throw new Exception("Ya existe un área con el código especificado.");
                }
                _dao.Insertar(area);
            }
            else
            {
                _dao.Actualizar(area);
            }
        }

        public void Eliminar(string codArea)
        {
            if (string.IsNullOrWhiteSpace(codArea))
                throw new ArgumentException("El código de área es inválido.");
            
            try
            {
                _dao.Eliminar(codArea);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("No se puede eliminar el área porque tiene empleados asociados.");
                }
                throw;
            }
        }
    }
}
