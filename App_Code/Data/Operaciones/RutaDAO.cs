using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Operaciones;

namespace BriSky.Data.Operaciones
{
    public class RutaDAO
    {
        public List<Ruta> ObtenerTodos()
        {
            var lista = new List<Ruta>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT r.cod_ruta, 
                           r.cod_aeropuerto_origen, a1.nombre AS nombre_origen, 
                           r.cod_aeropuerto_destino, a2.nombre AS nombre_destino
                    FROM brisky.ruta r
                    INNER JOIN brisky.aeropuerto a1 ON r.cod_aeropuerto_origen = a1.cod_aeropuerto
                    INNER JOIN brisky.aeropuerto a2 ON r.cod_aeropuerto_destino = a2.cod_aeropuerto";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Ruta
                            {
                                CodRuta = reader["cod_ruta"].ToString(),
                                CodAeropuertoOrigen = reader["cod_aeropuerto_origen"].ToString(),
                                NombreOrigen = reader["nombre_origen"].ToString(),
                                CodAeropuertoDestino = reader["cod_aeropuerto_destino"].ToString(),
                                NombreDestino = reader["nombre_destino"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public Ruta ObtenerPorId(string codRuta)
        {
            Ruta r = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT r.cod_ruta, 
                           r.cod_aeropuerto_origen, a1.nombre AS nombre_origen, 
                           r.cod_aeropuerto_destino, a2.nombre AS nombre_destino
                    FROM brisky.ruta r
                    INNER JOIN brisky.aeropuerto a1 ON r.cod_aeropuerto_origen = a1.cod_aeropuerto
                    INNER JOIN brisky.aeropuerto a2 ON r.cod_aeropuerto_destino = a2.cod_aeropuerto
                    WHERE r.cod_ruta = @cod";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codRuta);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            r = new Ruta
                            {
                                CodRuta = reader["cod_ruta"].ToString(),
                                CodAeropuertoOrigen = reader["cod_aeropuerto_origen"].ToString(),
                                NombreOrigen = reader["nombre_origen"].ToString(),
                                CodAeropuertoDestino = reader["cod_aeropuerto_destino"].ToString(),
                                NombreDestino = reader["nombre_destino"].ToString()
                            };
                        }
                    }
                }
            }
            return r;
        }

        public void Insertar(Ruta r)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.ruta (cod_ruta, cod_aeropuerto_origen, cod_aeropuerto_destino) VALUES (@cod, @orig, @dest)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", r.CodRuta);
                    cmd.Parameters.AddWithValue("@orig", r.CodAeropuertoOrigen);
                    cmd.Parameters.AddWithValue("@dest", r.CodAeropuertoDestino);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Ruta r)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.ruta SET cod_aeropuerto_origen = @orig, cod_aeropuerto_destino = @dest WHERE cod_ruta = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", r.CodRuta);
                    cmd.Parameters.AddWithValue("@orig", r.CodAeropuertoOrigen);
                    cmd.Parameters.AddWithValue("@dest", r.CodAeropuertoDestino);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codRuta)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.ruta WHERE cod_ruta = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codRuta);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
