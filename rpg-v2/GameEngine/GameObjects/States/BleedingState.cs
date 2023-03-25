using System;
using System.Text.Json.Serialization;
using game.GameEngine.Components;
using rpg_v2;

namespace game.GameEngine.GameObjects.States;

public class BleedingState : IState
{
    public void OnCreate(StateData data, Entity entity)
    {
        if (data is not BleedingData bleedingData)
            throw new Exception($"Incorrect data passed to Bleeding: {data}");

        bleedingData.Status = StateStatus.Active;
    }

    public void OnDestruct(StateData data, Entity entity)
    {
        if (data is not BleedingData bleedingData)
            throw new Exception($"Incorrect data passed to Bleeding: {data}");

        var health = (Health) entity.Components[6];
        health.Hp -= bleedingData.TickDamage;
    }

    public void Act(StateData data, Entity entity)
    {
        if (data is not BleedingData bleedingData)
            throw new Exception($"Incorrect data passed to Bleeding: {data}");

        var health = (Health) entity.Components[6];
        health.Hp -= bleedingData.TickDamage;

        bleedingData.CurrentTick++;

        if (bleedingData.CurrentTick >= bleedingData.MaxTick)
            bleedingData.Status = StateStatus.Destructed;
    }
}

public class BleedingData : StateData
{
    public int CurrentTick { get; set; }
    public int MaxTick { get; }
    public int TickDamage { get; }

    public override string OwnerType => typeof(BleedingState).ToString();

    public BleedingData(int maxTick, int tickDamage)
    {
        CurrentTick = 0;
        MaxTick = maxTick;
        TickDamage = tickDamage;
        Status = StateStatus.Created;
    }
    
}