using System.Text.Json.Serialization;
using rpg_v2;

namespace game.GameEngine.Components
{
    public class Vision : Component
    {
        public bool[][] VisitedCells { get; set; }
        public bool[][] CellsInLightOfSight { get; set; }
        public int Sight { get; set; }

        public Vision()
        {
            Sight = 5;
            VisitedCells = new bool[MainGame.MapSize][];
            CellsInLightOfSight = new bool[MainGame.MapSize][];
            for (var i = 0; i < VisitedCells.Length; i++)
            {
                VisitedCells[i] = new bool[MainGame.MapSize];
                CellsInLightOfSight[i] = new bool[MainGame.MapSize];
            }
        }
    }
}