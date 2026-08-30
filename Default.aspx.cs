using System;
using System.Web.UI;

public partial class _Default : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
    }

    protected void btnUser_Click(object sender, EventArgs e)
    {
        Session["Role"] = "User";
        Response.Redirect("~/Pages/Cliente/Compra.aspx"); // Redirige al flujo de compra web
    }

    protected void btnAdmin_Click(object sender, EventArgs e)
    {
        Session["Role"] = "Admin";
        Response.Redirect("~/Pages/Flota.aspx");
    }
}