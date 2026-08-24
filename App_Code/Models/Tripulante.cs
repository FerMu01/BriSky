public class Tripulante : Empleado
{
    public string Licencia { get; set; }

    public Tripulante() : base() { }

    public static new Tripulante FromReader(System.Data.SqlClient.SqlDataReader rdr)
    {
        var t = new Tripulante();
        var e = Empleado.FromReader(rdr);
        // copiar campos comunes
        t.CodEmpleado = e.CodEmpleado;
        t.Nombre = e.Nombre;
        t.Apellido = e.Apellido;
        t.Documento = e.Documento;
        t.Telefono = e.Telefono;
        t.Correo = e.Correo;
        t.FechaIngreso = e.FechaIngreso;
        t.EstadoLaboral = e.EstadoLaboral;
        t.Area = e.Area;
        // licencia si existe en columna adicional (no asumo índice)
        return t;
    }
}
