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

        Level thisLevel;

        Texture2D brickTexture;

        Camera camera;

        proceduralGenerator generator;

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
            loadFunction();
            // TODO: use this.Content to load your game content here
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
            updateFunction(gameTime);

            base.Update(gameTime);
        }

        public void updateFunction(GameTime theGameTime)
        {
            camera.Update(theGameTime);
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
            drawLevel(thisLevel);
            spriteBatch.End();

            base.Draw(gameTime);
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
                        Rectangle drawRect = new Rectangle((int)screenPos.X, (int)screenPos.Y, (int)grid.size, (int)grid.size);
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
    }
}
