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
        public List<VueloDetalle> BuscarVuelosWeb(string origen, string destino, DateTime fecha, int cantidadPasajeros)
        {
            List<VueloDetalle> lista = new List<VueloDetalle>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string query = @"
                    SELECT v.id_vuelo, v.num_vuelo, v.fecha, v.hora_salida, v.hora_llegada,
                           ao.cod_ciudad AS cod_origen, ao.nombre AS origen_nombre,
                           ad.cod_ciudad AS cod_destino, ad.nombre AS destino_nombre,
                           m.nombre AS modelo,
                           (SELECT COUNT(*) FROM brisky.asiento a WHERE a.id_vuelo = v.id_vuelo AND a.disponible = 1) AS asientos_libres,
                           (SELECT precio_base FROM brisky.tarifa WHERE cod_tarifa = 'TAR01') AS precio_base
                    FROM brisky.vuelo v
                    JOIN brisky.ruta r ON v.cod_ruta = r.cod_ruta
                    JOIN brisky.aeropuerto ao ON r.cod_aeropuerto_origen = ao.cod_aeropuerto
                    JOIN brisky.aeropuerto ad ON r.cod_aeropuerto_destino = ad.cod_aeropuerto
                    LEFT JOIN brisky.avion av ON v.cod_interno = av.cod_interno
                    LEFT JOIN brisky.modelo_avion m ON av.cod_modelo = m.cod_modelo
                    WHERE r.cod_aeropuerto_origen = @origen 
                      AND r.cod_aeropuerto_destino = @destino 
                      AND v.fecha = @fecha 
                      AND v.estado = 'PROGRAMADO'
                      AND (SELECT COUNT(*) FROM brisky.asiento a WHERE a.id_vuelo = v.id_vuelo AND a.disponible = 1) >= @cantidadPasajeros";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@origen", origen);
                    cmd.Parameters.AddWithValue("@destino", destino);
                    cmd.Parameters.AddWithValue("@fecha", fecha.Date);
                    cmd.Parameters.AddWithValue("@cantidadPasajeros", cantidadPasajeros);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new VueloDetalle
                            {
                                IdVuelo = Convert.ToInt32(reader["id_vuelo"]),
                                NumVuelo = reader["num_vuelo"].ToString(),
                                Fecha = Convert.ToDateTime(reader["fecha"]),
                                HoraSalida = (TimeSpan)reader["hora_salida"],
                                HoraLlegada = (TimeSpan)reader["hora_llegada"],
                                CodigoOrigen = reader["cod_origen"].ToString(),
                                AeropuertoOrigen = reader["origen_nombre"].ToString(),
                                CodigoDestino = reader["cod_destino"].ToString(),
                                AeropuertoDestino = reader["destino_nombre"].ToString(),
                                ModeloAvion = reader["modelo"] != DBNull.Value ? reader["modelo"].ToString() : "Sin Asignar",
                                AsientosDisponibles = Convert.ToInt32(reader["asientos_libres"]),
                                PrecioBase = reader["precio_base"] != DBNull.Value ? Convert.ToDecimal(reader["precio_base"]) : 0
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public VueloDetalle ObtenerDetallePorId(int idVuelo)
        {
            VueloDetalle vd = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string query = @"
                    SELECT v.id_vuelo, v.num_vuelo, v.fecha, v.hora_salida, v.hora_llegada,
                           ao.cod_ciudad AS cod_origen, ao.nombre AS origen_nombre,
                           ad.cod_ciudad AS cod_destino, ad.nombre AS destino_nombre,
                           m.nombre AS modelo,
                           (SELECT COUNT(*) FROM brisky.asiento a WHERE a.id_vuelo = v.id_vuelo AND a.disponible = 1) AS asientos_libres,
                           (SELECT precio_base FROM brisky.tarifa WHERE cod_tarifa = 'TAR01') AS precio_base
                    FROM brisky.vuelo v
                    JOIN brisky.ruta r ON v.cod_ruta = r.cod_ruta
                    JOIN brisky.aeropuerto ao ON r.cod_aeropuerto_origen = ao.cod_aeropuerto
                    JOIN brisky.aeropuerto ad ON r.cod_aeropuerto_destino = ad.cod_aeropuerto
                    LEFT JOIN brisky.avion av ON v.cod_interno = av.cod_interno
                    LEFT JOIN brisky.modelo_avion m ON av.cod_modelo = m.cod_modelo
                    WHERE v.id_vuelo = @id";

                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@id", idVuelo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            vd = new VueloDetalle
                            {
                                IdVuelo = Convert.ToInt32(reader["id_vuelo"]),
                                NumVuelo = reader["num_vuelo"].ToString(),
                                Fecha = Convert.ToDateTime(reader["fecha"]),
                                HoraSalida = (TimeSpan)reader["hora_salida"],
                                HoraLlegada = (TimeSpan)reader["hora_llegada"],
                                CodigoOrigen = reader["cod_origen"].ToString(),
                                AeropuertoOrigen = reader["origen_nombre"].ToString(),
                                CodigoDestino = reader["cod_destino"].ToString(),
                                AeropuertoDestino = reader["destino_nombre"].ToString(),
                                ModeloAvion = reader["modelo"] != DBNull.Value ? reader["modelo"].ToString() : "Sin Asignar",
                                AsientosDisponibles = Convert.ToInt32(reader["asientos_libres"]),
                                PrecioBase = reader["precio_base"] != DBNull.Value ? Convert.ToDecimal(reader["precio_base"]) : 0
                            };
                        }
                    }
                }
            }
            return vd;
        }
    }
}
