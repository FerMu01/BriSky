using System;
using System.Web.UI.WebControls;
using BriSky.Models.Operaciones;
using BriSky.Services.Operaciones;
using BriSky.Services.Ubicaciones;

public partial class Pages_Rutas : System.Web.UI.Page
{
    private RutaService _rutaService;
    private AeropuertoService _aeropuertoService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _rutaService = new RutaService();
        _aeropuertoService = new AeropuertoService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarAeropuertos();
            CargarGrillaRutas();
        }
    }

    private void MostrarAlerta(string mensaje, bool esError)
    {
        pnlMensaje.Visible = true;
        lblMensaje.Text = mensaje;
        pnlMensaje.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    private void CargarAeropuertos()
    {
        try
        {
            var aeropuertos = _aeropuertoService.ObtenerTodos();

            ddlOrigen.DataSource = aeropuertos;
            ddlOrigen.DataTextField = "Nombre";
            ddlOrigen.DataValueField = "CodAeropuerto";
            ddlOrigen.DataBind();
            ddlOrigen.Items.Insert(0, new ListItem("-- Seleccione Origen --", ""));

            ddlDestino.DataSource = aeropuertos;
            ddlDestino.DataTextField = "Nombre";
            ddlDestino.DataValueField = "CodAeropuerto";
            ddlDestino.DataBind();
            ddlDestino.Items.Insert(0, new ListItem("-- Seleccione Destino --", ""));
        }
        catch { }
    }

    private void CargarGrillaRutas()
    {
        try
        {
            gvRutas.DataSource = _rutaService.ObtenerTodos();
            gvRutas.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void btnGuardar_Click(object sender, EventArgs e)
    {
        try
        {
            Ruta r = new Ruta
            {
                CodRuta = txtCodRuta.Text.Trim(),
                CodAeropuertoOrigen = ddlOrigen.SelectedValue,
                CodAeropuertoDestino = ddlDestino.SelectedValue
            };

            bool esNuevo = txtCodRuta.Enabled; // Si está habilitado es inserción, si no es actualización

            _rutaService.Guardar(r, esNuevo);

            MostrarAlerta("Ruta guardada correctamente.", false);
            LimpiarFormulario();
            CargarGrillaRutas();
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

    protected void gvRutas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string cod = e.CommandArgument.ToString();

        if (e.CommandName == "EditarRuta")
        {
            try
            {
                Ruta r = _rutaService.ObtenerPorId(cod);
                if (r != null)
                {
                    txtCodRuta.Text = r.CodRuta;
                    txtCodRuta.Enabled = false; // Bloquea PK
                    ddlOrigen.SelectedValue = r.CodAeropuertoOrigen;
                    ddlDestino.SelectedValue = r.CodAeropuertoDestino;

                    lblTituloForm.InnerText = "Editar Ruta";
                    pnlMensaje.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarRuta")
        {
            try
            {
                _rutaService.Eliminar(cod);
                MostrarAlerta("Ruta eliminada.", false);
                LimpiarFormulario();
                CargarGrillaRutas();
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, true);
            }
        }
    }

    private void LimpiarFormulario()
    {
        txtCodRuta.Text = "";
        txtCodRuta.Enabled = true;
        ddlOrigen.SelectedIndex = 0;
        ddlDestino.SelectedIndex = 0;
        lblTituloForm.InnerText = "Nueva Ruta";
    }
}
