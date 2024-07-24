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
    protected readonly InputManager InputManager;
    protected uint SelectPosition;
    protected abstract List<(Action Action, Func<string> Name)> Actions { get; }

    private void GoUp()
    {
        if (!Actions.Any())
            throw new ArgumentNullException(nameof(Actions), "Actions cannot be empty.");
        
        if (SelectPosition <= 0)
        {
            SelectPosition = (uint)Actions.Count - 1;
        }
        else
        {
            SelectPosition--;
        }
    }

    private void GoDown()
    {
        if (!Actions.Any())
            throw new ArgumentNullException(nameof(Actions), "Actions cannot be empty.");
        
        if (SelectPosition >= (uint)Actions.Count - 1)
        {
            SelectPosition = 0;
        }
        else
        {
            SelectPosition++;
        }
    }

    private void Select()
    {
        Actions.ElementAt((int) SelectPosition).Action.Invoke();
    }

    protected abstract void OnEscape();

    protected MenuBaseGameState()
    {
        InputManager = new InputManager();
        SelectPosition = 0;

        
        InputManager.StartTrackingKey(Keys.Up, GoUp, true);

        InputManager.StartTrackingKey(Keys.Down, GoDown, true);

        InputManager.StartTrackingKey(Keys.W, GoUp, true);

        InputManager.StartTrackingKey(Keys.S, GoDown, true);
        
        InputManager.StartTrackingKey(Keys.Enter, Select, true);
        
        InputManager.StartTrackingKey(Keys.Escape, OnEscape, false);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        var font = MainGame.FontSystem.GetFont(42);

        for (var i = 0; i < Actions.Count; i++)
        {
            spriteBatch.DrawString(font, Actions[i].Name.Invoke() , new Vector2(410, 300 + i * 42),
                SelectPosition == i ? Color.Blue : Color.White);
        }
    }

    public void Update(GameTime gameTime)
    {
        InputManager.CheckInputAndAct(gameTime);
        InputManager.UpdateStates(gameTime);
    }
}