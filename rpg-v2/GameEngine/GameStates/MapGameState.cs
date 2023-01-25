using game.GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace game.GameEngine.GameStates
{
    public class MapGameState : IGameState
    {
        private bool playerActed = false;
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawingSystem.Act(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (playerActed)
            {
                StatesSystem.Act();
                HealthSystem.Act();
            }
            
            playerActed = PlayerInputAndMovementSystem.Act(gameTime);
            if (playerActed)
            {
                ActionSystem.Act();
                PathfindingSystem.Act();
                FieldOfViewSystem.Act();
            }
            
        }

        public MapGameState()
        {
            FieldOfViewSystem.Act();
            PathfindingSystem.Act();
        }
    }
}