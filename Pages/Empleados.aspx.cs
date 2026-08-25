using System;
using System.Web.UI.WebControls;
using BriSky.Models.Personal;
using BriSky.Services.Personal;
using BriSky.Services.Ubicaciones;

public partial class Pages_Empleados : System.Web.UI.Page
{
    // Áreas
    private AreaService _areaService;
    
    // Empleados y Subclases
    private EmpleadoService _empleadoService;
    private PilotoService _pilotoService;
    private EmpleadoOficinaService _oficinaService;
    private TripulanteCabinaService _cabinaService;
    private PersonalMantenimientoService _mantenimientoService;
    
    // Dependencias externas para el combo de oficinas
    private OficinaService _ubicacionesOficinaService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _areaService = new AreaService();
        _empleadoService = new EmpleadoService();
        _pilotoService = new PilotoService();
        _oficinaService = new EmpleadoOficinaService();
        _cabinaService = new TripulanteCabinaService();
        _mantenimientoService = new PersonalMantenimientoService();
        
        _ubicacionesOficinaService = new OficinaService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarAreasGrid();
            CargarComboAreas();
            CargarComboOficinasFisicas();
            CargarEmpleadosGrid();
        }
    }

    // ==========================================
    // --- MANEJO DE PESTAÑAS ---
    // ==========================================
    protected void CambiarPestaña_Click(object sender, EventArgs e)
    {
        LinkButton btn = (LinkButton)sender;
        int viewIndex = int.Parse(btn.CommandArgument);
        mvPersonal.ActiveViewIndex = viewIndex;

        btnTabAreas.CssClass = "tab-button";
        btnTabEmpleados.CssClass = "tab-button";
        btn.CssClass = "tab-button active-tab";
    }

    // ==========================================
    // --- FASE 1: LÓGICA DE ÁREAS ---
    // ==========================================
    private void CargarAreasGrid()
    {
        try
        {
            gvAreas.DataSource = _areaService.ObtenerTodas();
            gvAreas.DataBind();
        }
        catch (Exception ex)
        {
            MostrarMensajeArea("Error al cargar áreas: " + ex.Message, true);
        }
    }

    protected void btnGuardarArea_Click(object sender, EventArgs e)
    {
        try
        {
            bool esNuevo = hfModoEdicionArea.Value == "false";

            Area a = new Area();
            a.CodArea = txtCodArea.Text.Trim();
            a.Nombre = txtNombreArea.Text.Trim();
            a.Funcion = string.IsNullOrWhiteSpace(txtFuncionArea.Text) ? null : txtFuncionArea.Text.Trim();

            _areaService.Guardar(a, esNuevo);

            MostrarMensajeArea("Área guardada correctamente.", false);
            LimpiarFormularioArea();
            CargarAreasGrid();
            CargarComboAreas();
        }
        catch (Exception ex)
        {
            MostrarMensajeArea(ex.Message, true);
        }
    }

    protected void btnCancelarArea_Click(object sender, EventArgs e)
    {
        LimpiarFormularioArea();
        pnlMensajeArea.Visible = false;
    }

    protected void gvAreas_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        string codArea = e.CommandArgument.ToString();

        if (e.CommandName == "EditarArea")
        {
            try
            {
                Area a = _areaService.ObtenerPorId(codArea);
                if (a != null)
                {
                    txtCodArea.Text = a.CodArea;
                    txtCodArea.Enabled = false; 
                    txtNombreArea.Text = a.Nombre;
                    txtFuncionArea.Text = a.Funcion;
                    
                    hfModoEdicionArea.Value = "true";
                    lblTituloFormArea.InnerText = "Editar Área";
                    pnlMensajeArea.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MostrarMensajeArea("Error al cargar el área: " + ex.Message, true);
            }
        }
        else if (e.CommandName == "EliminarArea")
        {
            try
            {
                _areaService.Eliminar(codArea);
                MostrarMensajeArea("Área eliminada correctamente.", false);
                LimpiarFormularioArea();
                CargarAreasGrid();
                CargarComboAreas();
            }
            catch (Exception ex)
            {
                MostrarMensajeArea("Error al eliminar: " + ex.Message, true);
            }
        }
    }

    private void LimpiarFormularioArea()
    {
        txtCodArea.Text = "";
        txtCodArea.Enabled = true;
        txtNombreArea.Text = "";
        txtFuncionArea.Text = "";
        hfModoEdicionArea.Value = "false";
        lblTituloFormArea.InnerText = "Registrar Área";
    }

    private void MostrarMensajeArea(string mensaje, bool esError)
    {
        pnlMensajeArea.Visible = true;
        lblMensajeArea.Text = mensaje;
        pnlMensajeArea.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    // ==========================================
    // --- FASE 5: LÓGICA DE EMPLEADOS (POLIMORFISMO) ---
    // ==========================================
    private void CargarComboAreas()
    {
        try
        {
            ddlArea.DataSource = _areaService.ObtenerTodas();
            ddlArea.DataTextField = "Nombre";
            ddlArea.DataValueField = "CodArea";
            ddlArea.DataBind();
            ddlArea.Items.Insert(0, new ListItem("-- Seleccione un Área --", ""));
        }
        catch (Exception ex)
        {
            MostrarMensajeEmpleado("Error al cargar áreas para empleados: " + ex.Message, true);
        }
    }

    private void CargarComboOficinasFisicas()
    {
        try
        {
            // Usa el servicio de Ubicaciones implementado previamente
            ddlOficina.DataSource = _ubicacionesOficinaService.ObtenerTodas();
            ddlOficina.DataTextField = "Nombre";
            ddlOficina.DataValueField = "CodOficina";
            ddlOficina.DataBind();
            ddlOficina.Items.Insert(0, new ListItem("-- Seleccione una Oficina --", ""));
        }
        catch (Exception)
        {
            // Si el módulo de ubicaciones está vacío, simplemente quedará en blanco
        }
    }

    private void CargarEmpleadosGrid()
    {
        try
        {
            gvEmpleados.DataSource = _empleadoService.ObtenerTodos();
            gvEmpleados.DataBind();
        }
        catch (Exception ex)
        {
            MostrarMensajeEmpleado("Error al cargar la lista de empleados: " + ex.Message, true);
        }
    }

    // El Switch Estratégico para ocultar/mostrar paneles
    protected void ddlTipoEmpleado_SelectedIndexChanged(object sender, EventArgs e)
    {
        pnlOficina.Visible = false;
        pnlPiloto.Visible = false;
        pnlCabina.Visible = false;
        pnlMantenimiento.Visible = false;

        string tipo = ddlTipoEmpleado.SelectedValue;

        switch (tipo)
        {
            case "EMPLEADO_OFICINA":
                pnlOficina.Visible = true;
                break;
            case "PILOTO":
                pnlPiloto.Visible = true;
                break;
            case "TRIPULANTE_CABINA":
                pnlCabina.Visible = true;
                break;
            case "PERSONAL_MANTENIMIENTO":
                pnlMantenimiento.Visible = true;
                break;
        }
    }

    // Instanciación y guardado polimórfico
    protected void btnGuardarEmpleado_Click(object sender, EventArgs e)
    {
        try
        {
            string tipo = ddlTipoEmpleado.SelectedValue;
            
            if (string.IsNullOrWhiteSpace(tipo))
            {
                throw new ArgumentException("Debe seleccionar el Tipo de Empleado (Rol) para registrarlo.");
            }

            // Datos base compartidos por todos
            string codEmpleado = txtCodEmpleado.Text.Trim();
            string nombre = txtNombreEmpleado.Text.Trim();
            string apellido = txtApellidoEmpleado.Text.Trim();
            string documento = txtDocumentoEmpleado.Text.Trim();
            string telefono = string.IsNullOrWhiteSpace(txtTelefonoEmpleado.Text) ? null : txtTelefonoEmpleado.Text.Trim();
            string correo = string.IsNullOrWhiteSpace(txtCorreoEmpleado.Text) ? null : txtCorreoEmpleado.Text.Trim();
            string codArea = ddlArea.SelectedValue;
            DateTime fechaIngreso = DateTime.Now; // En este demo, la fecha es la de hoy por simplicidad.
            string estadoLaboral = "ACTIVO";

            switch (tipo)
            {
                case "PILOTO":
                    Piloto p = new Piloto();
                    LlenarCamposBase(p, codEmpleado, nombre, apellido, documento, telefono, correo, codArea, fechaIngreso, estadoLaboral);
                    p.Licencia = txtLicenciaPiloto.Text.Trim();
                    p.RangoPiloto = txtRango.Text.Trim();
                    
                    _pilotoService.Insertar(p);
                    break;

                case "EMPLEADO_OFICINA":
                    EmpleadoOficina ofi = new EmpleadoOficina();
                    LlenarCamposBase(ofi, codEmpleado, nombre, apellido, documento, telefono, correo, codArea, fechaIngreso, estadoLaboral);
                    ofi.Cargo = txtCargo.Text.Trim();
                    ofi.CodOficina = ddlOficina.SelectedValue;

                    _oficinaService.Insertar(ofi);
                    break;

                case "TRIPULANTE_CABINA":
                    TripulanteCabina cab = new TripulanteCabina();
                    LlenarCamposBase(cab, codEmpleado, nombre, apellido, documento, telefono, correo, codArea, fechaIngreso, estadoLaboral);
                    cab.Licencia = txtLicenciaCabina.Text.Trim();

                    _cabinaService.Insertar(cab);
                    break;

                case "PERSONAL_MANTENIMIENTO":
                    PersonalMantenimiento manto = new PersonalMantenimiento();
                    LlenarCamposBase(manto, codEmpleado, nombre, apellido, documento, telefono, correo, codArea, fechaIngreso, estadoLaboral);
                    manto.Especialidad = txtEspecialidad.Text.Trim();

                    _mantenimientoService.Insertar(manto);
                    break;
            }

            MostrarMensajeEmpleado("Empleado registrado correctamente.", false);
            LimpiarFormularioEmpleado();
            CargarEmpleadosGrid();
        }
        catch (Exception ex)
        {
            MostrarMensajeEmpleado(ex.Message, true);
        }
    }

    private void LlenarCamposBase(Empleado e, string cod, string nom, string ape, string doc, string tel, string cor, string area, DateTime fecha, string estado)
    {
        e.CodEmpleado = cod;
        e.Nombre = nom;
        e.Apellido = ape;
        e.Documento = doc;
        e.Telefono = tel;
        e.Correo = cor;
        e.CodArea = area;
        e.FechaIngreso = fecha;
        e.EstadoLaboral = estado;
    }

    private void LimpiarFormularioEmpleado()
    {
        txtCodEmpleado.Text = "";
        txtNombreEmpleado.Text = "";
        txtApellidoEmpleado.Text = "";
        txtDocumentoEmpleado.Text = "";
        txtTelefonoEmpleado.Text = "";
        txtCorreoEmpleado.Text = "";
        ddlArea.SelectedIndex = 0;
        
        ddlTipoEmpleado.SelectedIndex = 0;
        ddlTipoEmpleado_SelectedIndexChanged(null, null); // Ocultar todo
        
        txtCargo.Text = "";
        ddlOficina.SelectedIndex = 0;
        txtLicenciaPiloto.Text = "";
        txtRango.Text = "";
        txtLicenciaCabina.Text = "";
        txtEspecialidad.Text = "";
    }

    private void MostrarMensajeEmpleado(string mensaje, bool esError)
    {
        pnlMensajeEmpleado.Visible = true;
        lblMensajeEmpleado.Text = mensaje;
        pnlMensajeEmpleado.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }
}
