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
            // Update camera to follow player
            
            if (playerActed)
            {
                CameraSystem.Update();
                StatesSystem.Act();
                HealthSystem.Act();
            }
        
            playerActed = PlayerInputAndMovementSystem.Act(gameTime);
            if (playerActed)
            {
                CameraSystem.Update();
                ActionSystem.Act();
                PathfindingSystem.Act();
                FieldOfViewSystem.Act();
            }

        }

        public MapGameState()
        {
            HealthSystem.Act();
            CameraSystem.Update();
            FieldOfViewSystem.Act();
            PathfindingSystem.Act();
        }
    }
}