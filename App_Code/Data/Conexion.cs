using System.Configuration;
using System.Data.SqlClient;

public static class Conexion
{
    public static System.Data.SqlClient.SqlConnection GetConnection()
    {
        var cs = ConfigurationManager.ConnectionStrings["BriSkyDB"]?.ConnectionString;
        return new System.Data.SqlClient.SqlConnection(cs);
    }
}
