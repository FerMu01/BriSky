using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class TarifaDAO
    {
        public List<Tarifa> ObtenerTodos()
        {
            var lista = new List<Tarifa>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_tarifa, nombre, precio_base, condiciones, equipaje_incluido FROM brisky.tarifa";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Tarifa
                            {
                                CodTarifa = reader["cod_tarifa"].ToString(),
                                Nombre = reader["nombre"].ToString(),
                                PrecioBase = Convert.ToDecimal(reader["precio_base"]),
                                Condiciones = reader["condiciones"] != DBNull.Value ? reader["condiciones"].ToString() : null,
                                EquipajeIncluido = Convert.ToDecimal(reader["equipaje_incluido"])
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public Tarifa ObtenerPorId(string codTarifa)
        {
            Tarifa t = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_tarifa, nombre, precio_base, condiciones, equipaje_incluido FROM brisky.tarifa WHERE cod_tarifa = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codTarifa);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            t = new Tarifa
                            {
                                CodTarifa = reader["cod_tarifa"].ToString(),
                                Nombre = reader["nombre"].ToString(),
                                PrecioBase = Convert.ToDecimal(reader["precio_base"]),
                                Condiciones = reader["condiciones"] != DBNull.Value ? reader["condiciones"].ToString() : null,
                                EquipajeIncluido = Convert.ToDecimal(reader["equipaje_incluido"])
                            };
                        }
                    }
                }
            }
            return t;
        }

        public void Insertar(Tarifa t)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.tarifa (cod_tarifa, nombre, precio_base, condiciones, equipaje_incluido) VALUES (@cod, @nombre, @precio, @cond, @equipaje)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", t.CodTarifa);
                    cmd.Parameters.AddWithValue("@nombre", t.Nombre);
                    cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = t.PrecioBase;
                    
                    cmd.Parameters.Add("@cond", SqlDbType.VarChar).Value = string.IsNullOrWhiteSpace(t.Condiciones) ? DBNull.Value : (object)t.Condiciones.Trim();
                    
                    cmd.Parameters.Add("@equipaje", SqlDbType.Decimal).Value = t.EquipajeIncluido;
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Tarifa t)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.tarifa SET nombre = @nombre, precio_base = @precio, condiciones = @cond, equipaje_incluido = @equipaje WHERE cod_tarifa = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", t.CodTarifa);
                    cmd.Parameters.AddWithValue("@nombre", t.Nombre);
                    cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = t.PrecioBase;
                    
                    cmd.Parameters.Add("@cond", SqlDbType.VarChar).Value = string.IsNullOrWhiteSpace(t.Condiciones) ? DBNull.Value : (object)t.Condiciones.Trim();
                    
                    cmd.Parameters.Add("@equipaje", SqlDbType.Decimal).Value = t.EquipajeIncluido;
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codTarifa)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.tarifa WHERE cod_tarifa = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codTarifa);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
