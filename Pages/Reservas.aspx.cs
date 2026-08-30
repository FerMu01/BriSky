using System;
using System.Web.UI.WebControls;
using BriSky.Models.Comercial;
using BriSky.Services.Comercial;
using BriSky.Services.Operaciones;
using BriSky.Services.Personal;

public partial class Pages_Reservas : System.Web.UI.Page
{
    private ReservaService _reservaService;
    private PasajeroService _pasajeroService;
    private TarifaService _tarifaService;
    private VueloService _vueloService;
    private EmpleadoService _empleadoService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _reservaService = new ReservaService();
        _pasajeroService = new PasajeroService();
        _tarifaService = new TarifaService();
        _vueloService = new VueloService();
        _empleadoService = new EmpleadoService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarGrillaReservas();
        }
    }

    private void MostrarAlerta(string mensaje, bool esError)
    {
        pnlMensaje.Visible = true;
        lblMensaje.Text = mensaje;
        pnlMensaje.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    protected string ClaseBadgeEstado(string estado)
    {
        switch ((estado ?? "").ToUpper())
        {
            case "CONFIRMADA": return "badge badge-conf";
            case "CANCELADA": return "badge badge-canc";
            default: return "badge badge-pend";
        }
    }

    // --- Carga de Datos ---


    private void CargarGrillaReservas()
    {
        try
        {
            gvReservas.DataSource = _reservaService.ObtenerTodos();
            gvReservas.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }


}
