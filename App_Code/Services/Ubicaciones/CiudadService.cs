using System;
using System.Collections.Generic;
using BriSky.Models.Ubicaciones;
using BriSky.Data.Ubicaciones;

namespace BriSky.Services.Ubicaciones
{
    public class CiudadService
    {
        private CiudadDAO _dao;

        public CiudadService()
        {
            _dao = new CiudadDAO();
        }

        public List<Ciudad> ObtenerTodas()
        {
            return _dao.ObtenerTodos();
        }

        public Ciudad ObtenerPorId(string codCiudad)
        {
            if (string.IsNullOrWhiteSpace(codCiudad)) return null;
            return _dao.ObtenerPorId(codCiudad);
        }

        public void Guardar(Ciudad ciudad, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(ciudad.CodCiudad))
                throw new ArgumentException("El código de ciudad es obligatorio.");
            
            if (string.IsNullOrWhiteSpace(ciudad.Nombre))
                throw new ArgumentException("El nombre de la ciudad es obligatorio.");

            if (esNuevo)
            {
                if (_dao.ObtenerPorId(ciudad.CodCiudad) != null)
                {
                    throw new Exception("Ya existe una ciudad con el código especificado.");
                }
                _dao.Insertar(ciudad);
            }
            else
            {
                _dao.Actualizar(ciudad);
            }
        }

        public void Eliminar(string codCiudad)
        {
            if (string.IsNullOrWhiteSpace(codCiudad))
                throw new ArgumentException("El código de ciudad es inválido.");
            
            try
            {
                _dao.Eliminar(codCiudad);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("No se puede eliminar la ciudad porque tiene aeropuertos u oficinas asociadas.");
                }
                throw;
            }
        }
    }
}
