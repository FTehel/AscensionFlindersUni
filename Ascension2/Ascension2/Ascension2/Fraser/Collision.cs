using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension2.Fraser
{
    class Collision
    {
        public Vector2 position;
        public GameObject other;

        public Collision(Vector2 newPosition, GameObject otherObject)
        {
            this.position = newPosition;
            this.other = otherObject;
        }
    }
}
