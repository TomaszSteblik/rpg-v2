using System;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine.Systems;

public static class ActionSystem
{
    public static void Act()
    {
        var entities = EcsManager.QueryEntitiesByComponentsIndexes(new[] {7});
        foreach (var entity in entities)
        {
            var action = (Action) entity.Components[7];
            action.EntityAction.Invoke();
        }
    }
}