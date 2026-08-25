using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Operaciones;
using BriSky.Data.Operaciones;

namespace BriSky.Services.Operaciones
{
    public class TripulacionService
    {
        private AsignacionTripulacionDAO _dao;

        public TripulacionService()
        {
            _dao = new AsignacionTripulacionDAO();
        }

        public List<AsignacionTripulacion> ObtenerPorVuelo(int idVuelo)
        {
            if (idVuelo <= 0) return new List<AsignacionTripulacion>();
            return _dao.ObtenerPorVuelo(idVuelo);
        }

        public void Asignar(AsignacionTripulacion at)
        {
            if (at.IdVuelo <= 0)
                throw new ArgumentException("Debe seleccionar un vuelo válido.");
            if (string.IsNullOrWhiteSpace(at.CodEmpleado))
                throw new ArgumentException("Debe seleccionar un empleado.");
            if (string.IsNullOrWhiteSpace(at.Rol))
                throw new ArgumentException("El rol es obligatorio.");

            try
            {
                _dao.Asignar(at);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("Este empleado ya se encuentra asignado a este vuelo.");
                }
                if (ex.Number == 547)
                {
                    throw new Exception("El vuelo o el empleado seleccionado no son válidos.");
                }
                if (ex.Number == 50000)
                {
                    throw new Exception(ex.Message);
                }
                throw;
            }
        }

        public void Eliminar(int idVuelo, string codEmpleado)
        {
            if (idVuelo <= 0 || string.IsNullOrWhiteSpace(codEmpleado))
                throw new ArgumentException("Parámetros de eliminación inválidos.");

            try
            {
                _dao.Eliminar(idVuelo, codEmpleado);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 50000)
                {
                    throw new Exception(ex.Message);
                }
                throw;
            }
        }
    }
}
