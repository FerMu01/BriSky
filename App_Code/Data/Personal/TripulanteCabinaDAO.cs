using System;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Personal;

namespace BriSky.Data.Personal
{
    public class TripulanteCabinaDAO
    {
        public void Insertar(TripulanteCabina t)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.crear_tripulante_cabina", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    cmd.Parameters.AddWithValue("@p_cod_empleado", t.CodEmpleado);
                    cmd.Parameters.AddWithValue("@p_nombre", t.Nombre);
                    cmd.Parameters.AddWithValue("@p_apellido", t.Apellido);
                    cmd.Parameters.AddWithValue("@p_documento", t.Documento);
                    cmd.Parameters.AddWithValue("@p_telefono", (object)t.Telefono ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_correo", (object)t.Correo ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@p_fecha_ingreso", t.FechaIngreso);
                    cmd.Parameters.AddWithValue("@p_estado_laboral", t.EstadoLaboral);
                    cmd.Parameters.AddWithValue("@p_cod_area", t.CodArea);
                    
                    // Específicos Cabina
                    cmd.Parameters.AddWithValue("@p_licencia", t.Licencia);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
