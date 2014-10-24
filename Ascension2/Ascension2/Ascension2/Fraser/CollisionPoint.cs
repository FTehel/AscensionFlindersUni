using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension2.Fraser
{
    public class CollisionPoint
    {
        public Vector2 origin;
        public Vector2 collision;
        public float length;
        public Vector2 otherSide;

        public CollisionPoint(Vector2 newOrigin, Vector2 newCollision, Vector2 newSide)
        {
            this.origin = newOrigin;
            this.collision = newCollision;
            this.length = Vector2.Distance(this.origin, this.collision);
            this.otherSide = newSide;
        }
        public CollisionPoint(Vector2 newOrigin, Vector2 newCollision, Vector2 newSide1, Vector2 newSide2)
        {
            this.origin = newOrigin;
            this.collision = newCollision;
            this.length = Vector2.Distance(this.origin, this.collision);
            this.otherSide = newSide2 - newSide1;
        }
    }
}
