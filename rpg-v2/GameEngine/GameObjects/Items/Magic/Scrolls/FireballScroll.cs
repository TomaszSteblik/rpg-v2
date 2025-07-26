using System;
using System.Linq;
using game.GameEngine.Components;
using game.GameEngine.GameStates;
using game.GameEngine.Systems.Helpers;
using rpg_v2;
using Serilog;

namespace game.GameEngine.GameObjects.Items.Magic.Scrolls;

public class FireballScroll : Item
{
    public override string Name => "Scroll of Fireball";
    public override string Description => "A scroll that allows you to cast a powerful fireball spell. It consumes mana when used.";
    public override int Capacity => 2;
    public override Rarity Rarity => Rarity.Rare;
    
    private readonly int _manaCost = 3;
    private readonly int _damage = 15;
    public override CanUseResult CanUse(Entity entity)
    {
        var mana = (Mana)MainGame.PlayerEntity.Components[EcsConsts.ECS_COMPONENT_INDEX_MANA];
        return mana.CurrentMana < _manaCost ? new CanUseResult(false, "Not enough mana to use this scroll.") : new CanUseResult(true);
    }

    public override void Use(Entity entity)
    {
        MainGame.CurrentGameState = new TargetingGameState(OnTargetSelected, OnTargetCancelled, range: 10);
    }


    private void OnTargetSelected(Position position)
    {
        var mana = (Mana)MainGame.PlayerEntity.Components[EcsConsts.ECS_COMPONENT_INDEX_MANA];
        mana.CurrentMana -= _manaCost;
        
        var entities = EcsQueries.GetEntitiesAtPosition(position.X, position.Y);

        foreach (var entity in entities.Where(e => e.Mask[EcsConsts.ECS_COMPONENT_INDEX_HEALTH]))
        {
            var health = (Health)entity.Components[EcsConsts.ECS_COMPONENT_INDEX_HEALTH];
            health.CurrentHp -= _damage;
        }
        
        Log.Information("Used '{Name}' for {ManaCost} mana.", Name, _manaCost);

        MainGame.CurrentGameState = new MapGameState();
    }

    private void OnTargetCancelled()
    {
        MainGame.CurrentGameState = new MapGameState();
    }
    
}