using System;
using System.Collections.Generic;
using BriSky.Models.Ubicaciones;
using BriSky.Data.Ubicaciones;

namespace BriSky.Services.Ubicaciones
{
    public class AeropuertoService
    {
        private AeropuertoDAO _dao;

        public AeropuertoService()
        {
            _dao = new AeropuertoDAO();
        }

        public List<Aeropuerto> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public Aeropuerto ObtenerPorId(string codAeropuerto)
        {
            if (string.IsNullOrWhiteSpace(codAeropuerto)) return null;
            return _dao.ObtenerPorId(codAeropuerto);
        }

        public void Guardar(Aeropuerto aeropuerto, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(aeropuerto.CodAeropuerto))
                throw new ArgumentException("El código de aeropuerto es obligatorio.");
            
            if (string.IsNullOrWhiteSpace(aeropuerto.Nombre))
                throw new ArgumentException("El nombre del aeropuerto es obligatorio.");

            if (string.IsNullOrWhiteSpace(aeropuerto.CodCiudad))
                throw new ArgumentException("Debe seleccionar una ciudad para el aeropuerto.");

            if (esNuevo)
            {
                if (_dao.ObtenerPorId(aeropuerto.CodAeropuerto) != null)
                {
                    throw new Exception("Ya existe un aeropuerto con el código especificado.");
                }
                _dao.Insertar(aeropuerto);
            }
            else
            {
                _dao.Actualizar(aeropuerto);
            }
        }

        public void Eliminar(string codAeropuerto)
        {
            if (string.IsNullOrWhiteSpace(codAeropuerto))
                throw new ArgumentException("El código de aeropuerto es inválido.");
            
            _dao.Eliminar(codAeropuerto);
        }
    }
}
