using System;
using System.Collections.Generic;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class EquipajeService
    {
        private const double PESO_INCLUIDO_POR_DEFECTO = 23.0;

        private EquipajeDAO _dao;

        public EquipajeService()
        {
            _dao = new EquipajeDAO();
        }

        public List<Equipaje> ObtenerPorBoleto(string numBoleto)
        {
            if (string.IsNullOrWhiteSpace(numBoleto)) return new List<Equipaje>();
            return _dao.ObtenerPorBoleto(numBoleto);
        }

        public void Registrar(Equipaje eq)
        {
            if (string.IsNullOrWhiteSpace(eq.CodEquipaje)) throw new ArgumentException("El código de equipaje es obligatorio.");
            if (string.IsNullOrWhiteSpace(eq.NumBoleto)) throw new ArgumentException("Debe indicar el boleto asociado.");
            if (eq.Peso < 0) throw new ArgumentException("El peso no puede ser negativo.");
            if (eq.Cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");

            if (_dao.ObtenerPorId(eq.CodEquipaje) != null)
                throw new ArgumentException("Ya existe un equipaje con ese código.");

            _dao.Insertar(eq);
        }

        // Equipaje.calcularExceso() -- respecto al equipaje incluido en la tarifa (kg por unidad)
        public double CalcularExceso(Equipaje eq, double equipajeIncluidoKg)
        {
            double incluido = equipajeIncluidoKg > 0 ? equipajeIncluidoKg : PESO_INCLUIDO_POR_DEFECTO;
            double pesoTotal = eq.Peso * eq.Cantidad;
            double exceso = pesoTotal - incluido;
            return exceso > 0 ? exceso : 0;
        }

        public void Eliminar(string codEquipaje)
        {
            if (string.IsNullOrWhiteSpace(codEquipaje)) throw new ArgumentException("Identificador de equipaje inválido.");
            _dao.Eliminar(codEquipaje);
        }
    }
}
