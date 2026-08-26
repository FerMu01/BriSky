using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class PagoDAO
    {
        private const string COLUMNAS = "cod_pago, cod_reserva, monto, fecha, metodo";

        private Pago Mapear(SqlDataReader reader)
        {
            return new Pago
            {
                CodPago = reader["cod_pago"].ToString(),
                CodReserva = reader["cod_reserva"].ToString(),
                Monto = Convert.ToDecimal(reader["monto"]),
                Fecha = Convert.ToDateTime(reader["fecha"]),
                Metodo = reader["metodo"] != DBNull.Value ? reader["metodo"].ToString() : null
            };
        }

        public List<Pago> ObtenerPorReserva(string codReserva)
        {
            var lista = new List<Pago>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = $"SELECT {COLUMNAS} FROM brisky.pago WHERE cod_reserva = @cod ORDER BY fecha DESC";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codReserva);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(Mapear(reader));
                    }
                }
            }
            return lista;
        }

        public void Insertar(Pago p)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    INSERT INTO brisky.pago (cod_pago, cod_reserva, monto, fecha, metodo)
                    VALUES (@cod, @reserva, @monto, @fecha, @metodo)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", p.CodPago);
                    cmd.Parameters.AddWithValue("@reserva", p.CodReserva);
                    cmd.Parameters.Add("@monto", SqlDbType.Decimal).Value = p.Monto;
                    cmd.Parameters.Add("@fecha", SqlDbType.DateTime).Value = p.Fecha;
                    cmd.Parameters.AddWithValue("@metodo", (object)p.Metodo ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codPago)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.pago WHERE cod_pago = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codPago);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
