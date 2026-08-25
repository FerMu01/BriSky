using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Flota;

namespace BriSky.Data.Flota
{
    public class CompatibilidadAeropuertoModeloDAO
    {
        public List<CompatibilidadAeropuertoModelo> ObtenerTodas()
        {
            var lista = new List<CompatibilidadAeropuertoModelo>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT c.cod_aeropuerto, a.nombre AS nombre_aeropuerto, 
                           c.cod_modelo, (m.fabricante + ' - ' + m.nombre) AS nombre_modelo, 
                           c.restricciones
                    FROM brisky.compatibilidad_aeropuerto_modelo c
                    INNER JOIN brisky.aeropuerto a ON c.cod_aeropuerto = a.cod_aeropuerto
                    INNER JOIN brisky.modelo_avion m ON c.cod_modelo = m.cod_modelo";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new CompatibilidadAeropuertoModelo
                            {
                                CodAeropuerto = reader["cod_aeropuerto"].ToString(),
                                NombreAeropuerto = reader["nombre_aeropuerto"].ToString(),
                                CodModelo = reader["cod_modelo"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString(),
                                Restricciones = reader["restricciones"] != DBNull.Value ? reader["restricciones"].ToString() : null
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public CompatibilidadAeropuertoModelo ObtenerPorId(string codAeropuerto, string codModelo)
        {
            CompatibilidadAeropuertoModelo comp = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT c.cod_aeropuerto, a.nombre AS nombre_aeropuerto, 
                           c.cod_modelo, (m.fabricante + ' - ' + m.nombre) AS nombre_modelo, 
                           c.restricciones
                    FROM brisky.compatibilidad_aeropuerto_modelo c
                    INNER JOIN brisky.aeropuerto a ON c.cod_aeropuerto = a.cod_aeropuerto
                    INNER JOIN brisky.modelo_avion m ON c.cod_modelo = m.cod_modelo
                    WHERE c.cod_aeropuerto = @codA AND c.cod_modelo = @codM";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@codA", codAeropuerto);
                    cmd.Parameters.AddWithValue("@codM", codModelo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            comp = new CompatibilidadAeropuertoModelo
                            {
                                CodAeropuerto = reader["cod_aeropuerto"].ToString(),
                                NombreAeropuerto = reader["nombre_aeropuerto"].ToString(),
                                CodModelo = reader["cod_modelo"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString(),
                                Restricciones = reader["restricciones"] != DBNull.Value ? reader["restricciones"].ToString() : null
                            };
                        }
                    }
                }
            }
            return comp;
        }

        public void Insertar(CompatibilidadAeropuertoModelo comp)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.compatibilidad_aeropuerto_modelo (cod_aeropuerto, cod_modelo, restricciones) VALUES (@codA, @codM, @rest)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@codA", comp.CodAeropuerto);
                    cmd.Parameters.AddWithValue("@codM", comp.CodModelo);
                    
                    var paramRest = new SqlParameter("@rest", SqlDbType.VarChar);
                    paramRest.Value = string.IsNullOrWhiteSpace(comp.Restricciones) ? DBNull.Value : (object)comp.Restricciones.Trim();
                    cmd.Parameters.Add(paramRest);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(CompatibilidadAeropuertoModelo comp)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                // En esta tabla relacional solo se actualizan las restricciones, ya que las PKs no cambian
                string sql = "UPDATE brisky.compatibilidad_aeropuerto_modelo SET restricciones = @rest WHERE cod_aeropuerto = @codA AND cod_modelo = @codM";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@codA", comp.CodAeropuerto);
                    cmd.Parameters.AddWithValue("@codM", comp.CodModelo);
                    
                    var paramRest = new SqlParameter("@rest", SqlDbType.VarChar);
                    paramRest.Value = string.IsNullOrWhiteSpace(comp.Restricciones) ? DBNull.Value : (object)comp.Restricciones.Trim();
                    cmd.Parameters.Add(paramRest);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codAeropuerto, string codModelo)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.compatibilidad_aeropuerto_modelo WHERE cod_aeropuerto = @codA AND cod_modelo = @codM";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@codA", codAeropuerto);
                    cmd.Parameters.AddWithValue("@codM", codModelo);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
