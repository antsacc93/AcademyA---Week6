using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week6.ADO.Demo
{
    static class ADOConnectedDemo
    {
        static string ConnectionString = @"Server=(localdb)\mssqllocaldb;Database=CinemaDB;Trusted_Connection=True;";

        #region PROVA CONNESSIONE
        public static void ConnectionDemo()
        {
            using SqlConnection connection = new SqlConnection(ConnectionString);
            connection.Open();

            if (connection.State == System.Data.ConnectionState.Open)
                Console.WriteLine("Connessi al DB");
            else
                Console.WriteLine("NON connessi al DB");

            connection.Close();
        }


        #endregion

        #region READ
        public static void DataReaderDemo()
        {
            //CREO LA CONNESSIONE
            using SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                //OPERAZIONI DA ESEGUIRE SUL DATABASE
                conn.Open();
                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connessi al DB");
                else
                    Console.WriteLine("Non connessi al DB");

                //ISTANZIARE UN SQLCOMMAND (1)
                SqlCommand readCommand = new SqlCommand();
                readCommand.Connection = conn;
                readCommand.CommandType = System.Data.CommandType.Text;
                readCommand.CommandText = "SELECT * FROM MOVIE"; //Sql Statement

                //MODALITA' EQUIVALENTE DI INSTANZIAMENTO DEL COMANDO (2)
                string sqlStatement = "SELECT * FROM MOVIE";
                SqlCommand readCommand2 = new SqlCommand(sqlStatement, conn);

                //MADALITA' N. (3)
                SqlCommand readCommand3 = conn.CreateCommand();
                readCommand3.CommandText = sqlStatement;

                SqlDataReader reader = readCommand3.ExecuteReader();
                Console.WriteLine("--- MOVIES ----");
                while (reader.Read())
                {
                    Console.WriteLine("{0} - {1} {2} {3}",
                        reader["ID"],
                        reader["Title"],
                        reader["Genre"],
                        reader["Duration"]);
                }

            }
            catch( Exception ex)
            {
                Console.WriteLine($"Errore :{ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }


        #endregion

        #region INSERT
        public static void InsertDemo()
        {
            using SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                var title = "Forrest Gump";
                var genre = "Drammatico";
                var duration = 160;

                //INSERT INTO MOVIE VALUES ('Forrest Gump', 'Drammatico', 160)
                string insertSqlStatement = "INSERT INTO Movie " +
                    "VALUES ('" + title + "', '" + genre + "', " + duration + ")";

                SqlCommand insertCommand = conn.CreateCommand();
                insertCommand.CommandText = insertSqlStatement;

                int result = insertCommand.ExecuteNonQuery();
                if (result == 1)
                    Console.WriteLine("Record inserito con successo!");
                else
                    Console.WriteLine("OOOPS.. è successo qualcosa");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error {ex.Message}");
            }
            finally
            {
                conn.Close();
            }

        }

        #endregion

        #region INSERT WITH PARAMETER
        public static void InsertWithParameter()
        {
            using SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                if (conn.State == System.Data.ConnectionState.Open)
                    Console.WriteLine("Connesso al DB");
                else
                    Console.WriteLine("NON connesso al DB");

                var title = "Matrix";
                var genre = "Fantascienza";
                var duration = 155;

                string insertSqlStatement = "INSERT INTO Movie VALUES (@title, @genre, @duration)";

                SqlCommand insertCommand = conn.CreateCommand();
                insertCommand.CommandText = insertSqlStatement;

                //METODO 1
                insertCommand.Parameters.AddWithValue("@title", title);

                //METODO 2
                SqlParameter genreParameter = new SqlParameter();
                genreParameter.ParameterName = "@genre";
                genreParameter.Value = genre;
                genreParameter.DbType = System.Data.DbType.String;
                genreParameter.Size = 50;
                insertCommand.Parameters.Add(genreParameter);

                insertCommand.Parameters.AddWithValue("@duration", duration);
                int result = insertCommand.ExecuteNonQuery();

                if (result == 1)
                    Console.WriteLine("Inserimento avvenuto con successo");
                else
                    Console.WriteLine("Errore di inserimento");

            }
            catch (SqlException ex)
            {
                Console.WriteLine($"SQL Error: {ex.Message}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Generic Error {ex.Message}");
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion

        #region DELETE WITH PARAMETER
        public static void DeleteWithParameter()
        {
            using SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                string deleteSqlStatement = "DELETE FROM Movie WHERE ID = @id";
                SqlCommand deleteCommand = conn.CreateCommand();

                SqlParameter idParameter = new SqlParameter();
                idParameter.ParameterName = "@id";
                idParameter.Value = 5;
                idParameter.DbType = System.Data.DbType.Int32;
                deleteCommand.Parameters.Add(idParameter);

                deleteCommand.CommandText = deleteSqlStatement;

                int result = deleteCommand.ExecuteNonQuery();
                if (result == 1)
                    Console.WriteLine("Cancellazione avvenuta con successo");
                else
                    Console.WriteLine("Errore");
            }
            catch(SqlException ex)
            {

            }catch(Exception ex)
            {

            }
            finally
            {
                conn.Close();
            }
        }
        #endregion

        #region SCALAR DEMO
        public static void ExecuteScalarDemo()
        {
            using SqlConnection conn = new SqlConnection(ConnectionString);

            try
            {
                conn.Open();

                SqlCommand scalarCommand = new SqlCommand();
                scalarCommand.Connection = conn;
                scalarCommand.CommandType = System.Data.CommandType.Text;
                scalarCommand.CommandText = "SELECT COUNT(*) FROM Movie";

                //SqlCommand scalarCommand2 = new SqlCommand(
                //    "SELECT COUNT(*) FROM Movie",
                //    conn
                //);

                int count = (int)scalarCommand.ExecuteScalar();

                Console.WriteLine($"Il DB contiene {count} film");
            }
            catch(SqlException ex)
            {
                Console.WriteLine($"SQL Error {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally {
                conn.Close(); 
            }
        }

        #endregion

        #region MULTIPLE QUERY

        public static void MultipleQueryDemo()
        {
            using SqlConnection conn = new SqlConnection(ConnectionString);
            try
            {
                conn.Open();
                string sqlStatement = "SELECT * FROM Movie; SELECT * FROM Actor;";

                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = conn;
                sqlCommand.CommandText = sqlStatement;
                sqlCommand.CommandType = System.Data.CommandType.Text;

                SqlDataReader reader = sqlCommand.ExecuteReader();
                int idx = 0;
                while (reader.HasRows)
                {
                    Console.WriteLine($" ---- Result set n. {idx + 1} --- ");
                    while (reader.Read())
                    {
                        Console.WriteLine($"[ {reader["ID"]} ] - {reader.GetString(1)}");
                    }
                    reader.NextResult();
                    idx++;
                    Console.WriteLine("------------");
                    Console.WriteLine();
                }


            }catch (SqlException ex)
            {
                Console.WriteLine(ex.Message);
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                conn.Close();
            }
        }

        #endregion
    }
}
