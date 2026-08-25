using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Operaciones;
using BriSky.Data.Operaciones;

namespace BriSky.Services.Operaciones
{
    public class MantenimientoService
    {
        private MantenimientoDAO _dao;

        public MantenimientoService()
        {
            _dao = new MantenimientoDAO();
        }

        public List<Mantenimiento> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public void RealizarMantenimiento(Mantenimiento m, string codEmpleado)
        {
            if (string.IsNullOrWhiteSpace(m.CodMantenimiento))
                throw new ArgumentException("El código de mantenimiento es obligatorio.");
            if (string.IsNullOrWhiteSpace(m.CodInterno))
                throw new ArgumentException("Debe seleccionar un avión.");
            if (string.IsNullOrWhiteSpace(m.Tipo))
                throw new ArgumentException("El tipo de mantenimiento es obligatorio.");
            if (string.IsNullOrWhiteSpace(codEmpleado))
                throw new ArgumentException("Debe seleccionar al personal de mantenimiento responsable.");
            if (string.IsNullOrWhiteSpace(m.Descripcion))
                throw new ArgumentException("La descripción es obligatoria.");

            try
            {
                _dao.RealizarMantenimiento(m, codEmpleado);
            }
            catch (SqlException ex)
            {
                // Atrapar errores lanzados por el THROW de SQL Server
                if (ex.Number == 50000)
                {
                    throw new Exception(ex.Message);
                }
                // Si es un error de unique key u otro
                throw;
            }
        }

        public void FinalizarMantenimiento(string codMantenimiento)
        {
            if (string.IsNullOrWhiteSpace(codMantenimiento))
                throw new ArgumentException("El código de mantenimiento es inválido.");

            try
            {
                _dao.FinalizarMantenimiento(codMantenimiento);
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
