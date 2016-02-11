using Microsoft.Owin;
using Owin;
using MovieGuess;

namespace MovieGuess
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}