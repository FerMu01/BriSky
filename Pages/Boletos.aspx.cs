using System;
using System.Web.UI.WebControls;
using BriSky.Models.Comercial;
using BriSky.Services.Comercial;
using BriSky.Services.Operaciones;

public partial class Pages_Boletos : System.Web.UI.Page
{
    private BoletoService _boletoService;
    private VueloService _vueloService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _boletoService = new BoletoService();
        _vueloService = new VueloService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            CargarGrillaBoletos();
        }
    }

    private void MostrarAlerta(string mensaje, bool esError)
    {
        pnlMensaje.Visible = true;
        lblMensaje.Text = mensaje;
        pnlMensaje.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    // --- Carga de Datos ---


    private void CargarGrillaBoletos()
    {
        try
        {
            gvBoletos.DataSource = _boletoService.ObtenerTodos();
            gvBoletos.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }


}
