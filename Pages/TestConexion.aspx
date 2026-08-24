<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Prueba de conexión</h2>
    <asp:Literal ID="litResultado" runat="server" />

    <script runat="server">
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (var cn = Conexion.GetConnection())
                {
                    cn.Open();
                    litResultado.Text = "<span style='color:green;font-weight:bold;'>CONEXIÓN EXITOSA</span>";
                }
            }
            catch (System.Exception ex)
            {
                litResultado.Text = "<span style='color:red;font-weight:bold;'>ERROR: " + System.Web.HttpUtility.HtmlEncode(ex.Message) + "</span>";
            }
        }
    </script>
</asp:Content>
