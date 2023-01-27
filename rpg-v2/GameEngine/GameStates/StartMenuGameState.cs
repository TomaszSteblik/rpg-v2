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
        private readonly InputManager _inputManager;
        private uint _selectPosition;

        public void Draw(SpriteBatch spriteBatch)
        {
            var font = MainGame.FontSystem.GetFont(42);
            spriteBatch.DrawString(font, "New game", new Vector2(410, 300), _selectPosition == 0 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Continue", new Vector2(408, 342), _selectPosition == 1 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Settings", new Vector2(415, 384), _selectPosition == 2 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Credits", new Vector2(420, 426), _selectPosition == 3 ? Color.Blue : Color.White);
            spriteBatch.DrawString(font, "Exit", new Vector2(430, 468), _selectPosition == 4 ? Color.Blue : Color.White);

        }

        public void Update(GameTime gameTime)
        {
            _inputManager.CheckInputAndAct(gameTime);
            _inputManager.UpdateStates(gameTime);
        }

        public StartMenuGameState()
        {
            _inputManager = new InputManager();
            _selectPosition = 0;

            _inputManager.StartTrackingKey(Keys.Up, () =>
            {
                if (_selectPosition <= 0)
                {
                    _selectPosition = 4;
                }
                else
                {
                    _selectPosition--;
                }
            }, true);

            _inputManager.StartTrackingKey(Keys.Down, () =>
            {
                if (_selectPosition >= 4)
                {
                    _selectPosition = 0;
                }
                else
                {
                    _selectPosition++;
                }
            }, true);

            _inputManager.StartTrackingKey(Keys.W, () =>
            {
                if (_selectPosition <= 0)
                {
                    _selectPosition = 4;
                }
                else
                {
                    _selectPosition--;
                }
            }, true);

            _inputManager.StartTrackingKey(Keys.S, () =>
            {
                if (_selectPosition >= 4)
                {
                    _selectPosition = 0;
                }
                else
                {
                    _selectPosition++;
                }
            }, true);

            _inputManager.StartTrackingKey(Keys.Enter, () =>
            {
                switch (_selectPosition)
                {
                    case 0:

                        Map.GenerateWallsAndFloors(MainGame.MapSize, MainGame.MapSize * 20, MainGame.MapSize / 10);
                        MainGame.CurrentGameState = new MapGameState();
                        break;
                    case 1:
                        MainGame.CurrentGameState = new LoadMenuState();
                        break;
                    case 4:
                        Program.Game.Exit();
                        break;
                    default:

                        break;
                }
            }, true);

            _inputManager.StartTrackingKey(Keys.Escape, () =>
            {
                Program.Game.Exit();
            }, false);

        }
    }
}