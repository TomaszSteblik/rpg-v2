using System.Text.Json.Serialization;
using game.GameEngine.Shared;

namespace game.GameEngine.GameObjects.Items;

[JsonConverter(typeof(SerializationSomConverter<Item>))]
public abstract class Item
{
    public abstract string Name { get; }
    public abstract string Description { get; }
    public abstract CanUseResult CanUse(Entity entity);
    public abstract void Use(Entity entity);
    public abstract int Capacity { get; }
    public abstract Rarity Rarity { get; }
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}