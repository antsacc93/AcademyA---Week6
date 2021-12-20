using System;
using System.Data.SqlClient;
using Week6.ADO.Esercitazioni.Lib;

namespace Week6.ADO.Esercitazione1
{
    internal class ConnectedModeClient
    {
        static string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=Ticketing;Trusted_Connection=True;";
        internal static void ListTickets(bool prompt = true)
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                connection.Open();
                SqlCommand selectCommand = connection.CreateCommand();
                selectCommand.CommandType = System.Data.CommandType.Text;
                selectCommand.CommandText = "SELECT * FROM Ticket";

                SqlDataReader reader = selectCommand.ExecuteReader();

                Console.Clear();
                Console.WriteLine("---- Elenco Tickets ----");
                Console.WriteLine();
                Console.WriteLine("{0,-5}{1,-40}{2,10}{3,20}", "ID", "Descrizione", "Stato", "Creato");
                Console.WriteLine(new String('-', 75));
                while (reader.Read())
                {
                    string formattedDate = ((DateTime)reader["Insert_date"]).ToString("dd-MMM-yyyy");
                    Console.WriteLine("{0,-5}{1,-40}{2,10}{3,20}", reader["Id"], reader["Description"], reader["Status"], formattedDate);
                }
                Console.WriteLine(new String('-', 75));
            }
            catch (Exception ex)
            {
                Console.Write($"Errore: {ex.Message}");
            }
            finally
            {
                connection.Close();
                if (prompt)
                {
                    Console.WriteLine("---- Premi un tasto ----");
                    Console.ReadKey();
                }
            }
        }

        internal static void AddTicket()
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                connection.Open();
                SqlCommand insertCommand = connection.CreateCommand();
                insertCommand.CommandType = System.Data.CommandType.Text;
                insertCommand.CommandText = "INSERT INTO Ticket VALUES(@descrizione, @data, @utente, @stato)";

                Console.Clear();
                Console.WriteLine("---- Inserire un nuovo Ticket ----");

                string descrizione = ConsoleHelpers.GetData("Descrizione");
                insertCommand.Parameters.AddWithValue("@descrizione", descrizione);

                insertCommand.Parameters.AddWithValue("@data", DateTime.Now);

                string utente = ConsoleHelpers.GetData("Utente");
                insertCommand.Parameters.AddWithValue("@utente", utente);

                insertCommand.Parameters.AddWithValue("@stato", "New");

                int result = insertCommand.ExecuteNonQuery();

                if (result != 1)
                    Console.WriteLine("Si è verificato un problema nell'inserimento del ticket.");
                else
                    Console.WriteLine("Ticket aggiunto.");
            }
            catch (Exception ex)
            {
                Console.Write($"Errore: {ex.Message}");
            }
            finally
            {
                connection.Close();
                Console.WriteLine("---- Premi un tasto ----");
                Console.ReadKey();
            }
        }

        internal static void DeleteTicket()
        {
            ListTickets(false);
            using SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                connection.Open();
                SqlCommand deleteCommand = connection.CreateCommand();
                deleteCommand.CommandType = System.Data.CommandType.Text;
                deleteCommand.CommandText = "DELETE FROM Ticket WHERE ID = @id";

                Console.WriteLine();
                string idValue = ConsoleHelpers.GetData("ID del ticket da cancellare");

                deleteCommand.Parameters.AddWithValue("@id", idValue);

                int result = deleteCommand.ExecuteNonQuery();

                if (result != 1)
                    Console.WriteLine("Si è verificato un problema nella cancellazione del ticket.");
                else
                    Console.WriteLine("Ticket cancellato.");
            }
            catch (Exception ex)
            {
                Console.Write($"Errore: {ex.Message}");
            }
            finally
            {
                connection.Close();
                Console.WriteLine("---- Premi un tasto ----");
                Console.ReadKey();
            }
        }
    }
}