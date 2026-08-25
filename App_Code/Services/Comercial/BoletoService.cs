using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class BoletoService
    {
        private BoletoDAO _dao;

        public BoletoService()
        {
            _dao = new BoletoDAO();
        }

        public List<Boleto> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public Boleto ObtenerPorId(string numBoleto)
        {
            if (string.IsNullOrWhiteSpace(numBoleto)) return null;
            return _dao.ObtenerPorId(numBoleto);
        }

        public void Guardar(Boleto boleto, bool esNuevo)
        {
            if (string.IsNullOrWhiteSpace(boleto.NumBoleto))
                throw new ArgumentException("El número de boleto es obligatorio.");
            if (boleto.IdVuelo <= 0)
                throw new ArgumentException("Debe seleccionar un vuelo válido.");
            if (string.IsNullOrWhiteSpace(boleto.CodReserva))
                throw new ArgumentException("Debe escribir un código de reserva.");
            if (string.IsNullOrWhiteSpace(boleto.NumAsiento))
                throw new ArgumentException("El asiento es obligatorio.");
                
            if (boleto.Precio < 0)
                throw new Exception("El precio no puede ser un valor negativo.");

            try
            {
                if (esNuevo)
                {
                    if (_dao.ObtenerPorId(boleto.NumBoleto) != null)
                        throw new Exception("Ya existe un boleto con ese número en el sistema.");
                    
                    _dao.Insertar(boleto);
                }
                else
                {
                    _dao.Actualizar(boleto);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    throw new Exception("Error: El número de boleto ya existe o el asiento se encuentra duplicado.");
                }
                if (ex.Number == 547)
                {
                    throw new Exception("Verifique que el código de reserva exista en la base de datos (Violación de Llave Foránea).");
                }
                throw;
            }
        }

        public void Eliminar(string numBoleto)
        {
            if (string.IsNullOrWhiteSpace(numBoleto))
                throw new ArgumentException("Identificador de boleto inválido.");

            try
            {
                _dao.Eliminar(numBoleto);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    throw new Exception("No se puede eliminar el boleto porque existen registros dependientes.");
                }
                throw;
            }
        }
    }
}
