using System;
using System.Diagnostics;
using game.GameEngine.Components;
using game.GameEngine.Systems.Helpers;
using Microsoft.Xna.Framework;
using rpg_v2;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine.GameObjects.Npcs;

public class Zombie
{
    public static Entity GenerateOnRandomPosition()
    {
        var randomPosition = Map.GetRandomNotOccupiedPosition();
        return GenerateOnPosition(randomPosition.X,randomPosition.Y);
    }

    public static Entity GenerateOnPosition(int x, int y)
    {
        //register zombie
        var zombie = EcsManager.RegisterNewEntity(new[] {0, 1, 2, 3, 4, 5, 6, 7});
        
        Debug.WriteLine($"Zombie position: x: {x} y: {y}");
        
        var pos = (Position) zombie.Components[0];
        pos.X = x;
        pos.Y = y;
        var sprite = (Sprite) zombie.Components[1];
        sprite.AtlasPositionX = 10;
        sprite.AtlasPositionY = 5;
        sprite.Layer = 1;
        sprite.Color = Color.DarkGreen;
        sprite.IsVisibleOutOfSight = false;
        var physics = (Physics) zombie.Components[3];
        physics.IsCollidable = true;
        physics.BlocksVision = false;
        var health = (Health) zombie.Components[6];
        health.Hp = 12;

        var pathfinding = (Pathfinding) zombie.Components[5];
        var action = (Action) zombie.Components[7];
        var vision = (Vision) zombie.Components[4];
        vision.Sight = 5;

        pathfinding.NeedToFindNewPath = true;
        
        var randomPositionTarget = Map.GetRandomNotOccupiedPosition();

        pathfinding.TargetX = randomPositionTarget.X;
        pathfinding.TargetY = randomPositionTarget.Y;
        
        action.ActionType = ActionType.ZombieAction;
        action.Delay = MainGame.Random.Next(1,3);
        action.DelayStep = 0;

        var data = (PlayerData) zombie.Components[2];

        data.MeleeDamage = 4;
        data.IsPlayerCharacter = false;

        return zombie;
    }
}