using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Primetime.Services;
using Primetime.Models;

namespace Primetime.Controllers
{
    public class AdminViewController : Controller
    {
        // GET: AdminView
        public ActionResult adminIndex()
        {
            var movies = Services.MovieServices.getAllMoviesWithGenres();
            return View(movies);
        }

        public ActionResult viewCheckedOut()
        {
            var movies = MovieServices.getCheckedOutMovies();
            return View(movies);
        }

        public ActionResult viewOverDue()
        {
            var rentals = RentalServices.getOverdueRentals();
            return View(rentals);
        }

        public ActionResult addAMovie(MovieViewModel movievm)
        {
            return View(movievm);
        }

        public ActionResult movieAddition(MovieViewModel movievm)
        {
            var movie = Services.MovieServices.ViewModeltoMovie(movievm);
            MovieServices.addAMovie(movie);
            return RedirectToAction("adminIndex");
        } //  finish edit and delete functionality.

        public ActionResult editAMovie(int id)
        {
            var movie = MovieServices.getAMovie(id);
            return View(movie);
        }

        public ActionResult movieEdit(Movie movie)
        { 
            MovieServices.editAMovie(movie);
            return RedirectToAction("adminIndex");
        }

        public ActionResult deleteAMovie(int id)
        {
            var movie =  MovieServices.getAMovie(id);
            return View(movie);
        }

        public ActionResult movieDeletion(Movie movie)
        { 
            MovieServices.deleteAMovie(movie);      
            return RedirectToAction("adminIndex");
        }

        public ActionResult adminGenreIndex()
        {
            var genres = GenreServices.getAllGenres();
            return View(genres);
        }

        public ActionResult addAGenre()
        {
            return View();
        }

        public ActionResult updateAGenre(int id)
        {
            var genre = GenreServices.getAGenre(id);
            return View(genre);
        }

        public ActionResult deleteAGenre(int id)
        {
            var genre = GenreServices.getAGenre(id);
            return View(genre);
        }

        public ActionResult genreDeletion(Genre genre)
        {
            GenreServices.deleteAGenre(genre);
            return RedirectToAction("adminGenreIndex");
        }
        public ActionResult genreAddition(Genre genre)
        {
            GenreServices.addAGenre(genre);
            return RedirectToAction("adminGenreIndex");
        }
        public ActionResult genreUpdating(Genre genre)
        {
            GenreServices.updateAGenre(genre.name, genre.Id);
            return RedirectToAction("adminGenreIndex");
        }
    }
}