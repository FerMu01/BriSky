using System;
using System.Collections.Generic;
using BriSky.Models.Flota;
using BriSky.Data.Flota;

namespace BriSky.Services.Flota
{
    public class ModeloAvionService
    {
        private ModeloAvionDAO _dao;

        public ModeloAvionService()
        {
            _dao = new ModeloAvionDAO();
        }

        public List<ModeloAvion> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public ModeloAvion ObtenerPorId(string codModelo)
        {
            if (string.IsNullOrWhiteSpace(codModelo)) return null;
            return _dao.ObtenerPorId(codModelo);
        }

        public void Guardar(ModeloAvion modelo, bool esNuevo)
        {
            // Validaciones de Texto (NOT NULL en BD)
            if (string.IsNullOrWhiteSpace(modelo.CodModelo))
                throw new ArgumentException("El código del modelo es obligatorio.");
            if (string.IsNullOrWhiteSpace(modelo.Fabricante))
                throw new ArgumentException("El fabricante es obligatorio.");
            if (string.IsNullOrWhiteSpace(modelo.Nombre))
                throw new ArgumentException("El nombre del modelo es obligatorio.");
            if (string.IsNullOrWhiteSpace(modelo.Tipo))
                throw new ArgumentException("El tipo de modelo es obligatorio.");

            // Validaciones Numéricas (CHECK Constraints)
            if (modelo.CapacidadPasajeros <= 0)
                throw new ArgumentException("La capacidad de pasajeros debe ser mayor a cero.");
            if (modelo.CapacidadEquipaje < 0)
                throw new ArgumentException("La capacidad de equipaje no puede ser negativa.");

            if (esNuevo)
            {
                if (_dao.ObtenerPorId(modelo.CodModelo) != null)
                {
                    throw new Exception("Ya existe un modelo registrado con ese código.");
                }
                _dao.Insertar(modelo);
            }
            else
            {
                _dao.Actualizar(modelo);
            }
        }

        public void Eliminar(string codModelo)
        {
            if (string.IsNullOrWhiteSpace(codModelo))
                throw new ArgumentException("Código de modelo inválido.");

            try
            {
                _dao.Eliminar(codModelo);
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("No se puede eliminar el modelo de avión porque ya tiene aviones físicos registrados o restricciones de aeropuerto asociadas.");
                }
                throw;
            }
        }
    }
}
