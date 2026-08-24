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
SELECT sm.definition
FROM sys.triggers t
INNER JOIN sys.sql_modules sm ON t.object_id = sm.object_id
WHERE t.parent_id = OBJECT_ID('brisky.empleado')
";
                
                using (var cmd = new SqlCommand(q, cn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        Console.WriteLine("----------------");
                        Console.WriteLine(reader[0]);
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
