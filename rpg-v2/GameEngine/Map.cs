using System;
using System.Diagnostics;
using System.Linq;
using game.GameEngine.Components;
using game.GameEngine.GameObjects.Npcs;
using Microsoft.Xna.Framework;
using rpg_v2;
using Action = game.GameEngine.Components.Action;

namespace game.GameEngine
{
    public static class Map
    {

        public static void GenerateWallsAndFloors(int size, int tunnels, int maxLength)
        {
            var map = new bool[size][];
            for (var i = 0; i < size; i++)
            {
                map[i] = new bool[size];
                for (var j = 0; j < map[i].Length; j++)
                {
                    map[i][j] = true;
                }
            }

            var startingX = MainGame.Random.Next(1, size-1);
            var startingY = MainGame.Random.Next(1, size-1);
            
            MainGame.PlayerEntity = EcsManager.RegisterNewEntity(new[] {0, 1, 3, 2, 4, 6, 8});

            var position = (Position) MainGame.PlayerEntity.Components[0];
            var spriteP = (Sprite) MainGame.PlayerEntity.Components[1];
            var vision = (Vision) MainGame.PlayerEntity.Components[4];
            var physicsPlayer = (Physics) MainGame.PlayerEntity.Components[3];
            var playerData = (PlayerData) MainGame.PlayerEntity.Components[2];
            var playerHealth = (Health) MainGame.PlayerEntity.Components[6];
            playerHealth.Hp = 30;
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

            var currentColumn = startingX;
            var currentRow = startingY;


            var directions = new int[][]
            {
                new int[] {-1, 0},
                new int[] {1, 0},
                new int[] {0, -1},
                new int[] {0, 1}
            };
            var lastDirection = new[] {2, 2};


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


                var sizeMOne = size - 2;

                while (tunnelLength < randomLength)
                {
                    if (((currentRow == 1) && (randomDirection[0] == -1)) ||
                        ((currentColumn == 1) && (randomDirection[1] == -1)) ||
                        ((currentRow >= sizeMOne) && (randomDirection[0] == 1)) ||
                        ((currentColumn >= sizeMOne) && (randomDirection[1] == 1)))
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


            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j])
                    {

                        var e = EcsManager.RegisterNewEntity(new[] {0, 1,3});
                        var pos = (e.Components[0] as Position);
                        pos.X = j;
                        pos.Y = i;
                        var sprite = (e.Components[1] as Sprite);
                        sprite.AtlasPositionX = 0;
                        sprite.AtlasPositionY = 11;
                        var physics = (Physics) e.Components[3];
                        physics.IsCollidable = true;
                        physics.BlocksVision = true;
                        var visionA = e.Components[4] as Vision;
                        visionA.VisitedCells = null;
                        visionA.CellsInLightOfSight = null;
                    }
                    else
                    {

                        var e = EcsManager.RegisterNewEntity(new[] {0, 1,3});
                        var pos = (e.Components[0] as Position);
                        pos.X = j;
                        pos.Y = i;
                        var sprite = (e.Components[1] as Sprite);
                        sprite.AtlasPositionX = 10;
                        sprite.AtlasPositionY = 15;
                        var physics = (Physics) e.Components[3];
                        physics.IsCollidable = false;
                        physics.BlocksVision = false;
                        var visionA = e.Components[4] as Vision;
                        visionA.VisitedCells = null;
                        visionA.CellsInLightOfSight = null;
                    }
                }
            }
            
            Zombie.GenerateOnRandomPosition();
            Zombie.GenerateOnRandomPosition();
            Zombie.GenerateOnRandomPosition();
            Zombie.GenerateOnRandomPosition();
            Zombie.GenerateOnRandomPosition();
            Zombie.GenerateOnRandomPosition();

        }

        public static bool IsPositionOccupiedByCollidableEntity(int x,int y)
        {
            var entites = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0, 3});

            return entites.Any(z => ((Position) z.Components[0]).X == x 
                                    && ((Position) z.Components[0]).Y == y 
                                    && ((Physics) z.Components[3]).IsCollidable == true);

        }

        public static Position GetRandomNotOccupiedPosition()
        {
            var random = MainGame.Random;

            int x, y;
        
            var entites = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0, 3});
            var positions =  entites.Where(z => ((Physics) z.Components[3]).IsCollidable == false).ToList();
        
            while (true)
            {
            

                var entity = positions[random.Next(positions.Count)];
                var position = (Position) positions[random.Next(positions.Count)].Components[0];

            
                if (Map.IsPositionOccupiedByCollidableEntity(position.X, position.Y) is false)
                {
                    x = position.X;
                    y = position.Y;
                    break;
                }
            
                positions.Remove(entity);
            }

            return new Position() {X = x, Y = y};
        }
    }
}