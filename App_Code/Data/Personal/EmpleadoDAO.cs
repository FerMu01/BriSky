using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using BriSky.Models.Personal;

namespace BriSky.Data.Personal
{
    public class EmpleadoDAO
    {
        public List<Empleado> ObtenerTodos()
        {
            var lista = new List<Empleado>();
            using (var con = Conexion.GetConnection())
            {
                con.Open();
                string sql = @"
                    SELECT 
                        v.*, 
                        e.telefono, 
                        e.correo, 
                        e.fecha_ingreso, 
                        e.cod_area, 
                        a.nombre AS nombre_area 
                    FROM brisky.v_empleado_completo v
                    INNER JOIN brisky.empleado e ON v.cod_empleado = e.cod_empleado
                    LEFT JOIN brisky.area a ON e.cod_area = a.cod_area";

                
                using (var cmd = new SqlCommand(sql, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string tipoEmpleado = reader["tipo_empleado"] != DBNull.Value ? reader["tipo_empleado"].ToString().ToUpper() : "";
                            Empleado emp = null;

                            // Instanciación polimórfica según el tipo
                            switch (tipoEmpleado)
                            {
                                case "PILOTO":
                                    var piloto = new Piloto();
                                    piloto.Licencia = reader["licencia"] != DBNull.Value ? reader["licencia"].ToString() : null;
                                    piloto.HorasVuelo = reader["horas_vuelo"] != DBNull.Value ? Convert.ToDecimal(reader["horas_vuelo"]) : 0m;
                                    piloto.RangoPiloto = reader["rango_piloto"] != DBNull.Value ? reader["rango_piloto"].ToString() : null;
                                    emp = piloto;
                                    break;
                                    
                                case "TRIPULANTE_CABINA":
                                    var cabina = new TripulanteCabina();
                                    cabina.Licencia = reader["licencia"] != DBNull.Value ? reader["licencia"].ToString() : null;
                                    cabina.HorasVuelo = reader["horas_vuelo"] != DBNull.Value ? Convert.ToDecimal(reader["horas_vuelo"]) : 0m;
                                    emp = cabina;
                                    break;
                                    
                                case "EMPLEADO_OFICINA":
                                    var oficina = new EmpleadoOficina();
                                    oficina.Cargo = HasColumn(reader, "cargo") && reader["cargo"] != DBNull.Value ? reader["cargo"].ToString() : null;
                                    oficina.CodOficina = HasColumn(reader, "cod_oficina") && reader["cod_oficina"] != DBNull.Value ? reader["cod_oficina"].ToString() : null;
                                    oficina.NombreOficina = HasColumn(reader, "nombre_oficina") && reader["nombre_oficina"] != DBNull.Value ? reader["nombre_oficina"].ToString() : null;
                                    emp = oficina;
                                    break;
                                    
                                case "PERSONAL_MANTENIMIENTO":
                                    var manto = new PersonalMantenimiento();
                                    manto.Especialidad = reader["especialidad"] != DBNull.Value ? reader["especialidad"].ToString() : null;
                                    emp = manto;
                                    break;
                            }

                            if (emp != null)
                            {
                                // Mapeo de campos base compartidos
                                emp.CodEmpleado = reader["cod_empleado"].ToString();
                                emp.Nombre = reader["nombre"].ToString();
                                emp.Apellido = reader["apellido"].ToString();
                                emp.Documento = reader["documento"].ToString();
                                emp.EstadoLaboral = reader["estado_laboral"].ToString();
                                emp.TipoEmpleado = tipoEmpleado;
                                
                                // Lectura segura de campos que podrían no existir en la vista
                                emp.Telefono = HasColumn(reader, "telefono") && reader["telefono"] != DBNull.Value ? reader["telefono"].ToString() : null;
                                emp.Correo = HasColumn(reader, "correo") && reader["correo"] != DBNull.Value ? reader["correo"].ToString() : null;
                                emp.FechaIngreso = HasColumn(reader, "fecha_ingreso") && reader["fecha_ingreso"] != DBNull.Value ? Convert.ToDateTime(reader["fecha_ingreso"]) : DateTime.MinValue;
                                emp.CodArea = HasColumn(reader, "cod_area") && reader["cod_area"] != DBNull.Value ? reader["cod_area"].ToString() : null;
                                emp.NombreArea = HasColumn(reader, "nombre_area") && reader["nombre_area"] != DBNull.Value ? reader["nombre_area"].ToString() : null;
                                
                                lista.Add(emp);
                            }
                        }
                    }
                }
            }
            return lista;
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
