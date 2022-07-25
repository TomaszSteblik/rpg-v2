using System;
using game.GameEngine.Components;
using game.GameEngine.GameObjects.Actions;
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
            InvokeAction(entity.Guid, action);
        }
    }

    private static void InvokeAction(Guid entityGuid, Action action)
    {
        switch (action.ActionType)
        {
            case ActionType.ZombieAction:
                ZombieAction.Act(entityGuid);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}