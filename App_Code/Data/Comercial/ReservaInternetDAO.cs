using System;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class ReservaInternetDAO
    {
        // brisky.crear_reserva_internet calcula el precio internamente
        // (brisky.calcular_precio) y registra la fecha/hora web con GETDATE().
        public string Insertar(ReservaInternet r)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.crear_reserva_internet", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@p_cod_reserva", SqlDbType.VarChar, 12).Value = r.CodReserva;
                    cmd.Parameters.Add("@p_cod_pasajero", SqlDbType.VarChar, 10).Value = r.CodPasajero;
                    cmd.Parameters.Add("@p_id_vuelo", SqlDbType.Int).Value = r.IdVuelo;
                    cmd.Parameters.Add("@p_cod_tarifa", SqlDbType.VarChar, 10).Value = r.CodTarifa;
                    cmd.Parameters.Add("@p_ip_origen", SqlDbType.VarChar, 45).Value = (object)r.IpOrigen ?? DBNull.Value;

                    var resultado = cmd.Parameters.Add("@p_resultado", SqlDbType.VarChar, 12);
                    resultado.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    return resultado.Value != DBNull.Value ? resultado.Value.ToString() : null;
                }
            }
        }
    }
}
