using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Comercial;

namespace BriSky.Data.Comercial
{
    // DAO polimórfico de lectura sobre la vista brisky.v_reserva_completa,
    // que hace UNION entre brisky.reserva + brisky.reserva_oficina + brisky.reserva_internet
    // (mismo patrón que EmpleadoDAO / brisky.v_empleado_completo).
    public class ReservaDAO
    {
        public List<Reserva> ObtenerTodos()
        {
            var lista = new List<Reserva>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT r.cod_reserva, r.fecha, r.precio, r.estado, r.tipo_reserva,
                           r.cod_pasajero, r.id_vuelo, r.cod_tarifa,
                           (p.nombre + ' ' + p.apellido) AS nombre_pasajero,
                           v.num_vuelo,
                           t.nombre AS nombre_tarifa,
                           ro.empleado_atiende,
                           (e.nombre + ' ' + e.apellido) AS nombre_empleado,
                           ri.fecha_hora_web, ri.ip_origen
                    FROM brisky.reserva r
                    INNER JOIN brisky.pasajero p ON r.cod_pasajero = p.cod_pasajero
                    INNER JOIN brisky.vuelo v ON r.id_vuelo = v.id_vuelo
                    INNER JOIN brisky.tarifa t ON r.cod_tarifa = t.cod_tarifa
                    LEFT JOIN brisky.reserva_oficina ro ON r.cod_reserva = ro.cod_reserva
                    LEFT JOIN brisky.empleado e ON ro.empleado_atiende = e.cod_empleado
                    LEFT JOIN brisky.reserva_internet ri ON r.cod_reserva = ri.cod_reserva
                    ORDER BY r.fecha DESC";

                using (var cmd = new SqlCommand(sql, con))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var r = Mapear(reader);
                        if (r != null) lista.Add(r);
                    }
                }
            }
            return lista;
        }

        public Reserva ObtenerPorId(string codReserva)
        {
            Reserva r = null;
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT r.cod_reserva, r.fecha, r.precio, r.estado, r.tipo_reserva,
                           r.cod_pasajero, r.id_vuelo, r.cod_tarifa,
                           (p.nombre + ' ' + p.apellido) AS nombre_pasajero,
                           v.num_vuelo,
                           t.nombre AS nombre_tarifa,
                           ro.empleado_atiende,
                           (e.nombre + ' ' + e.apellido) AS nombre_empleado,
                           ri.fecha_hora_web, ri.ip_origen
                    FROM brisky.reserva r
                    INNER JOIN brisky.pasajero p ON r.cod_pasajero = p.cod_pasajero
                    INNER JOIN brisky.vuelo v ON r.id_vuelo = v.id_vuelo
                    INNER JOIN brisky.tarifa t ON r.cod_tarifa = t.cod_tarifa
                    LEFT JOIN brisky.reserva_oficina ro ON r.cod_reserva = ro.cod_reserva
                    LEFT JOIN brisky.empleado e ON ro.empleado_atiende = e.cod_empleado
                    LEFT JOIN brisky.reserva_internet ri ON r.cod_reserva = ri.cod_reserva
                    WHERE r.cod_reserva = @cod";

                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codReserva);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                            r = Mapear(reader);
                    }
                }
            }
            return r;
        }

        public bool Existe(string codReserva)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = "SELECT COUNT(1) FROM brisky.reserva WHERE cod_reserva = @cod";
                using (var cmd = new SqlCommand(sql, con))
                {
                    cmd.Parameters.AddWithValue("@cod", codReserva);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        // brisky.confirmar_reserva(@p_cod_reserva)
        public void Confirmar(string codReserva)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.confirmar_reserva", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_cod_reserva", SqlDbType.VarChar, 12).Value = codReserva;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // brisky.cancelar_reserva(@p_cod_reserva)
        public void Cancelar(string codReserva)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.cancelar_reserva", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_cod_reserva", SqlDbType.VarChar, 12).Value = codReserva;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // brisky.generar_boleto(@p_cod_reserva, @p_num_boleto, out @p_resultado)
        public string GenerarBoleto(string codReserva, string numBoleto)
        {
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                using (var cmd = new SqlCommand("brisky.generar_boleto", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@p_cod_reserva", SqlDbType.VarChar, 12).Value = codReserva;
                    cmd.Parameters.Add("@p_num_boleto", SqlDbType.VarChar, 15).Value = numBoleto;

                    var resultado = cmd.Parameters.Add("@p_resultado", SqlDbType.VarChar, 15);
                    resultado.Direction = ParameterDirection.Output;

                    cmd.ExecuteNonQuery();

                    return resultado.Value != DBNull.Value ? resultado.Value.ToString() : null;
                }
            }
        }

        // NOTA: No existe un procedimiento almacenado para eliminar una reserva
        // por completo (solo cancelar_reserva, que cambia el estado). Se deja
        // fuera de la interfaz hasta confirmar si existe una regla de negocio
        // distinta para el borrado físico.

        private Reserva Mapear(SqlDataReader reader)
        {
            string tipo = reader["tipo_reserva"] != DBNull.Value ? reader["tipo_reserva"].ToString().ToUpper() : "";
            Reserva r = null;

            switch (tipo)
            {
                case "OFICINA":
                    var oficina = new ReservaOficina();
                    oficina.CodEmpleado = HasColumn(reader, "empleado_atiende") && reader["empleado_atiende"] != DBNull.Value ? reader["empleado_atiende"].ToString() : null;
                    oficina.NombreEmpleado = HasColumn(reader, "nombre_empleado") && reader["nombre_empleado"] != DBNull.Value ? reader["nombre_empleado"].ToString() : null;
                    r = oficina;
                    break;

                case "INTERNET":
                    var internet = new ReservaInternet();
                    internet.FechaHoraWeb = HasColumn(reader, "fecha_hora_web") && reader["fecha_hora_web"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_hora_web"]) : DateTime.MinValue;
                    internet.IpOrigen = HasColumn(reader, "ip_origen") && reader["ip_origen"] != DBNull.Value ? reader["ip_origen"].ToString() : null;
                    r = internet;
                    break;
            }

            if (r != null)
            {
                r.CodReserva = reader["cod_reserva"].ToString();
                r.Fecha = Convert.ToDateTime(reader["fecha"]);
                r.Precio = Convert.ToDecimal(reader["precio"]);
                r.Estado = reader["estado"].ToString();
                r.TipoReserva = tipo;
                r.CodPasajero = reader["cod_pasajero"].ToString();
                r.IdVuelo = Convert.ToInt32(reader["id_vuelo"]);
                r.CodTarifa = reader["cod_tarifa"].ToString();
                r.NombrePasajero = HasColumn(reader, "nombre_pasajero") && reader["nombre_pasajero"] != DBNull.Value ? reader["nombre_pasajero"].ToString() : null;
                r.RutaFormateadaVuelo = HasColumn(reader, "num_vuelo") && reader["num_vuelo"] != DBNull.Value ? reader["num_vuelo"].ToString() : null;
                r.NombreTarifa = HasColumn(reader, "nombre_tarifa") && reader["nombre_tarifa"] != DBNull.Value ? reader["nombre_tarifa"].ToString() : null;
            }

            return r;
        }

        private bool HasColumn(SqlDataReader reader, string columnName)
        {
            for (int i = 0; i < reader.FieldCount; i++)
            {
                if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }
            return false;
        }
    }
}
