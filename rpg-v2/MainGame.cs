﻿using System;
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

namespace rpg_v2
{
    public class MainGame : Game
    {
        public static Random Random = new Random();
        public static Texture2D SpriteAtlas { get; set; }
        
        public static FontSystem FontSystem;
        public static int MapSize { get; set; } = 60;
        public static Entity PlayerEntity;

        public static IGameState CurrentGameState { get; set; }
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private double _frameRate =0;
        
        public MainGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            _graphics.SynchronizeWithVerticalRetrace = false;
            IsFixedTimeStep = false;
        }
        
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = (int) (1920 * 1);
            _graphics.PreferredBackBufferHeight = (int) (1080 * 1);
            //_graphics.IsFullScreen = true;
            
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
        }

        protected override void Update(GameTime gameTime)
        {
            CurrentGameState.Update(gameTime);
            
            _frameRate=1 / gameTime.ElapsedGameTime.TotalSeconds;

            
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            using var rt = new RenderTarget2D(GraphicsDevice, 1920, 1080);
            GraphicsDevice.SetRenderTarget(rt);

            CurrentGameState.Draw(_spriteBatch);

            SpriteFontBase font18 = FontSystem.GetFont(18);
            _spriteBatch.DrawString(font18, $"{_frameRate:F2}  FPS", new Vector2(0, 0), Color.White);
            _spriteBatch.DrawString(font18, $"{(GC.GetTotalMemory(false)/1000000.0):F2}  MB", new Vector2(0, 18), Color.White);

            _spriteBatch.End();
            
            _spriteBatch.Begin();
            
            GraphicsDevice.SetRenderTarget(null);
            _spriteBatch.Draw(rt, new Rectangle(0,0,(int) (1*1920),(int) (1*1080)), Color.White);

            _spriteBatch.End();
            
            
            base.Draw(gameTime);
        }
    }
}