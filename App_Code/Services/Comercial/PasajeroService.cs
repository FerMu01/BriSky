using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class PasajeroService
    {
        private PasajeroDAO _dao;

        public PasajeroService()
        {
            _dao = new PasajeroDAO();
        }

        public List<Pasajero> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }

        public Pasajero ObtenerPorId(string codPasajero)
        {
            if (string.IsNullOrWhiteSpace(codPasajero)) return null;
            return _dao.ObtenerPorId(codPasajero);
        }

        public void Guardar(Pasajero p, bool esNuevo)
        {
            Validar(p);

            try
            {
                if (esNuevo)
                {
                    if (_dao.ObtenerPorId(p.CodPasajero) != null)
                        throw new Exception("Ya existe un pasajero con ese código en el sistema.");

                    var existentePorDoc = _dao.ObtenerPorDocumento(p.NumDocumento);
                    if (existentePorDoc != null)
                        throw new Exception("Ya existe un pasajero registrado con ese número de documento.");

                    _dao.Insertar(p);
                }
                else
                {
                    _dao.Actualizar(p);
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                    throw new Exception("El código de pasajero o el número de documento ya existen en la base de datos.");
                throw;
            }
        }

        public void Eliminar(string codPasajero)
        {
            if (string.IsNullOrWhiteSpace(codPasajero))
                throw new ArgumentException("Identificador de pasajero inválido.");

            try
            {
                _dao.Eliminar(codPasajero);
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                    throw new Exception("No se puede eliminar: el pasajero tiene reservas asociadas.");
                throw;
            }
        }

        private void Validar(Pasajero p)
        {
            if (p == null) throw new ArgumentException("Datos de pasajero inválidos.");
            if (string.IsNullOrWhiteSpace(p.CodPasajero)) throw new ArgumentException("El código de pasajero es obligatorio.");
            if (string.IsNullOrWhiteSpace(p.Nombre)) throw new ArgumentException("El nombre es obligatorio.");
            if (string.IsNullOrWhiteSpace(p.Apellido)) throw new ArgumentException("El apellido es obligatorio.");
            if (string.IsNullOrWhiteSpace(p.NumDocumento)) throw new ArgumentException("El número de documento es obligatorio.");
            if (p.FechaNacimiento == DateTime.MinValue) throw new ArgumentException("La fecha de nacimiento es obligatoria.");
            if (p.FechaNacimiento > DateTime.Today) throw new ArgumentException("La fecha de nacimiento no puede ser futura.");
            if (!string.IsNullOrWhiteSpace(p.Correo) && !Regex.IsMatch(p.Correo, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                throw new ArgumentException("El correo electrónico no tiene un formato válido.");
        }
    }
}
