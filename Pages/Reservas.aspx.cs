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
            CargarCombos();
            CargarGrillaReservas();
            ActualizarPanelesCanal();
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

    private void CargarCombos()
    {
        try
        {
            var pasajeros = _pasajeroService.ObtenerTodos();
            ddlPasajero.DataSource = pasajeros;
            ddlPasajero.DataTextField = "NombreCompleto";
            ddlPasajero.DataValueField = "CodPasajero";
            ddlPasajero.DataBind();
            ddlPasajero.Items.Insert(0, new ListItem("-- Seleccione Pasajero --", ""));

            var vuelos = _vueloService.ObtenerTodos();
            foreach (var v in vuelos)
                v.RutaFormateada = $"{v.NumVuelo} - {v.Fecha:dd/MM/yyyy}";
            ddlVuelo.DataSource = vuelos;
            ddlVuelo.DataTextField = "RutaFormateada";
            ddlVuelo.DataValueField = "IdVuelo";
            ddlVuelo.DataBind();
            ddlVuelo.Items.Insert(0, new ListItem("-- Seleccione Vuelo --", "0"));

            var tarifas = _tarifaService.ObtenerTodos();
            ddlTarifa.DataSource = tarifas;
            ddlTarifa.DataTextField = "Nombre";
            ddlTarifa.DataValueField = "CodTarifa";
            ddlTarifa.DataBind();
            ddlTarifa.Items.Insert(0, new ListItem("-- Seleccione Tarifa --", ""));

            ddlEmpleado.Items.Clear();
            ddlEmpleado.Items.Add(new ListItem("-- Seleccione Empleado --", ""));
            foreach (var emp in _empleadoService.ObtenerTodos())
            {
                if (emp.TipoEmpleado == "EMPLEADO_OFICINA")
                    ddlEmpleado.Items.Add(new ListItem($"{emp.Nombre} {emp.Apellido}", emp.CodEmpleado));
            }
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

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

    private void ActualizarPanelesCanal()
    {
        bool esOficina = ddlTipoReserva.SelectedValue == "OFICINA";
        pnlOficina.Visible = esOficina;
        pnlInternet.Visible = !esOficina;
    }

    // --- Eventos ---

    protected void ddlTipoReserva_SelectedIndexChanged(object sender, EventArgs e)
    {
        ActualizarPanelesCanal();
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(ddlPasajero.SelectedValue))
            {
                MostrarAlerta("Debe seleccionar un pasajero.", true);
                return;
            }

            string codReserva = txtCodReserva.Text.Trim();
            string codPasajero = ddlPasajero.SelectedValue;
            int idVuelo = int.Parse(ddlVuelo.SelectedValue);
            string codTarifa = ddlTarifa.SelectedValue;

            if (ddlTipoReserva.SelectedValue == "OFICINA")
            {
                var reserva = new ReservaOficina
                {
                    CodReserva = codReserva,
                    CodPasajero = codPasajero,
                    IdVuelo = idVuelo,
                    CodTarifa = codTarifa,
                    CodEmpleado = ddlEmpleado.SelectedValue,
                    NumBoleto = txtNumBoleto.Text.Trim(),
                    CodPago = txtCodPago.Text.Trim(),
                    MetodoPago = ddlMetodoPago.SelectedValue
                };

                _reservaService.RegistrarVenta(reserva);
                MostrarAlerta("Venta registrada: reserva, boleto y pago generados exitosamente.", false);
            }
            else
            {
                var reserva = new ReservaInternet
                {
                    CodReserva = codReserva,
                    CodPasajero = codPasajero,
                    IdVuelo = idVuelo,
                    CodTarifa = codTarifa,
                    IpOrigen = string.IsNullOrWhiteSpace(txtIpOrigen.Text) ? Request.UserHostAddress : txtIpOrigen.Text.Trim()
                };

                _reservaService.RegistrarReservaInternet(reserva);
                MostrarAlerta("Reserva por internet registrada. Queda pendiente de confirmación y emisión de boleto.", false);
            }

            LimpiarFormulario();
            CargarGrillaReservas();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void gvReservas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string codReserva = e.CommandArgument.ToString();

        try
        {
            switch (e.CommandName)
            {
                case "ConfirmarReserva":
                    _reservaService.Confirmar(codReserva);
                    MostrarAlerta("Reserva confirmada.", false);
                    break;

                case "CancelarReserva":
                    _reservaService.Cancelar(codReserva);
                    MostrarAlerta("Reserva cancelada.", false);
                    break;

                case "GenerarBoletoReserva":
                    string numBoleto = "B-" + codReserva; // num_boleto es varchar(15); cod_reserva es varchar(12)
                    _reservaService.GenerarBoleto(codReserva, numBoleto);
                    MostrarAlerta($"Boleto {numBoleto} generado para la reserva.", false);
                    break;
            }

            CargarGrillaReservas();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void btnLimpiar_Click(object sender, EventArgs e)
    {
        LimpiarFormulario();
        pnlMensaje.Visible = false;
    }

    private void LimpiarFormulario()
    {
        txtCodReserva.Text = "";
        ddlPasajero.SelectedIndex = 0;
        ddlVuelo.SelectedIndex = 0;
        ddlTarifa.SelectedIndex = 0;
        ddlTipoReserva.SelectedIndex = 0;
        ddlEmpleado.SelectedIndex = 0;
        txtNumBoleto.Text = "";
        txtCodPago.Text = "";
        ddlMetodoPago.SelectedIndex = 0;
        txtIpOrigen.Text = "";
        ActualizarPanelesCanal();
    }
}
