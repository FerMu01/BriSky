using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class TarifaService
    {
        private TarifaDAO _dao;

        public TarifaService()
        {
            _dao = new TarifaDAO();
        }

        public List<Tarifa> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public Tarifa ObtenerPorId(string codTarifa)
        {
            if (string.IsNullOrWhiteSpace(codTarifa)) return null;
            return _dao.ObtenerPorId(codTarifa);
        }

        public void Guardar(Tarifa tarifa, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(tarifa.CodTarifa))
                throw new ArgumentException("El código de tarifa es obligatorio.");
            if (string.IsNullOrWhiteSpace(tarifa.Nombre))
                throw new ArgumentException("El nombre de la tarifa es obligatorio.");
                
            if (tarifa.PrecioBase < 0)
                throw new Exception("El precio base de la tarifa no puede ser negativo.");
            if (tarifa.EquipajeIncluido < 0)
                throw new Exception("El equipaje incluido no puede tener un valor negativo.");

            try
            {
                if (esNuevo)
                {
                    if (_dao.ObtenerPorId(tarifa.CodTarifa) != null)
                        throw new Exception("Ya existe una tarifa con ese código.");
                    _dao.Insertar(tarifa);
                }
                else
                {
                    _dao.Actualizar(tarifa);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("Error en base de datos al guardar la tarifa. Código duplicado.");
                }
                throw;
            }
        }

        public void Eliminar(string codTarifa)
        {
            if (string.IsNullOrWhiteSpace(codTarifa))
                throw new ArgumentException("Identificador inválido.");

            try
            {
                _dao.Eliminar(codTarifa);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("No se puede eliminar esta tarifa porque ya se encuentra asociada a pasajes o reservas comerciales.");
                }
                throw;
            }
        }
    }
}
