using Microsoft.Xna.Framework;

namespace game.GameEngine.Components
{
    public class Sprite : Component
    {
        public int AtlasPositionX { get; set; }
        public int AtlasPositionY { get; set; }
        public Color Color { get; set; }
        public int Layer { get; set; }
        public bool IsVisibleOutOfSight { get; set; }

        public Sprite()
        {
            Color = Color.White;
            Layer = 0;
            IsVisibleOutOfSight = true;
        }
    }
}