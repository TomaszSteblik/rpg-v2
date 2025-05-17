using System;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using game.GameEngine.Components;
using game.GameEngine.GameObjects.Items.Utility.Potions;
using game.GameEngine.GameObjects.Npcs;
using game.GameEngine.Systems.Helpers;
using Microsoft.Xna.Framework;
using rpg_v2;
using Serilog;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine
{
    public static class Map
    {

        public static void GenerateWallsAndFloors(int width, int height, int tunnels, int maxLength)
        {
            // Use the full map size instead of the function parameters
            var map = new bool[width][];
            for (var i = 0; i < width; i++)
            {
                map[i] = new bool[height];
                for (var j = 0; j < map[i].Length; j++)
                {
                    map[i][j] = true;
                }
            }

            var startingX = MainGame.Random.Next(1, height - 1);
            var startingY = MainGame.Random.Next(1, width - 1);

            MainGame.PlayerEntity = EcsManager.RegisterNewEntity(new[] { 0, 1, 3, 2, 4, 6, 8, 9 });

            var position = (Position)MainGame.PlayerEntity.Components[0];
            var spriteP = (Sprite)MainGame.PlayerEntity.Components[1];
            var vision = (Vision)MainGame.PlayerEntity.Components[4];
            var physicsPlayer = (Physics)MainGame.PlayerEntity.Components[3];
            var playerData = (PlayerData)MainGame.PlayerEntity.Components[2];
            var playerHealth = (Health)MainGame.PlayerEntity.Components[6];
            var playerInventory = (Inventory)MainGame.PlayerEntity.Components[9];
            playerHealth.CurrentHp = 30;
            position.X = startingX;
            position.Y = startingY;
            spriteP.Color = Color.White;
            spriteP.AtlasPositionX = 0;
            spriteP.AtlasPositionY = 4;
            spriteP.Layer = 1;
            vision.Sight = 10;
            physicsPlayer.IsCollidable = true;
            playerData.MeleeDamage = 4;
            playerData.IsPlayerCharacter = true;
            playerInventory.Items.Add(new SmallHealthPotion());

            var currentColumn = startingX;
            var currentRow = startingY;


            var directions = new int[][]
            {
                new int[] {-1, 0},
                new int[] {1, 0},
                new int[] {0, -1},
                new int[] {0, 1}
            };
            var lastDirection = new[] { 2, 2 };


            while (tunnels > 0)
            {
                int[] randomDirection;
                do
                {
                    randomDirection = directions[MainGame.Random.Next(0, directions.Length)];
                } while ((randomDirection[0] == -lastDirection[0] &&
                          randomDirection[1] == -lastDirection[1]) ||
                         (randomDirection[0] == lastDirection[0] &&
                          randomDirection[1] == lastDirection[1]));


                var randomLength = MainGame.Random.Next(1, maxLength);
                var tunnelLength = 0;

                
                while (tunnelLength < randomLength)
                {
                    if (((currentRow == 1) && (randomDirection[0] == -1)) ||
                        ((currentColumn == 1) && (randomDirection[1] == -1)) ||
                        ((currentRow >= width - 2) && (randomDirection[0] == 1)) ||
                        ((currentColumn >= height - 2) && (randomDirection[1] == 1)))
                    {
                        break;
                    }

                    map[currentRow][currentColumn] = false;
                    currentRow += randomDirection[0];
                    currentColumn += randomDirection[1];
                    tunnelLength++;
                }

                if (tunnelLength < 1) continue;
                lastDirection = randomDirection;
                tunnels--;
            }


            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j])
                    {

                        var e = EcsManager.RegisterNewEntity(new[] { 0, 1, 3 });
                        var pos = (e.Components[0] as Position);
                        pos.X = j;
                        pos.Y = i;
                        var sprite = (e.Components[1] as Sprite);
                        sprite.AtlasPositionX = 0;
                        sprite.AtlasPositionY = 11;
                        var physics = (Physics)e.Components[3];
                        physics.IsCollidable = true;
                        physics.BlocksVision = true;
                        var visionA = e.Components[4] as Vision;
                        visionA.VisitedCells = null;
                        visionA.CellsInLightOfSight = null;
                    }
                    else
                    {

                        var e = EcsManager.RegisterNewEntity(new[] { 0, 1, 3 });
                        var pos = (e.Components[0] as Position);
                        pos.X = j;
                        pos.Y = i;
                        var sprite = (e.Components[1] as Sprite);
                        sprite.AtlasPositionX = 10;
                        sprite.AtlasPositionY = 15;
                        var physics = (Physics)e.Components[3];
                        physics.IsCollidable = false;
                        physics.BlocksVision = false;
                        var visionA = e.Components[4] as Vision;
                        visionA.VisitedCells = null;
                        visionA.CellsInLightOfSight = null;
                    }
                }
            }
            
            for (int i = 0; i < 100; i++)
            {
                Zombie.GenerateOnRandomPosition();
            }
            Log.Information("Game started");
        }

        public static bool IsPositionOccupiedByCollidableEntity(int x, int y)
        {
            var positions = EcsQueries.GetCollidablePositions();

            for (int i = 0; i < positions.Length; i++)
            {
                if (positions[i].X == x && positions[i].Y == y) return true;
            }
            
            return false;
        }

        public static Position GetRandomNotOccupiedPosition(int searchRange = Int32.MaxValue , int x = 0 , int y = 0)
        {
            var random = MainGame.Random;

            // var entities = EcsManager.QueryEntitiesByComponentsIndexes(new[] { 0, 3 }).ToImmutableArray();
            //
            // var freePositions = entities
            
            //     .Select(x=> ((Position)x.Components[0]))
            //     .ToHashSet();
            //
            // var occupiedPositions = entities
            //     .Where(z => ((Physics)z.Components[3]).IsCollidable == true)
            //     .Select(x=> ((Position)x.Components[0]))
            //     .ToHashSet();
            //
            // var toChoose = freePositions.Except(occupiedPositions).ToArray();

            // while (true)
            // {
            //
            //
            //     var entity = freePositions[random.Next(freePositions.Count)];
            //     var position = (Position)freePositions[random.Next(freePositions.Count)].Components[0];
            //     
            //     if (Map.IsPositionOccupiedByCollidableEntity(position.X, position.Y) is false)
            //     {
            //         return new Position() { X = position.X, Y = position.Y };
            //     }
            //
            //     freePositions.Remove(entity);
            // }
            
            var positions = EcsQueries.GetFreePositions()
                .Where(
                    z => 
                         z.X > x - searchRange &&
                         z.X < x + searchRange &&
                         z.Y > y - searchRange &&
                         z.Y < y + searchRange).ToArray();
            var chosen = positions[random.Next(positions.Length)];
            return chosen;
            
        }
    }
}