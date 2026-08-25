using System;
using System.Linq;
using System.Web.UI.WebControls;
using BriSky.Models.Operaciones;
using BriSky.Services.Operaciones;
using BriSky.Services.Personal;

public partial class Pages_Tripulacion : System.Web.UI.Page
{
    private TripulacionService _tripulacionService;
    private VueloService _vueloService;
    private EmpleadoService _empleadoService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _tripulacionService = new TripulacionService();
        _vueloService = new VueloService();
        _empleadoService = new EmpleadoService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarVuelosFiltro();
            CargarEmpleadosHabilitados();
            ActualizarAreaDeTrabajo();
        }
    }

    private void MostrarAlerta(string mensaje, bool esError)
    {
        pnlMensaje.Visible = true;
        lblMensaje.Text = mensaje;
        pnlMensaje.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    private void CargarVuelosFiltro()
    {
        try
        {
            var vuelos = _vueloService.ObtenerTodos();
            
            ddlVueloFiltro.DataSource = vuelos;
            // Mostramos Vuelo + Fecha + Ruta para que el usuario sepa exactamente qué elige
            foreach (var v in vuelos)
            {
                v.RutaFormateada = $"{v.NumVuelo} | {v.Fecha:dd/MM/yyyy} | {v.RutaFormateada}";
            }
            
            ddlVueloFiltro.DataTextField = "RutaFormateada";
            ddlVueloFiltro.DataValueField = "IdVuelo";
            ddlVueloFiltro.DataBind();
            
            ddlVueloFiltro.Items.Insert(0, new ListItem("-- Seleccione un Vuelo --", ""));
        }
        catch { }
    }

    private void CargarEmpleadosHabilitados()
    {
        try
        {
            // Filtro LINQ estricto para aislar Pilotos y Tripulantes de Cabina
            var empleados = _empleadoService.ObtenerTodos()
                            .Where(e => e.TipoEmpleado == "PILOTO" || e.TipoEmpleado == "TRIPULANTE_CABINA")
                            .ToList();
                            
            ddlEmpleado.DataSource = empleados;
            foreach (var emp in empleados)
            {
                emp.Nombre = $"{emp.Nombre} {emp.Apellido} ({emp.TipoEmpleado})";
            }
            
            ddlEmpleado.DataTextField = "Nombre";
            ddlEmpleado.DataValueField = "CodEmpleado";
            ddlEmpleado.DataBind();
        }
        catch { }
    }

    protected void ddlVueloFiltro_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlMensaje.Visible = false;
        ActualizarAreaDeTrabajo();
    }

    private void ActualizarAreaDeTrabajo()
    {
        if (string.IsNullOrEmpty(ddlVueloFiltro.SelectedValue))
        {
            pnlAreaTrabajo.Visible = false;
            return;
        }

        pnlAreaTrabajo.Visible = true;
        CargarGrillaDetalle();
    }

    private void CargarGrillaDetalle()
    {
        try
        {
            int idVuelo = int.Parse(ddlVueloFiltro.SelectedValue);
            gvTripulacion.DataSource = _tripulacionService.ObtenerPorVuelo(idVuelo);
            gvTripulacion.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void btnAsignar_Click(object sender, EventArgs e)
    {
        try
        {
            AsignacionTripulacion at = new AsignacionTripulacion
            {
                IdVuelo = int.Parse(ddlVueloFiltro.SelectedValue),
                CodEmpleado = ddlEmpleado.SelectedValue,
                Rol = ddlRol.SelectedValue
            };

            _tripulacionService.Asignar(at);
            MostrarAlerta("Empleado asignado correctamente a la tripulación.", false);
            CargarGrillaDetalle();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void gvTripulacion_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Remover")
        {
            string[] argumentos = e.CommandArgument.ToString().Split('|');
            if (argumentos.Length == 2)
            {
                int idVuelo = int.Parse(argumentos[0]);
                string codEmpleado = argumentos[1];

                try
                {
                    _tripulacionService.Eliminar(idVuelo, codEmpleado);
                    MostrarAlerta("Empleado removido del vuelo.", false);
                    CargarGrillaDetalle();
                }
                catch (Exception ex)
                {
                    MostrarAlerta(ex.Message, true);
                }
            }
        }
    }
}
