using System;

public class Empleado
{
    public string CodEmpleado { get; set; }
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string Documento { get; set; }
    public string Telefono { get; set; }
    public string Correo { get; set; }
    public DateTime FechaIngreso { get; set; }
    public bool EstadoLaboral { get; set; }
    public Area Area { get; set; }

    // Constructor vacío
    public Empleado() { }

    // Factory method para crear desde SqlDataReader
    public static Empleado FromReader(System.Data.SqlClient.SqlDataReader rdr)
    {
        var emp = new Empleado
        {
            CodEmpleado = rdr.GetString(0),
            Nombre = rdr.IsDBNull(1) ? null : rdr.GetString(1),
            Apellido = rdr.IsDBNull(2) ? null : rdr.GetString(2),
            Documento = rdr.IsDBNull(3) ? null : rdr.GetString(3),
            Telefono = rdr.IsDBNull(4) ? null : rdr.GetString(4),
            Correo = rdr.IsDBNull(5) ? null : rdr.GetString(5),
            FechaIngreso = rdr.IsDBNull(6) ? DateTime.MinValue : rdr.GetDateTime(6),
            EstadoLaboral = !rdr.IsDBNull(7) && (rdr.GetString(7).ToUpper() == "ACTIVO" || rdr.GetString(7) == "1")
        };
        if (!rdr.IsDBNull(8)) emp.Area = new Area { Id = rdr.GetString(8) };
        return emp;
    }
}
