using System;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Pages_Demo : System.Web.UI.Page
{
    private readonly EmpleadoService _service = new EmpleadoService();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarDatos();
        }
    }

    private void CargarDatos()
    {
        try
        {
            // Solo mostramos Tripulantes y Pilotos en esta pantalla simplificada para ver la herencia
            gvTripulantes.DataSource = _service.ObtenerTripulantes();
            gvTripulantes.DataBind();
        }
        catch (Exception ex)
        {
            MostrarMensaje(false, "Error al cargar datos: " + ex.Message);
        }
    }

    protected void btnCreateEmployee_Click(object sender, EventArgs e)
    {
        try
        {
            var emp = new Empleado
            {
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Documento = txtDocumento.Text,
                FechaIngreso = DateTime.Now,
                EstadoLaboral = true
            };

            string tipo = ddlTipo.SelectedValue;
            string nuevoId = _service.CrearEmpleadoCompleto(emp, tipo);

            MostrarMensaje(true, $"&Eacute;xito: {tipo} registrado correctamente en BD (Cod: {nuevoId}).");
            CargarDatos();
            LimpiarFormulario();
        }
        catch (Exception ex)
        {
            MostrarMensaje(false, $"Error al registrar: {ex.Message}");
        }
    }

    private void MostrarMensaje(bool esExito, string mensaje)
    {
        pnlMessage.Visible = true;
        pnlMessage.CssClass = esExito ? "status-message status-success" : "status-message status-error";
        
        pnlMessage.Controls.Clear();
        pnlMessage.Controls.Add(new LiteralControl(mensaje));
    }

    private void LimpiarFormulario()
    {
        txtNombre.Text = "";
        txtApellido.Text = "";
        txtDocumento.Text = "";
        ddlTipo.SelectedIndex = 0;
    }

    protected string ObtenerHorasVuelo(object dataItem)
    {
        if (dataItem is Piloto p)
        {
            return p.HorasVuelo.ToString();
        }
        return "-";
    }

    protected string ObtenerRol(object dataItem)
    {
        if (dataItem is Piloto) return "Piloto";
        if (dataItem is Tripulante) return "Tripulante";
        return "Empleado";
    }
}
