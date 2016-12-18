using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Primetime.Models;
using System.Data.SqlClient;

namespace Primetime.Services
{
    public class CustomerServices
    {
        private static string connectionStrings = @"Server=DESKTOP-577TSME\SQLEXPRESS;Database=PrimetimeDB;Trusted_Connection=True;";
        // TODO: debug all, create edit. create views.
        public static List<Customer> initializeCustomers()
        {
            var rv = new List<Customer>();
            var LizTiller = new Customer
            {
                name = "LizTiller",
                email = "LizTiller@tiy.com",
                phoneNumber = "8675309"
            };
            var JasmineFrantz = new Customer
            {
                name = "JasmineFrantz",
                email = "JasmineFrantz@tiy.com",
                phoneNumber = "8675309" //add a discount.
            };
            var HollyValenty = new Customer
            {
                name = "HollyValenty",
                email = "LizTiller@tiy.com",
                phoneNumber = "8675309"
            };
            var CalebSanderson = new Customer
            {
                name = "CalebSanderson",
                email = "CalebSanderson@tiy.com",
                phoneNumber = "8675309"
            };
            var SaraToftegaard = new Customer
            {
                name = "SaraToftegaard",
                email = "SaraToftegaard@tiy.com",
                phoneNumber = "8675309"
            };
            addACustomer(LizTiller);
            addACustomer(SaraToftegaard);
            addACustomer(CalebSanderson);
            addACustomer(HollyValenty);
            addACustomer(JasmineFrantz);
            return rv;
        }
        public static List<Customer> getAllCustomers()
        {
            var rv = new List<Customer>();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"SELECT Id, Name, Email, Phone FROM Customer";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"];
                        var Name = reader[1];
                        var Email = reader[2];
                        var Phone = reader[3];

                        var customer = new Customer
                        {
                            Id = (int)id,
                            name = Name as string,
                            email = Email as string,
                            phoneNumber = Phone as string
                        };
                        rv.Add(customer);
                    }
                    connection.Close();
                }
                return rv;
            }
        } //read
        public static Customer getACustomer(int id)
        {
            var rv = new Customer();
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $@"SELECT Name, Email, Phone FROM Customer WHERE Id = {id}";

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        var Name = reader[0];
                        var Email = reader[1];
                        var Phone = reader[2];

                        var customer = new Customer
                        {
                            Id = id,
                            name = Name as string,
                            email = Email as string,
                            phoneNumber = Phone as string
                        };
                        rv = customer;
                    }
                    connection.Close();
                }
                return rv;
            }
        }
        public static void editACustomer(int id, string name, string email, string phoneNumber) // give these values to method when edit is clicked.
        {
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = $@"UPDATE Customer SET Name= @name, Email = @email, Phone = @phoneNumber  WHERE Id={id};";
                    cmd.Parameters.AddWithValue("name", "aoaoa");
                    cmd.Parameters.AddWithValue("email", "aoaoa");
                    cmd.Parameters.AddWithValue("phoneNumber", "aoaoa");

                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        } // update
        public static void addACustomer(Customer cust)
        {
            var customertoadd = new Customer
            {
                name = cust.name,
                email = cust.email,
                phoneNumber = cust.phoneNumber
            };
            using (var connection = new SqlConnection(connectionStrings))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.CommandText = @"INSERT INTO Customer (Name, Email, Phone) VALUES (@Name, @Email, @Phone)";
                    cmd.Connection = connection;
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.Parameters.AddWithValue("@Name", cust.name);
                    cmd.Parameters.AddWithValue("@Email", cust.email);
                    cmd.Parameters.AddWithValue("@Phone", cust.phoneNumber);
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    connection.Close();
                }
            }
        } // create
 
    }
}