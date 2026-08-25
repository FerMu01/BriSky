using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Flota;

namespace BriSky.Data.Flota
{
    public class ModeloAvionDAO
    {
        public List<ModeloAvion> ObtenerTodos()
        {
            var lista = new List<ModeloAvion>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_modelo, fabricante, nombre, tipo, capacidad_pasajeros, capacidad_equipaje, categoria FROM brisky.modelo_avion";
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ModeloAvion
                            {
                                CodModelo = reader.GetString(0),
                                Fabricante = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Tipo = reader.GetString(3),
                                CapacidadPasajeros = reader.GetInt32(4),
                                CapacidadEquipaje = reader.GetDecimal(5),
                                Categoria = reader.IsDBNull(6) ? null : reader.GetString(6)
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public ModeloAvion ObtenerPorId(string codModelo)
        {
            ModeloAvion modelo = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT cod_modelo, fabricante, nombre, tipo, capacidad_pasajeros, capacidad_equipaje, categoria FROM brisky.modelo_avion WHERE cod_modelo = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codModelo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            modelo = new ModeloAvion
                            {
                                CodModelo = reader.GetString(0),
                                Fabricante = reader.GetString(1),
                                Nombre = reader.GetString(2),
                                Tipo = reader.GetString(3),
                                CapacidadPasajeros = reader.GetInt32(4),
                                CapacidadEquipaje = reader.GetDecimal(5),
                                Categoria = reader.IsDBNull(6) ? null : reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return modelo;
        }

        public void Insertar(ModeloAvion modelo)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "INSERT INTO brisky.modelo_avion (cod_modelo, fabricante, nombre, tipo, capacidad_pasajeros, capacidad_equipaje, categoria) VALUES (@cod, @fab, @nom, @tipo, @pasajeros, @equipaje, @cat)";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", modelo.CodModelo);
                    cmd.Parameters.AddWithValue("@fab", modelo.Fabricante);
                    cmd.Parameters.AddWithValue("@nom", modelo.Nombre);
                    cmd.Parameters.AddWithValue("@tipo", modelo.Tipo);
                    
                    var paramPasajeros = new SqlParameter("@pasajeros", SqlDbType.Int);
                    paramPasajeros.Value = modelo.CapacidadPasajeros;
                    cmd.Parameters.Add(paramPasajeros);
                    
                    var paramEquipaje = new SqlParameter("@equipaje", SqlDbType.Decimal);
                    paramEquipaje.Value = modelo.CapacidadEquipaje;
                    cmd.Parameters.Add(paramEquipaje);
                    
                    cmd.Parameters.AddWithValue("@cat", (object)modelo.Categoria ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Actualizar(ModeloAvion modelo)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "UPDATE brisky.modelo_avion SET fabricante = @fab, nombre = @nom, tipo = @tipo, capacidad_pasajeros = @pasajeros, capacidad_equipaje = @equipaje, categoria = @cat WHERE cod_modelo = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", modelo.CodModelo);
                    cmd.Parameters.AddWithValue("@fab", modelo.Fabricante);
                    cmd.Parameters.AddWithValue("@nom", modelo.Nombre);
                    cmd.Parameters.AddWithValue("@tipo", modelo.Tipo);
                    
                    var paramPasajeros = new SqlParameter("@pasajeros", SqlDbType.Int);
                    paramPasajeros.Value = modelo.CapacidadPasajeros;
                    cmd.Parameters.Add(paramPasajeros);
                    
                    var paramEquipaje = new SqlParameter("@equipaje", SqlDbType.Decimal);
                    paramEquipaje.Value = modelo.CapacidadEquipaje;
                    cmd.Parameters.Add(paramEquipaje);
                    
                    cmd.Parameters.AddWithValue("@cat", (object)modelo.Categoria ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string codModelo)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.modelo_avion WHERE cod_modelo = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codModelo);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
