using System;
using game.GameEngine.Components;
using Microsoft.Xna.Framework;
using rpg_v2;

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
            
            MainGame.PlayerEntity = EcsManager.RegisterNewEntity(new[] {0, 1, 2,4});

            var position = (Position) MainGame.PlayerEntity.Components[0];
            var spriteP = (Sprite) MainGame.PlayerEntity.Components[1];
            var vision = (Vision) MainGame.PlayerEntity.Components[4];
            position.X = startingX;
            position.Y = startingY;
            spriteP.Color = Color.White;
            spriteP.AtlasPositionX = 0;
            spriteP.AtlasPositionY = 4;
            spriteP.Layer = 1;
            vision.Sight = 10;

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

            var random = new Random();
            while (true)
            {
                var x = random.Next(1, maxLength - 1);
                var y = random.Next(1, maxLength - 1);
                if (!map[x][y])
                {
                    //register zombie
                    var zombie = EcsManager.RegisterNewEntity(new[] {0, 1, 3, 4, 5});
                    
                    var pos = (zombie.Components[0] as Position);
                    pos.X = x;
                    pos.Y = y;
                    var sprite = (zombie.Components[1] as Sprite);
                    sprite.AtlasPositionX = 10;
                    sprite.AtlasPositionY = 5;
                    sprite.Layer = 1;
                    sprite.Color = Color.DarkGreen;
                    var physics = (Physics) zombie.Components[3];
                    physics.IsCollidable = true;
                    physics.BlocksVision = false;
                    
                    break;
                }
            }

        }
    }
}