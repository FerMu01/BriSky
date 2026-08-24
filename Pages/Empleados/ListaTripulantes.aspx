<%@ Page Title="Lista de Tripulantes" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Tripulantes</h2>
    <asp:GridView ID="gvTripulantes" runat="server" AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField DataField="CodEmpleado" HeaderText="ID" />
            <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
            <asp:BoundField DataField="Apellido" HeaderText="Apellido" />
            <asp:BoundField DataField="Licencia" HeaderText="Licencia" />
            <asp:BoundField DataField="HorasVuelo" HeaderText="Horas Vuelo" />
        </Columns>
    </asp:GridView>

    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarTripulantes();
            }
        }

        private void CargarTripulantes()
        {
            var dao = new EmpleadoDAO();
            var lista = dao.ObtenerTripulantes();
            gvTripulantes.DataSource = lista;
            gvTripulantes.DataBind();
        }
    </script>
</asp:Content>
