using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

public class EmpleadoDAO
{
    public List<Empleado> ObtenerEmpleados()
    {
        var lista = new List<Empleado>();
        using (var cn = Conexion.GetConnection())
        using (var cmd = new SqlCommand("SELECT cod_empleado, nombre, apellido, documento, telefono, correo, fecha_ingreso, estado_laboral, cod_area FROM brisky.empleado", cn))
        {
            cn.Open();
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var emp = Empleado.FromReader(rdr);
                    lista.Add(emp);
                }
            }
        }
        return lista;
    }

    public Empleado ObtenerEmpleado(string id)
    {
        using (var cn = Conexion.GetConnection())
        using (var cmd = new SqlCommand("SELECT cod_empleado, nombre, apellido, documento, telefono, correo, fecha_ingreso, estado_laboral, cod_area FROM brisky.empleado WHERE cod_empleado = @id", cn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            cn.Open();
            using (var rdr = cmd.ExecuteReader())
            {
                if (rdr.Read())
                {
                    return Empleado.FromReader(rdr);
                }
            }
        }
        return null;
    }

    public string CrearEmpleado(Empleado emp)
    {
        using (var cn = Conexion.GetConnection())
        using (var cmd = new SqlCommand("INSERT INTO brisky.empleado (nombre, apellido, documento, telefono, correo, fecha_ingreso, estado_laboral, cod_area) VALUES (@nombre,@apellido,@documento,@telefono,@correo,@fechaIngreso,@estadoLaboral,@idArea); SELECT SCOPE_IDENTITY();", cn))
        {
            cmd.Parameters.AddWithValue("@nombre", (object)emp.Nombre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@apellido", (object)emp.Apellido ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@documento", (object)emp.Documento ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@telefono", (object)emp.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@correo", (object)emp.Correo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@fechaIngreso", emp.FechaIngreso == DateTime.MinValue ? (object)DBNull.Value : emp.FechaIngreso);
            cmd.Parameters.AddWithValue("@estadoLaboral", emp.EstadoLaboral);
            cmd.Parameters.AddWithValue("@idArea", emp.Area != null ? (object)emp.Area.Id : DBNull.Value);
            cn.Open();
            var result = cmd.ExecuteScalar();
            return result != null ? result.ToString() : "";
        }
    }

    // Transactional overloads to participate in a transaction
    public string CrearEmpleado(Empleado emp, System.Data.SqlClient.SqlConnection cn, System.Data.SqlClient.SqlTransaction tx)
    {
        using (var cmd = new SqlCommand("INSERT INTO brisky.empleado (nombre, apellido, documento, telefono, correo, fecha_ingreso, estado_laboral, cod_area) VALUES (@nombre,@apellido,@documento,@telefono,@correo,@fechaIngreso,@estadoLaboral,@idArea); SELECT SCOPE_IDENTITY();", cn, tx))
        {
            cmd.Parameters.AddWithValue("@nombre", (object)emp.Nombre ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@apellido", (object)emp.Apellido ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@documento", (object)emp.Documento ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@telefono", (object)emp.Telefono ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@correo", (object)emp.Correo ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@fechaIngreso", emp.FechaIngreso == DateTime.MinValue ? (object)DBNull.Value : emp.FechaIngreso);
            cmd.Parameters.AddWithValue("@estadoLaboral", emp.EstadoLaboral);
            cmd.Parameters.AddWithValue("@idArea", emp.Area != null ? (object)emp.Area.Id : DBNull.Value);
            var result = cmd.ExecuteScalar();
            return result != null ? result.ToString() : "";
        }
    }

    public void CrearTripulante(string codEmpleado, Tripulante trip, System.Data.SqlClient.SqlConnection cn, System.Data.SqlClient.SqlTransaction tx)
    {
        // Inserta en brisky.tripulante. Ajusta columnas según esquema real.
        using (var cmd = new SqlCommand("INSERT INTO brisky.tripulante (cod_empleado, licencia, horas_vuelo) VALUES (@codEmpleado, @licencia, @horasVuelo)", cn, tx))
        {
            cmd.Parameters.AddWithValue("@codEmpleado", codEmpleado);
            cmd.Parameters.AddWithValue("@licencia", (object)trip.Licencia ?? DBNull.Value);
            Piloto p = trip as Piloto;
            cmd.Parameters.AddWithValue("@horasVuelo", p != null ? p.HorasVuelo : 0);
            cmd.ExecuteNonQuery();
        }
    }

    public void CrearPiloto(string codEmpleado, Piloto piloto, System.Data.SqlClient.SqlConnection cn, System.Data.SqlClient.SqlTransaction tx)
    {
        // Inserta en brisky.piloto. Ajusta columnas según esquema real.
        using (var cmd = new SqlCommand("INSERT INTO brisky.piloto (cod_empleado, rango_piloto) VALUES (@codEmpleado, @rango_piloto)", cn, tx))
        {
            cmd.Parameters.AddWithValue("@codEmpleado", codEmpleado);
            cmd.Parameters.AddWithValue("@rango_piloto", "Capitán");
            cmd.ExecuteNonQuery();
        }
    }

    public List<Tripulante> ObtenerTripulantes()
    {
        var lista = new List<Tripulante>();
        using (var cn = Conexion.GetConnection())
        using (var cmd = new SqlCommand(@"SELECT e.cod_empleado, e.nombre, e.apellido, e.documento, e.telefono, e.correo, e.fecha_ingreso, e.estado_laboral, e.cod_area,
                                               t.licencia, t.horas_vuelo
                                          FROM brisky.tripulante t
                                          INNER JOIN brisky.empleado e ON e.cod_empleado = t.cod_empleado
                                          LEFT JOIN brisky.piloto p ON p.cod_empleado = t.cod_empleado", cn))
        {
            cn.Open();
            using (var rdr = cmd.ExecuteReader())
            {
                while (rdr.Read())
                {
                    var t = new Tripulante
                    {
                        CodEmpleado = rdr.GetString(rdr.GetOrdinal("cod_empleado")),
                        Nombre = rdr.IsDBNull(rdr.GetOrdinal("nombre")) ? null : rdr.GetString(rdr.GetOrdinal("nombre")),
                        Apellido = rdr.IsDBNull(rdr.GetOrdinal("apellido")) ? null : rdr.GetString(rdr.GetOrdinal("apellido")),
                        Documento = rdr.IsDBNull(rdr.GetOrdinal("documento")) ? null : rdr.GetString(rdr.GetOrdinal("documento")),
                        Telefono = rdr.IsDBNull(rdr.GetOrdinal("telefono")) ? null : rdr.GetString(rdr.GetOrdinal("telefono")),
                        Correo = rdr.IsDBNull(rdr.GetOrdinal("correo")) ? null : rdr.GetString(rdr.GetOrdinal("correo")),
                        FechaIngreso = rdr.IsDBNull(rdr.GetOrdinal("fecha_ingreso")) ? DateTime.MinValue : rdr.GetDateTime(rdr.GetOrdinal("fecha_ingreso")),
                        EstadoLaboral = !rdr.IsDBNull(rdr.GetOrdinal("estado_laboral")) && (rdr.GetString(rdr.GetOrdinal("estado_laboral")).ToUpper() == "ACTIVO" || rdr.GetString(rdr.GetOrdinal("estado_laboral")) == "1"),
                        Area = !rdr.IsDBNull(rdr.GetOrdinal("cod_area")) ? new Area { Id = rdr.GetString(rdr.GetOrdinal("cod_area")) } : null,
                        Licencia = rdr.IsDBNull(rdr.GetOrdinal("licencia")) ? null : rdr.GetString(rdr.GetOrdinal("licencia"))
                    };
                    if (!rdr.IsDBNull(rdr.GetOrdinal("horas_vuelo")))
                    {
                        var p = new Piloto
                        {
                            CodEmpleado = t.CodEmpleado,
                            Nombre = t.Nombre,
                            Apellido = t.Apellido,
                            Documento = t.Documento,
                            Telefono = t.Telefono,
                            Correo = t.Correo,
                            FechaIngreso = t.FechaIngreso,
                            EstadoLaboral = t.EstadoLaboral,
                            Area = t.Area,
                            Licencia = t.Licencia,
                            HorasVuelo = Convert.ToInt32(rdr["horas_vuelo"])
                        };
                        lista.Add(p);
                    }
                    else
                    {
                        lista.Add(t);
                    }
                }
            }
        }
        return lista;
    }

    public string CrearRegistroDemo(Empleado emp, string tipo)
    {
        using (var cn = Conexion.GetConnection())
        {
            cn.Open();
            string sp = "";
            if (tipo == "Piloto") sp = "brisky.crear_piloto";
            else if (tipo == "Tripulante") sp = "brisky.crear_tripulante_cabina";
            else sp = "brisky.crear_empleado_oficina";

            using (var cmd = new SqlCommand(sp, cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_cod_empleado", emp.CodEmpleado);
                cmd.Parameters.AddWithValue("@p_nombre", (object)emp.Nombre ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_apellido", (object)emp.Apellido ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_documento", (object)emp.Documento ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_telefono", (object)emp.Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_correo", (object)emp.Correo ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@p_cod_area", "AR01"); // Hardcoded for demo

                if (tipo == "Piloto" || tipo == "Tripulante")
                {
                    cmd.Parameters.AddWithValue("@p_licencia", "LIC-" + new Random().Next(1000, 9999));
                }
                
                if (tipo == "Piloto")
                {
                    cmd.Parameters.AddWithValue("@p_rango_piloto", "Capitán");
                }
                
                if (tipo == "Empleado")
                {
                    cmd.Parameters.AddWithValue("@p_cargo", "Administrativo");
                    cmd.Parameters.AddWithValue("@p_cod_oficina", "OF01"); // Hardcoded
                }

                cmd.ExecuteNonQuery();
                return emp.CodEmpleado;
            }
        }
    }
}
