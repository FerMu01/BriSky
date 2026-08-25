using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Personal;

namespace BriSky.Data.Personal
{
    public class AreaDAO
    {
        public List<Area> ObtenerTodos()
        {
            var lista = new List<Area>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_area, nombre, funcion FROM brisky.area";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Area(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.IsDBNull(2) ? null : reader.GetString(2)
                            ));
                        }
                    }
                }
            }
            return lista;
        }

        public Area ObtenerPorId(string codArea)
        {
            Area area = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_area, nombre, funcion FROM brisky.area WHERE cod_area = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codArea);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            area = new Area(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.IsDBNull(2) ? null : reader.GetString(2)
                            );
                        }
                    }
                }
            }
            return area;
        }

        public void Insertar(Area area)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.area (cod_area, nombre, funcion) VALUES (@cod, @nom, @fun)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", area.CodArea);
                    cmd.Parameters.AddWithValue("@nom", area.Nombre);
                    cmd.Parameters.AddWithValue("@fun", (object)area.Funcion ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Area area)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.area SET nombre = @nom, funcion = @fun WHERE cod_area = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", area.CodArea);
                    cmd.Parameters.AddWithValue("@nom", area.Nombre);
                    cmd.Parameters.AddWithValue("@fun", (object)area.Funcion ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codArea)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.area WHERE cod_area = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codArea);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
