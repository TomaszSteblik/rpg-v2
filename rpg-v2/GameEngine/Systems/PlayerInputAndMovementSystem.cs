using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using game.GameEngine.Components;
using game.GameEngine.GameStates;
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

                var collidablesAtPosition = collidables.Where(x =>
                    ((Position) x.Components[0]).X == ((Position) MainGame.PlayerEntity.Components[0]).X - 1
                    && ((Position) x.Components[0]).Y == ((Position) MainGame.PlayerEntity.Components[0]).Y).ToArray();

                if (!collidablesAtPosition.Any())
                {
                    ((Position) MainGame.PlayerEntity.Components[0]).X--;
                    return;
                }
                
                var collidablesAtPositionHittable = collidablesAtPosition.Where(x => x.Mask[6]);

                foreach (var entity in collidablesAtPositionHittable)
                {
                    var enemyHealth = (Health) entity.Components[6];
                    var playerData = (PlayerData) MainGame.PlayerEntity.Components[2];
                    enemyHealth.Hp -= playerData.MeleeDamage;
                }
                    
            },true);
            
            
            InputManager.StartTrackingKey(Keys.Up, () =>
            {
                var collidables = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0,3})
                    .Where(x=>((Physics) x.Components[3]).IsCollidable);
                
                var collidablesAtPosition = collidables.Where(x=>
                    ((Position) x.Components[0]).X == ((Position) MainGame.PlayerEntity.Components[0]).X 
                    && ((Position) x.Components[0]).Y == ((Position) MainGame.PlayerEntity.Components[0]).Y-1).ToArray();

                if (!collidablesAtPosition.Any())
                {
                    ((Position) MainGame.PlayerEntity.Components[0]).Y--;
                    return;
                }
          
                var collidablesAtPositionHittable = collidablesAtPosition.Where(x => x.Mask[6]);

                foreach (var entity in collidablesAtPositionHittable)
                {
                    var enemyHealth = (Health) entity.Components[6];
                    var playerData = (PlayerData) MainGame.PlayerEntity.Components[2];
                    enemyHealth.Hp -= playerData.MeleeDamage;
                }
                
            },true);
            
            
            InputManager.StartTrackingKey(Keys.Right, () =>
            {
                var collidables = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0,3})
                    .Where(x=>((Physics) x.Components[3]).IsCollidable);

                var collidablesAtPosition = collidables.Where(x =>
                    ((Position) x.Components[0]).X == ((Position) MainGame.PlayerEntity.Components[0]).X + 1
                    && ((Position) x.Components[0]).Y == ((Position) MainGame.PlayerEntity.Components[0]).Y).ToArray();

                if (!collidablesAtPosition.Any())
                {
                    ((Position) MainGame.PlayerEntity.Components[0]).X++;
                    return;
                }
                
                var collidablesAtPositionHittable = collidablesAtPosition.Where(x => x.Mask[6]);

                foreach (var entity in collidablesAtPositionHittable)
                {
                    var enemyHealth = (Health) entity.Components[6];
                    var playerData = (PlayerData) MainGame.PlayerEntity.Components[2];
                    enemyHealth.Hp -= playerData.MeleeDamage;
                }
                
            },true);
            
            
            InputManager.StartTrackingKey(Keys.Down, () =>
            {
                var collidables = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0,3})
                    .Where(x=>((Physics) x.Components[3]).IsCollidable);

                var collidablesAtPosition = collidables.Where(x =>
                    ((Position) x.Components[0]).X == ((Position) MainGame.PlayerEntity.Components[0]).X
                    && ((Position) x.Components[0]).Y == ((Position) MainGame.PlayerEntity.Components[0]).Y + 1).ToArray();

                if (!collidablesAtPosition.Any())
                {
                    ((Position) MainGame.PlayerEntity.Components[0]).Y++;
                    return;
                }
                    
                var collidablesAtPositionHittable = collidablesAtPosition.Where(x => x.Mask[6]);

                foreach (var entity in collidablesAtPositionHittable)
                {
                    var enemyHealth = (Health) entity.Components[6];
                    var playerData = (PlayerData) MainGame.PlayerEntity.Components[2];
                    enemyHealth.Hp -= playerData.MeleeDamage;
                }
                
            },true);
            
            InputManager.StartTrackingKey(Keys.S,  () =>
            {
                MainGame.CurrentGameState = new SaveMenuState();
            });
        }

        public static bool Act(GameTime gameTime)
        {
            var acted = InputManager.CheckInputAndAct(gameTime);
            InputManager.UpdateStates(gameTime);
            return acted;
        }
    }
}