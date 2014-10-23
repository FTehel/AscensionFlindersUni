using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ascension2.Fraser;

namespace Ascension2.Fraser
{
    public class PhysicsObject : GameObject
    {

        Boolean collided = false;
        public Collision[] currentCollisions = new Collision[0];
        Boolean hasCollision = true;
        Vector2 velocity;

        public enum sides
        {
            top,
            bottom,
            left,
            right,
            topRight,
            topLeft,
            bottomRight,
            bottomLeft
        };

        /*public List<Collision> getCollision(Vector2 currentPos, Vector2 newPos, GameObject other, List<Collision> collisions)
        {
            if (other.hasCollision)
            {
                int otherMaxX = (int)other.position.X + (int)other.size.X;
                int otherMinX = (int)other.position.X;
                int otherMaxY = (int)other.position.Y + (int)other.size.Y;
                int otherMinY = (int)other.position.Y;

                int thisMaxX = (int)newPos.X + (int)size.X;
                int thisMinX = (int)newPos.X;
                int thisMaxY = (int)newPos.Y + (int)size.Y;
                int thisMinY = (int)newPos.Y;

                Vector2 newPosition;

                sides newSides = sides.top;

                int currentMaxX = (int)currentPos.X + (int)size.X;
                int currentMinX = (int)currentPos.X;
                int currentMaxY = (int)currentPos.Y + (int)size.Y;
                int currentMinY = (int)currentPos.Y;

                if (isBetweenValues(thisMaxX, otherMaxX, otherMinX) || isBetweenValues(thisMinX, otherMaxX, otherMinX))
                {

                    if (isBetweenValues(thisMaxY, otherMaxY, otherMinY) || isBetweenValues(thisMinY, otherMaxY, otherMinY))
                    {
                        if (currentMaxY < otherMinY)
                        {
                            
                        }
                        else if (currentMinY > otherMaxY)
                        {

                        }

                        newPosition = getCollisionPosition(position, other.position);
                        if(other.GetType() == typeof(gridSpace)){
                            gridSpace grid = (gridSpace)other;
                            if (grid.level != 0){
                                collisions = getCollisionOnChildren(currentPos, newPos, grid, collisions);
                            }
                            else
                            {
                                collisions.Add(new Collision(newPosition, other));
                            }
                        }
                        else
                        {
                            collisions.Add(new Collision(newPosition, other));
                        }
                    }
                }
            }
            return collisions;
        }*/

        public List<Collision> getCollision(Vector2 currentPos, Vector2 newPos, GameObject other, List<Collision> collisions)
        {
            if (other != null && other.hasCollision)
            {
                int otherMaxX = (int)other.position.X + (int)other.size.X;
                int otherMinX = (int)other.position.X;
                int otherMaxY = (int)other.position.Y + (int)other.size.Y;
                int otherMinY = (int)other.position.Y;

                Vector2 corner1 = position;
                Vector2 corner2 = position;
                corner2.Y += size.Y;
                Vector2 corner3 = position;
                corner3 += size;
                Vector2 corner4 = position;
                corner4.X += size.X;

                Vector2 corner1Projection = corner1 + velocity;
                Vector2 corner2Projection = corner2 + velocity;
                Vector2 corner3Projection = corner3 + velocity;
                Vector2 corner4Projection = corner4 + velocity;

                int thisMaxX = (int)newPos.X + (int)size.X;
                int thisMinX = (int)newPos.X;
                int thisMaxY = (int)newPos.Y + (int)size.Y;
                int thisMinY = (int)newPos.Y;

                Vector2 newPosition;

                sides newSides = sides.top;

                int currentMaxX = (int)currentPos.X + (int)size.X;
                int currentMinX = (int)currentPos.X;
                int currentMaxY = (int)currentPos.Y + (int)size.Y;
                int currentMinY = (int)currentPos.Y;

                if (isBetweenValues(thisMaxX, otherMaxX, otherMinX) || isBetweenValues(thisMinX, otherMaxX, otherMinX))
                {

                    if (isBetweenValues(thisMaxY, otherMaxY, otherMinY) || isBetweenValues(thisMinY, otherMaxY, otherMinY))
                    {
                        if (currentMaxY < otherMinY)
                        {
                            if (currentMaxX < otherMinX)
                            {
                                newSides = sides.bottomLeft;
                            }
                            else if (currentMinX > otherMaxX)
                            {
                                newSides = sides.bottomRight;
                            }
                            else
                            {
                                newSides = sides.bottom;
                            }
                        }
                        else if (currentMinY > otherMaxY)
                        {
                            if (currentMaxX < otherMinX)
                            {
                                newSides = sides.topLeft;
                            }
                            else if (currentMinX > otherMaxX)
                            {
                                newSides = sides.topRight;
                            }
                            else
                            {
                                newSides = sides.top;
                            }
                        }
                        else
                        {
                            if (currentMaxX < otherMinX)
                            {
                                newSides = sides.left;
                            }
                            else
                            {
                                newSides = sides.right;
                            }
                        }

                        newPosition = getCollisionPosition(position, other.position);
                        if (other.GetType() == typeof(gridSpace))
                        {
                            gridSpace grid = (gridSpace)other;
                            if (grid.level != 0)
                            {
                                collisions = getCollisionOnChildren(currentPos, newPos, grid, collisions);
                            }
                            else
                            {
                                collisions.Add(new Collision(newSides, newPosition, other));
                            }
                        }
                        else
                        {
                            collisions.Add(new Collision(newSides, newPosition, other));
                        }
                    }
                }
            }
            return collisions;
        }

