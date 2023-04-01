using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using game.GameEngine.Shared;

namespace game.GameEngine.GameObjects.States;

[JsonConverter(typeof(SerializationSomConverter<StateData>))]
public abstract class StateData
{
    public abstract string OwnerType { get; }
    public StateStatus Status { get; set; }
}

public enum StateStatus
{
    Created,
    Active,
    Destructed
}