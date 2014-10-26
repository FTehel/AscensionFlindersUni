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

namespace Ascension2
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        GraphicsDevice device;

        int screenWidth;
        int screenHeight;

        //Create game states
        enum GameState
        {
            MainMenu,
            Options,
            Playing,
        }
        //set initial game state to main menu
        GameState CurrentGameState = GameState.MainMenu;

        cButton btnPlay;
        cButton btnOption;
        cButton btnMenu;
        fBar fuel;

        public Level thisLevel;

        Texture2D brickTexture;

        Camera camera;

        proceduralGenerator generator;

        GameObject[] gameObjects = new GameObject[0];

        Texture2D playerTexture;
        Player player;

        private SpriteFont debugFont;
        private bool enableDebug = true;

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
            SoundEffect.MasterVolume = 0.2f;
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            IsMouseVisible = true;

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            loadFunction();
            loadInterface();
            // TODO: use this.Content to load your game content here
        }
        
        public void loadInterface()
        {
            btnPlay = new cButton(Content.Load<Texture2D>("Matthew/playButton"), graphics.GraphicsDevice, new Vector2(screenWidth / 5, screenHeight / 20));
            btnPlay.setPosition(new Vector2((screenWidth / 2) - ((screenWidth / 5) / 2), screenHeight - 100));

            btnMenu = new cButton(Content.Load<Texture2D>("Matthew/menuButton"), graphics.GraphicsDevice, new Vector2(screenWidth / 8, screenHeight / 25));
            btnMenu.setPosition(new Vector2((screenWidth -75) - ((screenWidth / 8) / 2), (screenHeight - 575)));

            fuel = new fBar(Content.Load<Texture2D>("Matthew/hBar"), graphics.GraphicsDevice, player.getFuelLevel);
            fuel.setPosition(new Vector2((screenWidth - 924) - ((screenWidth / 8) / 2), (screenHeight - 575)));
        }

        public void loadFunction()
        {

            device = graphics.GraphicsDevice;
            graphics.PreferredBackBufferWidth = 1024;
            graphics.PreferredBackBufferHeight = 600;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            Window.Title = "Tower Climb";

            screenWidth = device.PresentationParameters.BackBufferWidth;
            screenHeight = device.PresentationParameters.BackBufferHeight;

            loadTextures();

            thisLevel = new Level();
            camera = new Camera();

            generator = new proceduralGenerator();
            generator.currentHeight = generator.towerBaseHeight;
            generator.generateGround(brickTexture, thisLevel);

            playerTexture = Content.Load<Texture2D>("Simon/Player");
<<<<<<< HEAD
            player = new Player(playerTexture, new Vector2(1, 100), spriteBatch);
            player.size = new Vector2(40,90);
            player.screenWidth = screenWidth;
            player.screenHeight = screenHeight;
            player.camera = camera;

            Console.WriteLine("camera " + screenHeight + " " + screenWidth);

            
=======
            player = new Player(playerTexture, new Vector2(300, 400), spriteBatch, Content);
>>>>>>> origin/master

            camera.parent = player;
            debugFont = Content.Load<SpriteFont>("Simon/DebugFont");
        }

        private void generateLevel()
        {

            generator.generate(thisLevel.gridSize, (int)camera.position.Y, brickTexture, thisLevel);
            
        }

        private void loadTextures()
        {
            brickTexture = Content.Load<Texture2D>("Fraser/bricksTexture");
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
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    if (btnPlay.isClicked == true) CurrentGameState = GameState.Playing;
                    btnPlay.Update(mouse);
                    break;
                case GameState.Playing:
                    if (btnMenu.isClicked == true) CurrentGameState = GameState.MainMenu;
                    btnMenu.Update(mouse);
                    if (player.getFuelLevel >= 0) { fuel.Update(player.getFuelLevel); }
                    updateFunction(gameTime);
                    break;
            }
            

            base.Update(gameTime);
        }

        public void updateFunction(GameTime theGameTime)
        {
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].Update(theGameTime);
            }
            camera.Update(theGameTime);
            player.Update(theGameTime, thisLevel);
            generateLevel();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            switch (CurrentGameState)
            {
                case GameState.MainMenu:
                    spriteBatch.Draw(Content.Load<Texture2D>("Matthew/MainMenu"), new Rectangle(0, 0, screenWidth, screenHeight), Color.White);
                    btnPlay.Draw(spriteBatch);
                    break;
                case GameState.Playing:               
                    drawLevel(thisLevel);

                    string debugInfo = string.Format("Jetpack Fuel: {0:0.0}", player.getFuelLevel);
                    string debugInfo2 = string.Format("Location: {0:0.0}", player.getPlayerBounds);
                    string debugInfo3 = string.Format("X Velocity: {0:0}, Y Velocity: {1:0}", player.GetHorizontalVelocity, player.GetVerticalVelocity);

                    player.Draw(gameTime, camera.worldToScreen(player.position, screenWidth, screenHeight));
                    fuel.Draw(spriteBatch);
                    if (enableDebug)
                        {
                            spriteBatch.DrawString(debugFont, debugInfo, new Vector2(10, 0), Color.White);
                            spriteBatch.DrawString(debugFont, debugInfo2, new Vector2(10, 20), Color.White);
                            spriteBatch.DrawString(debugFont, debugInfo3, new Vector2(10, 40), Color.White);
                        }
                    btnMenu.Draw(spriteBatch);
                    
                    break;
            }





            base.Draw(gameTime);
            spriteBatch.End();

        }

        private void drawTile(gridSpace grid)
        {
            if (grid != null)
            {
                if (grid.isFilled)
                {
                    if (grid.level == 0)
                    {
                        Vector2 screenPos = camera.worldToScreen(grid.position, screenWidth, screenHeight);
                        Rectangle drawRect = new Rectangle((int)screenPos.X, (int)screenPos.Y, (int)grid.size.X, (int)grid.size.Y);
                        spriteBatch.Draw(grid.texture, drawRect, Color.White);
                    }
                    else
                    {
                        for (var i = 0; i < grid.childrenNumber; i++)
                        {
                            for (var j = 0; j < grid.childrenNumber; j++)
                            {
                                drawTile(grid.children[i, j]);
                            }
                        }
                    }
                }
            }
        }

        private void drawTileLine(gridLine grid)
        {
            if (grid != null)
            {
                gridSpace[] line = grid.grids;
                for (var i = 0; i < line.Length; i++)
                {
                    drawTile(line[i]);
                }
            }
        }

        private void drawLevel(Level levelToDraw)
        {

            for (var i = 0; i < levelToDraw.tilesXPositive.Length; i++)
            {
                drawTileLine(levelToDraw.tilesXPositive[i]);
            }
            for (var i = 0; i < levelToDraw.tilesXNegative.Length; i++)
            {
                drawTileLine(levelToDraw.tilesXNegative[i]);
            }
        }

        public void addGameObject(GameObject newObject)
        {
            GameObject[] temp = new GameObject[gameObjects.Length + 1];
            for (int i = 0; i < gameObjects.Length; i++)
            {
                temp[i] = gameObjects[i];

            }
            temp[gameObjects.Length] = newObject;
            gameObjects = temp;
        }
    }
}
