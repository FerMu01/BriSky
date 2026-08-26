using System;
using System.Collections.Generic;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class PagoService
    {
        private PagoDAO _dao;

        public PagoService()
        {
            _dao = new PagoDAO();
        }

        public List<Pago> ObtenerPorReserva(string codReserva)
        {
            if (string.IsNullOrWhiteSpace(codReserva)) return new List<Pago>();
            return _dao.ObtenerPorReserva(codReserva);
        }

        // Pago.procesarPago()
        public bool ProcesarPago(Pago p)
        {
            if (string.IsNullOrWhiteSpace(p.CodPago)) throw new ArgumentException("El código de pago es obligatorio.");
            if (string.IsNullOrWhiteSpace(p.CodReserva)) throw new ArgumentException("Debe indicar la reserva asociada.");
            if (p.Monto <= 0) throw new ArgumentException("El monto del pago debe ser mayor a cero.");
            if (string.IsNullOrWhiteSpace(p.Metodo)) throw new ArgumentException("Debe indicar el método de pago.");

            if (p.Fecha == DateTime.MinValue) p.Fecha = DateTime.Now;

            _dao.Insertar(p);
            return true;
        }

        public void Eliminar(string codPago)
        {
            if (string.IsNullOrWhiteSpace(codPago)) throw new ArgumentException("Identificador de pago inválido.");
            _dao.Eliminar(codPago);
        }
    }
}
