<%@ Page Title="Crear Empleado" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Crear Empleado</h2>
    <asp:Label ID="lblMsg" runat="server" EnableViewState="false" />
    <table>
        <tr><td>Código empleado</td><td><asp:TextBox ID="txtCodigo" runat="server" Enabled="false" /></td></tr>
        <tr><td>Nombre</td><td><asp:TextBox ID="txtNombre" runat="server" /></td></tr>
        <tr><td>Apellido</td><td><asp:TextBox ID="txtApellido" runat="server" /></td></tr>
        <tr><td>Documento</td><td><asp:TextBox ID="txtDocumento" runat="server" /></td></tr>
        <tr><td>Teléfono</td><td><asp:TextBox ID="txtTelefono" runat="server" /></td></tr>
        <tr><td>Correo</td><td><asp:TextBox ID="txtCorreo" runat="server" /></td></tr>
        <tr><td>Fecha ingreso</td><td><asp:TextBox ID="txtFechaIngreso" runat="server" /></td></tr>
        <tr><td>Área (id)</td><td><asp:TextBox ID="txtArea" runat="server" /></td></tr>
        <tr><td>Estado laboral</td><td><asp:CheckBox ID="chkEstado" runat="server" /></td></tr>
        <tr><td>Tipo empleado</td><td>
            <asp:DropDownList ID="ddlTipo" runat="server">
                <asp:ListItem Value="Empleado">Empleado</asp:ListItem>
                <asp:ListItem Value="Tripulante">Tripulante</asp:ListItem>
                <asp:ListItem Value="Piloto">Piloto</asp:ListItem>
            </asp:DropDownList>
        </td></tr>
        <tr><td colspan="2"><asp:Button ID="btnGuardar" runat="server" Text="Guardar empleado" OnClick="btnGuardar_Click" /></td></tr>
    </table>

    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                Empleado emp;
                var tipo = ddlTipo.SelectedValue;

                if (tipo == "Piloto")
                {
                    var p = new Piloto();
                    p.HorasVuelo = 0; // valor por defecto o campo adicional si se añade al formulario
                    p.Licencia = txtDocumento.Text; // ejemplo: asignar documento como licencia temporal
                    emp = p;
                }
                else if (tipo == "Tripulante")
                {
                    var t = new Tripulante();
                    t.Licencia = txtDocumento.Text; // asignación por defecto
                    emp = t;
                }
                else
                {
                    emp = new Empleado();
                }

                emp.Nombre = txtNombre.Text;
                emp.Apellido = txtApellido.Text;
                emp.Documento = txtDocumento.Text;
                emp.Telefono = txtTelefono.Text;
                emp.Correo = txtCorreo.Text;
                emp.FechaIngreso = DateTime.TryParse(txtFechaIngreso.Text, out var dt) ? dt : DateTime.MinValue;
                emp.EstadoLaboral = chkEstado.Checked;
                emp.Area = string.IsNullOrEmpty(txtArea.Text) ? null : new Area { Id = txtArea.Text };

                var service = new EmpleadoService();
                var id = service.CrearEmpleadoCompleto(emp, tipo);
                if (!string.IsNullOrEmpty(id))
                {
                    lblMsg.Text = "Empleado creado con ID: " + id;
                    txtCodigo.Text = id;
                }
                else
                {
                    lblMsg.Text = "No se pudo crear el empleado.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "ERROR: " + Server.HtmlEncode(ex.Message);
            }
        }
    </script>
</asp:Content>
