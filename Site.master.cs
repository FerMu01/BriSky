using System;
using System.Web.UI;

public partial class SiteMaster : MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        // AppRelativeVirtualPath siempre devuelve el archivo físico exacto (~/Default.aspx) sin importar cómo configuró IIS la URL.
        bool isDefaultPage = this.Page.AppRelativeVirtualPath.Equals("~/default.aspx", StringComparison.OrdinalIgnoreCase);
        
        if (isDefaultPage)
        {
            sidebar.Visible = false;
            topNavbar.Visible = false;
        }
        else
        {
            if (Session["Role"] != null && Session["Role"].ToString() == "User")
            {
                // Modo Usuario: Ocultar sidebar por completo y mostrar botón para volver
                sidebar.Visible = false;
                sidebarCollapse.Visible = false;
                btnVolverInicio.Visible = true;
            }
            else
            {
                // Modo Admin: Mostrar todo y el botón para volver
                sidebar.Visible = true;
                sidebarCollapse.Visible = true;
                btnVolverInicio.Visible = true;
            }
        }
    }

    protected void btnVolverInicio_Click(object sender, EventArgs e)
    {
        Session.Clear(); // Limpiamos para obligar a elegir rol de nuevo
        Response.Redirect("~/Default.aspx");
    }
}