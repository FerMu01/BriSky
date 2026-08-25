using System;
using System.Linq;
using System.Web.UI.WebControls;
using BriSky.Models.Operaciones;
using BriSky.Services.Operaciones;
using BriSky.Services.Flota;
using BriSky.Services.Personal;

public partial class Pages_Mantenimiento : System.Web.UI.Page
{
    private MantenimientoService _mantenimientoService;
    private AvionService _avionService;
    private EmpleadoService _empleadoService;

    protected void Page_Init(object sender, EventArgs e)
    {
        _mantenimientoService = new MantenimientoService();
        _avionService = new AvionService();
        _empleadoService = new EmpleadoService();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            CargarAviones();
            CargarMecanicos();
            CargarGrillaMantenimientos();
        }
    }

    private void MostrarAlerta(string mensaje, bool esError)
    {
        pnlMensaje.Visible = true;
        lblMensaje.Text = mensaje;
        pnlMensaje.CssClass = esError ? "alert alert-error" : "alert alert-success";
    }

    private void CargarAviones()
    {
        try
        {
            ddlAvion.DataSource = _avionService.ObtenerTodos();
            ddlAvion.DataTextField = "Matricula";
            ddlAvion.DataValueField = "CodInterno";
            ddlAvion.DataBind();
            ddlAvion.Items.Insert(0, new ListItem("-- Seleccione Avión --", ""));
        }
        catch { /* Silencioso en caso de error de conexión inicial */ }
    }

    private void CargarMecanicos()
    {
        try
        {
            // Usamos LINQ para filtrar al personal de mantenimiento como pediste
            var mecanicos = _empleadoService.ObtenerTodos()
                                            .Where(e => e.TipoEmpleado == "PERSONAL_MANTENIMIENTO")
                                            .ToList();
                                            
            ddlPersonalMantenimiento.DataSource = mecanicos;
            // Concatenamos Nombre y Apellido para mostrarlo completo
            ddlPersonalMantenimiento.DataTextField = "Nombre"; 
            ddlPersonalMantenimiento.DataValueField = "CodEmpleado";
            ddlPersonalMantenimiento.DataBind();
            
            // Para mostrar nombre completo de manera rápida y eficiente sin alterar la clase base:
            foreach (ListItem item in ddlPersonalMantenimiento.Items)
            {
                var mecanico = mecanicos.FirstOrDefault(m => m.CodEmpleado == item.Value);
                if (mecanico != null)
                {
                    item.Text = mecanico.Nombre + " " + mecanico.Apellido;
                }
            }
            
            ddlPersonalMantenimiento.Items.Insert(0, new ListItem("-- Seleccione Mecánico --", ""));
        }
        catch { }
    }

    private void CargarGrillaMantenimientos()
    {
        try
        {
            gvMantenimiento.DataSource = _mantenimientoService.ObtenerTodos();
            gvMantenimiento.DataBind();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void btnRegistrar_Click(object sender, EventArgs e)
    {
        try
        {
            Mantenimiento m = new Mantenimiento
            {
                CodMantenimiento = txtCodMantenimiento.Text.Trim(),
                CodInterno = ddlAvion.SelectedValue,
                Tipo = ddlTipo.SelectedValue,
                Descripcion = txtDescripcion.Text.Trim(),
            };

            if (!string.IsNullOrWhiteSpace(txtProximaFecha.Text))
            {
                m.ProximaFecha = DateTime.Parse(txtProximaFecha.Text);
            }

            string mecanico = ddlPersonalMantenimiento.SelectedValue;

            // Llamada al servicio que invoca el Stored Procedure
            _mantenimientoService.RealizarMantenimiento(m, mecanico);
            
            MostrarAlerta("Mantenimiento programado correctamente.", false);
            LimpiarFormulario();
            CargarGrillaMantenimientos();
        }
        catch (Exception ex)
        {
            MostrarAlerta(ex.Message, true);
        }
    }

    protected void gvMantenimiento_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "FinalizarActividad")
        {
            try
            {
                string codMantenimiento = e.CommandArgument.ToString();
                
                // Llamada al servicio que invoca el Stored Procedure
                _mantenimientoService.FinalizarMantenimiento(codMantenimiento);
                
                MostrarAlerta("Mantenimiento marcado como finalizado con éxito.", false);
                CargarGrillaMantenimientos();
            }
            catch (Exception ex)
            {
                MostrarAlerta(ex.Message, true);
            }
        }
    }

    private void LimpiarFormulario()
    {
        txtCodMantenimiento.Text = "";
        ddlAvion.SelectedIndex = 0;
        ddlTipo.SelectedIndex = 0;
        ddlPersonalMantenimiento.SelectedIndex = 0;
        txtDescripcion.Text = "";
        txtProximaFecha.Text = "";
    }
}
