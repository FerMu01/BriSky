using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using BriSky.Models.Ubicaciones;

namespace BriSky.Data.Ubicaciones
{
    public class OficinaDAO
    {
        public List<Oficina> ObtenerTodos()
        {
            var lista = new List<Oficina>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT o.cod_oficina, o.nombre, o.direccion, o.telefono, o.correo, o.cod_ciudad, c.nombre AS nombre_ciudad
                    FROM brisky.oficina o
                    INNER JOIN brisky.ciudad c ON o.cod_ciudad = c.cod_ciudad";
                
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ofi = new Oficina(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.IsDBNull(3) ? null : reader.GetString(3),
                                reader.IsDBNull(4) ? null : reader.GetString(4),
                                reader.GetString(5)
                            );
                            ofi.NombreCiudad = reader.GetString(6);
                            lista.Add(ofi);
                        }
                    }
                }
            }
            return lista;
        }

        public Oficina ObtenerPorId(string codOficina)
        {
            Oficina ofi = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_oficina, nombre, direccion, telefono, correo, cod_ciudad FROM brisky.oficina WHERE cod_oficina = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codOficina);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ofi = new Oficina(
                                reader.GetString(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.IsDBNull(3) ? null : reader.GetString(3),
                                reader.IsDBNull(4) ? null : reader.GetString(4),
                                reader.GetString(5)
                            );
                        }
                    }
                }
            }
            return ofi;
        }

        public void Insertar(Oficina oficina)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.oficina (cod_oficina, nombre, direccion, telefono, correo, cod_ciudad) VALUES (@cod, @nom, @dir, @tel, @cor, @codCiu)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", oficina.CodOficina);
                    cmd.Parameters.AddWithValue("@nom", oficina.Nombre);
                    cmd.Parameters.AddWithValue("@dir", oficina.Direccion);
                    cmd.Parameters.AddWithValue("@tel", (object)oficina.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@cor", (object)oficina.Correo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@codCiu", oficina.CodCiudad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Oficina oficina)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.oficina SET nombre = @nom, direccion = @dir, telefono = @tel, correo = @cor, cod_ciudad = @codCiu WHERE cod_oficina = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", oficina.CodOficina);
                    cmd.Parameters.AddWithValue("@nom", oficina.Nombre);
                    cmd.Parameters.AddWithValue("@dir", oficina.Direccion);
                    cmd.Parameters.AddWithValue("@tel", (object)oficina.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@cor", (object)oficina.Correo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@codCiu", oficina.CodCiudad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codOficina)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.oficina WHERE cod_oficina = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codOficina);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
