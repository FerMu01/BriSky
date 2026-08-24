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
FROM sys.sql_modules sm
JOIN sys.objects o ON sm.object_id = o.object_id
WHERE o.name IN ('crear_piloto', 'crear_tripulante_cabina', 'crear_empleado_oficina')";
                
                using (var cmd = new SqlCommand(q, cn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) 
                    {
                        Console.WriteLine("-------------------------");
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
