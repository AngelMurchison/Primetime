using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Primetime.Models;

namespace Primetime.Services
{
    public class GenreServices
    {
        private static string connectionStrings = @"Server=DESKTOP-577TSME\SQLEXPRESS;Database=PrimetimeDB;Trusted_Connection=True;";

        public static List<Genre> getAllGenres()
        {
            var rv = new List<Genre>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT Id, Name FROM Genre";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var Name = reader[1];

                        var genre = new Genre
                        {
                            Id = (int)id,
                            name = Name as string,
                        };
                        rv.Add(genre);
                    }
                    connection.Close();
                }
                return rv;
            }
        }

        public static void addAGenre(Genre genre)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = @"INSERT INTO Genre (Name) VALUES (@Name)";
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", genre.name);
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public static void updateAGenre(string name, int id)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $@"UPDATE Genre SET Name= @name WHERE Id=@id;";
                    cmd.Parameters.AddWithValue("name", name);
                    cmd.Parameters.AddWithValue("id", id);

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public static void deleteAGenre(Genre genre)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = @"DELETE FROM genre WHERE Id = @id;";
                    cmd.Parameters.AddWithValue("@id", genre.Id);
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public static Genre getAGenre(int id)
        {
            var rv = new Genre();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $@"SELECT Name FROM Genre WHERE Id = {id}";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var Name = reader[0];
                        rv = new Genre
                        {
                            Id = id,
                            name = Name as string,
                        };

                    }
                    connection.Close();
                }
                return rv;
            }
        }
    }
}