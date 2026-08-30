using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class AsientoDAO
    {
        private const string COLUMNAS = "num_asiento, id_vuelo, disponible, clase";

        private Asiento Mapear(SqlDataReader reader)
        {
            return new Asiento
            {
                NumAsiento = reader["num_asiento"].ToString(),
                IdVuelo = Convert.ToInt32(reader["id_vuelo"]),
                Disponible = Convert.ToBoolean(reader["disponible"]),
                Clase = reader["clase"] != DBNull.Value ? reader["clase"].ToString() : null
            };
        }

        public List<Asiento> ObtenerPorVuelo(int idVuelo)
        {
            var lista = new List<Asiento>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = $"SELECT {COLUMNAS} FROM brisky.asiento WHERE id_vuelo = @idVuelo ORDER BY num_asiento";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@idVuelo", idVuelo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(Mapear(reader));
                    }
                }
            }
            return lista;
        }

        public Asiento ObtenerPorId(int idVuelo, string numAsiento)
        {
            Asiento a = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = $"SELECT {COLUMNAS} FROM brisky.asiento WHERE id_vuelo = @idVuelo AND num_asiento = @num";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@idVuelo", idVuelo);
                    cmd.Parameters.AddWithValue("@num", numAsiento);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            a = Mapear(reader);
                    }
                }
            }
            return a;
        }

        public void Insertar(Asiento a)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.asiento (num_asiento, id_vuelo, disponible, clase) VALUES (@num, @idVuelo, @disp, @clase)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", a.NumAsiento);
                    cmd.Parameters.AddWithValue("@idVuelo", a.IdVuelo);
                    cmd.Parameters.Add("@disp", SqlDbType.Bit).Value = a.Disponible;
                    cmd.Parameters.AddWithValue("@clase", (object)a.Clase ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void CambiarDisponibilidad(int idVuelo, string numAsiento, bool disponible)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.asiento SET disponible = @disp WHERE id_vuelo = @idVuelo AND num_asiento = @num";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.Add("@disp", SqlDbType.Bit).Value = disponible;
                    cmd.Parameters.AddWithValue("@idVuelo", idVuelo);
                    cmd.Parameters.AddWithValue("@num", numAsiento);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int idVuelo, string numAsiento)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.asiento WHERE id_vuelo = @idVuelo AND num_asiento = @num";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@idVuelo", idVuelo);
                    cmd.Parameters.AddWithValue("@num", numAsiento);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void LlamarSpReservarAsiento(int idVuelo, string numAsiento)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                // Simulación o llamada al SP real
                using (var cmd = new SqlCommand("brisky.reservar_asiento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id_vuelo", idVuelo);
                    cmd.Parameters.AddWithValue("@p_num_asiento", numAsiento);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Asiento> ObtenerAsientosLibres(int idVuelo)
        {
            List<Asiento> libres = new List<Asiento>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string query = "SELECT * FROM brisky.asientos_disponibles(@IdVuelo)";
                using (var cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@IdVuelo", idVuelo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            libres.Add(Mapear(reader));
                        }
                    }
                }
            }
            return libres;
        }
    }
}
