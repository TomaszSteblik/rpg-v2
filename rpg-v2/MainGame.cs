using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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

namespace rpg_v2
{
    public class MainGame : Game
    {
        private const int TileSize = 16;
        public static Random Random = new Random();
        public static Texture2D SpriteAtlas { get; set; }
        public static FontSystem FontSystem;
        public static int MapSize { get; set; } = 60;
        public static Entity PlayerEntity;

        public static IGameState CurrentGameState { get; set; }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private double _frameRate = 0;
        private SpriteFontBase _font18;
        private readonly Rectangle _destinationScalingRectangle;
        private RenderTarget2D _gamertRenderTarget2D;

        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
            _destinationScalingRectangle = new Rectangle(0, 0, MapSize * TileSize, MapSize * TileSize);
        }

        private int smth => 3024 / 1964;

        protected override void Initialize()
        {
            _gamertRenderTarget2D = new RenderTarget2D(GraphicsDevice, MapSize * TileSize, MapSize * TileSize);
            _graphics.HardwareModeSwitch = false;
            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();
            EcsManager.Init();
            SoundManager.Init(Content);

            CurrentGameState = new StartMenuGameState();

            SoundManager.Play();

            base.Initialize();
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
            _spriteBatch.Begin();
            GraphicsDevice.SetRenderTarget(_gamertRenderTarget2D);
            CurrentGameState.Draw(_spriteBatch);

            _spriteBatch.End();

            _spriteBatch.Begin();

            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Draw(_gamertRenderTarget2D, _destinationScalingRectangle, Color.White);
            _spriteBatch.DrawString(_font18, $"{_frameRate:F2}  FPS", new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(_font18, $"{(GC.GetTotalMemory(false) / 1000000.0):F2}  MB", new Vector2(0, 18), Color.White);

            var position = 36;
            foreach (var @event in InMemorySomSink.Instance.Events)
            {
                _spriteBatch.DrawString(_font18, $"{@event.RenderMessage()}", new Vector2(0, position), Color.White);
                position += 18;
            }
            _spriteBatch.End();


            base.Draw(gameTime);
        }
    }
}