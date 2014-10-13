using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ascension2
{
    class Camera
    {
        public Vector2 position;
        Vector2 velocity;
        float speed = 10000;
        public GameObject parent;

        public Vector2 worldToScreen(Vector2 other, int width, int height)
        {
            Vector2 offset = other - position;
            Vector2 centre = new Vector2(width / 2, height / 2);
            offset.Y *= -1;
            return centre + offset;
        }

        public void Update(GameTime theGameTime)
        {
            //velocity = getMovementDirection();
            move(theGameTime);
            if (parent != null)
            {
                position = parent.position;
            }
        }

        public void move(GameTime theGameTime)
        {
            position += velocity * (float)theGameTime.ElapsedGameTime.TotalSeconds;
        }

        public Vector2 getMovementDirection()
        {
            Vector2 returnVelocity = new Vector2(0,0);
            KeyboardState state = Keyboard.GetState();
            if(state.IsKeyDown(Keys.W)){
                returnVelocity.Y += 1;
            }
            if (state.IsKeyDown(Keys.S))
            {
                returnVelocity.Y -= 1;
            }
            if (state.IsKeyDown(Keys.A))
            {
                returnVelocity.X -= 1;
            }
            if (state.IsKeyDown(Keys.D))
            {
                returnVelocity.X += 1;

            }
            if (state.IsKeyDown(Keys.Space))
            {
                position = Vector2.Zero;
            }
            return returnVelocity * speed;
        }
    }
}
