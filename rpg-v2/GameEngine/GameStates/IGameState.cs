using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rpg_v2;

namespace game.GameEngine.GameStates
{
    public interface IGameState
    {
        public void Draw(SpriteBatch spriteBatch);
        public void Update(GameTime gameTime);

    }
}