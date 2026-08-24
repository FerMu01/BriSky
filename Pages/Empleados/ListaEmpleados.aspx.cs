using System;
using System.Collections.Generic;

public partial class Pages_Empleados_ListaEmpleados : System.Web.UI.Page
{
    private readonly EmpleadoService _service = new EmpleadoService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarEmpleados();
        }
    }

    private void CargarEmpleados()
    {
        var lista = _service.ObtenerEmpleados();
        if (gvEmpleados != null)
        {
            gvEmpleados.DataSource = lista;
            gvEmpleados.DataBind();
        }
    }
}
