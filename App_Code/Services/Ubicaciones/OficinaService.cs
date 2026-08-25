using System;
using System.Collections.Generic;
using BriSky.Models.Ubicaciones;
using BriSky.Data.Ubicaciones;

namespace BriSky.Services.Ubicaciones
{
    public class OficinaService
    {
        private OficinaDAO _dao;

        public OficinaService()
        {
            _dao = new OficinaDAO();
        }

        public List<Oficina> ObtenerTodas()
        {
            return _dao.ObtenerTodos();
        }

        public Oficina ObtenerPorId(string codOficina)
        {
            if (string.IsNullOrWhiteSpace(codOficina)) return null;
            return _dao.ObtenerPorId(codOficina);
        }

        public void Guardar(Oficina oficina, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(oficina.CodOficina))
                throw new ArgumentException("El código de oficina es obligatorio.");
            
            if (string.IsNullOrWhiteSpace(oficina.Nombre))
                throw new ArgumentException("El nombre de la oficina es obligatorio.");

            if (string.IsNullOrWhiteSpace(oficina.Direccion))
                throw new ArgumentException("La dirección es obligatoria.");

            if (string.IsNullOrWhiteSpace(oficina.CodCiudad))
                throw new ArgumentException("Debe seleccionar una ciudad para la oficina.");

            if (esNuevo)
            {
                if (_dao.ObtenerPorId(oficina.CodOficina) != null)
                {
                    throw new Exception("Ya existe una oficina con el código especificado.");
                }
                _dao.Insertar(oficina);
            }
            else
            {
                _dao.Actualizar(oficina);
            }
        }

        public void Eliminar(string codOficina)
        {
            if (string.IsNullOrWhiteSpace(codOficina))
                throw new ArgumentException("El código de oficina es inválido.");
            
            _dao.Eliminar(codOficina);
        }
    }
}
