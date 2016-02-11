using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MovieGuess.Models
{
    public class Movie
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Genre { get; set; }
        public string Director { get; set; }
        public string Actors { get; set; }
        public string Plot { get; set; }
        public string Poster { get; set; }
        public double imdbRating { get; set; }
    }
}
//"Title": "Aliens",
//"Year": "1986",
//"Rated": "R",
//"Released": "18 Jul 1986",
//"Runtime": "137 min",
//"Genre": "Action, Horror, Sci-Fi",
//"Director": "James Cameron",
//"Writer": "James Cameron (story), David Giler (story), Walter Hill (story), Dan O'Bannon (characters), Ronald Shusett (characters), James Cameron (screenplay)",
//"Actors": "Sigourney Weaver, Carrie Henn, Michael Biehn, Paul Reiser",
//"Plot": "The planet from Alien (1979) has been colonized, but contact is lost. This time, the rescue team has impressive firepower, but will it be enough?",
//"Language": "English",
//"Country": "USA, UK",
//"Awards": "Won 2 Oscars. Another 16 wins & 22 nominations.",
//"Poster": "http://ia.media-imdb.com/images/M/MV5BMTYzNzU5MzQ4OV5BMl5BanBnXkFtZTcwMDcxNDg3OA@@._V1_SX300.jpg",
//"Metascore": "87",
//"imdbRating": "8.4",
//"imdbVotes": "465,003",
//"imdbID": "tt0090605",
//"Type": "movie",
//"Response": "True"