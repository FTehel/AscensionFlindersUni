using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Ascension2;

namespace MainMenu
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //Create game states
        enum GameState
        {
            MainMenu,
            Options,
            Playing,
        }
        //set initial game state to main menu
        GameState CurrentGameState = GameState.MainMenu;

        //set screen size variables
        int screenWidth = 1024, screenHeight = 768;
        
        //create buttons 
        cButton btnPlay;
        cButton btnOption;
        cButton btnMenu;
        
        int mCheck = 0;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //set screen size to screen variables
            graphics.PreferredBackBufferWidth = screenWidth;
            graphics.PreferredBackBufferHeight = screenHeight;
            graphics.ApplyChanges();
            IsMouseVisible = true;

            //initialize buttons and set position
            btnPlay = new cButton(Content.Load<Texture2D>("playButton"), graphics.GraphicsDevice, new Vector2(screenWidth / 8, screenHeight / 25));
            btnPlay.setPosition(new Vector2((screenWidth/2) - ((screenWidth/8)/2), screenHeight - 100));

            btnOption = new cButton(Content.Load<Texture2D>("settingButton"), graphics.GraphicsDevice, new Vector2(screenWidth / 44, screenHeight / 25));
            btnOption.setPosition(new Vector2((screenWidth - (screenWidth - screenWidth / 8)), screenHeight - 100));

            btnMenu = new cButton(Content.Load<Texture2D>("menuButton"), graphics.GraphicsDevice, new Vector2(screenWidth / 3, screenHeight / 20));
            btnMenu.setPosition(new Vector2((screenWidth / 2) - ((screenWidth / 3) / 2), (screenHeight/3)));

            
                       
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            MouseState mouse = Mouse.GetState();

            //load functonality to buttons
            switch(CurrentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    btnPlay.Update(mouse);
                    if (btnOption.isClicked == true) CurrentGameState = GameState.Options;
                    btnOption.Update(mouse);
                    break;
                case GameState.Playing:
                    break;
                case GameState.Options:
                    if (btnMenu.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnMenu.Update(mouse);
                    break;
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // draw buttons and set background
            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"),new Rectangle(0,0, screenWidth, screenHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    btnOption.Draw(spriteBatch);
                    break;
                case GameState.Playing:
                    break;
                case GameState.Options:
                    spriteBatch.Draw(Content.Load<Texture2D>("MainMenu"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    btnMenu.Draw(spriteBatch);
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
