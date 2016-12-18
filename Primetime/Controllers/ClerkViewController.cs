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
        public static List<Customer> allCustomers = new List<Customer>();

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
        [HttpPost]
        public ActionResult confirmCheckIn(int id)
        {
            MovieServices.checkInAMovie(id);
            return RedirectToAction("clerkMovieIndex");
        }
 
        public ActionResult checkOut(int id)
        {
            var movie = MovieServices.getAMovie(id);
            return View(movie);
        }
        [HttpPost]
        public ActionResult confirmCheckOut(int id)
        {
            MovieServices.checkOutAMovie(id);
            return RedirectToAction("clerkMovieIndex");
        }

        public ActionResult clerkCustomerIndex()
        {
            var customers = Services.CustomerServices.getAllCustomers();
            if(customers.Count == 0)
            {
                Services.CustomerServices.initializeCustomers();
            }

            return View(customers);
        }

        public ActionResult createACustomer()
        {

            return View();
        }

        public ActionResult customerCreation(string name, string email, string phone)
        {
            var customer = new Customer()
            {
                name = name,
                email = email,
                phoneNumber = phone
            };
            Services.CustomerServices.addACustomer(customer);
            return RedirectToAction("clerkCustomerIndex");
        }

        public ActionResult deleteACustomer()
        {
            return View();
        }
        // v make these ^
        public ActionResult editACustomer()
        {
            return View();
        }


    }
}