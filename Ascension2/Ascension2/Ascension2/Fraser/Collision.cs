using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension2.Fraser
{
    public class Collision
    {
        public Vector2 position;
        public GameObject other;
        public CollisionPoint point;


        public PhysicsObject.sides side = PhysicsObject.sides.top;

        public Collision(PhysicsObject.sides newSide, Vector2 newPosition, GameObject otherObject)
        {
            this.position = newPosition;
            this.other = otherObject;
            this.side = newSide;
        }

        public Collision(GameObject otherObject, CollisionPoint otherPoint)
        {
            this.other = otherObject;
            this.point = otherPoint;
        }

        public Collision(GameObject otherObject)
        {
            this.other = otherObject;
        }
    }
}
