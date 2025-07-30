using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using game.GameEngine.Components;

namespace game.GameEngine.Systems.Helpers;

public static class EcsQueries
{
    private static Position[] _collidablesAtPosition = null;
    private static ulong _collidablesAtPositionVersion = ulong.MaxValue;
    public static Position[] GetCollidablePositions()
    {
        if (EcsManager.Version != _collidablesAtPositionVersion)
        {
            _collidablesAtPosition = EcsManager.QueryEntitiesByComponentsIndexes(new[] { EcsConsts.ECS_COMPONENT_INDEX_POSITION, EcsConsts.ECS_COMPONENT_INDEX_PHYSICS })
                .Where(x => ((Physics)x.Components[EcsConsts.ECS_COMPONENT_INDEX_PHYSICS]).IsCollidable)
                .Select(x => (Position)x.Components[EcsConsts.ECS_COMPONENT_INDEX_POSITION])
                .ToArray();
            _collidablesAtPositionVersion = EcsManager.Version;
        }
        
        return _collidablesAtPosition;
    }
    
    private static Position[] _freePositions = null;
    private static ulong _freePositionsVersion = ulong.MaxValue;
    public static Position[] GetFreePositions()
    {
        if (EcsManager.Version != _freePositionsVersion)
        {
            var freePositions = EcsManager.QueryEntitiesByComponentsIndexes(new[] { EcsConsts.ECS_COMPONENT_INDEX_POSITION, EcsConsts.ECS_COMPONENT_INDEX_PHYSICS })
                .Where(x => ((Physics)x.Components[EcsConsts.ECS_COMPONENT_INDEX_PHYSICS]).IsCollidable is false)
                .Select(x => (Position)x.Components[EcsConsts.ECS_COMPONENT_INDEX_POSITION])
                .ToArray();

            _freePositions = freePositions.Except(GetCollidablePositions()).ToArray();
            
            _freePositionsVersion = EcsManager.Version;
        }
        
        return _freePositions;
    }
    
    public static Entity[] GetEntitiesAtPosition(int x, int y)
    {
        return EcsManager.QueryEntitiesByComponentsIndexes(new[] { EcsConsts.ECS_COMPONENT_INDEX_POSITION })
            .Where(entity => 
            {
                var position = (Position)entity.Components[EcsConsts.ECS_COMPONENT_INDEX_POSITION];
                return position.X == x && position.Y == y;
            })
            .ToArray();
    }
}