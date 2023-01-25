using System;
using game.GameEngine.Components;
using game.GameEngine.GameObjects.States;

namespace game.GameEngine.Systems;

public static class StatesSystem
{
    public static void Act()
    {
        var entities = EcsManager.QueryEntitiesByComponentsIndexes(new[] {8});
        foreach (var entity in entities)
        {
            var states = (EntityStates) entity.Components[8];
            foreach (var stateData in states.Data)
            {
                var state = Activator.CreateInstance(stateData.OwnerType) as IState ?? 
                            throw new Exception("Failed to create state");

                switch(stateData.Status)
                {
                    case StateStatus.Created:
                        state.OnCreate(stateData);
                        break;
                    case StateStatus.Active:
                        state.Act(stateData);
                        break;
                    case StateStatus.Destructed:
                        state.OnDestruct(stateData);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                };
            }

            states.Data.RemoveAll(data => data.Status is StateStatus.Destructed);
        }
    }
}