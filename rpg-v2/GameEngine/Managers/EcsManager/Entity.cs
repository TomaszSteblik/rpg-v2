using System;
using System.Text.Json.Serialization;

namespace game.GameEngine
{
    public class Entity
    {
        public Guid Guid { get; set; }
        
        [JsonInclude]
        public Component[] Components;
        
        [JsonInclude]
        public bool[] Mask;
        

        public Entity()
        {
            Guid = Guid.NewGuid();

            InitEntity();
        }

        public Entity(Guid guid)
        {
            Guid = guid;
            InitEntity();
        }
        
        private void InitEntity()
        {
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