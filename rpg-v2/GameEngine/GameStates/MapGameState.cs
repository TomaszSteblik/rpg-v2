using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        private Mutex _mutex = new Mutex();
        
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawingSystem.Act(spriteBatch);

        }

        public void Update(GameTime gameTime)
        {
            PlayerInputAndMovementSystem.Act(gameTime);
            HealthSystem.Act();
            
            _mutex.WaitOne();
            FieldOfViewSystem.Act();
            _mutex.ReleaseMutex();
        }

        public MapGameState()
        {
            Task.Factory.StartNew(() =>
            {
                while (MainGame.CurrentGameState == this)
                {
                    //ActionSystem - actions done by entities for example: select target for pathfinding, move to next tile, heal, wait, cast etc.
                    _mutex.WaitOne();
                    ActionSystem.Act();
                    _mutex.ReleaseMutex();
                    
                    PathfindingSystem.Act();

                    Thread.Sleep(250);
                }
                
            });
        }
    }
}