        public Vector2 getCollisionPosition(Vector2 thisPos, Vector2 otherPos)
        {
            Vector2 returnPos = new Vector2(0,0);
            returnPos.Y = thisPos.Y > otherPos.Y? position.Y:position.Y + size.Y;
            returnPos.X = thisPos.X > otherPos.X ? position.X : position.X + size.X;

            return returnPos;
        }

        public sides getCollisionSide(Vector2 thisPos, Vector2 otherPos)
        {
            Vector2 position = getCollisionPosition(thisPos, otherPos);
            sides returnSide = sides.top;
            if (thisPos.Y > position.Y)
            {
                
            }

            return returnSide;
        }

        public void addCollision(Collision item){
            Collision[] temp = new Collision[currentCollisions.Length];
            for(var i = 0;i < currentCollisions.Length;i++){
                temp[i] = currentCollisions[i];
            }
            temp[currentCollisions.Length] = item;
            currentCollisions = temp;
        }

        public List<Collision> getCollisionOnChildren(Vector2 currentPos, Vector2 newPos, gridSpace thisSpace, List<Collision> collisions){
            for(int i = 0;i < thisSpace.children.GetLength(0);i ++){
                for (int j = 0; j < thisSpace.children.GetLength(1); j++)
                {
                    collisions = getCollision(currentPos, newPos, thisSpace.children[i,j], collisions);
                }
            }
            return collisions;
        }

        public List<Collision> getCollisionForGridLine(Vector2 currentPos, Vector2 newPos, gridLine line, List<Collision> collisions){
            for (var i = 0; i < line.grids.Length; i++)
            {
                collisions = getCollision(currentPos, newPos, line.grids[i], collisions);
            }
            return collisions;
        }

        public void getCollisionForLevel(Level otherLevel, GameTime gameTime){
            Vector2 newPos = (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) + position;
            Vector2 currentPos = position;
            List<Collision> collisions = new List<Collision>();
            for (var i = 0; i < otherLevel.tilesXPositive.Length; i++)
            {
                collisions = getCollisionForGridLine(currentPos, newPos, otherLevel.tilesXPositive[i], collisions);
            }
            for (var i = 0; i < otherLevel.tilesXNegative.Length; i++)
            {
                collisions = getCollisionForGridLine(currentPos, newPos, otherLevel.tilesXNegative[i], collisions);
            }
            currentCollisions = collisions.ToArray();
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

        public void getCollisionEnter()
        {
            if (collided)
            {
                onCollisionEnter();
            }
        }

        public Vector2 updatePhysics(GameTime gameTime, Level thisLevel, Vector2 currentVelocity){
            velocity = currentVelocity;
            getCollisionForLevel(thisLevel, gameTime);
            getCollisionEnter();
            stopOnCollision();
            return velocity;
        }

        public void stopOnCollision(){
            if(hasCollision){
                if(currentCollisions.Length > 0){
                    for (int i = 0; i < currentCollisions.Length;i++){
                        velocity = getCollisionVelocity(currentCollisions[i]);
                    }
                }
            }
        }

        public Vector2 getCollisionVelocity(Collision collision)
        {
            GameObject other = collision.other;
            Vector2 centre = new Vector2(position.X + size.X / 2, position.Y + size.Y / 2);
            Vector2 otherCentre = new Vector2(other.position.X + other.size.X / 2, other.position.Y + other.size.Y / 2);
            Vector2 newVelocity = velocity;
            Vector2 counterVelocity = Vector2.Zero;
            float dot = 0;
            float speed = velocity.Length();
            if (collision.side.Equals(sides.top))
            {
                counterVelocity.Y = 1;
                dot = Vector2.Dot(-counterVelocity, velocity);
            }
            else if (collision.side.Equals(sides.bottom))
            {
                counterVelocity.Y = -1;
                dot = Vector2.Dot(-counterVelocity, velocity);
            }
            else if (collision.side.Equals(sides.left))
            {
                counterVelocity.X = -1;
                dot = Vector2.Dot(-counterVelocity, velocity);
            }
            else if (collision.side.Equals(sides.right))
            {
                counterVelocity.X = 1;
                dot = Vector2.Dot(-counterVelocity, velocity);
            }
            else if (collision.side.Equals(sides.bottomLeft))
            {
                counterVelocity.Y = -1;
                counterVelocity.X = -1;
                counterVelocity.Normalize();
                dot = Vector2.Dot(-counterVelocity, velocity);
            }
            else if (collision.side.Equals(sides.bottomRight))
            {
                counterVelocity.Y = -1;
                counterVelocity.X = 1;
                counterVelocity.Normalize();
                dot = Vector2.Dot(-counterVelocity, velocity);
            }
            else if (collision.side.Equals(sides.topRight))
            {
                counterVelocity.Y = 1;
                counterVelocity.X = 1;
                counterVelocity.Normalize();
                dot = Vector2.Dot(-counterVelocity, velocity);
            }
            else if (collision.side.Equals(sides.topLeft))
            {
                counterVelocity.Y = 1;
                counterVelocity.X = -1;
                counterVelocity.Normalize();
                dot = Vector2.Dot(-counterVelocity, velocity);
            }
            counterVelocity *= dot * speed;
            newVelocity = velocity + counterVelocity;

            return newVelocity;
        }
    }
}
