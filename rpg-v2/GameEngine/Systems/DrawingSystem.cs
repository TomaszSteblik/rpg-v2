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
            var entities = EcsManager.QueryEntitiesByComponentsIndexes(new[] { 0, 1 });

            // Filter entities to only those in viewport
            var visibleEntities = entities.Where(entity => 
            {
                var position = (Position)entity.Components[0];
                return CameraSystem.IsInViewport(position.X, position.Y);
            });

            var layers = visibleEntities.GroupBy(x => ((Sprite)x.Components[1]).Layer);
            foreach (var layer in layers.OrderBy(x => x.Key))
            {
                DrawLayer(layer.AsEnumerable(), spriteBatch);
            }
        }

        private static void DrawEntity(SpriteBatch spriteBatch, Position position, Sprite sprite, Color color)
        {
            // Convert world position to screen position
            Vector2 screenPos = CameraSystem.WorldToScreen(position.X, position.Y);
            
            spriteBatch.Draw(MainGame.SpriteAtlas, screenPos,
                new Rectangle(11 * 16, 13 * 16, 16, 16), Color.Black);
            spriteBatch.Draw(MainGame.SpriteAtlas, screenPos,
                new Rectangle(sprite.AtlasPositionX * 16, sprite.AtlasPositionY * 16, 16, 16), color);
        }

        private static void DrawLayer(IEnumerable<Entity> entities, SpriteBatch spriteBatch)
        {
            foreach (var entity in entities)
            {
                var position = entity.Components[0] as Position;
                var sprite = entity.Components[1] as Sprite;

                var playerVision = (Vision)MainGame.PlayerEntity.Components[4];

                // Check if position is within the bounds of the visibility arrays
                if (position.X < playerVision.CellsInLightOfSight.Length && 
                    position.Y < playerVision.CellsInLightOfSight[position.X].Length &&
                    playerVision.CellsInLightOfSight[position.X][position.Y])
                {
                    DrawEntity(spriteBatch, position, sprite, sprite.Color);
                }
                else if (position.X < playerVision.VisitedCells.Length && 
                         position.Y < playerVision.VisitedCells[position.X].Length && 
                         playerVision.VisitedCells[position.X][position.Y])
                {
                    if (sprite.IsVisibleOutOfSight)
                    {
                        DrawEntity(spriteBatch, position, sprite, Color.Gray);
                    }
                    else
                    {
                        Vector2 screenPos = CameraSystem.WorldToScreen(position.X, position.Y);
                        spriteBatch.Draw(MainGame.SpriteAtlas, screenPos,
                            new Rectangle(11 * 16, 13 * 16, 16, 16), Color.Black);
                        spriteBatch.Draw(MainGame.SpriteAtlas, screenPos,
                            new Rectangle(10 * 16, 15 * 16, 16, 16), Color.Gray);
                    }
                }
                else
                {
                    Vector2 screenPos = CameraSystem.WorldToScreen(position.X, position.Y);
                    spriteBatch.Draw(MainGame.SpriteAtlas, screenPos,
                        new Rectangle(11 * 16, 13 * 16, 16, 16), Color.Black);
                }
            }
        }
    }
}