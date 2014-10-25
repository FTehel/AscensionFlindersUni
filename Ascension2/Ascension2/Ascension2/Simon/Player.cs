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
using Ascension2.Fraser;
using Ascension2;

namespace Ascension2
{
    public class Player : GameObject
    {
        ContentManager content;
        SoundEffect jumpSound;
        SoundEffectInstance jet;
        SoundEffect jetSound;

        Texture2D texture;

        PhysicsObject physics = new PhysicsObject();

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
        public SpriteBatch spriteBatch { get; set; }
        SpriteEffects spriteEffects = SpriteEffects.None;
        //Used to shift sprite sheet
        int playerEvolution = 0;
        bool running = false;
        bool jumping = false;
        bool flying = false;
        ///////////////////////////

        //Player Variables
        float runSpeed = 2.0f;
        float jetPackSpeed = 5.0f;
        float gravity = 2.0f;
        float jumpForce = 3.0f;

        ///////////////////////

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

        public Player(Texture2D texture, Vector2 position, SpriteBatch batch, ContentManager content)
        {
            this.texture = texture;
            this.content = content;
            oldState = Keyboard.GetState();
            spriteBatch = batch;
            jetpackFuel = maxFuel;
            this.position = position;

            jumpSound = content.Load<SoundEffect>("Simon/jump");
            jetSound = content.Load<SoundEffect>("Simon/jet");

            jet = jetSound.CreateInstance();
            jet.IsLooped = true;
        }

        public Rectangle getPlayerBounds
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y,
                          playerWidth, playerHeight);
            }
        }

        public void Draw(GameTime gameTime, Vector2 camPosition)
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
                spriteBatch.Draw(texture, camPosition, new Rectangle(startingFrame * playerWidth, playerEvolution * playerHeight, playerWidth, playerHeight), Color.White, 0.0f, Vector2.Zero, 1.0f, spriteEffects, 0.5f);
            }
        }

        public void Update(GameTime gameTime, Level thisLevel)
        {
            KeyboardState newState = Keyboard.GetState();

            physics.position = position;

            //Running
            if (newState.IsKeyDown(Keys.Left))
            {
                running = true;
                HMovement += new Vector2(-runSpeed, 0) * getGameTime(gameTime);
                spriteEffects = SpriteEffects.FlipHorizontally;
            }
            else if (newState.IsKeyDown(Keys.Right))
            {
                running = true;
                HMovement += new Vector2(runSpeed, 0) * getGameTime(gameTime);
                spriteEffects = SpriteEffects.None;
            }
            else
            {
                running = false;
            }

            //Jumping (    NEED COLLISION DETECTION FOR FLOOR - isOnGround()    )
            if (newState.IsKeyDown(Keys.Up))
            {
                if (!oldState.IsKeyDown(Keys.Up))
                {
                    jumpSound.Play();
                    VMovement = Vector2.Zero;
                    VMovement = Vector2.UnitY * 30;
                    HMovement += Vector2.UnitY * jumpForce * getGameTime(gameTime);
                    jumping = true;
                }
                else
                {
                    //Do nothing
                }
            }
            //if isOnGround() then jumping false
            else if (position.Y == 2000)
            {
                jumping = false;
            }
            oldState = newState;

            //Jetpack
            if (playerEvolution > 0)
            {
                jetpackMovement(gameTime);
            }

            //Simulate gravity
            //IF !isOnGround() THEN
            if (position.Y > 2000)
            {
                VMovement -= Vector2.UnitY * 2.4f;
                VMovement = Vector2.Zero;
                HMovement -= Vector2.UnitY * gravity * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 25);
            }

            //Simulate friction
            //HMovement -= HMovement * new Vector2(.1f, 0);

            //Updating Vertical and Horizontal position
            physics.position = position;
            physics.size = size;
            HMovement = physics.updatePhysics(gameTime, thisLevel, HMovement);
            position += HMovement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 25;
            VMovement = Vector2.Zero;
            position += VMovement * (float)gameTime.ElapsedGameTime.TotalMilliseconds / 25;

            //Temporary measures to prevent character leaving screen and test item progression
            /*if (position.Y < 2000)
            {
                position = new Vector2(position.X, 2000);
            }
            if (position.X < 0)

            if (position.X < -200)
            {
                position = new Vector2(-200, position.Y);
            }

            if (position.X > 500)
            {
                position = new Vector2(500, position.Y);
            }*/
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

        public float getGameTime(GameTime gameTime)
        {
            return (float)(gameTime.ElapsedGameTime.TotalSeconds);
        }

        public void jetpackMovement(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();
            
            if (newState.IsKeyDown(Keys.Space))
            {
                
                jet.Play(); 
               
                //VMovement = Vector2.UnitY * 15;
                //jetpackFuel -= 20;
                HMovement += Vector2.UnitY * jetPackSpeed * (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 25);
                flying = true;
            }
            else
            {
                jet.Stop();
                flying = false;
            }
            if (jetpackFuel < maxFuel && !newState.IsKeyDown(Keys.Space))
            {
                jetpackFuel += 5 * (int)getGameTime(gameTime);
            }
        }

    }
}
