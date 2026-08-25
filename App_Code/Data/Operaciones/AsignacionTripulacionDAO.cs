using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Operaciones;

namespace BriSky.Data.Operaciones
{
    public class AsignacionTripulacionDAO
    {
        public List<AsignacionTripulacion> ObtenerPorVuelo(int idVuelo)
        {
            var lista = new List<AsignacionTripulacion>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT at.id_vuelo, v.num_vuelo, at.cod_empleado, 
                           (e.nombre + ' ' + e.apellido) AS nombre_empleado, at.rol
                    FROM brisky.asignacion_tripulacion at
                    INNER JOIN brisky.vuelo v ON at.id_vuelo = v.id_vuelo
                    INNER JOIN brisky.empleado e ON at.cod_empleado = e.cod_empleado
                    WHERE at.id_vuelo = @idVuelo";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@idVuelo", idVuelo);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new AsignacionTripulacion
                            {
                                IdVuelo = Convert.ToInt32(reader["id_vuelo"]),
                                NumVuelo = reader["num_vuelo"].ToString(),
                                CodEmpleado = reader["cod_empleado"].ToString(),
                                NombreEmpleado = reader["nombre_empleado"].ToString(),
                                Rol = reader["rol"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public void Asignar(AsignacionTripulacion at)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.asignar_tripulante", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_id_vuelo", at.IdVuelo);
                    cmd.Parameters.AddWithValue("@p_cod_empleado", at.CodEmpleado);
                    cmd.Parameters.AddWithValue("@p_rol", at.Rol);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(int idVuelo, string codEmpleado)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "DELETE FROM brisky.asignacion_tripulacion WHERE id_vuelo = @idVuelo AND cod_empleado = @codEmpleado";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@idVuelo", idVuelo);
                    cmd.Parameters.AddWithValue("@codEmpleado", codEmpleado);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
