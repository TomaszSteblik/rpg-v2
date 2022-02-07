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
            //ActionSystem - actions done by entities for example: select target for pathfinding, move to next tile, heal, wait, cast etc.
            ActionSystem.Act();
            //Pathfinding - if entity have bool toPathFind or sth then find path to target tile
            PathfindingSystem.Act();
            HealthSystem.Act();
            FieldOfViewSystem.Act();
        }

        public MapGameState()
        {
            
        }
    }
}