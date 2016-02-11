using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieGuess.Models;

namespace MovieGuess.Controllers
{
    public class GameController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            int numberOfMovies;
            using (var db = new DBModel())
            {
                
                numberOfMovies = db.Movies.Count();
            }
            return View(numberOfMovies);
        }
    }
}