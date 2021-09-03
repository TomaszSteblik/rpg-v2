using System;
using System.Collections.Generic;
using System.Reflection;

namespace game.GameEngine
{
    public class Entity
    {
        public Guid Guid { get; set; }
        
        public Component[] Components;
        public bool[] Mask;
        

        public Entity()
        {
            Guid = Guid.NewGuid();
            
            //init mask, all entries at false
            Mask = new bool[EcsManager.ComponentsCount];
            
            //initialize components using reflections
            Components = new Component[EcsManager.ComponentsCount];
            var count = 0;
            foreach (var componentType in EcsManager.ComponentsTypes)
            {
                Components[count] = Activator.CreateInstance(componentType) as Component;
                count++;
            }
        }
    }
}