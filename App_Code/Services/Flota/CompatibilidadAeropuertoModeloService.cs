using System;
using System.Collections.Generic;
using BriSky.Models.Flota;
using BriSky.Data.Flota;

namespace BriSky.Services.Flota
{
    public class CompatibilidadAeropuertoModeloService
    {
        private CompatibilidadAeropuertoModeloDAO _dao;

        public CompatibilidadAeropuertoModeloService()
        {
            _dao = new CompatibilidadAeropuertoModeloDAO();
        }

        public List<CompatibilidadAeropuertoModelo> ObtenerTodas()
        {
            return _dao.ObtenerTodas();
        }

        public CompatibilidadAeropuertoModelo ObtenerPorId(string codAeropuerto, string codModelo)
        {
            if (string.IsNullOrWhiteSpace(codAeropuerto) || string.IsNullOrWhiteSpace(codModelo)) return null;
            return _dao.ObtenerPorId(codAeropuerto, codModelo);
        }

        public void Guardar(CompatibilidadAeropuertoModelo comp, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(comp.CodAeropuerto))
                throw new ArgumentException("Debe seleccionar un aeropuerto.");
            if (string.IsNullOrWhiteSpace(comp.CodModelo))
                throw new ArgumentException("Debe seleccionar un modelo de avión.");

            if (esNuevo)
            {
                // Validación para evitar colisión de la llave primaria compuesta en SQL
                if (_dao.ObtenerPorId(comp.CodAeropuerto, comp.CodModelo) != null)
                {
                    throw new Exception("Esta compatibilidad ya se encuentra registrada en el sistema.");
                }
                _dao.Insertar(comp);
            }
            else
            {
                _dao.Actualizar(comp);
            }
        }

        public void Eliminar(string codAeropuerto, string codModelo)
        {
            if (string.IsNullOrWhiteSpace(codAeropuerto) || string.IsNullOrWhiteSpace(codModelo))
                throw new ArgumentException("Identificadores inválidos.");

            _dao.Eliminar(codAeropuerto, codModelo);
        }
    }
}
