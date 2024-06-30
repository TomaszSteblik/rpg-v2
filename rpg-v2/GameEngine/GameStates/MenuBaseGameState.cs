using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg_v2;
using FontStashSharp;


namespace game.GameEngine.GameStates;

public abstract class MenuBaseGameState : IGameState
{
    private readonly InputManager _inputManager;
    private uint _selectPosition;
    private readonly List<(Action Action, string Name)> _actions;

    protected MenuBaseGameState(List<(Action Action, string Name)> actions, Action onEscape)
    {
        _inputManager = new InputManager();
        _selectPosition = 0;
        _actions = actions;

        var actionsCount = (uint) actions.Count;
        
        _inputManager.StartTrackingKey(Keys.Up, () =>
        {
            if (_selectPosition <= 0)
            {
                _selectPosition = actionsCount - 1;
            }
            else
            {
                _selectPosition--;
            }
        }, true);

        _inputManager.StartTrackingKey(Keys.Down, () =>
        {
            if (_selectPosition >= actionsCount - 1)
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
                _selectPosition = actionsCount;
            }
            else
            {
                _selectPosition--;
            }
        }, true);

        _inputManager.StartTrackingKey(Keys.S, () =>
        {
            if (_selectPosition >= actionsCount - 1)
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
            actions.ElementAt((int) _selectPosition).Action.Invoke();
        }, true);
        
        _inputManager.StartTrackingKey(Keys.Escape, onEscape.Invoke, false);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        var font = MainGame.FontSystem.GetFont(42);

        for (var i = 0; i < _actions.Count; i++)
        {
            spriteBatch.DrawString(font, _actions[i].Name, new Vector2(410, 300 + i * 42),
                _selectPosition == i ? Color.Blue : Color.White);
        }
    }

    public void Update(GameTime gameTime)
    {
        _inputManager.CheckInputAndAct(gameTime);
        _inputManager.UpdateStates(gameTime);
    }
}