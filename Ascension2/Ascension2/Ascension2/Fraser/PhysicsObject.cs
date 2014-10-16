using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ascension2.Fraser
{
    class PhysicsObject : GameObject
    {

        Boolean collided = false;

        public void getCollision(GameObject other)
        {
            if (other.hasCollision)
            {
                int otherMaxX = (int)other.position.X + (int)other.size.X;
                int otherMinX = (int)other.position.X;
                int otherMaxY = (int)other.position.Y + (int)other.size.Y;
                int otherMinY = (int)other.position.Y;

                int thisMaxX = (int)position.X + (int)size.X;
                int thisMinX = (int)position.X;
                int thisMaxY = (int)position.Y + (int)size.Y;
                int thisMinY = (int)position.Y;

                if (isBetweenValues(thisMaxX, otherMaxX, otherMinX) || isBetweenValues(thisMinX, otherMaxX, otherMinX))
                {
                    if (isBetweenValues(thisMaxY, otherMaxY, otherMinY) || isBetweenValues(thisMinY, otherMaxY, otherMinY))
                    {
                        collided = true;
                    }
                }
            }
        }

        public Boolean isBetweenValues(int x, int y, int z)
        {
            if (x > y && x < z)
            {
                return true;
            }
            return false;
        }

        public void onCollisionEnter()
        {

        }
    }
}
