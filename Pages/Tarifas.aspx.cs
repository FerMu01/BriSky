using System;
using System.Web.UI.WebControls;
using BriSky.Models.Comercial;
using BriSky.Services.Comercial;

public partial class Pages_Tarifas : System.Web.UI.Page
{
    private TarifaService _tarifaService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _tarifaService = new TarifaService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarGrillaTarifas();
        }
    }

    private void MostrarAlerta(string mensaje, bool esError)
    {
        pnlMensaje.Visible = true;
        lblMensaje.Text = mensaje;
        pnlMensaje.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    private void CargarGrillaTarifas()
    {
        try
        {
            gvTarifas.DataSource = _tarifaService.ObtenerTodos();
            gvTarifas.DataBind();
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
            decimal precioBase, equipajeIncluido;

            // Validación de parseo de decimales de manera segura para evitar caídas si el usuario inyecta texto
            if (!decimal.TryParse(txtPrecioBase.Text, out precioBase))
            {
                MostrarAlerta("El Precio Base ingresado no tiene un formato válido.", true);
                return;
            }

            if (!decimal.TryParse(txtEquipajeIncluido.Text, out equipajeIncluido))
            {
                MostrarAlerta("El peso del Equipaje Incluido no tiene un formato numérico válido.", true);
                return;
            }

            Tarifa t = new Tarifa
            {
                CodTarifa = txtCodTarifa.Text.Trim(),
                Nombre = txtNombre.Text.Trim(),
                PrecioBase = precioBase,
                EquipajeIncluido = equipajeIncluido,
                Condiciones = txtCondiciones.Text.Trim()
            };

            bool esNuevo = txtCodTarifa.Enabled; // Si está activo es nuevo, si está bloqueado es update

            _tarifaService.Guardar(t, esNuevo);

            MostrarAlerta("Tarifa guardada exitosamente.", false);
            LimpiarFormulario();
            CargarGrillaTarifas();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void gvTarifas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string cod = e.CommandArgument.ToString();

        if (e.CommandName == "EditarTarifa")
        {
            try
            {
                Tarifa t = _tarifaService.ObtenerPorId(cod);
                if (t != null)
                {
                    txtCodTarifa.Text = t.CodTarifa;
                    txtCodTarifa.Enabled = false; // Bloquea PK
                    txtNombre.Text = t.Nombre;
                    txtPrecioBase.Text = t.PrecioBase.ToString("0.00");
                    txtEquipajeIncluido.Text = t.EquipajeIncluido.ToString("0.0");
                    txtCondiciones.Text = t.Condiciones;

                    lblTituloForm.InnerText = "Editar Tarifa";
                    pnlMensaje.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarTarifa")
        {
            try
            {
                _tarifaService.Eliminar(cod);
                MostrarAlerta("Tarifa eliminada de la base de datos.", false);
                LimpiarFormulario();
                CargarGrillaTarifas();
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
        txtCodTarifa.Text = "";
        txtCodTarifa.Enabled = true;
        txtNombre.Text = "";
        txtPrecioBase.Text = "";
        txtEquipajeIncluido.Text = "";
        txtCondiciones.Text = "";
        lblTituloForm.InnerText = "Nueva Tarifa";
    }
}
