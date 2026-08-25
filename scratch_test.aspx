<%@ Page Language="C#" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="BriSky.Data" %>

<%
    try
    {
        using (var con = Conexion.GetConnection())
        {
            con.Open();
            using (var cmd = new SqlCommand("SELECT TOP 1 * FROM brisky.v_empleado_completo", con))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Response.Write(reader.GetName(i) + "<br/>");
                    }
                }
            }
        }
    }
    catch (Exception ex)
    {
        Response.Write("Error: " + ex.Message);
    }
%>
