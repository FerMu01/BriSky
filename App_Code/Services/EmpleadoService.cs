using System.Collections.Generic;

public class EmpleadoService
{
    private readonly EmpleadoDAO _dao = new EmpleadoDAO();

    public List<Empleado> ObtenerEmpleados()
    {
        return _dao.ObtenerEmpleados();
    }

    public List<Tripulante> ObtenerTripulantes()
    {
        return _dao.ObtenerTripulantes();
    }

    public Empleado ObtenerEmpleado(string id)
    {
        return _dao.ObtenerEmpleado(id);
    }

    public string CrearEmpleado(Empleado emp)
    {
        // delega al DAO la creación y devuelve el nuevo id (si aplica)
        return _dao.CrearEmpleado(emp);
    }

    public string CrearEmpleadoCompleto(Empleado emp, string tipo, bool simulateFailure = false)
    {
        // Generar Codigo
        string cod = "EMP" + new System.Random().Next(100, 9999).ToString();
        emp.CodEmpleado = cod;

        // tipo: "Empleado", "Tripulante", "Piloto"
        return _dao.CrearRegistroDemo(emp, tipo);
    }
}
