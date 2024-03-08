using game.GameEngine.Components;

namespace game.GameEngine.Systems
{
    public static class HealthSystem
    {
        public static void Act()
        {
            var entities = EcsManager.QueryEntitiesByComponentsIndexes(new[] { 6 });
            foreach (var entity in entities)
            {
                if (((Health)entity.Components[6]).CurrentHp <= 0)
                {
                    EcsManager.UnregisterEntity(entity);
                }
            }
        }
    }
}