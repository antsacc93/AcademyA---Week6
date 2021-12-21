using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week6.ADODisconected.SupportLibrary
{
    public static class SupportDisconnected
    {
        public static SqlDataAdapter InitMovieDataSetAndAdapter(DataSet movieDs, SqlConnection conn)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();

            //SELECT
            adapter.SelectCommand = GenerateSelectCommand(conn);
            //INSERT
            adapter.InsertCommand = GenerateInsertCommand(conn);
            //UPDATE
            adapter.UpdateCommand = GenerateUpdateCommand(conn);
            //DELETE
            adapter.DeleteCommand = GenerateDeleteCommand(conn);

            //primo parametro: dataset -- secondo parametro: nome della tabella nel db
            adapter.Fill(movieDs, "Movie");

            return adapter;
        }

        private static SqlCommand GenerateDeleteCommand(SqlConnection conn)
        {
            SqlCommand deleteCommand = new SqlCommand();
            deleteCommand.Connection = conn;
            deleteCommand.CommandType = CommandType.Text;
            deleteCommand.CommandText = "DELETE FROM MOVIE WHERE ID = @id";

            deleteCommand.Parameters.Add(new SqlParameter(
                "@id", SqlDbType.Int, 0, "ID"));

            return deleteCommand;
        }

        private static SqlCommand GenerateUpdateCommand(SqlConnection conn)
        {
            SqlCommand updateMovieCommand = new SqlCommand();
            updateMovieCommand.Connection = conn;
            updateMovieCommand.CommandType = CommandType.Text;
            updateMovieCommand.CommandText = "UPDATE Movie SET Title = @title, " +
                "Genre = @genre, Duration = @duration WHERE ID = @id";

            updateMovieCommand.Parameters.Add(new SqlParameter(
                "@title", SqlDbType.NVarChar, 50, "title"
                ));
            updateMovieCommand.Parameters.Add(new SqlParameter(
                "@genre", SqlDbType.NVarChar, 20, "Genre"));
            updateMovieCommand.Parameters.Add(new SqlParameter(
                "@duration", SqlDbType.Int, 0, "duration"));
            updateMovieCommand.Parameters.Add(new SqlParameter(
                "@id", SqlDbType.Int, 0, "ID"));

            return updateMovieCommand;
        }

        private static SqlCommand GenerateInsertCommand(SqlConnection conn)
        {
            SqlCommand movieInsertCommand = new SqlCommand();
            movieInsertCommand.Connection = conn;
            movieInsertCommand.CommandType = CommandType.Text;
            movieInsertCommand.CommandText = "INSERT INTO Movie VALUES (@title, @genre, @duration)";

            movieInsertCommand.Parameters.Add(new SqlParameter(
                "@title", SqlDbType.NVarChar, 50, "Title"
            ));
            movieInsertCommand.Parameters.Add(new SqlParameter(
                "@genre", SqlDbType.NVarChar, 20, "Genre"
            ));
            movieInsertCommand.Parameters.Add(new SqlParameter(
               "@duration", SqlDbType.Int, 0, "Duration"
            ));

            return movieInsertCommand;
        }

        private static SqlCommand GenerateSelectCommand(SqlConnection conn)
        {
            SqlCommand movieSelectCommand = new SqlCommand();
            movieSelectCommand.Connection = conn;
            movieSelectCommand.CommandType = CommandType.Text;
            movieSelectCommand.CommandText = "SELECT * FROM Movie";

            return movieSelectCommand;
        }
    }
}
