using System;
using game.GameEngine.Components;
using game.GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg_v2;
using Action = System.Action;

namespace game.GameEngine.GameStates;

public class TargetingGameState : IGameState
{
    
    private readonly InputManager _inputManager;
    private readonly Position _target;

    public TargetingGameState(Action<Position> onTargetSelected, Action onTargetCancelled, uint range)
    {
        var playerPosition = ((Position)MainGame.PlayerEntity.Components[EcsConsts.ECS_COMPONENT_INDEX_POSITION]);
        _target = new Position(){X = playerPosition.X, Y = playerPosition.Y};
        _inputManager = new InputManager();
        _inputManager.StartTrackingKey(Keys.Escape, onTargetCancelled);
        _inputManager.StartTrackingKey(Keys.Enter, () => onTargetSelected.Invoke(_target));
        _inputManager.StartTrackingKey(Keys.Left, () =>
        {
            if (_target.X > playerPosition.X - range)
            {
                _target.X--;
            }
        }, true);
        _inputManager.StartTrackingKey(Keys.Right, () =>
        {
            if (_target.X < playerPosition.X + range)
            {
                _target.X++;
            }
        }, true);
        _inputManager.StartTrackingKey(Keys.Up, () =>
        {
            if (_target.Y > playerPosition.Y - range)
            {
                _target.Y--;
            }
        },true);
        _inputManager.StartTrackingKey(Keys.Down, () =>
        {
            if (_target.Y < playerPosition.Y + range)
            {
                _target.Y++;
            }
        }, true);
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        DrawingSystem.Act(spriteBatch);
        Vector2 screenPos = CameraSystem.WorldToScreen(_target.X, _target.Y);
        spriteBatch.Draw(MainGame.SpriteAtlas, screenPos,
            new Rectangle(1 * 16, 1 * 16, 16, 16), Color.White);
    }

    public void Update(GameTime gameTime)
    {
        _inputManager.CheckInputAndAct(gameTime);
        _inputManager.UpdateStates(gameTime);
    }
}