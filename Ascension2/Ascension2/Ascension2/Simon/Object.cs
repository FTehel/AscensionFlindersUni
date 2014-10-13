using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension2
{
    //Class for collision detection and other object related tasks
    public class Object
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { set; get; }

        public Object(Texture2D texture, Vector2 position)
        {
            Texture = texture;
            Position = position;
        }

        public Rectangle getBounds
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y,
                          Texture.Width, Texture.Height);
            }

        }

        //Collision detection goes here
        //needed functions:
        //  - check if player on floor
        //  - check if player hits tile with head/side [diagonal?]



    }
}
