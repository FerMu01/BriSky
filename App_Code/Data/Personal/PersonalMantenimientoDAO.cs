using System;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Personal;

namespace BriSky.Data.Personal
{
    public class PersonalMantenimientoDAO
    {
        public void Insertar(PersonalMantenimiento m)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.crear_personal_mantenimiento", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@p_cod_empleado", m.CodEmpleado);
                    cmd.Parameters.AddWithValue("@p_nombre", m.Nombre);
                    cmd.Parameters.AddWithValue("@p_apellido", m.Apellido);
                    cmd.Parameters.AddWithValue("@p_documento", m.Documento);
                    cmd.Parameters.AddWithValue("@p_telefono", (object)m.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_correo", (object)m.Correo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_fecha_ingreso", m.FechaIngreso);
                    cmd.Parameters.AddWithValue("@p_estado_laboral", m.EstadoLaboral);
                    cmd.Parameters.AddWithValue("@p_cod_area", m.CodArea);
                    
                    // Específicos Mantenimiento
                    cmd.Parameters.AddWithValue("@p_especialidad", m.Especialidad);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
