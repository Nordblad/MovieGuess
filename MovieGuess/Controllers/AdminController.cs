using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieGuess.Models;
//using Newtonsoft.Json;
using System.Web.Helpers;

namespace MovieGuess.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            DBModel db = new DBModel();
            return View(db.Movies.OrderBy(m => m.Title).ToList());
        }

        [HttpPost]
        public ActionResult AddMovie(Movie movie, string imdbId)
        {
            movie.Id = imdbId;
            movie.imdbRating = movie.imdbRating / 10; //Ful lösning

            using (var db = new DBModel())
            {
                if (db.Movies.Any(m => m.Id == movie.Id))
                {
                    return Json(new { success = false, message = movie.Title + " is already in the database." });
                }
                db.Movies.Add(movie);
                db.SaveChanges();
            }
            return Json(new { success = true, message = movie.Title + " was added to the database." });
        }
    }
}
//"Title": "Independence Day",
//"Year": "1996",
//"Rated": "PG-13",
//"Released": "03 Jul 1996",
//"Runtime": "145 min",
//"Genre": "Action, Adventure, Sci-Fi",
//"Director": "Roland Emmerich",
//"Writer": "Dean Devlin, Roland Emmerich",
//"Actors": "Will Smith, Bill Pullman, Jeff Goldblum, Mary McDonnell",
//"Plot": "The aliens are coming and their goal is to invade and destroy Earth. Fighting superior technology, mankind's best weapon is the will to survive.",
//"Language": "English",
//"Country": "USA",
//"Awards": "Won 1 Oscar. Another 31 wins & 32 nominations.",
//"Poster": "http://ia.media-imdb.com/images/M/MV5BMTMwODY3NzQzNF5BMl5BanBnXkFtZTcwNzUxNjc0MQ@@._V1_SX300.jpg",
//"Metascore": "59",
//"imdbRating": "6.9",
//"imdbVotes": "402,795",
//"imdbID": "tt0116629",
//"Type": "movie",
//"Response": "True"