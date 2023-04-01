using System;
using System.Collections.Generic;
using FontStashSharp;
using game.GameEngine.Components;
using game.GameEngine.GameObjects.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg_v2;

namespace game.GameEngine.GameStates;

public class InventoryGameState : IGameState
{
    private readonly List<Item> _items;
    private readonly InputManager _inputManager;
    private int _selectPosition;
    private string _currentMessage = "";

    public InventoryGameState()
    {
        var playerInventory = (Inventory) MainGame.PlayerEntity.Components[9];
        _items = playerInventory.Items;
        
        _inputManager = new InputManager();
        _selectPosition = 0;
        
        _inputManager.StartTrackingKey(Keys.Up, MoveSelectUp, true);

        _inputManager.StartTrackingKey(Keys.Down, MoveSelectDown, true);

        _inputManager.StartTrackingKey(Keys.W, MoveSelectUp, true);

        _inputManager.StartTrackingKey(Keys.S, MoveSelectDown, true);

        _inputManager.StartTrackingKey(Keys.Enter, ConfirmSelection);

        _inputManager.StartTrackingKey(Keys.Escape, () => MainGame.CurrentGameState = new MapGameState());
        _inputManager.StartTrackingKey(Keys.I, () => MainGame.CurrentGameState = new MapGameState());


    }

    private void ConfirmSelection()
    {
        if (_items.Count <= _selectPosition)
            return;
        
        var canUseResult = _items[_selectPosition].CanUse(MainGame.PlayerEntity);
        
        if (!canUseResult.CanUse)
        {
            _currentMessage = canUseResult.ErrorMessage ?? "UNKNOWN ERROR WHILE USING ITEM";
            return;
        }
        
        _items[_selectPosition].Use(MainGame.PlayerEntity);
        _currentMessage = $"Used: {_items[_selectPosition].Name}";
        _items.RemoveAt(_selectPosition);
    }

    private void MoveSelectDown()
    {
        if (_selectPosition >= _items.Count - 1)
            _selectPosition = 0;
        else
            _selectPosition++;
    }

    private void MoveSelectUp()
    {
        if (_selectPosition <= 0)
            _selectPosition = _items.Count - 1;
        else
            _selectPosition--;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        var font = MainGame.FontSystem.GetFont(22);
        const int xPosition = 120;
        var yPosition = 300;
        
        spriteBatch.DrawString(font, _currentMessage,new Vector2(xPosition, yPosition-84), Color.White);
        
        for (var index = 0; index < _items.Count; index++)
        {
            var item = _items[index];
            yPosition += 42;
            spriteBatch.DrawString(font, item.Name,
                new Vector2(xPosition, yPosition), _selectPosition == index ? Color.Blue : Color.White);
        }
    }

    public void Update(GameTime gameTime)
    {
        _inputManager.CheckInputAndAct(gameTime);
        _inputManager.UpdateStates(gameTime);
    }
}