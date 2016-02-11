using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MovieGuess.Models;
using Newtonsoft.Json;
using System.Threading;
using System.Text.RegularExpressions;

namespace MovieGuess
{
    public static class Global
    {
        //public static HashSet<string> ConnectedIds = new HashSet<string>(); //Anslutna användare
        public static Dictionary<string, string> ConnectedIds = new Dictionary<string, string>(); //Anslutna användare
        static Regex _htmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

        /// <summary>
        /// Remove HTML from string with compiled Regex.
        /// </summary>
        public static string StripTags(string source)
        {
            return _htmlRegex.Replace(source, string.Empty);
        }
    }
}