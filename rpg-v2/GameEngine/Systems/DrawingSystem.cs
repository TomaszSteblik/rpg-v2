using System.Collections.Generic;
using System.Linq;
using game.GameEngine.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using rpg_v2;

namespace game.GameEngine.Systems
{
    public static class DrawingSystem
    {
        public static void Act(SpriteBatch spriteBatch)
        {
            var entities = EcsManager.QueryEntitiesByComponentsIndexes(new[] {0, 1});

            var layers = entities.GroupBy(x => ((Sprite) x.Components[1]).Layer);
            foreach (var layer in layers.OrderBy(x=>x.Key))
            {
                DrawLayer(layer.AsEnumerable(),spriteBatch);
            }

            
            
        }

        private static void DrawEntity(SpriteBatch spriteBatch, Position position, Sprite sprite, Color color)
        {
            spriteBatch.Draw(MainGame.SpriteAtlas,new Vector2(position.X*16,position.Y*16),
                new Rectangle(11*16,13*16,16,16),Color.Black);
            spriteBatch.Draw(MainGame.SpriteAtlas,new Vector2(position.X*16,position.Y*16),
                new Rectangle(sprite.AtlasPositionX*16,sprite.AtlasPositionY*16,16,16), color);
        }

        private static void DrawLayer(IEnumerable<Entity> entities, SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                
                var position = entity.Components[0] as Position;
                var sprite = entity.Components[1] as Sprite;

                var playerVision = (Vision) MainGame.PlayerEntity.Components[4];
                //TODO:JUST FOR TESTING THIS IS
                //if (playerVision.CellsInLightOfSight[position.X][position.Y])
                if (true)
                {
                    DrawEntity(spriteBatch,position,sprite,sprite.Color);
                }
                else if(playerVision.VisitedCells[position.X][position.Y])
                {
                    if (sprite.IsVisibleOutOfSight)
                    {
                        DrawEntity(spriteBatch,position,sprite,Color.Gray);
                    }
                    else
                    {
                        spriteBatch.Draw(MainGame.SpriteAtlas,new Vector2(position.X*16,position.Y*16),
                            new Rectangle(11*16,13*16,16,16),Color.Black);
                        spriteBatch.Draw(MainGame.SpriteAtlas,new Vector2(position.X*16,position.Y*16),
                            new Rectangle(10*16,15*16,16,16),Color.Gray);
                    }
                    
                }
                else
                {
                    spriteBatch.Draw(MainGame.SpriteAtlas,new Vector2(position.X*16,position.Y*16),
                        new Rectangle(11*16,13*16,16,16),Color.Black);
                }
                
            }
        }
    }
}