using System;
using System.Linq;
using System.Web.UI.WebControls;
using BriSky.Models.Operaciones;
using BriSky.Services.Operaciones;
using BriSky.Services.Flota;

public partial class Pages_Vuelos : System.Web.UI.Page
{
    private VueloService _vueloService;
    private RutaService _rutaService;
    private AvionService _avionService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _vueloService = new VueloService();
        _rutaService = new RutaService();
        _avionService = new AvionService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarRutas();
            CargarAviones();
            CargarGrillaVuelos();
        }
    }

    // --- Métodos de Interfaz UI ---
    
    protected string ObtenerClaseBadge(string estado)
    {
        switch (estado)
        {
            case "PROGRAMADO": return "badge badge-prog";
            case "ABORDANDO": return "badge badge-abrd";
            case "EN_VUELO": return "badge badge-vuel";
            case "ATERRIZADO": return "badge badge-ater";
            case "CANCELADO": return "badge badge-canc";
            case "DEMORADO": return "badge badge-demo";
            default: return "badge badge-prog";
        }
    }

    private void MostrarAlerta(string mensaje, bool esError)
    {
        pnlMensaje.Visible = true;
        lblMensaje.Text = mensaje;
        pnlMensaje.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    // --- Carga de Datos ---

    private void CargarRutas()
    {
        try
        {
            // Usamos el DAO o Service que ya trae el nombre de los aeropuertos
            var rutas = _rutaService.ObtenerTodos();
            
            ddlRuta.DataSource = rutas;
            // Concatenamos en tiempo real para el DropDown
            foreach (var r in rutas)
            {
                r.NombreOrigen = r.NombreOrigen + " - " + r.NombreDestino;
            }
            ddlRuta.DataTextField = "NombreOrigen";
            ddlRuta.DataValueField = "CodRuta";
            ddlRuta.DataBind();
            ddlRuta.Items.Insert(0, new ListItem("-- Seleccione Ruta --", ""));
        }
        catch { }
    }

    private void CargarAviones()
    {
        try
        {
            var aviones = _avionService.ObtenerTodos();
            ddlAvion.DataSource = aviones;
            ddlAvion.DataTextField = "Matricula";
            ddlAvion.DataValueField = "CodInterno";
            ddlAvion.DataBind();
            ddlAvion.Items.Insert(0, new ListItem("-- Sin Asignar --", ""));
        }
        catch { }
    }

    private void CargarGrillaVuelos()
    {
        try
        {
            gvVuelos.DataSource = _vueloService.ObtenerTodos();
            gvVuelos.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    // --- Eventos ---

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = string.IsNullOrEmpty(hdfIdVuelo.Value);
            
            Vuelo v = new Vuelo();
            if (!esNuevo)
            {
                v.IdVuelo = int.Parse(hdfIdVuelo.Value);
            }
            
            v.NumVuelo = txtNumVuelo.Text.Trim();
            v.CodRuta = ddlRuta.SelectedValue;
            
            if (!string.IsNullOrWhiteSpace(txtFecha.Text))
            {
                v.Fecha = DateTime.Parse(txtFecha.Text);
            }
            
            // Parseo seguro de TimeSpan de los TextBox type="time"
            if (!string.IsNullOrWhiteSpace(txtHoraSalida.Text))
            {
                v.HoraSalida = TimeSpan.Parse(txtHoraSalida.Text);
            }
            if (!string.IsNullOrWhiteSpace(txtHoraLlegada.Text))
            {
                v.HoraLlegada = TimeSpan.Parse(txtHoraLlegada.Text);
            }

            string codInternoAvion = ddlAvion.SelectedValue;
            string nuevoEstado = ddlEstado.SelectedValue;

            _vueloService.Guardar(v, esNuevo, codInternoAvion, nuevoEstado);

            MostrarAlerta("Vuelo guardado correctamente.", false);
            LimpiarFormulario();
            CargarGrillaVuelos();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void gvVuelos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditarVuelo")
        {
            int id = int.Parse(e.CommandArgument.ToString());
            try
            {
                Vuelo v = _vueloService.ObtenerPorId(id);
                if (v != null)
                {
                    hdfIdVuelo.Value = v.IdVuelo.ToString();
                    txtNumVuelo.Text = v.NumVuelo;
                    ddlRuta.SelectedValue = v.CodRuta;
                    txtFecha.Text = v.Fecha.ToString("yyyy-MM-dd");
                    txtHoraSalida.Text = v.HoraSalida.ToString(@"hh\:mm");
                    txtHoraLlegada.Text = v.HoraLlegada.ToString(@"hh\:mm");
                    
                    if (!string.IsNullOrEmpty(v.CodInterno))
                        ddlAvion.SelectedValue = v.CodInterno;
                    else
                        ddlAvion.SelectedIndex = 0;
                        
                    ddlEstado.SelectedValue = v.Estado;
                    
                    lblTituloForm.InnerText = "Editar Vuelo";
                    pnlMensaje.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarVuelo")
        {
            int id = int.Parse(e.CommandArgument.ToString());
            try
            {
                _vueloService.Eliminar(id);
                MostrarAlerta("Vuelo cancelado/eliminado.", false);
                LimpiarFormulario();
                CargarGrillaVuelos();
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, true);
            }
        }
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        pnlMensaje.Visible = false;
    }

    private void LimpiarFormulario()
    {
        hdfIdVuelo.Value = "";
        txtNumVuelo.Text = "";
        ddlRuta.SelectedIndex = 0;
        txtFecha.Text = "";
        txtHoraSalida.Text = "";
        txtHoraLlegada.Text = "";
        ddlAvion.SelectedIndex = 0;
        ddlEstado.SelectedValue = "PROGRAMADO"; // Default
        lblTituloForm.InnerText = "Programar Vuelo";
    }
}
