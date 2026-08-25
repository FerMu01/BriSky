using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Ubicaciones;

namespace BriSky.Data.Ubicaciones
{
    public class CiudadDAO
    {
        public List<Ciudad> ObtenerTodos()
        {
            var lista = new List<Ciudad>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_ciudad, nombre, departamento FROM brisky.ciudad";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Ciudad(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2)
                            ));
                        }
                    }
                }
            }
            return lista;
        }

        public Ciudad ObtenerPorId(string codCiudad)
        {
            Ciudad ciudad = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_ciudad, nombre, departamento FROM brisky.ciudad WHERE cod_ciudad = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codCiudad);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ciudad = new Ciudad(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2)
                            );
                        }
                    }
                }
            }
            return ciudad;
        }

        public void Insertar(Ciudad ciudad)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.ciudad (cod_ciudad, nombre, departamento) VALUES (@cod, @nom, @dep)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", ciudad.CodCiudad);
                    cmd.Parameters.AddWithValue("@nom", ciudad.Nombre);
                    cmd.Parameters.AddWithValue("@dep", ciudad.Departamento);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Ciudad ciudad)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.ciudad SET nombre = @nom, departamento = @dep WHERE cod_ciudad = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", ciudad.CodCiudad);
                    cmd.Parameters.AddWithValue("@nom", ciudad.Nombre);
                    cmd.Parameters.AddWithValue("@dep", ciudad.Departamento);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codCiudad)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.ciudad WHERE cod_ciudad = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codCiudad);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
