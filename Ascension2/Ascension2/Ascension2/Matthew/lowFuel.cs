using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension2
{
    class lowFuel
    {
        Texture2D texture;
        Vector2 position;
        Rectangle rectangle;
        public bool visible;
        bool down;

        Color colour = new Color(255, 255, 255, 255);
        public Vector2 size;

        public lowFuel(Texture2D newTexture, GraphicsDevice graphics)
        {
            texture = newTexture;
            //width = 100;
            //height = graphics.Viewport.Height / 40;
            colour.B = 0; colour.G = 0; colour.R = 255; colour.A = 255;
            visible = false;

            size = new Vector2(graphics.Viewport.Width, graphics.Viewport.Height);
            rectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            
        }

        
        

        public void Update()
        {
            
                if (colour.A == 255) down = false;
                if (colour.A == 0) down = true;
                if (down) colour.A += 5; else colour.A -= 5;

        }

        public void Draw (SpriteBatch spriteBatch)
        {
            if (visible)
            {
                spriteBatch.Draw(texture, rectangle, colour);
            }
        }

    }

}
