using System;
using System.Collections.Generic;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class AsientoService
    {
        private AsientoDAO _dao;

        public AsientoService()
        {
            _dao = new AsientoDAO();
        }

        public List<Asiento> ObtenerPorVuelo(int idVuelo)
        {
            if (idVuelo <= 0) return new List<Asiento>();
            return _dao.ObtenerPorVuelo(idVuelo);
        }

        public List<Asiento> AsientosDisponibles(int idVuelo)
        {
            var lista = new List<Asiento>();
            foreach (var a in ObtenerPorVuelo(idVuelo))
                if (a.Disponible) lista.Add(a);
            return lista;
        }

        public void Registrar(Asiento a)
        {
            if (a.IdVuelo <= 0) throw new ArgumentException("Debe indicar un vuelo válido.");
            if (string.IsNullOrWhiteSpace(a.NumAsiento)) throw new ArgumentException("El número de asiento es obligatorio.");
            if (_dao.ObtenerPorId(a.IdVuelo, a.NumAsiento) != null)
                throw new ArgumentException("Ese asiento ya existe para el vuelo indicado.");

            _dao.Insertar(a);
        }

        // Asiento.reservar()
        public void Reservar(int idVuelo, string numAsiento)
        {
            var a = _dao.ObtenerPorId(idVuelo, numAsiento);
            if (a == null) throw new ArgumentException("El asiento indicado no existe.");
            if (!a.Disponible) throw new Exception("El asiento ya se encuentra ocupado.");
            _dao.CambiarDisponibilidad(idVuelo, numAsiento, false);
        }

        // Asiento.liberar()
        public void Liberar(int idVuelo, string numAsiento)
        {
            var a = _dao.ObtenerPorId(idVuelo, numAsiento);
            if (a == null) throw new ArgumentException("El asiento indicado no existe.");
            _dao.CambiarDisponibilidad(idVuelo, numAsiento, true);
        }

        public void Eliminar(int idVuelo, string numAsiento)
        {
            _dao.Eliminar(idVuelo, numAsiento);
        }
    }
}
