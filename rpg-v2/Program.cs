﻿using game.GameEngine;
using Microsoft.Xna.Framework;
using rpg_v2.Utils;
using Serilog;

namespace rpg_v2
{
    public static class Program
    {

        public static Game Game;

        static void Main()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Debug()
                .WriteTo.InMemorySom()
                .CreateLogger();
            EcsManager.Init();
            using (Game = new MainGame())
                Game.Run();
        }
    }
}