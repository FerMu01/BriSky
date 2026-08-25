using System;
using System.Collections.Generic;
using BriSky.Models.Comercial;
using BriSky.Data.Comercial;

namespace BriSky.Services.Comercial
{
    public class PersonaService
    {
        private PersonaDAO _dao;

        public PersonaService()
        {
            _dao = new PersonaDAO();
        }

        public List<Persona> ObtenerTodos()
        {
            return _dao.ObtenerTodos();
        }
    }
}
