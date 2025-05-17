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
            _collidablesAtPosition = EcsManager.QueryEntitiesByComponentsIndexes(new[] { 0, 3 })
                .Where(x => ((Physics)x.Components[3]).IsCollidable)
                .Select(x => (Position)x.Components[0])
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
            var freePositions = EcsManager.QueryEntitiesByComponentsIndexes(new[] { 0, 3 })
                .Where(x => ((Physics)x.Components[3]).IsCollidable is false)
                .Select(x => (Position)x.Components[0])
                .ToArray();

            _freePositions = freePositions.Except(GetCollidablePositions()).ToArray();
            
            _freePositionsVersion = EcsManager.Version;
        }
        
        return _freePositions;
    }
}