using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using FontStashSharp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg_v2;

namespace game.GameEngine.GameStates
{
    public class StartMenuGameState : IGameState
    {
        private InputManager _inputManager;
        private uint selectPosition;
        
        public void Draw(SpriteBatch spriteBatch)
        {
            var font = MainGame.FontSystem.GetFont(42);
            spriteBatch.DrawString(font, "New game", new Vector2(410, 300), selectPosition == 0 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Save game", new Vector2(408, 342), selectPosition == 1 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Continue", new Vector2(415, 384), selectPosition == 2 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Settings", new Vector2(420, 426), selectPosition == 3 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Credits", new Vector2(430, 468), selectPosition == 4 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Exit", new Vector2(450, 510), selectPosition == 5 ? Color.Blue : Color.White);

        }

        public void Update(GameTime gameTime)
        {
            _inputManager.CheckInputAndAct(gameTime);
            _inputManager.UpdateStates(gameTime);
        }

        public StartMenuGameState()
        {
            _inputManager = new InputManager();
            selectPosition = 0;
            
            _inputManager.StartTrackingKey(Keys.Up, () =>
            {
                if (selectPosition <= 0)
                {
                    selectPosition = 5;
                }
                else
                {
                    selectPosition--;
                }
            },true);
            
            _inputManager.StartTrackingKey(Keys.Down, () =>
            {
                if (selectPosition >= 5)
                {
                    selectPosition = 0;
                }
                else
                {
                    selectPosition++;
                }
            },true);
            
            _inputManager.StartTrackingKey(Keys.W, () =>
            {
                if (selectPosition <= 0)
                {
                    selectPosition = 5;
                }
                else
                {
                    selectPosition--;
                }
            },true);
            
            _inputManager.StartTrackingKey(Keys.S, () =>
            {
                if (selectPosition >= 5)
                {
                    selectPosition = 0;
                }
                else
                {
                    selectPosition++;
                }
            },true);
            
            _inputManager.StartTrackingKey(Keys.Enter, () =>
            {
                switch (selectPosition)
                {
                    case 0:
                        
                        Map.GenerateWallsAndFloors(MainGame.MapSize,MainGame.MapSize*20, MainGame.MapSize/10);
                        MainGame.CurrentGameState = new MapGameState();
                        break;
                    case 2:
                        MainGame.CurrentGameState = new LoadMenuState();
                        break;
                    case 5:
                        Program.Game.Exit();
                        break;
                    default:

                        break;
                }
            },true);
            
            _inputManager.StartTrackingKey(Keys.Escape, () =>
            {
                Program.Game.Exit();
            },true);

        }
    }
}