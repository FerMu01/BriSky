using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class EquipajeDAO
    {
        private const string COLUMNAS = "cod_equipaje, num_boleto, tipo, peso, cantidad";

        private Equipaje Mapear(SqlDataReader reader)
        {
            return new Equipaje
            {
                CodEquipaje = reader["cod_equipaje"].ToString(),
                NumBoleto = reader["num_boleto"].ToString(),
                Tipo = reader["tipo"] != DBNull.Value ? reader["tipo"].ToString() : null,
                Peso = reader["peso"] != DBNull.Value ? Convert.ToDouble(reader["peso"]) : 0,
                Cantidad = reader["cantidad"] != DBNull.Value ? Convert.ToInt32(reader["cantidad"]) : 0
            };
        }

        public List<Equipaje> ObtenerPorBoleto(string numBoleto)
        {
            var lista = new List<Equipaje>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = $"SELECT {COLUMNAS} FROM brisky.equipaje WHERE num_boleto = @num";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@num", numBoleto);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            lista.Add(Mapear(reader));
                    }
                }
            }
            return lista;
        }

        public Equipaje ObtenerPorId(string codEquipaje)
        {
            Equipaje eq = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = $"SELECT {COLUMNAS} FROM brisky.equipaje WHERE cod_equipaje = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codEquipaje);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            eq = Mapear(reader);
                    }
                }
            }
            return eq;
        }

        public void Insertar(Equipaje eq)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    INSERT INTO brisky.equipaje (cod_equipaje, num_boleto, tipo, peso, cantidad)
                    VALUES (@cod, @num, @tipo, @peso, @cant)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", eq.CodEquipaje);
                    cmd.Parameters.AddWithValue("@num", eq.NumBoleto);
                    cmd.Parameters.AddWithValue("@tipo", (object)eq.Tipo ?? DBNull.Value);
                    cmd.Parameters.Add("@peso", SqlDbType.Float).Value = eq.Peso;
                    cmd.Parameters.AddWithValue("@cant", eq.Cantidad);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codEquipaje)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.equipaje WHERE cod_equipaje = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codEquipaje);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
