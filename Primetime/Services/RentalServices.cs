using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using Primetime.Models;

namespace Primetime.Services
{
    public class RentalServices
    {
        private static string connectionStrings = @"Server=DESKTOP-577TSME\SQLEXPRESS;Database=PrimetimeDB;Trusted_Connection=True;";

        public static List<RentalViewModel> getOverdueRentals()
        {
            var rv = new List<RentalViewModel>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT RentalLog.Id, Movie.Name as Movie, Customer.Name as Customer, Customer.Email, Customer.Phone, RentalLog.RentalDate, RentalLog.DueDate 
                                        FROM RentalLog 
                                        JOIN Movie on Movie.Id = RentalLog.MovieId
                                        JOIN Customer on Customer.Id = RentalLog.CustomerId
                                        WHERE RentalLog.DueDate < GETDATE() AND RentalLog.ReturnDate is NULL";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var rentalId = reader[0];
                        var movieName = reader[1];
                        var customerName = reader[2];
                        var customerEmail = reader[3];
                        var customerPhone = reader[4];
                        var rentalDate = reader[5];
                        var dueDate = reader[6];

                        var rental = new RentalViewModel
                        {
                            Id = rentalId as int?,
                            movieName = movieName as string,
                            customerName = customerName as string,
                            customerEmail = customerEmail as string,
                            customerPhone = customerPhone as string,
                            rentalDate = rentalDate as DateTime?,
                            dueDate = dueDate as DateTime?,
                            checkedOut = true
                        };
                        rv.Add(rental);
                    }
                    connection.Close();
                }
                return rv;
            }
        }

        public static List<RentalViewModel> getCheckedOutRentals()
        {
            var rv = new List<RentalViewModel>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT RentalLog.Id, Movie.Name as Movie, Movie.isCheckedOut, Customer.Name as Customer, RentalLog.RentalDate, RentalLog.DueDate 
                                        FROM RentalLog 
                                        JOIN Movie on Movie.Id = RentalLog.MovieId
                                        JOIN Customer on Customer.Id = RentalLog.CustomerId
                                        WHERE Movie.isCheckedOut = 1";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var rentalId = reader[0];
                        var movieName = reader[1];
                        var isCheckedOut = reader[2];
                        var customerName = reader[3];
                        var rentalDate = reader[4];
                        var dueDate = reader[5];

                        var rental = new RentalViewModel
                        {
                            Id = rentalId as int?,
                            movieName = movieName as string,
                            customerName = customerName as string,
                            rentalDate = rentalDate as DateTime?,
                            dueDate = dueDate as DateTime?,
                            checkedOut = isCheckedOut as bool?
                        };
                        rv.Add(rental);
                    }
                    connection.Close();
                }
                return rv;
            }
        }

        public static void checkOutARental(Rental rental)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"INSERT INTO RentalLog (MovieId, CustomerId, RentalDate, DueDate) VALUES (@movieid, @customerid, @rentaldate, @duedate)
                                        UPDATE Movie SET isCheckedOut = @ischeckedout WHERE Id = @movieid";
                    cmd.Parameters.AddWithValue("movieid", rental.movieID);
                    cmd.Parameters.AddWithValue("customerid", rental.customerID);
                    cmd.Parameters.AddWithValue("rentaldate", rental.rentalDate);
                    cmd.Parameters.AddWithValue("duedate", rental.dueDate);
                    cmd.Parameters.AddWithValue("ischeckedout", rental.checkedOut);

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }

        public static void checkInARental(Rental rental)
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"UPDATE RentalLog SET ReturnDate = @returndate WHERE Id = @rentalid; UPDATE Movie SET isCheckedOut = @ischeckedout WHERE Id = @movieid";
                    cmd.Parameters.AddWithValue("returndate", DateTime.Today);
                    cmd.Parameters.AddWithValue("rentalid", rental.Id);
                    cmd.Parameters.AddWithValue("@ischeckedout", false);
                    cmd.Parameters.AddWithValue("movieid", rental.movieID);

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        }
        public static Rental getARental(int id)
        {
            var rv = new Rental();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT * FROM RentalLog WHERE Id = @rentalid";
                    cmd.Parameters.AddWithValue("rentalid", id);

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var rentalid = reader[0];
                        var customerid = reader[1];
                        var movieid = reader[2];
                        var rentaldate = reader[3];
                        var duedate = reader[4];
                        var returndate = reader[5];
                        rv = new Rental()
                        {
                            Id = (int)rentalid,
                            customerID = customerid as int?,
                            movieID = movieid as int?,
                            rentalDate = rentaldate as DateTime?,
                            dueDate = duedate as DateTime?,
                            returnDate = returndate as DateTime?
                        };
                    }
                    connection.Close();
                }
            } return rv;
        }
    }
}