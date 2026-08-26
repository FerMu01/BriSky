using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class ReservaService
    {
        private ReservaDAO _dao;
        private ReservaOficinaDAO _oficinaDao;
        private ReservaInternetDAO _internetDao;

        public ReservaService()
        {
            _dao = new ReservaDAO();
            _oficinaDao = new ReservaOficinaDAO();
            _internetDao = new ReservaInternetDAO();
        }

        public List<Reserva> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public Reserva ObtenerPorId(string codReserva)
        {
            if (string.IsNullOrWhiteSpace(codReserva)) return null;
            return _dao.ObtenerPorId(codReserva);
        }

        // Venta en oficina: brisky.registrar_venta crea reserva + boleto + pago
        // en una sola transacción.
        public string RegistrarVenta(ReservaOficina reserva)
        {
            ValidarCamposBase(reserva);

            if (string.IsNullOrWhiteSpace(reserva.CodEmpleado))
                throw new ArgumentException("Debe indicar el empleado que atiende la venta.");
            if (string.IsNullOrWhiteSpace(reserva.NumBoleto))
                throw new ArgumentException("Debe indicar el número de boleto a emitir.");
            if (string.IsNullOrWhiteSpace(reserva.MetodoPago))
                throw new ArgumentException("Debe indicar el método de pago.");
            if (string.IsNullOrWhiteSpace(reserva.CodPago))
                throw new ArgumentException("Debe indicar el código de pago.");

            try
            {
                return _oficinaDao.Insertar(reserva);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        // Reserva por internet: brisky.crear_reserva_internet solo crea la
        // reserva (queda PENDIENTE); el boleto y el pago se generan después
        // con GenerarBoleto()/Pago por separado.
        public string RegistrarReservaInternet(ReservaInternet reserva)
        {
            ValidarCamposBase(reserva);

            try
            {
                return _internetDao.Insertar(reserva);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        // Reserva.confirmar()
        public void Confirmar(string codReserva)
        {
            try
            {
                _dao.Confirmar(codReserva);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        // Reserva.cancelar()
        public void Cancelar(string codReserva)
        {
            try
            {
                _dao.Cancelar(codReserva);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        // Reserva.generarBoleto() -- pensado para el flujo de Internet, ya que
        // en oficina el boleto se genera dentro de RegistrarVenta().
        public string GenerarBoleto(string codReserva, string numBoleto)
        {
            if (string.IsNullOrWhiteSpace(numBoleto))
                throw new ArgumentException("Debe indicar el número de boleto a generar.");

            try
            {
                return _dao.GenerarBoleto(codReserva, numBoleto);
            }
            catch (SqlException ex)
            {
                throw TraducirError(ex);
            }
        }

        private void ValidarCamposBase(Reserva r)
        {
            if (r == null) throw new ArgumentException("Datos de reserva inválidos.");
            if (string.IsNullOrWhiteSpace(r.CodReserva)) throw new ArgumentException("El código de reserva es obligatorio.");
            if (string.IsNullOrWhiteSpace(r.CodPasajero)) throw new ArgumentException("Debe seleccionar un pasajero.");
            if (r.IdVuelo <= 0) throw new ArgumentException("Debe seleccionar un vuelo válido.");
            if (string.IsNullOrWhiteSpace(r.CodTarifa)) throw new ArgumentException("Debe seleccionar una tarifa.");
        }

        private Exception TraducirError(SqlException ex)
        {
            if (ex.Number == 2627 || ex.Number == 2601)
                return new Exception("Alguno de los códigos (reserva, boleto o pago) ya existe en la base de datos.");
            if (ex.Number == 547)
                return new Exception("Verifique que el pasajero, empleado, vuelo o tarifa existan (violación de llave foránea).");
            return new Exception("Error de base de datos: " + ex.Message);
        }
    }
}
