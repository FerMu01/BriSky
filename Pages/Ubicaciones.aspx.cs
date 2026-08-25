using System;
using System.Web.UI.WebControls;
using BriSky.Models.Ubicaciones;
using BriSky.Services.Ubicaciones;

public partial class Pages_Ubicaciones : System.Web.UI.Page
{
    private CiudadService _ciudadService;
    private AeropuertoService _aeropuertoService;
    private OficinaService _oficinaService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _ciudadService = new CiudadService();
        _aeropuertoService = new AeropuertoService();
        _oficinaService = new OficinaService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarCiudades();
            CargarComboCiudades();
            CargarAeropuertos();
            CargarOficinas();
        }
    }

    // --- MANEJO DE PESTAÑAS ---
    protected void CambiarPestaña_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int viewIndex = int.Parse(btn.CommandArgument);
        mvUbicaciones.ActiveViewIndex = viewIndex;

        // Limpiar clases activas
        btnTabCiudades.CssClass = "tab-button";
        btnTabAeropuertos.CssClass = "tab-button";
        btnTabOficinas.CssClass = "tab-button";

        // Asignar clase activa
        btn.CssClass = "tab-button active-tab";
    }

    // ==========================================
    // --- FUNCIONES CIUDAD (FASE 1) ---
    // ==========================================
    private void CargarCiudades()
    {
        try
        {
            gvCiudades.DataSource = _ciudadService.ObtenerTodas();
            gvCiudades.DataBind();
        }
        catch (Exception ex)
        {
            MostrarMensajeCiudad("Error al cargar ciudades: " + ex.Message, true);
        }
    }

    protected void btnGuardarCiudad_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = hfModoEdicionCiudad.Value == "false";

            Ciudad c = new Ciudad();
            c.CodCiudad = txtCodCiudad.Text.Trim();
            c.Nombre = txtNombreCiudad.Text.Trim();
            c.Departamento = txtDepartamentoCiudad.Text.Trim();

            _ciudadService.Guardar(c, esNuevo);

            MostrarMensajeCiudad("Ciudad guardada correctamente.", false);
            LimpiarFormularioCiudad();
            CargarCiudades();
            CargarComboCiudades(); // Recargar el combo en Aeropuertos y Oficinas
        }
        catch (Exception ex)
        {
            MostrarMensajeCiudad(ex.Message, true);
        }
    }

    protected void btnCancelarCiudad_Click(object sender, EventArgs e)
    {
        LimpiarFormularioCiudad();
        pnlMensajeCiudad.Visible = false;
    }

    protected void gvCiudades_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string codCiudad = e.CommandArgument.ToString();

        if (e.CommandName == "EditarCiudad")
        {
            try
            {
                Ciudad c = _ciudadService.ObtenerPorId(codCiudad);
                if (c != null)
                {
                    txtCodCiudad.Text = c.CodCiudad;
                    txtCodCiudad.Enabled = false; 
                    txtNombreCiudad.Text = c.Nombre;
                    txtDepartamentoCiudad.Text = c.Departamento;
                    
                    hfModoEdicionCiudad.Value = "true";
                    lblTituloFormCiudad.InnerText = "Editar Ciudad";
                    pnlMensajeCiudad.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeCiudad("Error al cargar la ciudad: " + ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarCiudad")
        {
            try
            {
                _ciudadService.Eliminar(codCiudad);
                MostrarMensajeCiudad("Ciudad eliminada correctamente.", false);
                LimpiarFormularioCiudad();
                CargarCiudades();
                CargarComboCiudades();
            }
            catch (Exception ex)
            {
                MostrarMensajeCiudad("No se puede eliminar: " + ex.Message, true);
            }
        }
    }

    private void LimpiarFormularioCiudad()
    {
        txtCodCiudad.Text = "";
        txtCodCiudad.Enabled = true;
        txtNombreCiudad.Text = "";
        txtDepartamentoCiudad.Text = "";
        hfModoEdicionCiudad.Value = "false";
        lblTituloFormCiudad.InnerText = "Registrar Ciudad";
    }

    private void MostrarMensajeCiudad(string mensaje, bool esError)
    {
        pnlMensajeCiudad.Visible = true;
        lblMensajeCiudad.Text = mensaje;
        pnlMensajeCiudad.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }


    // ==========================================
    // --- FUNCIONES COMUNES (Combos) ---
    // ==========================================
    private void CargarComboCiudades()
    {
        try
        {
            var ciudades = _ciudadService.ObtenerTodas();
            
            // Llenar combo de Aeropuertos
            ddlCiudad.DataSource = ciudades;
            ddlCiudad.DataTextField = "Nombre";
            ddlCiudad.DataValueField = "CodCiudad";
            ddlCiudad.DataBind();
            ddlCiudad.Items.Insert(0, new ListItem("-- Seleccione una Ciudad --", ""));

            // Llenar combo de Oficinas
            ddlCiudadOficina.DataSource = ciudades;
            ddlCiudadOficina.DataTextField = "Nombre";
            ddlCiudadOficina.DataValueField = "CodCiudad";
            ddlCiudadOficina.DataBind();
            ddlCiudadOficina.Items.Insert(0, new ListItem("-- Seleccione una Ciudad --", ""));
        }
        catch (Exception ex)
        {
            MostrarMensajeAeropuerto("Error al cargar ciudades en combo: " + ex.Message, true);
            MostrarMensajeOficina("Error al cargar ciudades en combo: " + ex.Message, true);
        }
    }

    // ==========================================
    // --- FUNCIONES AEROPUERTO (FASE 2) ---
    // ==========================================
    private void CargarAeropuertos()
    {
        try
        {
            gvAeropuertos.DataSource = _aeropuertoService.ObtenerTodos();
            gvAeropuertos.DataBind();
        }
        catch (Exception ex)
        {
            MostrarMensajeAeropuerto("Error al cargar aeropuertos: " + ex.Message, true);
        }
    }

    protected void btnGuardarAeropuerto_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = hfModoEdicionAeropuerto.Value == "false";

            Aeropuerto a = new Aeropuerto();
            a.CodAeropuerto = txtCodAeropuerto.Text.Trim();
            a.Nombre = txtNombreAeropuerto.Text.Trim();
            a.Pais = txtPaisAeropuerto.Text.Trim();
            a.Caracteristicas = string.IsNullOrWhiteSpace(txtCaracteristicas.Text) ? null : txtCaracteristicas.Text.Trim();
            a.CodCiudad = ddlCiudad.SelectedValue;

            _aeropuertoService.Guardar(a, esNuevo);

            MostrarMensajeAeropuerto("Aeropuerto guardado correctamente.", false);
            LimpiarFormularioAeropuerto();
            CargarAeropuertos();
        }
        catch (Exception ex)
        {
            MostrarMensajeAeropuerto(ex.Message, true);
        }
    }

    protected void btnCancelarAeropuerto_Click(object sender, EventArgs e)
    {
        LimpiarFormularioAeropuerto();
        pnlMensajeAeropuerto.Visible = false;
    }

    protected void gvAeropuertos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string codAeropuerto = e.CommandArgument.ToString();

        if (e.CommandName == "EditarAeropuerto")
        {
            try
            {
                Aeropuerto a = _aeropuertoService.ObtenerPorId(codAeropuerto);
                if (a != null)
                {
                    txtCodAeropuerto.Text = a.CodAeropuerto;
                    txtCodAeropuerto.Enabled = false; // PK
                    txtNombreAeropuerto.Text = a.Nombre;
                    txtPaisAeropuerto.Text = a.Pais;
                    txtCaracteristicas.Text = a.Caracteristicas;
                    
                    ddlCiudad.SelectedValue = a.CodCiudad;
                    
                    hfModoEdicionAeropuerto.Value = "true";
                    lblTituloFormAeropuerto.InnerText = "Editar Aeropuerto";
                    pnlMensajeAeropuerto.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeAeropuerto("Error al cargar el aeropuerto: " + ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarAeropuerto")
        {
            try
            {
                _aeropuertoService.Eliminar(codAeropuerto);
                MostrarMensajeAeropuerto("Aeropuerto eliminado correctamente.", false);
                LimpiarFormularioAeropuerto();
                CargarAeropuertos();
            }
            catch (Exception ex)
            {
                MostrarMensajeAeropuerto("No se puede eliminar: " + ex.Message, true);
            }
        }
    }

    private void LimpiarFormularioAeropuerto()
    {
        txtCodAeropuerto.Text = "";
        txtCodAeropuerto.Enabled = true;
        txtNombreAeropuerto.Text = "";
        txtPaisAeropuerto.Text = "";
        txtCaracteristicas.Text = "";
        ddlCiudad.SelectedIndex = 0;
        
        hfModoEdicionAeropuerto.Value = "false";
        lblTituloFormAeropuerto.InnerText = "Registrar Aeropuerto";
    }

    private void MostrarMensajeAeropuerto(string mensaje, bool esError)
    {
        pnlMensajeAeropuerto.Visible = true;
        lblMensajeAeropuerto.Text = mensaje;
        pnlMensajeAeropuerto.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    // ==========================================
    // --- FUNCIONES OFICINAS (FASE 3) ---
    // ==========================================
    private void CargarOficinas()
    {
        try
        {
            gvOficinas.DataSource = _oficinaService.ObtenerTodas();
            gvOficinas.DataBind();
        }
        catch (Exception ex)
        {
            MostrarMensajeOficina("Error al cargar oficinas: " + ex.Message, true);
        }
    }

    protected void btnGuardarOficina_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = hfModoEdicionOficina.Value == "false";

            Oficina o = new Oficina();
            o.CodOficina = txtCodOficina.Text.Trim();
            o.Nombre = txtNombreOficina.Text.Trim();
            o.Direccion = txtDireccionOficina.Text.Trim();
            o.Telefono = string.IsNullOrWhiteSpace(txtTelefonoOficina.Text) ? null : txtTelefonoOficina.Text.Trim();
            o.Correo = string.IsNullOrWhiteSpace(txtCorreoOficina.Text) ? null : txtCorreoOficina.Text.Trim();
            o.CodCiudad = ddlCiudadOficina.SelectedValue;

            _oficinaService.Guardar(o, esNuevo);

            MostrarMensajeOficina("Oficina guardada correctamente.", false);
            LimpiarFormularioOficina();
            CargarOficinas();
        }
        catch (Exception ex)
        {
            MostrarMensajeOficina(ex.Message, true);
        }
    }

    protected void btnCancelarOficina_Click(object sender, EventArgs e)
    {
        LimpiarFormularioOficina();
        pnlMensajeOficina.Visible = false;
    }

    protected void gvOficinas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string codOficina = e.CommandArgument.ToString();

        if (e.CommandName == "EditarOficina")
        {
            try
            {
                Oficina o = _oficinaService.ObtenerPorId(codOficina);
                if (o != null)
                {
                    txtCodOficina.Text = o.CodOficina;
                    txtCodOficina.Enabled = false; // PK
                    txtNombreOficina.Text = o.Nombre;
                    txtDireccionOficina.Text = o.Direccion;
                    txtTelefonoOficina.Text = o.Telefono;
                    txtCorreoOficina.Text = o.Correo;
                    
                    ddlCiudadOficina.SelectedValue = o.CodCiudad;
                    
                    hfModoEdicionOficina.Value = "true";
                    lblTituloFormOficina.InnerText = "Editar Oficina";
                    pnlMensajeOficina.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeOficina("Error al cargar la oficina: " + ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarOficina")
        {
            try
            {
                _oficinaService.Eliminar(codOficina);
                MostrarMensajeOficina("Oficina eliminada correctamente.", false);
                LimpiarFormularioOficina();
                CargarOficinas();
            }
            catch (Exception ex)
            {
                MostrarMensajeOficina("No se puede eliminar: " + ex.Message, true);
            }
        }
    }

    private void LimpiarFormularioOficina()
    {
        txtCodOficina.Text = "";
        txtCodOficina.Enabled = true;
        txtNombreOficina.Text = "";
        txtDireccionOficina.Text = "";
        txtTelefonoOficina.Text = "";
        txtCorreoOficina.Text = "";
        ddlCiudadOficina.SelectedIndex = 0;
        
        hfModoEdicionOficina.Value = "false";
        lblTituloFormOficina.InnerText = "Registrar Oficina";
    }

    private void MostrarMensajeOficina(string mensaje, bool esError)
    {
        pnlMensajeOficina.Visible = true;
        lblMensajeOficina.Text = mensaje;
        pnlMensajeOficina.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }
}
