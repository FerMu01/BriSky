using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Operaciones;
using BriSky.Data.Operaciones;

namespace BriSky.Services.Operaciones
{
    public class RutaService
    {
        private RutaDAO _dao;

        public RutaService()
        {
            _dao = new RutaDAO();
        }

        public List<Ruta> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }
        
        public Ruta ObtenerPorId(string codRuta)
        {
            if (string.IsNullOrWhiteSpace(codRuta)) return null;
            return _dao.ObtenerPorId(codRuta);
        }

        public void Guardar(Ruta ruta, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(ruta.CodRuta))
                throw new ArgumentException("El código de ruta es obligatorio.");
            if (string.IsNullOrWhiteSpace(ruta.CodAeropuertoOrigen))
                throw new ArgumentException("Debe seleccionar un aeropuerto de origen.");
            if (string.IsNullOrWhiteSpace(ruta.CodAeropuertoDestino))
                throw new ArgumentException("Debe seleccionar un aeropuerto de destino.");

            if (ruta.CodAeropuertoOrigen == ruta.CodAeropuertoDestino)
            {
                throw new Exception("Operación inválida: El aeropuerto de origen y destino no pueden ser el mismo.");
            }

            try
            {
                if (esNuevo)
                {
                    if (_dao.ObtenerPorId(ruta.CodRuta) != null)
                        throw new Exception("El código de ruta ingresado ya existe.");
                        
                    _dao.Insertar(ruta);
                }
                else
                {
                    _dao.Actualizar(ruta);
                }
            }
            catch (SqlException ex)
            {
                // Validación para Unique Key si intentan duplicar orígenes/destinos.
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("Error en base de datos al guardar la ruta. Es posible que el registro esté duplicado.");
                }
                throw;
            }
        }

        public void Eliminar(string codRuta)
        {
            if (string.IsNullOrWhiteSpace(codRuta))
                throw new ArgumentException("Código inválido.");

            try
            {
                _dao.Eliminar(codRuta);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("No se puede eliminar esta ruta porque ya tiene vuelos programados asociados.");
                }
                throw;
            }
        }
    }
}
