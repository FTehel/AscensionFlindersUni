using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension2
{
    public class GameObject
    {
        public Vector2 position;
        public Vector2 size;
        public Texture2D texture;
        public Boolean hasCollision = false;

        public void Update(GameTime theGameTime)
        {
            //Update Functions go here
        }

        public void Start()
        {
            //Start functions go here
        }
    }
}
