using System;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Personal;

namespace BriSky.Data.Personal
{
    public class PilotoDAO
    {
        public void Insertar(Piloto p)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.crear_piloto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@p_cod_empleado", p.CodEmpleado);
                    cmd.Parameters.AddWithValue("@p_nombre", p.Nombre);
                    cmd.Parameters.AddWithValue("@p_apellido", p.Apellido);
                    cmd.Parameters.AddWithValue("@p_documento", p.Documento);
                    cmd.Parameters.AddWithValue("@p_telefono", (object)p.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_correo", (object)p.Correo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_fecha_ingreso", p.FechaIngreso);
                    cmd.Parameters.AddWithValue("@p_estado_laboral", p.EstadoLaboral);
                    cmd.Parameters.AddWithValue("@p_cod_area", p.CodArea);
                    
                    // Específicos Piloto/Tripulante
                    cmd.Parameters.AddWithValue("@p_licencia", p.Licencia);
                    cmd.Parameters.AddWithValue("@p_rango_piloto", p.RangoPiloto);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
