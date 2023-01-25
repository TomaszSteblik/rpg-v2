using System.Collections.Generic;
using game.GameEngine.GameObjects.States;

namespace game.GameEngine.Components;

public class EntityStates : Component
{
    public List<IStateData> Data { get; set; }

    public EntityStates()
    {
        Data = new List<IStateData>();
    }
    
}