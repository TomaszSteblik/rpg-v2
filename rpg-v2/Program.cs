using game.GameEngine;
using Microsoft.Xna.Framework;
using rpg_v2.Utils;
using Serilog;
using Serilog.Events;

namespace rpg_v2
{
    public static class Program
    {

        public static Game Game;

        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Debug(LogEventLevel.Verbose)
                .WriteTo.Seq("http://localhost:5341", LogEventLevel.Verbose)
                .WriteTo.InMemorySom(LogEventLevel.Information)
                .MinimumLevel.Verbose()
                .CreateLogger();
            EcsManager.Init();
            using (Game = new MainGame())
                Game.Run();
        }
    }
}