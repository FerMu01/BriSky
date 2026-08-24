using System.Configuration;

public static class Conexion
{
    public static System.Data.SqlClient.SqlConnection GetConnection()
    {
        var csObj = ConfigurationManager.ConnectionStrings["BriSkyDB"];
        return new System.Data.SqlClient.SqlConnection(csObj != null ? csObj.ConnectionString : "");
    }
}
