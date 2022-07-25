using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace game.GameEngine.Components
{
    public class Physics : Component
    {
        public bool IsCollidable { get; set; }
        public bool BlocksVision { get; set; }
    }
}