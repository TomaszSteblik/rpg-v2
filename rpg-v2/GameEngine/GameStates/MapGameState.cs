using System.Diagnostics;
using System.Linq;
using game.GameEngine.Components;
using game.GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg_v2;

namespace game.GameEngine.GameStates
{
    public class MapGameState : IGameState
    {
        

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawingSystem.Act(spriteBatch);

        }

        public void Update(GameTime gameTime)
        {
            PlayerInputAndMovementSystem.Act(gameTime);
            HealthSystem.Act();
            FieldOfViewSystem.Act();
        }

        public MapGameState()
        {
            
        }
    }
}