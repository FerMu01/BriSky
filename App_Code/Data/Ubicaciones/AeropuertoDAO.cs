using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Ubicaciones;

namespace BriSky.Data.Ubicaciones
{
    public class AeropuertoDAO
    {
        public List<Aeropuerto> ObtenerTodos()
        {
            var lista = new List<Aeropuerto>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT a.cod_aeropuerto, a.nombre, a.pais, a.caracteristicas, a.cod_ciudad, c.nombre AS nombre_ciudad
                    FROM brisky.aeropuerto a
                    INNER JOIN brisky.ciudad c ON a.cod_ciudad = c.cod_ciudad";
                
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var apto = new Aeropuerto(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.IsDBNull(3) ? null : reader.GetString(3),
                                reader.GetString(4)
                            );
                            apto.NombreCiudad = reader.GetString(5);
                            lista.Add(apto);
                        }
                    }
                }
            }
            return lista;
        }

        public Aeropuerto ObtenerPorId(string codAeropuerto)
        {
            Aeropuerto apto = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_aeropuerto, nombre, pais, caracteristicas, cod_ciudad FROM brisky.aeropuerto WHERE cod_aeropuerto = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codAeropuerto);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            apto = new Aeropuerto(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.IsDBNull(3) ? null : reader.GetString(3),
                                reader.GetString(4)
                            );
                        }
                    }
                }
            }
            return apto;
        }

        public void Insertar(Aeropuerto aeropuerto)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.aeropuerto (cod_aeropuerto, nombre, pais, caracteristicas, cod_ciudad) VALUES (@cod, @nom, @pais, @carac, @codCiu)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", aeropuerto.CodAeropuerto);
                    cmd.Parameters.AddWithValue("@nom", aeropuerto.Nombre);
                    cmd.Parameters.AddWithValue("@pais", aeropuerto.Pais);
                    cmd.Parameters.AddWithValue("@carac", (object)aeropuerto.Caracteristicas ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@codCiu", aeropuerto.CodCiudad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Aeropuerto aeropuerto)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.aeropuerto SET nombre = @nom, pais = @pais, caracteristicas = @carac, cod_ciudad = @codCiu WHERE cod_aeropuerto = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", aeropuerto.CodAeropuerto);
                    cmd.Parameters.AddWithValue("@nom", aeropuerto.Nombre);
                    cmd.Parameters.AddWithValue("@pais", aeropuerto.Pais);
                    cmd.Parameters.AddWithValue("@carac", (object)aeropuerto.Caracteristicas ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@codCiu", aeropuerto.CodCiudad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codAeropuerto)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.aeropuerto WHERE cod_aeropuerto = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codAeropuerto);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
