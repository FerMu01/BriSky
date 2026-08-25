using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Operaciones;
using BriSky.Data.Operaciones;

namespace BriSky.Services.Operaciones
{
    public class VueloService
    {
        private VueloDAO _dao;

        public VueloService()
        {
            _dao = new VueloDAO();
        }

        public List<Vuelo> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public Vuelo ObtenerPorId(int idVuelo)
        {
            return _dao.ObtenerPorId(idVuelo);
        }

        public void Guardar(Vuelo v, bool esNuevo, string codInternoAvion, string nuevoEstado)
        {
            if (string.IsNullOrWhiteSpace(v.NumVuelo))
                throw new ArgumentException("El número de vuelo es obligatorio.");
            if (v.Fecha == DateTime.MinValue)
                throw new ArgumentException("La fecha es inválida.");
            if (string.IsNullOrWhiteSpace(v.CodRuta))
                throw new ArgumentException("Debe seleccionar una ruta.");

            try
            {
                if (esNuevo)
                {
                    // Inserta y recupera el Identity
                    int idGenerado = _dao.Insertar(v);
                    
                    // Si el usuario seleccionó un avión de entrada, lo asignamos usando el SP
                    if (!string.IsNullOrWhiteSpace(codInternoAvion))
                    {
                        _dao.AsignarAvion(idGenerado, codInternoAvion);
                    }
                    
                    // El estado inicial es PROGRAMADO según el DEFAULT de SQL, pero si el usuario seleccionó otro:
                    if (!string.IsNullOrWhiteSpace(nuevoEstado) && nuevoEstado != "PROGRAMADO")
                    {
                        _dao.CambiarEstado(idGenerado, nuevoEstado);
                    }
                }
                else
                {
                    // Actualizamos datos base
                    _dao.ActualizarDatosBase(v);
                    
                    // Asignamos o cambiamos avión
                    _dao.AsignarAvion(v.IdVuelo, codInternoAvion);
                    
                    // Cambiamos estado si fue solicitado
                    if (!string.IsNullOrWhiteSpace(nuevoEstado))
                    {
                        _dao.CambiarEstado(v.IdVuelo, nuevoEstado);
                    }
                }
            }
            catch (SqlException ex)
            {
                // Error de constraints únicos
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("Ya existe un vuelo programado con ese número de vuelo para esta fecha.");
                }
                
                // Excepciones lanzadas desde los Procedimientos Almacenados (ej: avión no disponible)
                if (ex.Number == 50000)
                {
                    throw new Exception(ex.Message);
                }
                throw;
            }
        }

        public void Eliminar(int idVuelo)
        {
            try
            {
                _dao.Eliminar(idVuelo);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("No se puede eliminar el vuelo porque ya tiene reservas, asientos o tripulación asignada.");
                }
                throw;
            }
        }
    }
}
