using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class PersonaDAO
    {
        public List<Persona> ObtenerTodos()
        {
            var lista = new List<Persona>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_persona, (nombre + ' ' + apellido) AS nombre_completo FROM brisky.persona";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Persona
                            {
                                CodPersona = reader["cod_persona"].ToString(),
                                NombreCompleto = reader["nombre_completo"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
