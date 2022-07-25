using System;

namespace game.GameEngine.GameObjects.Npcs;

public class NpcFactory
{
    public Entity CreateNpc(NpcType npcType)
    {
        return npcType switch
        {
            NpcType.Zombie => Zombie.GenerateOnRandomPosition(),
            _ => throw new Exception("Not supported npc yet")
        };
    }
}