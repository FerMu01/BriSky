public class Piloto : Tripulante
{
    public int HorasVuelo { get; set; }

    public Piloto() : base() { }

    public static new Piloto FromReader(System.Data.SqlClient.SqlDataReader rdr)
    {
        var p = new Piloto();
        var t = Tripulante.FromReader(rdr);
        p.CodEmpleado = t.CodEmpleado;
        p.Nombre = t.Nombre;
        p.Apellido = t.Apellido;
        p.Documento = t.Documento;
        p.Telefono = t.Telefono;
        p.Correo = t.Correo;
        p.FechaIngreso = t.FechaIngreso;
        p.EstadoLaboral = t.EstadoLaboral;
        p.Area = t.Area;
        // HorasVuelo se puede mapear si está en reader
        return p;
    }

    public void Pilotar()
    {
        // Lógica de dominio específica de piloto
    }
}
