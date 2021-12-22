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

        #region SELECT
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
                List<Film> movies = new List<Film>();
                foreach (DataRow row in movieDs.Tables["Movie"].Rows)
                {
                    Console.WriteLine($"[ {row["Id"]} ] Title: {row["Title"]} " +
                        $"Genre: {row["Genre"]} Duration: {row["Duration"]}");
                    movies.Add(new Film
                    {
                        Id = (int)row["Id"],
                        Title = (string)row["Title"],
                        Genre = (string)row["Genre"],
                        Duration = (int)row["Duration"]
                    });
                }
                movies.Where(x => x.Duration > 100);
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
        #endregion

        #region INSERT
        public static void InsertRowDemo()
        {
            DataSet movieDs = new DataSet();
            using SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
                connection.Open();

                SqlDataAdapter movieAdapter = SupportDisconnected.InitMovieDataSetAndAdapter(
                    movieDs, connection);

                connection.Close();

                //eseguo una modifica del dataset offline
                DataRow newRow = movieDs.Tables["Movie"].NewRow();
                newRow["Title"] = "Natale sul Nilo";
                newRow["Genre"] = "Commedia";
                newRow["Duration"] = 100;
                movieDs.Tables["Movie"].Rows.Add(newRow); // AGGIUNTA DEL FILM AL DATASET 
                                                          // (IL DB NON E' ANCORA AGGIORNATO!)
                movieAdapter.Update(movieDs, "Movie"); //aggiorna il contenuto della tabella sul db
                //senza questa istruzione non ottengo nessun salvataggio
            }
            catch( SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Generic Error: {ex.Message}");
            }
            finally {connection.Close(); }
        }
        #endregion

        #region UPDATE
        public static void UpdateRowDemo()
        {
            DataSet movieDs = new DataSet();

            using SqlConnection connection = new SqlConnection(ConnectionString);

            try
            {
               
                connection.Open();
                SqlDataAdapter movieAdapter = SupportDisconnected.InitMovieDataSetAndAdapter(
                    movieDs, connection);
                connection.Close();
                //FINE FASE CONNESSIONE
                DataRow rowToUpdate = movieDs.Tables["Movie"].Rows.Find(6);
                if(rowToUpdate != null)
                {
                    rowToUpdate["Title"] = "Natale a casa propria"; //MODIFICA SOLO IN LOCALE
                }
                movieAdapter.Update(movieDs, "Movie");
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Generic Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }
        }
        #endregion

        #region DELETE
        public static void DeleteRowDemo()
        {
            DataSet movieDs = new DataSet();
            
            using SqlConnection connection = new SqlConnection(ConnectionString);
            try
            {
                connection.Open();
                SqlDataAdapter adapter = SupportDisconnected.InitMovieDataSetAndAdapter(
                    movieDs, connection);
                connection.Close();
                DataRow rowToDelete = movieDs.Tables["Movie"].Rows.Find(6);
                if (rowToDelete != null)
                    rowToDelete.Delete(); //cancellata dal dataset (ma non dal db)
                adapter.Update(movieDs, "Movie");
            }
            catch( SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch( Exception ex)
            {
                Console.WriteLine($"Generic Error: {ex.Message}");
            }
            finally
            {
                connection.Close();
            }

        }
        #endregion

        #region MULTIPLE SELECT
        public static void MultipleSelectDemo()
        {
            DataSet dataset = new DataSet();
            using SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                SqlDataAdapter adapter = new SqlDataAdapter();

                string movieStatement = "SELECT * FROM Movie";
                adapter.SelectCommand = new SqlCommand(movieStatement, conn);
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(dataset, "Movie");
              
                string actorStatement = "SELECT * FROM Actor";
                adapter.SelectCommand = new SqlCommand(actorStatement, conn);
                adapter.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                adapter.Fill(dataset, "Actor");

                conn.Close();

                //lavoro in modalità disconessa
                foreach(DataTable dt in dataset.Tables)
                {
                    Console.WriteLine($"Nome tabella: {dt.TableName} - Numero righe: {dt.Rows.Count}");
                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }

        }
        #endregion

        #region GESTIONE DELLA CONCORRENZA
        public static void ConcorrenzaOttimistica()
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();

            //select command che preleva i film
            SqlCommand selectCommand = new SqlCommand()
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = "SELECT ID, Title FROM MOVIE"
            };

            SqlCommand updateCommand = new SqlCommand()
            {
                Connection = connection,
                CommandType = CommandType.Text,
                CommandText = "UPDATE Movie SET Title = @newTitle WHERE Title = @oldTitle"
            };

            updateCommand.Parameters.Add("@newTitle", SqlDbType.NVarChar, 50, "Title");
            var parameterSource = updateCommand.Parameters.Add("@oldTitle", SqlDbType.NVarChar, 50, "Title");
            parameterSource.SourceVersion = DataRowVersion.Original; //marchio il parametro @oldTitle come
                                                                     //spia di controllo per la concorrenza ottimistica
            adapter.SelectCommand = selectCommand;
            adapter.UpdateCommand = updateCommand;

            adapter.RowUpdated += new SqlRowUpdatedEventHandler(OnRowUpdated);

            DataSet movieDs = new DataSet();
            try
            {
                connection.Open();
                adapter.Fill(movieDs, "Movie");
                connection.Close();
                DataRow movie = movieDs.Tables["Movie"].Rows[0];

                movie["Title"] = "Il padrino";

                adapter.Update(movieDs, "Movie");
                foreach(DataRow row in movieDs.Tables["Movie"].Rows)
                {
                    if (row.HasErrors)
                    {
                        Console.WriteLine($"Modifica non accettata per {row["Title"]}");
                        Console.WriteLine(row.RowError);
                    }
                }

            }
            catch(SqlException ex)
            {
                Console.WriteLine($"Sql Error: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Generic error: {ex.Message}");

            }
        }

        public static void OnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            if(args.Errors != null)
            {
                args.Row.RowError = "Concorrenza ottimistica";
                args.Status = UpdateStatus.SkipCurrentRow;
            }
        }
        #endregion

    }
}
