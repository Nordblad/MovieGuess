using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Concurrent;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MovieGuess.Models;

namespace MovieGuess
{
    public class GameTicker
    {
        private readonly static Lazy<GameTicker> _instance = new Lazy<GameTicker>(() => new GameTicker(GlobalHost.ConnectionManager.GetHubContext<GameHub>().Clients));
        public static GameTicker Instance { get { return _instance.Value; } }
        private IHubConnectionContext<dynamic> Clients { get; set; }
        public static Movie CurrentMovie { get; set; }
        private static Clue[] ClueList;
        private static int CurrentClueNumber;
        private static int SecondsToNextClue;
        private static Timer _timer { get; set; }
        public static bool GameIsActive = false;
        public static bool AcceptingAnswers = false;

        public bool GameInProgress () { return GameIsActive; }

        private GameTicker(IHubConnectionContext<dynamic> clients)
        {
            Clients = clients;
        }

        internal void SendCurrentClues(dynamic caller)
        {
            for (int i = 0; i < CurrentClueNumber; i++)
            {
                Clue c = ClueList[i];
                caller.AddClue(c.FieldId, c.Value);
            }
            if (CurrentClueNumber < ClueList.Length)
                caller.StartCountdown(ClueList[CurrentClueNumber].DisplayName, SecondsToNextClue * 1000); //Text, milliseconds
            else
                caller.StartCountdown("Next round starting...", 0); //Text, milliseconds
        }

        public void ResetClues ()
        {
            Clients.All.resetClues();
        }

        public void AddClue (string property, string message)
        {
            Clients.All.addClue(property, message);
        }
        
        public void NewGame (object state)
        {
            if (Global.ConnectedIds.Count <= 0)
            {
                GameIsActive = false;
                return;
            }
            //if (GameIsActive)
            //    return;
            Clients.All.ResetClues();
            CurrentMovie = GetRandomMovie();
            ClueList = GetCluesForMovie(CurrentMovie);
            CurrentClueNumber = 0;
            _timer = new Timer(NextGameTick, null, 0, 1000);
            SecondsToNextClue = 0;
            GameIsActive = true; //LOCK?
            AcceptingAnswers = true;
        }

        internal void CheckAnswer(string name, string message)
        {
            if (AcceptingAnswers && message.Equals(CurrentMovie.Title, StringComparison.OrdinalIgnoreCase)) //Lock?
            {
                EndRound(name);
            }
        }

        public static Movie GetRandomMovie()
        {
            using (var db = new DBModel())
            {
                //Smart sätt att få en random, minns detta.
                return db.Database.SqlQuery<Movie>("SELECT TOP 1 * FROM dbo.Movies ORDER BY NEWID()").FirstOrDefault();

            }
        }

        public void NextGameTick(object state)
        {
            //Dags att visa nästa ledtråd
            if (SecondsToNextClue <= 0)
            {
                if (CurrentClueNumber >= ClueList.Length)
                {
                    EndRound("");
                }
                else
                {
                    Clue currentClue = ClueList[CurrentClueNumber];

                    //Skicka info till klienterna
                    Clients.All.AddClue(currentClue.FieldId, currentClue.Value);
                    if (CurrentClueNumber < ClueList.Length - 1)
                    {
                        SendTimerToClients(ClueList[CurrentClueNumber + 1].DisplayName, currentClue.SecondsToShow);
                        //Sätt timern rätt. Observera -1
                        SecondsToNextClue = currentClue.SecondsToShow - 1;
                        //Gå till nästa ledtråd
                        CurrentClueNumber++;
                    }
                    else
                    {
                        EndRound("");
                    }
                }
            }
            else
            {
                SecondsToNextClue--;
            }
        }

        public static Clue[] GetCluesForMovie(Movie movie)
        {
            //Fult sätt att göra det på. Lateeers.
            var splitActors = movie.Actors.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            var clues = new Clue[]
            {
                new Clue("", 3, "year", movie.Year.ToString()),
                new Clue("Genre", 5, "genre", movie.Genre),
                new Clue("Actor #4", 7, "actor4", splitActors[3].Trim()),
                new Clue("Actor #3", 7, "actor3", splitActors[2].Trim()),
                new Clue("Actor #2", 8, "actor2", splitActors[1].Trim()),
                new Clue("Lead actor", 8, "actor1", splitActors[0].Trim()),
                new Clue("Director", 8, "director", movie.Director),
                new Clue("Plot", 11, "plot", movie.Plot),
                new Clue("Game over", 6, "title", movie.Title),
                //new Clue("Next round starting", 0, "", ""), //TA BORT! Sluta en tidigare, gå till endround
            };
            //foreach (Clue c in clues) //FÖR DEBUGGING ENDAST
            //    c.SecondsToShow = 2;
            return clues;
        }

        public void EndRound(string winner)
        {
            AcceptingAnswers = false;
            //GameIsActive = false;
            _timer.Dispose(); //HMM
            if (winner != "")
            {
                SendRemainingClues();
            }
            Clients.All.AnnounceWinner(winner);
            
            _timer = new Timer(NewGame, null, 7000, Timeout.Infinite);
            SendTimerToClients("Next round", 7);
        }
        private void SendTimerToClients (string message, int seconds)
        {
            Clients.All.StartCountdown(message, seconds * 1000);
        }

        private void SendRemainingClues ()
        {
            for (int i = CurrentClueNumber; i < ClueList.Length; i++)
            {
                Clue c = ClueList[i];
                Clients.All.AddClue(c.FieldId, c.Value);
            }
            CurrentClueNumber = ClueList.Length;
        }

    }
}