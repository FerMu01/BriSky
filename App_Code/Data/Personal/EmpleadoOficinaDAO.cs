using System;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Personal;

namespace BriSky.Data.Personal
{
    public class EmpleadoOficinaDAO
    {
        public void Insertar(EmpleadoOficina e)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.crear_empleado_oficina", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@p_cod_empleado", e.CodEmpleado);
                    cmd.Parameters.AddWithValue("@p_nombre", e.Nombre);
                    cmd.Parameters.AddWithValue("@p_apellido", e.Apellido);
                    cmd.Parameters.AddWithValue("@p_documento", e.Documento);
                    cmd.Parameters.AddWithValue("@p_telefono", (object)e.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_correo", (object)e.Correo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_fecha_ingreso", e.FechaIngreso);
                    cmd.Parameters.AddWithValue("@p_estado_laboral", e.EstadoLaboral);
                    cmd.Parameters.AddWithValue("@p_cod_area", e.CodArea);
                    
                    // Específicos Oficina
                    cmd.Parameters.AddWithValue("@p_cargo", e.Cargo);
                    cmd.Parameters.AddWithValue("@p_cod_oficina", e.CodOficina);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
