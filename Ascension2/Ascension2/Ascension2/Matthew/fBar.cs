using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension2
{
    class fBar
    {
        Texture2D texture;
        Vector2 position;
        Vector2 size;
        Rectangle rectangle;
        Color colour = new Color(255, 255, 255, 255);
        float width, height;

        public fBar(Texture2D newTexture, GraphicsDevice graphics, float s)
        {
            texture = newTexture;
            width = s/6;
            height = graphics.Viewport.Height / 40;
            size = new  Vector2(width, height);
            colour.B = 8; colour.G = 255; colour.R = 0;
        }

        public void setPosition(Vector2 newPosition)
        {

            position = newPosition;
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
        }

        public void Update(int i, GameTime gameTime)
        {
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            if (width > 60) { colour.B = 8; colour.G = 255; colour.R = 0; }
            if (width <= 60) { colour.R = 255; colour.G = 247; }
            if (width <= 30) { colour.R = 255; colour.G = 147; }
            if (width <= 15) { colour.R = 255; colour.G = 0; }
            setWidth(i);

            int counter = 1;
            int limit = 50;
            float countDuration = 2f; //every  2s.
            float currentTime = 0f;

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

            if (currentTime >= countDuration)
            {
                counter++;
                currentTime -= countDuration; // "use up" the time
                //any actions to perform
                setWidth(100);
            }
            if (counter >= limit)
            {
                counter = 0;//Reset the counter;
                //any actions to perform
            }
        }

        public void setWidth(float i)
        {
           
            width = i/6;
            size = new Vector2(width, height);


        }

        public float getWidth()
        {
            float w = Convert.ToInt32(width);
            return w;
        }



        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, colour);
        }
    }
}
