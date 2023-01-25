using System;
using game.GameEngine.Components;
using rpg_v2;

namespace game.GameEngine.GameObjects.States;

public class BleedingState : IState
{
    public void OnCreate(IStateData data)
    {
        if (data is not BleedingData bleedingData)
            throw new Exception($"Incorrect data passed to Bleeding: {data}");

        bleedingData.Status = StateStatus.Active;
    }

    public void OnDestruct(IStateData data)
    {
        if (data is not BleedingData bleedingData)
            throw new Exception($"Incorrect data passed to Bleeding: {data}");
        bleedingData.Health.Hp -= bleedingData.TickDamage;
    }

    public void Act(IStateData data)
    {
        if (data is not BleedingData bleedingData)
            throw new Exception($"Incorrect data passed to Bleeding: {data}");

        bleedingData.Health.Hp -= bleedingData.TickDamage;

        bleedingData.CurrentTick++;

        if (bleedingData.CurrentTick >= bleedingData.MaxTick)
            bleedingData.Status = StateStatus.Destructed;
    }
}

public class BleedingData : IStateData
{
    public Health Health { get; }
    public int CurrentTick { get; set; }
    public int MaxTick { get; }
    public int TickDamage { get; }
    
    public Type OwnerType => typeof(BleedingState);
    public StateStatus Status { get; set; }

    public BleedingData(Health health, int maxTick, int tickDamage)
    {
        Health = health;
        CurrentTick = 0;
        MaxTick = maxTick;
        TickDamage = tickDamage;
        Status = StateStatus.Created;
    }

}