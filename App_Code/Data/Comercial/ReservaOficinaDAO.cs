using System;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    public class ReservaOficinaDAO
    {
        // brisky.registrar_venta crea reserva + boleto + pago en una sola
        // transacción (precio y fecha los calcula/asigna el propio SP).
        public string Insertar(ReservaOficina r)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.registrar_venta", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add("@p_cod_reserva", SqlDbType.VarChar, 12).Value = r.CodReserva;
                    cmd.Parameters.Add("@p_cod_pasajero", SqlDbType.VarChar, 10).Value = r.CodPasajero;
                    cmd.Parameters.Add("@p_id_vuelo", SqlDbType.Int).Value = r.IdVuelo;
                    cmd.Parameters.Add("@p_cod_tarifa", SqlDbType.VarChar, 10).Value = r.CodTarifa;
                    cmd.Parameters.Add("@p_cod_empleado", SqlDbType.VarChar, 10).Value = r.CodEmpleado;
                    cmd.Parameters.Add("@p_num_boleto", SqlDbType.VarChar, 15).Value = r.NumBoleto;
                    cmd.Parameters.Add("@p_metodo_pago", SqlDbType.VarChar, 25).Value = r.MetodoPago;
                    cmd.Parameters.Add("@p_cod_pago", SqlDbType.VarChar, 12).Value = r.CodPago;

                    var resultado = cmd.Parameters.Add("@p_resultado", SqlDbType.VarChar, 12);
                    resultado.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    return resultado.Value != DBNull.Value ? resultado.Value.ToString() : null;
                }
            }
        }
    }
}
