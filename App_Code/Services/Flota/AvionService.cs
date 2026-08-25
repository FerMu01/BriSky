using System;
using System.Collections.Generic;
using BriSky.Models.Flota;
using BriSky.Data.Flota;

namespace BriSky.Services.Flota
{
    public class AvionService
    {
        private AvionDAO _dao;

        public AvionService()
        {
            _dao = new AvionDAO();
        }

        public List<Avion> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public Avion ObtenerPorId(string codInterno)
        {
            if (string.IsNullOrWhiteSpace(codInterno)) return null;
            return _dao.ObtenerPorId(codInterno);
        }

        public void Guardar(Avion avion, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(avion.CodInterno))
                throw new ArgumentException("El código interno es obligatorio.");
            if (string.IsNullOrWhiteSpace(avion.Matricula))
                throw new ArgumentException("La matrícula es obligatoria.");
            if (string.IsNullOrWhiteSpace(avion.CodModelo))
                throw new ArgumentException("Debe asignar un modelo al avión.");

            if (string.IsNullOrEmpty(avion.Estado))
            {
                avion.Estado = "DISPONIBLE";
            }

            try
            {
                if (esNuevo)
                {
                    if (_dao.ObtenerPorId(avion.CodInterno) != null)
                    {
                        throw new Exception("Ya existe un avión con ese código interno.");
                    }
                    _dao.Insertar(avion);
                }
                else
                {
                    _dao.Actualizar(avion);
                }
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("La matrícula ingresada ya se encuentra registrada en otro avión.");
                }
                throw;
            }
        }

        public void Eliminar(string codInterno)
        {
            if (string.IsNullOrWhiteSpace(codInterno))
                throw new ArgumentException("Código interno inválido.");

            try
            {
                _dao.Eliminar(codInterno);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("No se puede eliminar este avión porque tiene un historial de mantenimientos o vuelos asociados.");
                }
                throw;
            }
        }
    }
}
