using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FontStashSharp;
using game.GameEngine;
using game.GameEngine.Components;
using game.GameEngine.GameStates;
using game.GameEngine.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using rpg_v2.Utils;
using Serilog;

namespace rpg_v2
{
    public class MainGame : Game
    {
        private const int TileSize = 16;
        public static Random Random = new Random();
        public static Texture2D SpriteAtlas { get; set; }
        public static FontSystem FontSystem;
        public static int MapSize { get; set; } = 200;  // Changed from 60 to 200
        public static int MapHeight => 45;
        public static int MapWidth => 80;
        public static Entity PlayerEntity;

        public static IGameState CurrentGameState { get; set; }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private double _frameRate = 0;
        private SpriteFontBase _font18;
        private Rectangle _destinationScalingRectangle;
        private RenderTarget2D _gameRenderTarget2D;
        
        // Virtual resolution - the resolution we design for
        private int _virtualWidth = 1280;  // MapWidth * TileSize
        private int _virtualHeight = 720;  // MapHeight * TileSize
        private float _scaleX;
        private float _scaleY;
        private Matrix _scaleMatrix;
        private const int BorderWidth = 0;  // Width of the black border in pixels

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.SynchronizeWithVerticalRetrace = false;
            //IsFixedTimeStep = false;
        }

        protected override void Initialize()
        {
            // Set initial window size if not in fullscreen
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.HardwareModeSwitch = false;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
            
            // Initialize virtual screen rendering
            _virtualWidth = MapWidth * TileSize;
            _virtualHeight = MapHeight * TileSize;
            _gameRenderTarget2D = new RenderTarget2D(GraphicsDevice, _virtualWidth, _virtualHeight);
            
            // Calculate scaling
            ResizeWindow();
            
            Window.ClientSizeChanged += (s, e) => ResizeWindow();
            
            // Initialize managers
            EcsManager.Init();
            SoundManager.Init(Content);
            VideoManager.Init(_graphics);
        
            CurrentGameState = new StartMenuGameState();
        
            SoundManager.PlayBackgroundMusic();
        
            base.Initialize();
        }
        
        private void ResizeWindow()
        {
            // Calculate the scaling factors for width and height
            _scaleX = GraphicsDevice.Viewport.Width / (float)_virtualWidth;
            _scaleY = GraphicsDevice.Viewport.Height / (float)_virtualHeight;
            
            // Use the smaller scale for uniform scaling (with letterboxing/pillarboxing)
            float scale = Math.Min(_scaleX, _scaleY);
            
            // Calculate the scaled dimensions
            int scaledWidth = (int)(_virtualWidth * scale);
            int scaledHeight = (int)(_virtualHeight * scale);
            
            // Center the scaled area on screen
            int posX = (GraphicsDevice.Viewport.Width - scaledWidth) / 2;
            int posY = (GraphicsDevice.Viewport.Height - scaledHeight) / 2;
            
            // Add border by shrinking the destination rectangle
            // This creates a visible black border around the game
            _destinationScalingRectangle = new Rectangle(
                posX + BorderWidth, 
                posY + BorderWidth, 
                scaledWidth - (BorderWidth * 2), 
                scaledHeight - (BorderWidth * 2)
            );
            
            // Create a scaling matrix for UI elements that will scale directly
            // Adjust the matrix to account for the border
            float adjustedScale = scale * ((float)(scaledWidth - (BorderWidth * 2)) / scaledWidth);
            _scaleMatrix = Matrix.CreateScale(adjustedScale, adjustedScale, 1f) * 
                           Matrix.CreateTranslation(posX + BorderWidth, posY + BorderWidth, 0);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            SpriteAtlas = Content.Load<Texture2D>("16x16-sm");

            FontSystem = new FontSystem();
            FontSystem.AddFont(File.ReadAllBytes(@"Content/Fonts/Girassol-Regular.ttf"));
            _font18 = FontSystem.GetFont(18);
        }

        protected override void Update(GameTime gameTime)
        {
            CurrentGameState.Update(gameTime);

            _frameRate = 1 / gameTime.ElapsedGameTime.TotalSeconds;


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear the screen with black color to create the border effect
            GraphicsDevice.Clear(Color.Black);
            
            // Render game content to the virtual resolution render target
            GraphicsDevice.SetRenderTarget(_gameRenderTarget2D);
            GraphicsDevice.Clear(Color.DarkSlateGray);
            
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp); // PointClamp for pixel-perfect scaling
            CurrentGameState.Draw(_spriteBatch);
            _spriteBatch.End();
        
            // Render the virtual screen to the actual display with scaling
            GraphicsDevice.SetRenderTarget(null);
            
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            _spriteBatch.Draw(_gameRenderTarget2D, _destinationScalingRectangle, Color.White);
            _spriteBatch.End();
            
            // Draw UI elements with the scaling matrix
            _spriteBatch.Begin(transformMatrix: _scaleMatrix);
            _spriteBatch.DrawString(_font18, $"{_frameRate:F2}  FPS", new Vector2(10, 10), Color.White);
            _spriteBatch.DrawString(_font18, $"{(GC.GetTotalMemory(false) / 1000000.0):F2}  MB", new Vector2(10, 40), Color.White);
            _spriteBatch.DrawString(_font18, $"{((Position)MainGame.PlayerEntity?.Components[0])?.X}, {((Position)MainGame.PlayerEntity?.Components[0])?.Y}", new Vector2(10, 70), Color.White);
            var position = 110;
            foreach (var @event in InMemorySomSink.Instance.Events.ToImmutableArray())
            {
                _spriteBatch.DrawString(_font18, $"{@event}", new Vector2(10, position), Color.White);
                position += 30;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}