using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Operaciones;

namespace BriSky.Data.Operaciones
{
    public class MantenimientoDAO
    {
        public List<Mantenimiento> ObtenerTodos()
        {
            var lista = new List<Mantenimiento>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT m.cod_mantenimiento, m.fecha, m.tipo, m.descripcion, m.proxima_fecha, 
                           m.finalizado, m.cod_interno, a.matricula AS matricula_avion
                    FROM brisky.mantenimiento m
                    INNER JOIN brisky.avion a ON m.cod_interno = a.cod_interno
                    ORDER BY m.fecha DESC";
                    
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Mantenimiento
                            {
                                CodMantenimiento = reader["cod_mantenimiento"].ToString(),
                                Fecha = Convert.ToDateTime(reader["fecha"]),
                                Tipo = reader["tipo"].ToString(),
                                Descripcion = reader["descripcion"].ToString(),
                                ProximaFecha = reader["proxima_fecha"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(reader["proxima_fecha"]) : null,
                                Finalizado = Convert.ToBoolean(reader["finalizado"]),
                                CodInterno = reader["cod_interno"].ToString(),
                                MatriculaAvion = reader["matricula_avion"].ToString()
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public void RealizarMantenimiento(Mantenimiento m, string codEmpleado)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.realizar_mantenimiento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@p_cod_empleado", codEmpleado);
                    cmd.Parameters.AddWithValue("@p_cod_mantenimiento", m.CodMantenimiento);
                    cmd.Parameters.AddWithValue("@p_cod_interno", m.CodInterno);
                    cmd.Parameters.AddWithValue("@p_tipo", m.Tipo);
                    cmd.Parameters.AddWithValue("@p_descripcion", m.Descripcion);
                    
                    var paramProxima = new SqlParameter("@p_proxima_fecha", SqlDbType.Date);
                    paramProxima.Value = m.ProximaFecha.HasValue ? (object)m.ProximaFecha.Value : DBNull.Value;
                    cmd.Parameters.Add(paramProxima);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void FinalizarMantenimiento(string codMantenimiento)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.finalizar_mantenimiento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_cod_mantenimiento", codMantenimiento);
                    
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
