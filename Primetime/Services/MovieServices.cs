using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Primetime.Models;
using System.Data.SqlClient;

namespace Primetime.Services
{
    public class MovieServices
    {
        private static string connectionStrings = @"Server=DESKTOP-577TSME\SQLEXPRESS;Database=PrimetimeDB;Trusted_Connection=True;";

        public static List<Movie> testMovies;

        public static void initializeMovies()
        {
            var passionOfTheMice = new Movie()
            {
                name = "passionOfTheMice",
                description = "These mice sure know how to party.",
                isCheckedOut = false
            };
            var passionOfTheLice = new Movie()
            {
                name = "passionOfTheLice",
                description = "You'll be itchin to watch it again!",
                isCheckedOut = false
            };
            var passionOfTheIce = new Movie()
            {
                name = "passionOfTheIce",
                description = "Two hearts of ice will never be the same.",
                isCheckedOut = false
            };
            var passionOfTheDice = new Movie()
            {
                name = "passionOfTheDice",
                description = "A high stakes affair.",
                isCheckedOut = false
            };
            var passionOfTheRice = new Movie()
            {
                name = "passionOfTheRice",
                description = "Sushi isn't the only thing getting rolled.",
                isCheckedOut = false
            };
            addAMovie(passionOfTheDice);
            addAMovie(passionOfTheIce);
            addAMovie(passionOfTheLice);
            addAMovie(passionOfTheMice);
            addAMovie(passionOfTheRice);
        }

        public static List<Movie> getAllMovies()
        {
            var rv = new List<Movie>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT Id, Name, Description, isCheckedOut FROM Movie";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var Name = reader[1];
                        var Description = reader[2];
                        var isCheckedOut = reader[3];

                        var movie = new Movie
                        {
                            Id = (int)id,
                            name = Name as string,
                            description = Description as string,
                            isCheckedOut = isCheckedOut as bool?
                        };
                        rv.Add(movie);
                    }
                    connection.Close();
                }
                return rv;
            }
        }

        public static Movie getAMovie(int id)
        {
            var rv = new Movie();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $@"SELECT Name, Description, isCheckedOut FROM Movie WHERE Id = {id}";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var Name = reader[0];
                        var Description = reader[1];
                        var isCheckedOut = reader[2];

                        var movie = new Movie
                        {
                            Id = id,
                            name = Name as string,
                            description = Description as string,
                            isCheckedOut = isCheckedOut as bool?
                        };
                        rv = movie;
                    }
                    connection.Close();
                }
                return rv;
            }
        }

        public static void checkInAMovie(int id) // need to make such that duebackdate is reset, rental log is filled.
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $@"UPDATE Movie SET isCheckedOut=0 WHERE Id={id};";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public static void checkOutAMovie(int id) // need to make such that duebackdate is datetime.now+10, rental log is filled.
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $@"UPDATE Movie SET isCheckedOut=1 WHERE Id={id};";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public static void addAMovie(Movie movie)
        {
            var movietoadd = new Movie
            {
                name = movie.name,
                description = movie.description,
                isCheckedOut = movie.isCheckedOut,
                genreID = movie.genreID
            };
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = @"INSERT INTO Movie (Name, Description, isCheckedOut, genreID) VALUES (@Name, @Description, @isCheckedOut, @genreId)";
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", movie.name);
                    cmd.Parameters.AddWithValue("@Description", movie.description);
                    cmd.Parameters.AddWithValue("@isCheckedOut", movie.isCheckedOut);
                    cmd.Parameters.AddWithValue("@genreID", movie.genreID);
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }

        //v--Admin Services--v//
        public static List<Movie> getCheckedOutMovies()
        {
            var rv = new List<Movie>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT Id, Name, Description, isCheckedOut FROM Movie WHERE isCheckedOut = 1";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var Name = reader[1];
                        var Description = reader[2];
                        var isCheckedOut = reader[3];

                        var movie = new Movie
                        {
                            Id = (int)id,
                            name = Name as string,
                            description = Description as string,
                            isCheckedOut = isCheckedOut as bool?
                        };
                        rv.Add(movie);
                    }
                    connection.Close();
                }
                return rv;
            }
        }

        public static List<MovieViewModel> getAllMoviesWithGenres()
        {
            var rv = new List<MovieViewModel>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT Movie.Id, Movie.Name, Movie.Description, movie.isCheckedOut, Genre.Name as Genre
                                        FROM Movie 
                                        JOIN Genre on Movie.genreID = Genre.Id";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var Name = reader[1];
                        var Description = reader[2];
                        var isCheckedOut = reader[3];
                        var genreName = reader[4];

                        var movie = new MovieViewModel
                        {
                            Id = (int)id,
                            name = Name as string,
                            description = Description as string,
                            isCheckedOut = isCheckedOut as bool?,
                            genreName = genreName as string
                        };
                        rv.Add(movie);
                    }
                    connection.Close();
                }
                return rv;
            }
        }

        public static Movie ViewModeltoMovie(MovieViewModel movievm)
        {
            var rv = new Movie();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = @"SELECT Genre.Id FROM Genre WHERE Genre.Name = @genrename";
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@genrename", movievm.genreName);

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var genreid = reader[0];

                        rv = new Movie()
                        {
                            name = movievm.name,
                            description = movievm.description,
                            genreID = genreid as int?,
                            isCheckedOut = false
                        };

                    }
                }
                return rv;
            }
        } // reader.read is not executing.
    }
}