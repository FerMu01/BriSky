using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class BoletoDAO
    {
        public List<Boleto> ObtenerTodos()
        {
            var lista = new List<Boleto>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT b.num_boleto, b.precio, b.cod_reserva, b.id_vuelo, b.num_asiento, b.anulado,
                           v.num_vuelo
                    FROM brisky.boleto b
                    INNER JOIN brisky.vuelo v ON b.id_vuelo = v.id_vuelo
                    ORDER BY b.num_boleto DESC";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Boleto
                            {
                                NumBoleto = reader["num_boleto"].ToString(),
                                Precio = Convert.ToDecimal(reader["precio"]),
                                CodReserva = reader["cod_reserva"].ToString(),
                                IdVuelo = Convert.ToInt32(reader["id_vuelo"]),
                                NumAsiento = reader["num_asiento"].ToString(),
                                Anulado = Convert.ToBoolean(reader["anulado"]),
                                RutaFormateadaVuelo = reader["num_vuelo"].ToString() // Usamos esta propiedad para mostrar info del vuelo
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public Boleto ObtenerPorId(string numBoleto)
        {
            Boleto b = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT b.num_boleto, b.precio, b.cod_reserva, b.id_vuelo, b.num_asiento, b.anulado,
                           v.num_vuelo
                    FROM brisky.boleto b
                    INNER JOIN brisky.vuelo v ON b.id_vuelo = v.id_vuelo
                    WHERE b.num_boleto = @num";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", numBoleto);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            b = new Boleto
                            {
                                NumBoleto = reader["num_boleto"].ToString(),
                                Precio = Convert.ToDecimal(reader["precio"]),
                                CodReserva = reader["cod_reserva"].ToString(),
                                IdVuelo = Convert.ToInt32(reader["id_vuelo"]),
                                NumAsiento = reader["num_asiento"].ToString(),
                                Anulado = Convert.ToBoolean(reader["anulado"]),
                                RutaFormateadaVuelo = reader["num_vuelo"].ToString()
                            };
                        }
                    }
                }
            }
            return b;
        }

        public void Insertar(Boleto b)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    INSERT INTO brisky.boleto (num_boleto, precio, cod_reserva, id_vuelo, num_asiento, anulado) 
                    VALUES (@num, @precio, @reserva, @id_vuelo, @asiento, @anulado)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", b.NumBoleto);
                    cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = b.Precio;
                    cmd.Parameters.AddWithValue("@reserva", b.CodReserva);
                    cmd.Parameters.AddWithValue("@id_vuelo", b.IdVuelo);
                    cmd.Parameters.AddWithValue("@asiento", b.NumAsiento);
                    cmd.Parameters.Add("@anulado", SqlDbType.Bit).Value = b.Anulado;
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Boleto b)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    UPDATE brisky.boleto 
                    SET precio = @precio, cod_reserva = @reserva, id_vuelo = @id_vuelo, num_asiento = @asiento, anulado = @anulado 
                    WHERE num_boleto = @num";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", b.NumBoleto);
                    cmd.Parameters.Add("@precio", SqlDbType.Decimal).Value = b.Precio;
                    cmd.Parameters.AddWithValue("@reserva", b.CodReserva);
                    cmd.Parameters.AddWithValue("@id_vuelo", b.IdVuelo);
                    cmd.Parameters.AddWithValue("@asiento", b.NumAsiento);
                    cmd.Parameters.Add("@anulado", SqlDbType.Bit).Value = b.Anulado;
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string numBoleto)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.boleto WHERE num_boleto = @num";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", numBoleto);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
