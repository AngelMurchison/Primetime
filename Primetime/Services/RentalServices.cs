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

    }
}