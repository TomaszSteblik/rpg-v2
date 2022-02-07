using System.Collections.Generic;

namespace game.GameEngine.Components
{
    public class Pathfinding : Component
    {
        public int TargetX { get; set; }
        public int TargetY { get; set; }
        public int Step { get; set; }
        public bool NeedToFindNewPath { get; set; }
        public List<Node> Path { get; set; }

        public Pathfinding()
        {
            Path = new List<Node>();
        }
    }
}