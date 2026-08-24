<%@ Page Title="Lista de Empleados" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Empleados</h2>
    <asp:GridView ID="gvEmpleados" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="CodEmpleado" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Documento" HeaderText="Documento" />
            <asp:BoundField DataField="Telefono" HeaderText="Teléfono" />
            <asp:BoundField DataField="Correo" HeaderText="Correo" />
        </Columns>
    </asp:GridView>

    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarEmpleados();
            }
        }

        private void CargarEmpleados()
        {
            var service = new EmpleadoService();
            var lista = service.ObtenerEmpleados();
            gvEmpleados.DataSource = lista;
            gvEmpleados.DataBind();
        }
    </script>
</asp:Content>
