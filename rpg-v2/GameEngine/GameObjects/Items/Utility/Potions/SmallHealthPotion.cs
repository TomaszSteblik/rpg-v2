using System;
using game.GameEngine.Components;

namespace game.GameEngine.GameObjects.Items.Utility.Potions;

public class SmallHealthPotion : Item
{
    public override string Name => "Small healing potion";
    public override string Description => "There appears to be some red fluid on the bottom of a small flask.";

    public int HealingAmount { get; init; }
    public override int Capacity => 3;

    public SmallHealthPotion()
    {
        HealingAmount = Random.Shared.Next(1,4);
    }

    public override CanUseResult CanUse(Entity entity)
    {
        var health = (Health) entity.Components[6];
        return health.CurrentHp < health.MaxHp
            ? new CanUseResult(true)
            : new CanUseResult(false, "Current hp need to be below max hp");
    }

    public override void Use(Entity entity)
    {
        var health = (Health) entity.Components[6];
        health.CurrentHp += HealingAmount;
    }
}