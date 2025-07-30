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
    private readonly uint _range;

    private readonly InputManager _inputManager;
    private Position _target;
    private readonly Rectangle _targetingSpriteRectangle = new Rectangle(8 * 16, 0 * 16, 16, 16);

    public TargetingGameState(Action<Position> onTargetSelected, Action onTargetCancelled, uint range)
    {
        _range = range;
        var playerPosition = ((Position)MainGame.PlayerEntity.Components[EcsConsts.ECS_COMPONENT_INDEX_POSITION]);
        _target = new Position(){X = playerPosition.X, Y = playerPosition.Y};
        _inputManager = new InputManager();
        _inputManager.StartTrackingKey(Keys.Escape, onTargetCancelled);
        _inputManager.StartTrackingKey(Keys.Enter, () => onTargetSelected.Invoke(_target));
        _inputManager.StartTrackingKey(Keys.Left, () =>
            CheckDistanceAndMoveAction(new Position(){X = _target.X - 1, Y = _target.Y}), true);
        _inputManager.StartTrackingKey(Keys.Right, () =>
            CheckDistanceAndMoveAction(new Position(){X = _target.X + 1, Y = _target.Y}), true);
        _inputManager.StartTrackingKey(Keys.Up, () => 
            CheckDistanceAndMoveAction(new Position(){X = _target.X, Y = _target.Y - 1}),true);
        _inputManager.StartTrackingKey(Keys.Down, () => 
            CheckDistanceAndMoveAction(new Position(){X = _target.X, Y = _target.Y + 1}), true);
    }
    
    private void CheckDistanceAndMoveAction (Position newTarget)
    {
        var playerPosition = ((Position)MainGame.PlayerEntity.Components[EcsConsts.ECS_COMPONENT_INDEX_POSITION]);
        if (PathfindingSystem.CalculateDistance(new Node(playerPosition.X, playerPosition.Y),
                new Node(newTarget.X, newTarget.Y)) <= _range)
            _target = newTarget;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        DrawingSystem.Act(spriteBatch);
        var screenPos = CameraSystem.WorldToScreen(_target.X, _target.Y);
        spriteBatch.Draw(MainGame.SpriteAtlas, screenPos, _targetingSpriteRectangle, Color.Red);
    }

    public void Update(GameTime gameTime)
    {
        _inputManager.CheckInputAndAct(gameTime);
        _inputManager.UpdateStates(gameTime);
    }
}