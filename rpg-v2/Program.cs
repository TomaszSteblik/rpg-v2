using System;
using game.GameEngine;
using Microsoft.Xna.Framework;

namespace rpg_v2
{
    public static class Program
    {

        public static Game Game;
        
        [STAThread]
        static void Main()
        {
            EcsManager.Init();
            using (Game = new MainGame())
                Game.Run();
        }
    }
}