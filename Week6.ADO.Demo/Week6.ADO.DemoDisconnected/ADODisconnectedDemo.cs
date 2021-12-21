using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week6.ADODisconected.SupportLibrary;

namespace Week6.ADO.DemoDisconnected
{
    static class ADODisconnectedDemo
    {
        static IConfiguration config = new ConfigurationBuilder()
            .SetBasePath(@"C:\Users\AntoniaSacchitella\Desktop\Week 6\Week6.ADO.Demo\Week6.ADO.DemoDisconnected")
            .AddJsonFile("appsettings.json")
            .Build();

        static string ConnectionString = config.GetConnectionString("CinemaDBDemo");

        public static void FillDataSet()
        {
            DataSet movieDs = new DataSet();

            using SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                if(conn.State == ConnectionState.Open)
                {
                    Console.WriteLine("Connessione stabilita");
                }
                else
                {
                    Console.WriteLine("Connessione non riuscita");
                    return;
                }

                //OPERAZIONI DI RECUPERO DEI DATI
                SupportDisconnected.InitMovieDataSetAndAdapter(movieDs, conn);

                conn.Close();
                //lavoro in modalità disconnessa
                Console.WriteLine("=== Movie List ===");
                //STAMPA DEI DATI IN LOCALE
                foreach (DataRow row in movieDs.Tables["Movie"].Rows)
                {
                    Console.WriteLine($"[ {row["Id"]} ] Title: {row["Title"]}" +
                        $"Genre: {row["Genre"]} Duration: {row["Duration"]}");
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Errror: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Generic Error: {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }
    }
}
