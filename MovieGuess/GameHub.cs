using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;

namespace MovieGuess
{
    public class GameHub : Hub
    {
        //NYTT

        private readonly GameTicker _gameTicker;

        public GameHub() : this(GameTicker.Instance) { }

        public GameHub(GameTicker gameTicker)
        {
            _gameTicker = gameTicker;
        }
        //Håll koll på antalet anslutna klienter

        private static Object thisLock = new Object();
        public override Task OnConnected()
        {
            Global.ConnectedIds.Add(Context.ConnectionId, "Anonymous");
            lock (thisLock) //VARFÖR EGENTLIGEN?
            {
                if (!_gameTicker.GameInProgress())
                {
                    _gameTicker.NewGame(null); //Overloada
                }
                //SendCurrentGameState(client?)
                else
                {
                    _gameTicker.SendCurrentClues(Clients.Caller);
                }
            }

            return base.OnConnected();
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Global.ConnectedIds.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
        //^

        //public void Send(string name, string message)
        //{
        //    Clients.All.BroadcastMessage(name, message); //Kan skapa metod till vad jag vill, dynamisk (tänk viewbag)
        //    //Clients.Caller.BroadcastMessage("[" + Context.ConnectionId + "]", "DU SKA SE DET HÄR");
        //}
        public void SetUserName (string name) //HELLO?
        {
            if (name.Length < 20)
            {
                Global.ConnectedIds[Context.ConnectionId] = Global.StripTags(name);
            }
        }

        //Ersätter send
        public void AddChatMessage (string message)
        {
            string name = Global.ConnectedIds[Context.ConnectionId];
            message = Global.StripTags(message);
            Clients.All.BroadcastMessage(name, message);
            _gameTicker.CheckAnswer(name, message);
        }

        public void Cheat(string name)
        {
            _gameTicker.EndRound(Global.ConnectedIds[Context.ConnectionId]);
        }
    }
}