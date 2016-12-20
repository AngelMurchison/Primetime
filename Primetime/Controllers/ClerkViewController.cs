using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Primetime.Models;
using Primetime.Services;

namespace Primetime.Controllers
{


    public class ClerkViewController : Controller
    {
        public static Rental rentalathand = new Rental() { };

        [HttpGet]
        public ActionResult clerkMovieIndex()
        {

            //if (null == MovieServices.testMovies) // why is testMovies null whenever I open the page?
            //{
            //    MovieServices.testMovies = new List<Movie>(); 
            //    MovieServices.initializeMovies();
            //}

            var movies = MovieServices.getAllMovies();
            return View(movies);
        }

        public ActionResult checkIn(int id)
        {
            var movie = MovieServices.getAMovie(id);
            return View(movie);
        }
        public ActionResult confirmCheckIn(int id)
        {
            var rental = RentalServices.getARental(id);
            RentalServices.checkInARental(rental);
            return RedirectToAction("clerkMovieIndex");
        }

        public ActionResult allCheckedOut()
        {
            var allCheckedOut = RentalServices.getCheckedOutRentals();
            return View(allCheckedOut);
        }
        public ActionResult checkOut(int movieid)
        {
            var movie = MovieServices.getAMovie(movieid);
            rentalathand.movieID = movieid;
            return View(movie);
        }
        public ActionResult confirmCheckOut(int customerid)
        {
            rentalathand.customerID = customerid;
            rentalathand.rentalDate = DateTime.Today;
            rentalathand.dueDate = DateTime.Now.AddDays(10);
            rentalathand.checkedOut = true;
            RentalServices.checkOutARental(rentalathand);
            MovieServices.checkOutAMovie((int)rentalathand.movieID);
            return RedirectToAction("clerkMovieIndex");
        }

        [HttpGet]
        public ActionResult clerkCustomerIndex()
        {
            var customers = Services.CustomerServices.getAllCustomers();
            if (customers.Count == 0)
            {
                Services.CustomerServices.initializeCustomers();
            }

            return View(customers);
        }

        public ActionResult getCustomerInfo()
        {
            var allcustomers = CustomerServices.getAllCustomers();
            return View(allcustomers);
        }

        public ActionResult createACustomer()
        {
            return View();
        }
        public ActionResult customerCreation(string name, string email, string phoneNumber)
        {
            var customer = new Customer()
            {
                name = name,
                email = email,
                phoneNumber = phoneNumber
            };
            Services.CustomerServices.addACustomer(customer);
            return RedirectToAction("clerkCustomerIndex");
        }

        public ActionResult deleteACustomer(int id)
        {
            var customer = Services.CustomerServices.getACustomer(id);
            return View(customer);
        }
        public ActionResult customerDeletion(int id)
        {
            Services.CustomerServices.removeACustomer(id);
            return RedirectToAction("clerkCustomerIndex");
        }

        public ActionResult editACustomer(int id)
        {
            var customer = Services.CustomerServices.getACustomer(id);
            return View(customer);
        }
        public ActionResult customerEdit(string name, string email, string phoneNumber, int id)
        {
            Services.CustomerServices.editACustomer(id, name, email, phoneNumber);
            return RedirectToAction("clerkCustomerIndex");
        }
    }
}