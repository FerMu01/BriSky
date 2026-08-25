using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Operaciones;

namespace BriSky.Data.Operaciones
{
    public class VueloDAO
    {
        public List<Vuelo> ObtenerTodos()
        {
            var lista = new List<Vuelo>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT v.id_vuelo, v.num_vuelo, v.fecha, v.hora_salida, v.hora_llegada, v.estado, 
                           v.cod_ruta, v.cod_interno,
                           (a1.nombre + ' - ' + a2.nombre) AS ruta_formateada,
                           ISNULL(av.matricula, 'Sin Asignar') AS matricula_avion
                    FROM brisky.vuelo v
                    INNER JOIN brisky.ruta r ON v.cod_ruta = r.cod_ruta
                    INNER JOIN brisky.aeropuerto a1 ON r.cod_aeropuerto_origen = a1.cod_aeropuerto
                    INNER JOIN brisky.aeropuerto a2 ON r.cod_aeropuerto_destino = a2.cod_aeropuerto
                    LEFT JOIN brisky.avion av ON v.cod_interno = av.cod_interno
                    ORDER BY v.fecha DESC, v.hora_salida DESC";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Vuelo
                            {
                                IdVuelo = Convert.ToInt32(reader["id_vuelo"]),
                                NumVuelo = reader["num_vuelo"].ToString(),
                                Fecha = Convert.ToDateTime(reader["fecha"]),
                                HoraSalida = (TimeSpan)reader["hora_salida"],
                                HoraLlegada = (TimeSpan)reader["hora_llegada"],
                                Estado = reader["estado"].ToString(),
                                CodRuta = reader["cod_ruta"].ToString(),
                                CodInterno = reader["cod_interno"] != DBNull.Value ? reader["cod_interno"].ToString() : null,
                                RutaFormateada = reader["ruta_formateada"].ToString(),
                                MatriculaAvion = reader["matricula_avion"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public Vuelo ObtenerPorId(int idVuelo)
        {
            Vuelo v = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT v.id_vuelo, v.num_vuelo, v.fecha, v.hora_salida, v.hora_llegada, v.estado, 
                           v.cod_ruta, v.cod_interno,
                           (a1.nombre + ' - ' + a2.nombre) AS ruta_formateada,
                           ISNULL(av.matricula, 'Sin Asignar') AS matricula_avion
                    FROM brisky.vuelo v
                    INNER JOIN brisky.ruta r ON v.cod_ruta = r.cod_ruta
                    INNER JOIN brisky.aeropuerto a1 ON r.cod_aeropuerto_origen = a1.cod_aeropuerto
                    INNER JOIN brisky.aeropuerto a2 ON r.cod_aeropuerto_destino = a2.cod_aeropuerto
                    LEFT JOIN brisky.avion av ON v.cod_interno = av.cod_interno
                    WHERE v.id_vuelo = @id";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", idVuelo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            v = new Vuelo
                            {
                                IdVuelo = Convert.ToInt32(reader["id_vuelo"]),
                                NumVuelo = reader["num_vuelo"].ToString(),
                                Fecha = Convert.ToDateTime(reader["fecha"]),
                                HoraSalida = (TimeSpan)reader["hora_salida"],
                                HoraLlegada = (TimeSpan)reader["hora_llegada"],
                                Estado = reader["estado"].ToString(),
                                CodRuta = reader["cod_ruta"].ToString(),
                                CodInterno = reader["cod_interno"] != DBNull.Value ? reader["cod_interno"].ToString() : null,
                                RutaFormateada = reader["ruta_formateada"].ToString(),
                                MatriculaAvion = reader["matricula_avion"].ToString()
                            };
                        }
                    }
                }
            }
            return v;
        }

        public int Insertar(Vuelo v)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    INSERT INTO brisky.vuelo (num_vuelo, fecha, hora_salida, hora_llegada, cod_ruta) 
                    VALUES (@num, @fecha, @salida, @llegada, @ruta);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", v.NumVuelo);
                    cmd.Parameters.AddWithValue("@fecha", v.Fecha);
                    cmd.Parameters.AddWithValue("@salida", v.HoraSalida);
                    cmd.Parameters.AddWithValue("@llegada", v.HoraLlegada);
                    cmd.Parameters.AddWithValue("@ruta", v.CodRuta);
                    
                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public void ActualizarDatosBase(Vuelo v)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                // Actualiza campos básicos. Estado y Avión se manejan por SP.
                string sql = "UPDATE brisky.vuelo SET num_vuelo = @num, fecha = @fecha, hora_salida = @salida, hora_llegada = @llegada, cod_ruta = @ruta WHERE id_vuelo = @id";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@id", v.IdVuelo);
                    cmd.Parameters.AddWithValue("@num", v.NumVuelo);
                    cmd.Parameters.AddWithValue("@fecha", v.Fecha);
                    cmd.Parameters.AddWithValue("@salida", v.HoraSalida);
                    cmd.Parameters.AddWithValue("@llegada", v.HoraLlegada);
                    cmd.Parameters.AddWithValue("@ruta", v.CodRuta);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int idVuelo)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("DELETE FROM brisky.vuelo WHERE id_vuelo = @id", con))
                {
                    cmd.Parameters.AddWithValue("@id", idVuelo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // --- MÉTODOS A PROCEDIMIENTOS ALMACENADOS ---

        public void AsignarAvion(int idVuelo, string codInterno)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.asignar_avion", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id_vuelo", idVuelo);
                    
                    var pCod = new SqlParameter("@p_cod_interno", SqlDbType.VarChar);
                    pCod.Value = string.IsNullOrWhiteSpace(codInterno) ? DBNull.Value : (object)codInterno;
                    cmd.Parameters.Add(pCod);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CambiarEstado(int idVuelo, string estado)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.cambiar_estado_vuelo", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id_vuelo", idVuelo);
                    cmd.Parameters.AddWithValue("@p_estado", estado);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
