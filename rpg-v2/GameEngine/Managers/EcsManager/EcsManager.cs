using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using game.GameEngine.Components;

namespace game.GameEngine
{
    public static class EcsManager
    {
        private static ICollection<Entity> _entities;
        public static HashSet<Type> ComponentsTypes;
        public static int ComponentsCount => ComponentsTypes.Count;
        public static void Init()
        {
            _entities = new List<Entity>();
            ComponentsTypes = new HashSet<Type>();

            ComponentsTypes.Add(typeof(Position));      //0
            ComponentsTypes.Add(typeof(Sprite));        //1
            ComponentsTypes.Add(typeof(PlayerData));    //2
            ComponentsTypes.Add(typeof(Physics));       //3
            ComponentsTypes.Add(typeof(Vision));        //4
            ComponentsTypes.Add(typeof(Pathfinding));   //5
            ComponentsTypes.Add(typeof(Health));        //6
        }

        public static ICollection<Entity> QueryEntitiesByComponentsIndexes(int[] componentsIndexes)
        {
            var entitiesThatMatchMask = new Collection<Entity>();

            foreach (var entity in _entities)
            {
                var didMatchMask = true;
                for (var i = 0; i < componentsIndexes.Length; i++)
                {
                    if (entity.Mask[componentsIndexes[i]]) continue;
                    didMatchMask = false;
                    break;
                }
                if(didMatchMask)
                    entitiesThatMatchMask.Add(entity);
            }
            
            //return entities
            return entitiesThatMatchMask;
        }

        public static Entity RegisterNewEntity(int[] componentsIndexes)
        {
            var entity = new Entity();

            for (var i = 0; i < componentsIndexes.Length; i++)
            {
                entity.Mask[componentsIndexes[i]] = true;
            }
            
            _entities.Add(entity);
            

            return entity;
        }
        public static Entity RegisterNewEntity()
        {
            var entity = new Entity();

            _entities.Add(entity);
            

            return entity;
        }

        public static void UnregisterEntity(Entity entity)
        {
            _entities.Remove(entity);
        }

        public static ICollection<Entity> GetAllEntities()
        {
            return _entities;
        }
    }
}