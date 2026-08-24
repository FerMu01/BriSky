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
                
                Console.WriteLine("\nColumns in brisky.empleado:");
                using (var cmd = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'empleado'", cn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) Console.WriteLine(reader[0]);
                }

                Console.WriteLine("\nColumns in brisky.tripulante:");
                using (var cmd = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'tripulante'", cn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) Console.WriteLine(reader[0]);
                }
                
                Console.WriteLine("\nColumns in brisky.piloto:");
                using (var cmd = new SqlCommand("SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'piloto'", cn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) Console.WriteLine(reader[0]);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("ERROR: " + ex.Message);
        }
    }
}
