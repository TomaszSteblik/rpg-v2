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
    private readonly int _range;

    private readonly InputManager _inputManager;
    private Position _target;
    private readonly Rectangle _targetingSpriteRectangle = new Rectangle(8 * 16, 0 * 16, 16, 16);
    private readonly Position _playerPosition;
    private readonly Vision _playerVision;
    
    public TargetingGameState(Action<Position> onTargetSelected, Action onTargetCancelled, int range)
    {
        _range = range;
        _playerPosition = ((Position)MainGame.PlayerEntity.Components[EcsConsts.ECS_COMPONENT_INDEX_POSITION]);
        _playerVision = ((Vision)MainGame.PlayerEntity.Components[EcsConsts.ECS_COMPONENT_INDEX_VISION]);
        _target = new Position(){X = _playerPosition.X, Y = _playerPosition.Y};
        _inputManager = new InputManager();
        InitInputManager(onTargetSelected, onTargetCancelled);
    }

    private void InitInputManager(Action<Position> onTargetSelected, Action onTargetCancelled)
    {
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
        if (Math.Round(PathfindingSystem.CalculateDistance(new Node(_playerPosition.X, _playerPosition.Y),
                new Node(newTarget.X, newTarget.Y))) <= _range &&
            newTarget.X < MainGame.MapSize && newTarget.Y < MainGame.MapSize)
            _target = newTarget;
    }
    
    public void Draw(SpriteBatch spriteBatch)
    {
        DrawingSystem.Act(spriteBatch);
        
        var startX = ((_playerPosition.X - _range >= 0) ? _playerPosition.X - _range : 0);
        var endX = ((_playerPosition.X + _range < MainGame.MapSize) ? _playerPosition.X + _range : MainGame.MapSize - 1);
        var startY = ((_playerPosition.Y - _range >= 0) ? _playerPosition .Y - _range : 0);
        var endY = ((_playerPosition.Y + _range < MainGame.MapSize) ? _playerPosition.Y + _range : MainGame.MapSize - 1);
        
        for (var x = startX; x <= endX; x++)
        {
            for (var y = startY; y <= endY; y++)
            {
                if (Math.Round(PathfindingSystem.CalculateDistance(new Node(_playerPosition.X, _playerPosition.Y),
                        new Node(x, y))) <= _range)
                {
                    var pos = CameraSystem.WorldToScreen(x,y);
                    spriteBatch.Draw(
                        MainGame.SpriteAtlas, 
                        pos, 
                        new Rectangle(0*16,11*16, 16,16),
                        (_playerVision.CellsInLightOfSight[x][y] ? Color.Green : Color.IndianRed) * 0.5f );
                }
            }
        }
        var screenPos = CameraSystem.WorldToScreen(_target.X, _target.Y);
        spriteBatch.Draw(MainGame.SpriteAtlas, 
            screenPos, 
            _targetingSpriteRectangle, 
            (_playerVision.CellsInLightOfSight[_target.X][_target.Y] ? Color.Gold : Color.Red));
    }

    public void Update(GameTime gameTime)
    {
        _inputManager.CheckInputAndAct(gameTime);
        _inputManager.UpdateStates(gameTime);
    }
}