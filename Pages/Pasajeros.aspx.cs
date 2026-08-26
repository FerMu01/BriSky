using System;
using System.Web.UI.WebControls;
using BriSky.Models.Comercial;
using BriSky.Services.Comercial;

public partial class Pages_Pasajeros : System.Web.UI.Page
{
    private PasajeroService _pasajeroService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _pasajeroService = new PasajeroService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarGrillaPasajeros();
        }
    }

    private void MostrarAlerta(string mensaje, bool esError)
    {
        pnlMensaje.Visible = true;
        lblMensaje.Text = mensaje;
        pnlMensaje.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    // --- Carga de Datos ---

    private void CargarGrillaPasajeros()
    {
        try
        {
            gvPasajeros.DataSource = _pasajeroService.ObtenerTodos();
            gvPasajeros.DataBind();
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
            DateTime fechaNacimiento;
            if (!DateTime.TryParse(txtFechaNacimiento.Text, out fechaNacimiento))
            {
                MostrarAlerta("Debe indicar una fecha de nacimiento válida.", true);
                return;
            }

            Pasajero p = new Pasajero
            {
                CodPasajero = txtCodPasajero.Text.Trim(),
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                NumDocumento = txtNumDocumento.Text.Trim(),
                Nacionalidad = txtNacionalidad.Text.Trim(),
                FechaNacimiento = fechaNacimiento,
                Telefono = txtTelefono.Text.Trim(),
                Correo = txtCorreo.Text.Trim()
            };

            bool esNuevo = txtCodPasajero.Enabled;

            _pasajeroService.Guardar(p, esNuevo);

            MostrarAlerta(esNuevo ? "Pasajero registrado exitosamente." : "Pasajero actualizado.", false);
            LimpiarFormulario();
            CargarGrillaPasajeros();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void gvPasajeros_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string codPasajero = e.CommandArgument.ToString();

        if (e.CommandName == "EditarPasajero")
        {
            try
            {
                Pasajero p = _pasajeroService.ObtenerPorId(codPasajero);
                if (p != null)
                {
                    txtCodPasajero.Text = p.CodPasajero;
                    txtCodPasajero.Enabled = false; // Bloquea PK
                    hdnCodPasajero.Value = p.CodPasajero;

                    txtNombre.Text = p.Nombre;
                    txtApellido.Text = p.Apellido;
                    txtNumDocumento.Text = p.NumDocumento;
                    txtNacionalidad.Text = p.Nacionalidad;
                    txtFechaNacimiento.Text = p.FechaNacimiento.ToString("yyyy-MM-dd");
                    txtTelefono.Text = p.Telefono;
                    txtCorreo.Text = p.Correo;

                    lblTituloForm.InnerText = "Editar Pasajero";
                    btnGuardar.Text = "Guardar Cambios";
                    pnlMensaje.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarPasajero")
        {
            try
            {
                _pasajeroService.Eliminar(codPasajero);
                MostrarAlerta("Pasajero eliminado del sistema.", false);
                LimpiarFormulario();
                CargarGrillaPasajeros();
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
        txtCodPasajero.Text = "";
        txtCodPasajero.Enabled = true;
        hdnCodPasajero.Value = "";
        txtNombre.Text = "";
        txtApellido.Text = "";
        txtNumDocumento.Text = "";
        txtNacionalidad.Text = "";
        txtFechaNacimiento.Text = "";
        txtTelefono.Text = "";
        txtCorreo.Text = "";

        lblTituloForm.InnerText = "Registrar Pasajero";
        btnGuardar.Text = "Guardar Pasajero";
    }
}
