using System;
using System.Diagnostics;
using game.GameEngine.Components;
using game.GameEngine.GameObjects.States;
using game.GameEngine.Systems.Helpers;
using rpg_v2;
using Serilog;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine.GameObjects.Actions;

public static class ZombieAction
{
    public static void Act(Guid entityGuid)
    {
        var entity = EcsManager.GetEntityByGuid(entityGuid);

        var pathfinding = (Pathfinding)entity.Components[5];
        var vision = (Vision)entity.Components[4];
        var pos = (Position)entity.Components[0];
        var data = (PlayerData)entity.Components[2];
        var action = (Action)entity.Components[7];


        if (action.DelayStep >= action.Delay)
        {
            var playerPosition = (Position)MainGame.PlayerEntity.Components[0];
            var isPlayerVisible = VisionHelpers.IsPositionInFov(vision, playerPosition);

            if (pathfinding.Step >= pathfinding.Path.Count)
            {
                pathfinding.NeedToFindNewPath = true;
                var randomPositionTargetLambda = Map.GetRandomNotOccupiedPosition(100, pos.X, pos.Y);

                pathfinding.TargetX = isPlayerVisible ? playerPosition.X : randomPositionTargetLambda.X;
                pathfinding.TargetY = isPlayerVisible ? playerPosition.Y : randomPositionTargetLambda.Y;
                Log.Debug("{EntityId}: Need to find new path to X:{TargetX} Y:{TargetY}, from X:{CurrentX} Y: {CurrentY}", entityGuid, pathfinding.TargetX, pathfinding.TargetY, pos.X, pos.Y);

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
                Log.Debug("{EntityId}: Moved to X:{TargetX} Y:{TargetY}, from X:{CurrentX} Y: {CurrentY}", entityGuid, pathfinding.Path[pathfinding.Step].X, pathfinding.Path[pathfinding.Step].Y, pos.X, pos.Y);

                pathfinding.Step++;

            }
            else
            {
                if (pathfinding.Path[pathfinding.Step].X == playerPosition.X &&
                    pathfinding.Path[pathfinding.Step].Y == playerPosition.Y)
                {
                    var playerHealth = (Health)MainGame.PlayerEntity.Components[6];
                    playerHealth.CurrentHp -= data.MeleeDamage;
                    Log.Information("player dmged");
                    var rollIfBleed = Random.Shared.Next(0, 100);
                    if (rollIfBleed <= 10)
                    {
                        var playerStates = (EntityStates)MainGame.PlayerEntity.Components[8];
                        playerStates.Data.Add(new BleedingData(Random.Shared.Next(3, 5), 1));
                        Log.Information("zombie applied bleed to player");
                    }
                }
                else
                {
                    pathfinding.NeedToFindNewPath = true;
                    Log.Debug("ZombieAction: Need to find new path");

                    var randomPositionTargetLambda = Map.GetRandomNotOccupiedPosition(100, pos.X, pos.Y);

                    pathfinding.TargetX = isPlayerVisible ? playerPosition.X : randomPositionTargetLambda.X;
                    pathfinding.TargetY = isPlayerVisible ? playerPosition.Y : randomPositionTargetLambda.Y;
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