using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using game.GameEngine.Components;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine
{
    public static class EcsManager
    {
        private static IDictionary<Guid, Entity> _entities;
        public static HashSet<Type> ComponentsTypes;
        public static int ComponentsCount => ComponentsTypes.Count;
        public static void Init()
        {
            _entities = new Dictionary<Guid, Entity>();
            ComponentsTypes = new HashSet<Type>();

            ComponentsTypes.Add(typeof(Position));      //0
            ComponentsTypes.Add(typeof(Sprite));        //1
            ComponentsTypes.Add(typeof(PlayerData));    //2
            ComponentsTypes.Add(typeof(Physics));       //3
            ComponentsTypes.Add(typeof(Vision));        //4
            ComponentsTypes.Add(typeof(Pathfinding));   //5
            ComponentsTypes.Add(typeof(Health));        //6
            ComponentsTypes.Add(typeof(Action));        //7
            ComponentsTypes.Add(typeof(EntityStates));  //8
        }

        public static ICollection<Entity> QueryEntitiesByComponentsIndexes(int[] componentsIndexes)
        {
            var entitiesThatMatchMask = new Collection<Entity>();

            foreach (var entity in _entities.Values)
            {
                var didMatchMask = true;
                for (var i = 0; i < componentsIndexes.Length; i++)
                {
                    if (entity.Mask[componentsIndexes[i]]) continue;
                    didMatchMask = false;
                    break;
                }
                if (didMatchMask)
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

            _entities.Add(entity.Guid, entity);


            return entity;
        }
        public static Entity RegisterNewEntity()
        {
            var entity = new Entity();

            _entities.Add(entity.Guid, entity);


            return entity;
        }

        public static void UnregisterEntity(Entity entity)
        {
            _entities.Remove(entity.Guid);
        }

        public static ICollection<Entity> GetAllEntities()
        {
            return _entities.Values;
        }

        public static Entity GetEntityByGuid(Guid guid)
        {
            return _entities[guid];
        }

        public static void LoadEntity(Entity entity)
        {
            var newEntity = RegisterNewEntity(entity.Guid);
            newEntity.Mask = entity.Mask;
            for (var i = 0; i < newEntity.Mask.Length; i++)
            {
                if (newEntity.Mask[i])
                    newEntity.Components[i] = entity.Components[i];
                else
                    newEntity.Components[i] = null;
            }
        }

        private static Entity RegisterNewEntity(Guid guid)
        {
            var entity = new Entity(guid);

            _entities.Add(entity.Guid, entity);

            return entity;
        }
    }
}