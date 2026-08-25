using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Flota;

namespace BriSky.Data.Flota
{
    public class AvionDAO
    {
        public List<Avion> ObtenerTodos()
        {
            var lista = new List<Avion>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT a.cod_interno, a.matricula, a.fecha_incorporacion, a.estado, 
                           a.ultimo_mantenimiento, a.proximo_mantenimiento, a.cod_modelo,
                           (m.fabricante + ' - ' + m.nombre) AS nombre_modelo
                    FROM brisky.avion a
                    INNER JOIN brisky.modelo_avion m ON a.cod_modelo = m.cod_modelo";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Avion
                            {
                                CodInterno = reader["cod_interno"].ToString(),
                                Matricula = reader["matricula"].ToString(),
                                FechaIncorporacion = Convert.ToDateTime(reader["fecha_incorporacion"]),
                                Estado = reader["estado"].ToString(),
                                UltimoMantenimiento = reader["ultimo_mantenimiento"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["ultimo_mantenimiento"]) : null,
                                ProximoMantenimiento = reader["proximo_mantenimiento"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["proximo_mantenimiento"]) : null,
                                CodModelo = reader["cod_modelo"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public Avion ObtenerPorId(string codInterno)
        {
            Avion avion = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT a.cod_interno, a.matricula, a.fecha_incorporacion, a.estado, 
                           a.ultimo_mantenimiento, a.proximo_mantenimiento, a.cod_modelo,
                           (m.fabricante + ' - ' + m.nombre) AS nombre_modelo
                    FROM brisky.avion a
                    INNER JOIN brisky.modelo_avion m ON a.cod_modelo = m.cod_modelo
                    WHERE a.cod_interno = @cod";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codInterno);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            avion = new Avion
                            {
                                CodInterno = reader["cod_interno"].ToString(),
                                Matricula = reader["matricula"].ToString(),
                                FechaIncorporacion = Convert.ToDateTime(reader["fecha_incorporacion"]),
                                Estado = reader["estado"].ToString(),
                                UltimoMantenimiento = reader["ultimo_mantenimiento"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["ultimo_mantenimiento"]) : null,
                                ProximoMantenimiento = reader["proximo_mantenimiento"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["proximo_mantenimiento"]) : null,
                                CodModelo = reader["cod_modelo"].ToString(),
                                NombreModelo = reader["nombre_modelo"].ToString()
                            };
                        }
                    }
                }
            }
            return avion;
        }

        public void Insertar(Avion avion)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.avion (cod_interno, matricula, fecha_incorporacion, estado, ultimo_mantenimiento, proximo_mantenimiento, cod_modelo) VALUES (@cod, @mat, @fecha, @est, @ult, @prox, @mod)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", avion.CodInterno);
                    cmd.Parameters.AddWithValue("@mat", avion.Matricula);
                    cmd.Parameters.AddWithValue("@fecha", avion.FechaIncorporacion);
                    cmd.Parameters.AddWithValue("@est", avion.Estado);
                    
                    var paramUltimo = new SqlParameter("@ult", SqlDbType.Date);
                    paramUltimo.Value = avion.UltimoMantenimiento.HasValue ? (object)avion.UltimoMantenimiento.Value : DBNull.Value;
                    cmd.Parameters.Add(paramUltimo);
                    
                    var paramProximo = new SqlParameter("@prox", SqlDbType.Date);
                    paramProximo.Value = avion.ProximoMantenimiento.HasValue ? (object)avion.ProximoMantenimiento.Value : DBNull.Value;
                    cmd.Parameters.Add(paramProximo);
                    
                    cmd.Parameters.AddWithValue("@mod", avion.CodModelo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Avion avion)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.avion SET matricula = @mat, fecha_incorporacion = @fecha, estado = @est, ultimo_mantenimiento = @ult, proximo_mantenimiento = @prox, cod_modelo = @mod WHERE cod_interno = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", avion.CodInterno);
                    cmd.Parameters.AddWithValue("@mat", avion.Matricula);
                    cmd.Parameters.AddWithValue("@fecha", avion.FechaIncorporacion);
                    cmd.Parameters.AddWithValue("@est", avion.Estado);
                    
                    var paramUltimo = new SqlParameter("@ult", SqlDbType.Date);
                    paramUltimo.Value = avion.UltimoMantenimiento.HasValue ? (object)avion.UltimoMantenimiento.Value : DBNull.Value;
                    cmd.Parameters.Add(paramUltimo);
                    
                    var paramProximo = new SqlParameter("@prox", SqlDbType.Date);
                    paramProximo.Value = avion.ProximoMantenimiento.HasValue ? (object)avion.ProximoMantenimiento.Value : DBNull.Value;
                    cmd.Parameters.Add(paramProximo);
                    
                    cmd.Parameters.AddWithValue("@mod", avion.CodModelo);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codInterno)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.avion WHERE cod_interno = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codInterno);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
