using System;

namespace game.GameEngine.GameObjects.States;

public interface IStateData
{
    public Type OwnerType { get; }
    public StateStatus Status { get; set; }
    
}

public enum StateStatus
{
    Created,
    Active,
    Destructed
}