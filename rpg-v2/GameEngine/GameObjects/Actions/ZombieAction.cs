using System;
using System.Diagnostics;
using game.GameEngine.Components;
using game.GameEngine.Systems.Helpers;
using rpg_v2;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine.GameObjects.Actions;

public static class ZombieAction
{
    public static void Act(Guid entityGuid)
    {
        var entity = EcsManager.GetEntityByGuid(entityGuid);
        
        var pathfinding = (Pathfinding) entity.Components[5];
        var vision = (Vision) entity.Components[4];
        var pos = (Position) entity.Components[0];
        var data = (PlayerData) entity.Components[2];
        var action = (Action) entity.Components[7];

        
        if (action.DelayStep >= action.Delay)
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
                    playerHealth.Hp -= data.MeleeDamage;
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

            action.DelayStep = 0;
        }
        else
        {
            action.DelayStep++;
        }
    }
}