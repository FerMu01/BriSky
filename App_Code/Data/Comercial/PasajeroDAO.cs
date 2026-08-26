using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class PasajeroDAO
    {
        private const string COLUMNAS = "cod_pasajero, nombre, apellido, num_documento, nacionalidad, fecha_nacimiento, telefono, correo";

        private Pasajero Mapear(SqlDataReader reader)
        {
            return new Pasajero
            {
                CodPasajero = reader["cod_pasajero"].ToString(),
                Nombre = reader["nombre"].ToString(),
                Apellido = reader["apellido"].ToString(),
                NumDocumento = reader["num_documento"].ToString(),
                Nacionalidad = reader["nacionalidad"] != DBNull.Value ? reader["nacionalidad"].ToString() : null,
                FechaNacimiento = reader["fecha_nacimiento"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_nacimiento"]) : DateTime.MinValue,
                Telefono = reader["telefono"] != DBNull.Value ? reader["telefono"].ToString() : null,
                Correo = reader["correo"] != DBNull.Value ? reader["correo"].ToString() : null
            };
        }

        public List<Pasajero> ObtenerTodos()
        {
            var lista = new List<Pasajero>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = $"SELECT {COLUMNAS} FROM brisky.pasajero ORDER BY apellido, nombre";
                using (var cmd = new SqlCommand(sql, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                        lista.Add(Mapear(reader));
                }
            }
            return lista;
        }

        public Pasajero ObtenerPorId(string codPasajero)
        {
            Pasajero p = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = $"SELECT {COLUMNAS} FROM brisky.pasajero WHERE cod_pasajero = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codPasajero);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            p = Mapear(reader);
                    }
                }
            }
            return p;
        }

        public Pasajero ObtenerPorDocumento(string numDocumento)
        {
            Pasajero p = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = $"SELECT {COLUMNAS} FROM brisky.pasajero WHERE num_documento = @doc";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@doc", numDocumento);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            p = Mapear(reader);
                    }
                }
            }
            return p;
        }

        public void Insertar(Pasajero p)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    INSERT INTO brisky.pasajero (cod_pasajero, nombre, apellido, num_documento, nacionalidad, fecha_nacimiento, telefono, correo)
                    VALUES (@cod, @nom, @ape, @doc, @nac, @fnac, @tel, @cor)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", p.CodPasajero);
                    cmd.Parameters.AddWithValue("@nom", p.Nombre);
                    cmd.Parameters.AddWithValue("@ape", p.Apellido);
                    cmd.Parameters.AddWithValue("@doc", p.NumDocumento);
                    cmd.Parameters.AddWithValue("@nac", (object)p.Nacionalidad ?? DBNull.Value);
                    cmd.Parameters.Add("@fnac", SqlDbType.Date).Value = p.FechaNacimiento;
                    cmd.Parameters.AddWithValue("@tel", (object)p.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@cor", (object)p.Correo ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(Pasajero p)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    UPDATE brisky.pasajero
                    SET nombre = @nom, apellido = @ape, num_documento = @doc, nacionalidad = @nac,
                        fecha_nacimiento = @fnac, telefono = @tel, correo = @cor
                    WHERE cod_pasajero = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", p.CodPasajero);
                    cmd.Parameters.AddWithValue("@nom", p.Nombre);
                    cmd.Parameters.AddWithValue("@ape", p.Apellido);
                    cmd.Parameters.AddWithValue("@doc", p.NumDocumento);
                    cmd.Parameters.AddWithValue("@nac", (object)p.Nacionalidad ?? DBNull.Value);
                    cmd.Parameters.Add("@fnac", SqlDbType.Date).Value = p.FechaNacimiento;
                    cmd.Parameters.AddWithValue("@tel", (object)p.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@cor", (object)p.Correo ?? DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codPasajero)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.pasajero WHERE cod_pasajero = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codPasajero);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
