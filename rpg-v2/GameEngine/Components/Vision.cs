using System.Text.Json.Serialization;
using rpg_v2;

namespace game.GameEngine.Components
{
    public class Vision : Component
    {
        public bool[][] VisitedCells { get; set; }
        public bool[][] CellsInLightOfSight { get; set; }
        public int Sight { get; set; }
        public int ArraySize { get; set; }
        public int ArrayWidth { get; set; }
        public int ArrayHeight { get; set; }

        public Vision()
        {
            Sight = 5;
            VisitedCells = new bool[MainGame.MapWidth][];
            ArrayWidth = MainGame.MapHeight;
            ArrayHeight = MainGame.MapWidth;
            CellsInLightOfSight = new bool[MainGame.MapWidth][];
            for (var i = 0; i < VisitedCells.Length; i++)
            {
                VisitedCells[i] = new bool[MainGame.MapHeight];
                CellsInLightOfSight[i] = new bool[MainGame.MapHeight];
            }
        }
    }
}