using System;
using game.GameEngine.Components;
using Microsoft.Xna.Framework;
using rpg_v2;

namespace game.GameEngine.Systems
{
    public static class CameraSystem
    {
        // Camera position (top-left corner of the visible area)
        public static int ViewportX { get; private set; } = 0;
        public static int ViewportY { get; private set; } = 0;
        
        // Update camera to follow player
        public static void Update()
        {
            if (MainGame.PlayerEntity == null || ((Health)MainGame.PlayerEntity.Components[6]).CurrentHp <= 0) return;
            
            var playerPosition = (Position)MainGame.PlayerEntity.Components[0];
            
            // Center the viewport on the player
            ViewportX = playerPosition.X - MainGame.MapWidth / 2;
            ViewportY = playerPosition.Y - MainGame.MapHeight / 2;
            
            // Clamp the viewport to the map boundaries
            ViewportX = Math.Max(0, Math.Min(ViewportX, MainGame.MapSize - MainGame.MapWidth));
            ViewportY = Math.Max(0, Math.Min(ViewportY, MainGame.MapSize - MainGame.MapHeight));
        }
        
        // Convert world position to screen position
        public static Vector2 WorldToScreen(int worldX, int worldY)
        {
            return new Vector2(
                (worldX - ViewportX) * 16, // 16 is the tile size
                (worldY - ViewportY) * 16
            );
        }
        
        // Check if a position is within the viewport
        public static bool IsInViewport(int worldX, int worldY)
        {
            return worldX >= ViewportX && worldX < ViewportX + MainGame.MapWidth &&
                   worldY >= ViewportY && worldY < ViewportY + MainGame.MapHeight;
        }
    }
}
