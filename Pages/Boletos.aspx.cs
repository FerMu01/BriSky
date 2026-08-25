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
            CargarVuelos();
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

    private void CargarVuelos()
    {
        try
        {
            var vuelos = _vueloService.ObtenerTodos();
            ddlVuelo.DataSource = vuelos;
            foreach (var v in vuelos)
            {
                v.RutaFormateada = $"{v.NumVuelo} - {v.Fecha:dd/MM/yyyy}";
            }
            ddlVuelo.DataTextField = "RutaFormateada";
            ddlVuelo.DataValueField = "IdVuelo";
            ddlVuelo.DataBind();
            ddlVuelo.Items.Insert(0, new ListItem("-- Seleccione Vuelo --", "0"));
        }
        catch { }
    }

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

    // --- Eventos ---

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            decimal precio;
            if (!decimal.TryParse(txtPrecio.Text, out precio))
            {
                MostrarAlerta("El precio pagado debe ser un monto numérico válido.", true);
                return;
            }

            Boleto b = new Boleto
            {
                NumBoleto = txtNumBoleto.Text.Trim(),
                IdVuelo = int.Parse(ddlVuelo.SelectedValue),
                CodReserva = txtCodReserva.Text.Trim(),
                NumAsiento = txtNumAsiento.Text.Trim().ToUpper(),
                Precio = precio,
                Anulado = bool.Parse(ddlAnulado.SelectedValue)
            };

            bool esNuevo = txtNumBoleto.Enabled;

            _boletoService.Guardar(b, esNuevo);

            MostrarAlerta(esNuevo ? "Boleto generado exitosamente." : "Boleto actualizado.", false);
            LimpiarFormulario();
            CargarGrillaBoletos();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void gvBoletos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string numBoleto = e.CommandArgument.ToString();

        if (e.CommandName == "EditarBoleto")
        {
            try
            {
                Boleto b = _boletoService.ObtenerPorId(numBoleto);
                if (b != null)
                {
                    txtNumBoleto.Text = b.NumBoleto;
                    txtNumBoleto.Enabled = false; // Bloquea PK
                    
                    ddlVuelo.SelectedValue = b.IdVuelo.ToString();
                    txtCodReserva.Text = b.CodReserva;
                    txtNumAsiento.Text = b.NumAsiento;
                    txtPrecio.Text = b.Precio.ToString("0.00");
                    ddlAnulado.SelectedValue = b.Anulado.ToString();
                    
                    lblTituloForm.InnerText = "Editar Boleto";
                    btnGuardar.Text = "Guardar Cambios";
                    pnlMensaje.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarBoleto")
        {
            try
            {
                _boletoService.Eliminar(numBoleto);
                MostrarAlerta("Boleto eliminado del sistema.", false);
                LimpiarFormulario();
                CargarGrillaBoletos();
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
        txtNumBoleto.Text = "";
        txtNumBoleto.Enabled = true;
        ddlVuelo.SelectedIndex = 0;
        txtCodReserva.Text = "";
        txtNumAsiento.Text = "";
        txtPrecio.Text = "";
        ddlAnulado.SelectedIndex = 0;
        
        lblTituloForm.InnerText = "Generar Boleto";
        btnGuardar.Text = "Guardar Boleto";
    }
}
