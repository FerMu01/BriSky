using System;
using System.Web.UI.WebControls;
using BriSky.Models.Flota;
using BriSky.Services.Flota;
using BriSky.Services.Ubicaciones;

public partial class Pages_Flota : System.Web.UI.Page
{
    private ModeloAvionService _modeloService;
    private AvionService _avionService;
    private CompatibilidadAeropuertoModeloService _compatService;
    private AeropuertoService _aeropuertoService; // Para llenar combo de Aeropuertos en la pestaña 3

    protected void Page_Init(object sender, EventArgs e)
    {
        _modeloService = new ModeloAvionService();
        _avionService = new AvionService();
        _compatService = new CompatibilidadAeropuertoModeloService();
        _aeropuertoService = new AeropuertoService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarGrillaModelos();
            
            CargarComboModelos(ddlModeloAvion);
            CargarGrillaAviones();
            
            CargarComboModelos(ddlModeloCompat);
            CargarComboAeropuertos();
            CargarGrillaCompatibilidad();
        }
    }

    protected void CambiarPestaña_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int index = int.Parse(btn.CommandArgument);
        mvFlota.ActiveViewIndex = index;

        btnTabModelos.CssClass = "tab-button";
        btnTabAviones.CssClass = "tab-button";
        btnTabCompat.CssClass = "tab-button";
        btn.CssClass = "tab-button active-tab";
    }

    private void MostrarAlerta(Panel pnl, Label lbl, string mensaje, bool esError)
    {
        pnl.Visible = true;
        lbl.Text = mensaje;
        pnl.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    private void OcultarAlerta(Panel pnl)
    {
        pnl.Visible = false;
    }

    // ==========================================
    // FASE 1: MODELOS DE AVIÓN
    // ==========================================
    private void CargarGrillaModelos()
    {
        try
        {
            gvModelos.DataSource = _modeloService.ObtenerTodos();
            gvModelos.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(pnlMensajeModelo, lblMensajeModelo, ex.Message, true);
        }
    }

    protected void btnGuardarModelo_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = hfModoEdicionModelo.Value == "false";
            ModeloAvion m = new ModeloAvion
            {
                CodModelo = txtCodModelo.Text.Trim(),
                Fabricante = txtFabricante.Text.Trim(),
                Nombre = txtNombreModelo.Text.Trim(),
                Tipo = txtTipoModelo.Text.Trim(),
                Categoria = string.IsNullOrWhiteSpace(txtCategoria.Text) ? null : txtCategoria.Text.Trim(),
                CapacidadPasajeros = string.IsNullOrWhiteSpace(txtPasajeros.Text) ? 0 : int.Parse(txtPasajeros.Text),
                CapacidadEquipaje = string.IsNullOrWhiteSpace(txtEquipaje.Text) ? 0m : decimal.Parse(txtEquipaje.Text)
            };

            _modeloService.Guardar(m, esNuevo);
            
            MostrarAlerta(pnlMensajeModelo, lblMensajeModelo, "Modelo guardado correctamente.", false);
            LimpiarFormModelo();
            CargarGrillaModelos();
            CargarComboModelos(ddlModeloAvion);
            CargarComboModelos(ddlModeloCompat);
        }
        catch (Exception ex)
        {
            MostrarAlerta(pnlMensajeModelo, lblMensajeModelo, ex.Message, true);
        }
    }

    protected void gvModelos_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string cod = e.CommandArgument.ToString();
        if (e.CommandName == "EditarModelo")
        {
            ModeloAvion m = _modeloService.ObtenerPorId(cod);
            if (m != null)
            {
                txtCodModelo.Text = m.CodModelo;
                txtCodModelo.Enabled = false;
                txtFabricante.Text = m.Fabricante;
                txtNombreModelo.Text = m.Nombre;
                txtTipoModelo.Text = m.Tipo;
                txtCategoria.Text = m.Categoria;
                txtPasajeros.Text = m.CapacidadPasajeros.ToString();
                txtEquipaje.Text = m.CapacidadEquipaje.ToString("0.00");
                
                hfModoEdicionModelo.Value = "true";
                lblTituloFormModelo.InnerText = "Editar Modelo";
                OcultarAlerta(pnlMensajeModelo);
            }
        }
        else if (e.CommandName == "EliminarModelo")
        {
            try
            {
                _modeloService.Eliminar(cod);
                MostrarAlerta(pnlMensajeModelo, lblMensajeModelo, "Modelo eliminado.", false);
                LimpiarFormModelo();
                CargarGrillaModelos();
                CargarComboModelos(ddlModeloAvion);
                CargarComboModelos(ddlModeloCompat);
            }
            catch (Exception ex)
            {
                MostrarAlerta(pnlMensajeModelo, lblMensajeModelo, ex.Message, true);
            }
        }
    }

    protected void btnCancelarModelo_Click(object sender, EventArgs e)
    {
        LimpiarFormModelo();
        OcultarAlerta(pnlMensajeModelo);
    }

    private void LimpiarFormModelo()
    {
        txtCodModelo.Text = "";
        txtCodModelo.Enabled = true;
        txtFabricante.Text = "";
        txtNombreModelo.Text = "";
        txtTipoModelo.Text = "";
        txtCategoria.Text = "";
        txtPasajeros.Text = "0";
        txtEquipaje.Text = "0.00";
        hfModoEdicionModelo.Value = "false";
        lblTituloFormModelo.InnerText = "Registrar Modelo";
    }

    // ==========================================
    // FASE 2: AVIONES FÍSICOS
    // ==========================================
    private void CargarComboModelos(DropDownList ddl)
    {
        try
        {
            ddl.DataSource = _modeloService.ObtenerTodos();
            ddl.DataTextField = "NombreCompleto"; // Usa la propiedad auxiliar que concatena Fabricante - Nombre
            ddl.DataValueField = "CodModelo";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("-- Seleccione --", ""));
        }
        catch { /* Controlado silenciosamente */ }
    }

    private void CargarGrillaAviones()
    {
        try
        {
            gvAviones.DataSource = _avionService.ObtenerTodos();
            gvAviones.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(pnlMensajeAvion, lblMensajeAvion, ex.Message, true);
        }
    }

    protected void btnGuardarAvion_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = hfModoEdicionAvion.Value == "false";
            Avion a = new Avion
            {
                CodInterno = txtCodInterno.Text.Trim(),
                Matricula = txtMatricula.Text.Trim(),
                CodModelo = ddlModeloAvion.SelectedValue,
                Estado = txtEstadoAvion.Text.Trim(),
                FechaIncorporacion = string.IsNullOrWhiteSpace(txtFechaInc.Text) ? DateTime.Now : DateTime.Parse(txtFechaInc.Text)
            };

            if (!string.IsNullOrWhiteSpace(txtUltimoMant.Text))
                a.UltimoMantenimiento = DateTime.Parse(txtUltimoMant.Text);
            if (!string.IsNullOrWhiteSpace(txtProxMant.Text))
                a.ProximoMantenimiento = DateTime.Parse(txtProxMant.Text);

            _avionService.Guardar(a, esNuevo);
            
            MostrarAlerta(pnlMensajeAvion, lblMensajeAvion, "Avión guardado correctamente.", false);
            LimpiarFormAvion();
            CargarGrillaAviones();
        }
        catch (Exception ex)
        {
            MostrarAlerta(pnlMensajeAvion, lblMensajeAvion, ex.Message, true);
        }
    }

    protected void gvAviones_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string cod = e.CommandArgument.ToString();
        if (e.CommandName == "EditarAvion")
        {
            Avion a = _avionService.ObtenerPorId(cod);
            if (a != null)
            {
                txtCodInterno.Text = a.CodInterno;
                txtCodInterno.Enabled = false;
                txtMatricula.Text = a.Matricula;
                ddlModeloAvion.SelectedValue = a.CodModelo;
                txtEstadoAvion.Text = a.Estado;
                txtFechaInc.Text = a.FechaIncorporacion.ToString("yyyy-MM-dd");
                
                txtUltimoMant.Text = a.UltimoMantenimiento.HasValue ? a.UltimoMantenimiento.Value.ToString("yyyy-MM-dd") : "";
                txtProxMant.Text = a.ProximoMantenimiento.HasValue ? a.ProximoMantenimiento.Value.ToString("yyyy-MM-dd") : "";

                hfModoEdicionAvion.Value = "true";
                lblTituloFormAvion.InnerText = "Editar Avión";
                OcultarAlerta(pnlMensajeAvion);
            }
        }
        else if (e.CommandName == "EliminarAvion")
        {
            try
            {
                _avionService.Eliminar(cod);
                MostrarAlerta(pnlMensajeAvion, lblMensajeAvion, "Avión retirado de flota.", false);
                LimpiarFormAvion();
                CargarGrillaAviones();
            }
            catch (Exception ex)
            {
                MostrarAlerta(pnlMensajeAvion, lblMensajeAvion, ex.Message, true);
            }
        }
    }

    protected void btnCancelarAvion_Click(object sender, EventArgs e)
    {
        LimpiarFormAvion();
        OcultarAlerta(pnlMensajeAvion);
    }

    private void LimpiarFormAvion()
    {
        txtCodInterno.Text = "";
        txtCodInterno.Enabled = true;
        txtMatricula.Text = "";
        ddlModeloAvion.SelectedIndex = 0;
        txtEstadoAvion.Text = "";
        txtFechaInc.Text = "";
        txtUltimoMant.Text = "";
        txtProxMant.Text = "";
        hfModoEdicionAvion.Value = "false";
        lblTituloFormAvion.InnerText = "Registrar Avión";
    }

    // ==========================================
    // FASE 3: MATRIZ DE COMPATIBILIDAD (N:M)
    // ==========================================
    private void CargarComboAeropuertos()
    {
        try
        {
            ddlAeropuertoCompat.DataSource = _aeropuertoService.ObtenerTodos();
            ddlAeropuertoCompat.DataTextField = "Nombre";
            ddlAeropuertoCompat.DataValueField = "CodAeropuerto";
            ddlAeropuertoCompat.DataBind();
            ddlAeropuertoCompat.Items.Insert(0, new ListItem("-- Seleccione Aeropuerto --", ""));
        }
        catch { /* Si ubicaciones no está configurado, quedará vacío */ }
    }

    private void CargarGrillaCompatibilidad()
    {
        try
        {
            gvCompat.DataSource = _compatService.ObtenerTodas();
            gvCompat.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(pnlMensajeCompat, lblMensajeCompat, ex.Message, true);
        }
    }

    protected void btnGuardarCompat_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = hfModoEdicionCompat.Value == "false";
            CompatibilidadAeropuertoModelo c = new CompatibilidadAeropuertoModelo
            {
                CodAeropuerto = ddlAeropuertoCompat.SelectedValue,
                CodModelo = ddlModeloCompat.SelectedValue,
                Restricciones = txtRestricciones.Text.Trim()
            };

            _compatService.Guardar(c, esNuevo);
            
            MostrarAlerta(pnlMensajeCompat, lblMensajeCompat, "Configuración guardada.", false);
            LimpiarFormCompat();
            CargarGrillaCompatibilidad();
        }
        catch (Exception ex)
        {
            MostrarAlerta(pnlMensajeCompat, lblMensajeCompat, ex.Message, true);
        }
    }

    protected void gvCompat_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditarCompat" || e.CommandName == "EliminarCompat")
        {
            // El CommandArgument contiene "CodAeropuerto|CodModelo"
            string[] codigos = e.CommandArgument.ToString().Split('|');
            if (codigos.Length != 2) return;
            string codA = codigos[0];
            string codM = codigos[1];

            if (e.CommandName == "EditarCompat")
            {
                CompatibilidadAeropuertoModelo c = _compatService.ObtenerPorId(codA, codM);
                if (c != null)
                {
                    ddlAeropuertoCompat.SelectedValue = c.CodAeropuerto;
                    ddlModeloCompat.SelectedValue = c.CodModelo;
                    txtRestricciones.Text = c.Restricciones;
                    
                    ddlAeropuertoCompat.Enabled = false;
                    ddlModeloCompat.Enabled = false;
                    hfModoEdicionCompat.Value = "true";
                    lblTituloFormCompat.InnerText = "Editar Restricción";
                    OcultarAlerta(pnlMensajeCompat);
                }
            }
            else if (e.CommandName == "EliminarCompat")
            {
                try
                {
                    _compatService.Eliminar(codA, codM);
                    MostrarAlerta(pnlMensajeCompat, lblMensajeCompat, "Vínculo eliminado.", false);
                    LimpiarFormCompat();
                    CargarGrillaCompatibilidad();
                }
                catch (Exception ex)
                {
                    MostrarAlerta(pnlMensajeCompat, lblMensajeCompat, ex.Message, true);
                }
            }
        }
    }

    protected void btnCancelarCompat_Click(object sender, EventArgs e)
    {
        LimpiarFormCompat();
        OcultarAlerta(pnlMensajeCompat);
    }

    private void LimpiarFormCompat()
    {
        ddlAeropuertoCompat.SelectedIndex = 0;
        ddlModeloCompat.SelectedIndex = 0;
        ddlAeropuertoCompat.Enabled = true;
        ddlModeloCompat.Enabled = true;
        txtRestricciones.Text = "";
        hfModoEdicionCompat.Value = "false";
        lblTituloFormCompat.InnerText = "Configurar Compatibilidad";
    }
}
