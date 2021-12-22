using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week6.ADO.Esercitazioni.Lib;

namespace Week6.ADO.Esercitazione2
{
    public class DisconnectedModeClient
    {
        static string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=Ticketing;Trusted_Connection=True;";
        #region Private members

        private static SqlConnection connection;
        private static DataSet ticketDs = new DataSet();
        private static SqlDataAdapter ticketAdapter = new SqlDataAdapter();

        private static SqlCommand ticketSelectCmd;
        private static SqlCommand ticketInsertCmd;
        private static SqlCommand ticketDeleteCmd;

        #endregion

        static DisconnectedModeClient()
        {
            connection = new SqlConnection(ConnectionString);

            try
            {
                ticketDs = new DataSet();
                ticketAdapter = new SqlDataAdapter();

                #region Select Command

                ticketSelectCmd = new SqlCommand("SELECT * FROM Ticket ORDER BY Insert_date DESC", connection);
                ticketAdapter.SelectCommand = ticketSelectCmd;

                #endregion

                #region Insert Command

                ticketInsertCmd = connection.CreateCommand();
                ticketInsertCmd.CommandText = "INSERT INTO Ticket VALUES(@descrizione, @data, @utente, @stato)";

                ticketInsertCmd.Parameters.Add(
                    new SqlParameter(
                        "@descrizione",
                        SqlDbType.NVarChar,
                        100,
                        "Description"
                    )
                );
                ticketInsertCmd.Parameters.Add(
                    new SqlParameter(
                        "@data",
                        SqlDbType.DateTime,
                        100,
                        "Insert_date"
                    )
                );
                ticketInsertCmd.Parameters.Add(
                    new SqlParameter(
                        "@utente",
                        SqlDbType.NVarChar,
                        100,
                        "username"
                    )
                );
                ticketInsertCmd.Parameters.Add(
                    new SqlParameter(
                        "@stato",
                        SqlDbType.NVarChar,
                        10,
                        "Status"
                    )
                );
                

                ticketAdapter.InsertCommand = ticketInsertCmd;

                #endregion

                #region Delete Command

                ticketDeleteCmd = connection.CreateCommand();
                ticketDeleteCmd.CommandType = System.Data.CommandType.Text;
                ticketDeleteCmd.CommandText = "DELETE FROM Ticket WHERE ID = @id";

                ticketDeleteCmd.Parameters.Add(
                    new SqlParameter(
                        "@id",
                        SqlDbType.Int,
                        100,
                        "Id"
                    )
                );

                ticketAdapter.DeleteCommand = ticketDeleteCmd;

                #endregion

                ticketAdapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                ticketAdapter.Fill(ticketDs, "Ticket");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DisconnectedModeClient] Ctor Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }

        

        public static void ListTickets(bool prompt = true)
        {
            Console.Clear();
            Console.WriteLine("---- Elenco Tickets ----");
            Console.WriteLine();
            Console.WriteLine("{0,-5}{1,-40}{2,10}{3,20}{4,5}", "ID", "Descrizione", "Stato", "Creato", " CHG ");
            Console.WriteLine(new String('-', 80));
            foreach (DataRow dataRow in ticketDs.Tables["Ticket"].Rows)
            {
                if (dataRow.RowState == DataRowState.Deleted)
                    continue;

                string formattedDate = ((DateTime)dataRow["Insert_date"]).ToString("dd-MMM-yyyy");

                //string formattedDate1 = dataRow.Field<DateTime>("Data").ToString("dd-MMM-yyyy");

                string state = dataRow.RowState != DataRowState.Unchanged ? "  *  " : "";
                Console.WriteLine("{0,-5}{1,-40}{2,10}{3,20}{4,5}",
                    dataRow["Id"], dataRow["Description"], dataRow["Status"], formattedDate, state);
            }
            Console.WriteLine(new String('-', 80));

            if (prompt)
            {
                Console.WriteLine("---- Premi un tasto ----");
                Console.ReadKey();
            }
        }

        public static void AddTicket()
        {
            Console.Clear();
            Console.WriteLine("---- Inserire un nuovo Ticket ----");

            string descrizione = ConsoleHelpers.GetData("Description");
            string utente = ConsoleHelpers.GetData("Username");

            DataRow newRow = ticketDs.Tables["Ticket"].NewRow();
            newRow["Description"] = descrizione;
            newRow["Username"] = utente;
            newRow["Insert_date"] = DateTime.Now;
            newRow["Status"] = "New";
            ticketDs.Tables["Ticket"].Rows.Add(newRow);

            Refresh();

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

        public static void DeleteTicket()
        {
            Console.Clear();
            Console.WriteLine("---- Cancellare un Ticket ----");

            ListTickets(false);

            string idValue = ConsoleHelpers.GetData("ID del ticket da cancellare");

            DataRow rowToBeDeleted = ticketDs.Tables["Ticket"].Rows.Find(idValue);
            // marco la riga come cancellata
            rowToBeDeleted?.Delete();

            Refresh();

            Console.WriteLine("---- Premi un tasto ----");
            Console.ReadKey();
        }

        public static void Refresh()
        {
            // update db
            ticketAdapter.Update(ticketDs, "Tickets");
            // refresh ds
            ticketDs.Reset();
            ticketAdapter.Fill(ticketDs, "Tickets");
        }


    }
}
