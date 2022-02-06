using System;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using game.GameEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using rpg_v2;

namespace game.GameEngine.Systems
{
    public static class PlayerInputAndMovementSystem
    {
        private static InputManager InputManager = new InputManager();
        static PlayerInputAndMovementSystem()
        {
            InputManager.StartTrackingKey(Keys.Left, () =>
            {
                var collidables = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0,3})
                    .Where(x=>((Physics) x.Components[3]).IsCollidable);
                
                if(!collidables.Any(x=>((Position) x.Components[0]).X == ((Position) MainGame.PlayerEntity.Components[0]).X-1 
                                       && ((Position) x.Components[0]).Y == ((Position) MainGame.PlayerEntity.Components[0]).Y))
                    ((Position) MainGame.PlayerEntity.Components[0]).X--;
                    
                Debug.WriteLine($"Player position: x: {((Position) MainGame.PlayerEntity.Components[0]).X} y: {((Position) MainGame.PlayerEntity.Components[0]).Y}");

            },true);
            
            
            InputManager.StartTrackingKey(Keys.Up, () =>
            {
                var collidables = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0,3})
                    .Where(x=>((Physics) x.Components[3]).IsCollidable);
                
                if(!collidables.Any(x=>((Position) x.Components[0]).X == ((Position) MainGame.PlayerEntity.Components[0]).X 
                                       && ((Position) x.Components[0]).Y == ((Position) MainGame.PlayerEntity.Components[0]).Y-1))
                    ((Position) MainGame.PlayerEntity.Components[0]).Y--;
          
                Debug.WriteLine($"Player position: x: {((Position) MainGame.PlayerEntity.Components[0]).X} y: {((Position) MainGame.PlayerEntity.Components[0]).Y}");

                
            },true);
            
            
            InputManager.StartTrackingKey(Keys.Right, () =>
            {
                var collidables = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0,3})
                    .Where(x=>((Physics) x.Components[3]).IsCollidable);
                
                if(!collidables.Any(x=>((Position) x.Components[0]).X == ((Position) MainGame.PlayerEntity.Components[0]).X+1 
                                       && ((Position) x.Components[0]).Y == ((Position) MainGame.PlayerEntity.Components[0]).Y))
                ((Position) MainGame.PlayerEntity.Components[0]).X++;
                
            
                Debug.WriteLine($"Player position: x: {((Position) MainGame.PlayerEntity.Components[0]).X} y: {((Position) MainGame.PlayerEntity.Components[0]).Y}");

                
            },true);
            
            
            InputManager.StartTrackingKey(Keys.Down, () =>
            {
                var collidables = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0,3})
                    .Where(x=>((Physics) x.Components[3]).IsCollidable);
                
                if(!collidables.Any(x=>((Position) x.Components[0]).X == ((Position) MainGame.PlayerEntity.Components[0]).X
                                       && ((Position) x.Components[0]).Y == ((Position) MainGame.PlayerEntity.Components[0]).Y+1))
                    ((Position) MainGame.PlayerEntity.Components[0]).Y++;
            
                Debug.WriteLine($"Player position: x: {((Position) MainGame.PlayerEntity.Components[0]).X} y: {((Position) MainGame.PlayerEntity.Components[0]).Y}");

            },true);
            
            InputManager.StartTrackingKey(Keys.S, () =>
            {
                //saving here
                var entities = EcsManager.GetAllEntities();
                var serialized = JsonSerializer.Serialize(entities,new JsonSerializerOptions()
                {
                });
                Console.WriteLine();
            });
        }

        public static void Act(GameTime gameTime)
        {
            InputManager.CheckInputAndAct(gameTime);
            InputManager.UpdateStates(gameTime);
        }
    }
}