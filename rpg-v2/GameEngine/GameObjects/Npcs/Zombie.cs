using System;
using System.Diagnostics;
using System.Linq;
using game.GameEngine.Components;
using game.GameEngine.Systems.Helpers;
using Microsoft.Xna.Framework;
using rpg_v2;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine.GameObjects.Npcs;

public class Zombie
{
    public static void GenerateOnRandomPosition()
    {
        var randomPosition = Map.GetRandomNotOccupiedPosition();
        //register zombie
        GenerateOnPosition(randomPosition.X,randomPosition.Y);
    }

    public static void GenerateOnPosition(int x, int y)
    {
        //register zombie
        var zombie = EcsManager.RegisterNewEntity(new[] {0, 1, 3, 4, 5, 6, 7});
        
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
        
        var delay = MainGame.Random.Next(1,3);
        var meleeDamageZombie = 4;
        var delayStep = 0;
        //TODO:ATTACK PLAYER WHEN COLLIDING WITH HIM
        action.EntityAction = () =>
        {
            if (delayStep >= delay)
            {
                var playerPosition = (Position) MainGame.PlayerEntity.Components[0];
                var isPlayerVisible = VisionHelpers.IsPositionInFov(vision, playerPosition);
                
                if (pathfinding.Step >= pathfinding.Path.Count)
                {
                    pathfinding.NeedToFindNewPath = true;
                
                    var randomPositionTargetLambda = Map.GetRandomNotOccupiedPosition();

                    pathfinding.TargetX = isPlayerVisible ? playerPosition.X : randomPositionTargetLambda.X;
                    pathfinding.TargetY = isPlayerVisible ? playerPosition.Y :randomPositionTargetLambda.Y;
                    return;
                }

                if (Map.IsPositionOccupiedByCollidableEntity(pathfinding.Path[pathfinding.Step].X,
                        pathfinding.Path[pathfinding.Step].Y) is false)
                {
                    if (isPlayerVisible)
                    {
                        pathfinding.NeedToFindNewPath = true;


                        pathfinding.TargetX = playerPosition.X;
                        pathfinding.TargetY = playerPosition.Y;
                    }
                    
                    pos.X = pathfinding.Path[pathfinding.Step].X;
                    pos.Y = pathfinding.Path[pathfinding.Step].Y;
                    pathfinding.Step++;
                    
                    
                }
                else
                {
                    if (pathfinding.Path[pathfinding.Step].X == playerPosition.X &&
                        pathfinding.Path[pathfinding.Step].Y == playerPosition.Y)
                    {
                        var playerHealth = (Health) MainGame.PlayerEntity.Components[6];
                        playerHealth.Hp -= meleeDamageZombie;
                        Debug.WriteLine("player dmged");
                    }
                    else
                    {
                        pathfinding.NeedToFindNewPath = true;
                
                        var randomPositionTargetLambda = Map.GetRandomNotOccupiedPosition();

                        pathfinding.TargetX = isPlayerVisible ? playerPosition.X : randomPositionTargetLambda.X;
                        pathfinding.TargetY = isPlayerVisible ? playerPosition.Y :randomPositionTargetLambda.Y;
                    }
                }

                delayStep = 0;
            }
            else
            {
                delayStep++;
            }
        };

    }
}