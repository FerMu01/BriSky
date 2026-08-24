using System;
using System.Data.SqlClient;

class Program
{
    static void Main()
    {
        string cs = "Data Source=FERNANDO;Initial Catalog=BriSkyDB;Integrated Security=True;";
        try
        {
            using (var cn = new SqlConnection(cs))
            {
                cn.Open();
                
                string q = @"
SELECT TABLE_NAME, COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME IN ('empleado', 'tripulante', 'piloto')
ORDER BY TABLE_NAME, ORDINAL_POSITION";
                
                using (var cmd = new SqlCommand(q, cn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        Console.WriteLine(string.Format("{0}.{1} ({2})", reader[0], reader[1], reader[2]));
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
        }
    }
}
