using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Ascension2
{
    public class Player : GameObject
    {
        //Used to keypress for jumping
        KeyboardState oldState;

        //Velocity vectors
        public Vector2 HMovement { get; set; }
        public Vector2 VMovement { get; set; }
        public int GetHorizontalVelocity
        {
            get
            {
                return (int)HMovement.X;
            }
        }
        public int GetVerticalVelocity
        {
            get
            {
                return (int)VMovement.Y;
            }
        }
        ////////////////////////

        //Size of sprite frame
        int playerWidth = 40;
        int playerHeight = 90;

        //Sprite variables
        public SpriteBatch SpriteBatch { get; set; }
        SpriteEffects spriteEffects = SpriteEffects.None;
        //Used to shift sprite sheet
        int playerEvolution = 0;
        bool running = false;
        bool jumping = false;
        bool flying = false;
        ///////////////////////////

        //Jetpack variables////////
        const int maxFuel = 600;
        int jetpackFuel;
        public int getFuelLevel
        {
            get
            {
                return jetpackFuel;
            }
        }
        ///////////////////////////

        public Player(Texture2D texture, Vector2 position, SpriteBatch batch)
        {
            oldState = Keyboard.GetState();
            SpriteBatch = batch;
            jetpackFuel = maxFuel;
        }

        public Rectangle getPlayerBounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                          playerWidth, playerHeight);
            }
        }

        public void Draw(GameTime gameTime)
        {
            float time = (float)gameTime.TotalGameTime.TotalSeconds;

            int startingFrame;
            int frameCount;
            int framesPerSecond = 8;

            if (flying) { startingFrame = 2; frameCount = 1; }
            else if (jumping) { startingFrame = 1; frameCount = 1; }
            else if (running) { startingFrame = 0; frameCount = 3; }
            else { startingFrame = 0; frameCount = 1; }

            startingFrame += (int)(time * framesPerSecond) % frameCount;
            if (texture != null)
            {
                SpriteBatch.Draw(texture, position, new Rectangle(startingFrame * playerWidth, playerEvolution * playerHeight, playerWidth, playerHeight), Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 0.5f);
            }
        }

        public void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            //Running
            if (newState.IsKeyDown(Keys.Left))
            {
                running = true;
                HMovement += new Vector2(-1.5f, 0);
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (newState.IsKeyDown(Keys.Right))
            {
                running = true;
                HMovement += new Vector2(1.5f, 0);
                spriteEffects = SpriteEffects.None;
            }
            else
            {
                running = false;
            }

            //Jumping (    NEED COLLISION DETECTION FOR FLOOR - isOnGround()    )
            if (newState.IsKeyDown(Keys.Up) && position.Y == 678)
            {
                if (!oldState.IsKeyDown(Keys.Up))
                {
                    VMovement = -Vector2.UnitY * 30;
                    jumping = true;
                }
                else
                {
                    //Do nothing
                }
            }
            //if isOnGround() then jumping false
            else if (position.Y == 678)
            {
                jumping = false;
            }
            oldState = newState;

            //Jetpack
            if (playerEvolution > 0)
            {
                jetpackMovement();
            }

            //Simulate gravity
            //IF !isOnGround() THEN
            if (position.Y < 678)
            {
                VMovement += Vector2.UnitY * 2.4f;
            }

            //Simulate friction
            HMovement -= HMovement * new Vector2(.1f, 0);

            //Updating Vertical and Horizontal position
            position += HMovement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 25;
            position += VMovement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 25;

            //Temporary measures to prevent character leaving screen and test item progression
            if (position.Y > 678)
            {
                //position = new Vector2(position.X, 678);
            }

            if (position.X < 0)
            {
                position = new Vector2(0, position.Y);
            }

            if (position.X > 984)
            {
                position = new Vector2(984, position.Y);
            }
            if (newState.IsKeyDown(Keys.U))
            {
                unlockItem("Jetpack");
            }
            //////////////////////////////////////////////////////////////////////////////////
        }

        public void unlockItem(String itemname)
        {
            switch (itemname)
            {
                case "Jetpack":
                    playerEvolution = 1;
                    break;
                case "Spacesuit":
                    playerEvolution = 2;
                    break;
            }
        }

        public void jetpackMovement()
        {
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(Keys.Space) && jetpackFuel > 0)
            {
                VMovement = -Vector2.UnitY * 15;
                jetpackFuel -= 20;
                flying = true;
            }
            else
            {
                flying = false;
            }
            if (jetpackFuel < maxFuel && !newState.IsKeyDown(Keys.Space))
            {
                jetpackFuel += 5;
            }
        }

    }
}
