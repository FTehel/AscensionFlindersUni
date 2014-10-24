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

        public List<Collision> getCollision(Vector2 currentPos, Vector2 newPos, GameObject other, List<Collision> collisions, Vector2 newVelocity)
        {
            List<Collision> newCollisions = collisions;
            if (other != null)
            {
                if (other.hasCollision)
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

                    Vector2 corner1Projection = corner1 + newVelocity;
                    Vector2 corner2Projection = corner2 + newVelocity;
                    Vector2 corner3Projection = corner3 + newVelocity;
                    Vector2 corner4Projection = corner4 + newVelocity;

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

                    List<CollisionPoint> collisionsA = getCollisionPointOnRectangle(corner1, corner1Projection, other);
                    List<CollisionPoint> collisionsB = getCollisionPointOnRectangle(corner2, corner2Projection, other);
                    List<CollisionPoint> collisionsC = getCollisionPointOnRectangle(corner3, corner3Projection, other);
                    List<CollisionPoint> collisionsD = getCollisionPointOnRectangle(corner4, corner4Projection, other);

                    collisionsA.AddRange(collisionsB);
                    collisionsA.AddRange(collisionsC);
                    collisionsA.AddRange(collisionsD);

                    if (collisionsA.Count > 0)
                    {
                        CollisionPoint[] points = collisionsA.ToArray();
                        if (other.GetType() == typeof(gridSpace))
                        {
                            gridSpace grid = (gridSpace)other;
                            if (grid.level != 0)
                            {
                                collisions = getCollisionOnChildren(currentPos, newPos, grid, collisions, newVelocity);
                            }
                            else
                            {
                                
                                for(var i = 0;i < points.Length;i++){
                                    collisions.Add(new Collision(other, points[i]));
                                }
                            }
                        }
                        else
                        {
                            for (var i = 0; i < points.Length; i++)
                            {
                                collisions.Add(new Collision(other, points[i]));
                            }
                        }
                    }

                    if (isBetweenValues(thisMaxX, otherMaxX, otherMinX) || isBetweenValues(thisMinX, otherMaxX, otherMinX))
                    {
                        if (isBetweenValues(thisMaxY, otherMaxY, otherMinY) || isBetweenValues(thisMinY, otherMaxY, otherMinY))
                        {

                            newPosition = getCollisionPosition(position, other.position);
                            if (other.GetType() == typeof(gridSpace))
                            {
                                gridSpace grid = (gridSpace)other;
                                if (grid.level != 0)
                                {
                                    collisions = getCollisionOnChildren(currentPos, newPos, grid, collisions, newVelocity);
                                }
                            }
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

        public List<Collision> getCollisionOnChildren(Vector2 currentPos, Vector2 newPos, gridSpace thisSpace, List<Collision> collisions, Vector2 newVelocity){
            for(int i = 0;i < thisSpace.children.GetLength(0);i ++){
                for (int j = 0; j < thisSpace.children.GetLength(1); j++)
                {
                    collisions = getCollision(currentPos, newPos, thisSpace.children[i,j], collisions, newVelocity);
                }
            }
            return collisions;
        }

        public List<Collision> getCollisionForGridLine(Vector2 currentPos, Vector2 newPos, gridLine line, List<Collision> collisions, Vector2 newVelocity){
            for (var i = 0; i < line.grids.Length; i++)
            {
                collisions = getCollision(currentPos, newPos, line.grids[i], collisions, newVelocity);
            }
            return collisions;
        }

        public void getCollisionForLevel(Level otherLevel, GameTime gameTime){
            Vector2 newPos = (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) + position;
            Vector2 newVelocity = velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            Vector2 currentPos = position;
            List<Collision> collisions = new List<Collision>();
            currentCollisions = new Collision[0];
            for (var i = 0; i < otherLevel.tilesXPositive.Length; i++)
            {
                collisions = getCollisionForGridLine(currentPos, newPos, otherLevel.tilesXPositive[i], collisions, newVelocity);
            }
            for (var i = 0; i < otherLevel.tilesXNegative.Length; i++)
            {
                collisions = getCollisionForGridLine(currentPos, newPos, otherLevel.tilesXNegative[i], collisions, newVelocity);
            }
            currentCollisions = collisions.ToArray();
        }

        public Boolean isBetweenValues(int x, int y, int z)
        {
            if ((x > y && x < z) || (x > z && x < y))
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
            //getCollisionForLevel(thisLevel, gameTime);
            getCollisionEnter();
            stopOnCollision(gameTime);
            return velocity;
        }

        public void stopOnCollision(GameTime gameTime)
        {
            if(hasCollision){
                if(currentCollisions.Length > 0){                        
                    velocity = getCollisionVelocity(currentCollisions, gameTime);
                }
            }
        }

        public Vector2 getCollisionVelocity(Collision[] collisions, GameTime time)
        {
            Vector2 newVelocity = velocity;
            Vector2 nVelocity = velocity;
            nVelocity.Normalize();
            float dot = 0;
            float speed = velocity.Length() * time.TotalGameTime.Seconds;

            Collision shortest = getShortestCollision(collisions);
            Vector2 side = shortest.point.otherSide;
            Vector2 collisionOverlap = (velocity * time.TotalGameTime.Seconds) - shortest.point.collision;
            float collisionOverlapLength = collisionOverlap.Length();
            side.Normalize();
            Vector2 sideVelocity = Vector2.Zero;
            dot = Vector2.Dot(side, nVelocity);
            if (dot >= 0)
            {
                sideVelocity = side;
            }
            else
            {
                dot = -dot;
                sideVelocity = -side;
            }
            newVelocity = (shortest.point.collision - shortest.point.origin) + (sideVelocity * dot * collisionOverlapLength);
            newVelocity /= time.TotalGameTime.Seconds;
            return newVelocity;
        }

        public Collision getShortestCollision(Collision[] collisions){
            float shortestDistance = 0;
            int shortestIndex = -1;
            for (int i = 0; i < collisions.Length; i++)
            {
                if (shortestIndex == -1 || shortestDistance > collisions[i].point.length)
                {
                    shortestDistance = collisions[i].point.length;
                    shortestIndex = i;
                }
            }
            if (shortestIndex != -1)
            {
                return collisions[shortestIndex];
            }
            return null;
        }

        public Vector2 getCollisionPoint(Vector2 originA, Vector2 destinationA, Vector2 originB, Vector2 destinationB){

            float percentA = getCollisionPercent(originA, destinationA, originB, destinationB);
            float percentB = getCollisionPercent(originB, destinationB, originA, destinationA);

            if (percentA != 0 && percentB != 0)
            {
                Vector2 pointVector = originA + (percentA * (destinationA - originA));
                return pointVector;
            }
            Vector2 returnVector = Vector2.Zero;
            return returnVector;
        }

        public Boolean doesCollide(Vector2 originA, Vector2 destinationA, Vector2 originB, Vector2 destinationB){

            float percentA = getCollisionPercent(originA, destinationA, originB, destinationB);
            float percentB = getCollisionPercent(originB, destinationB, originA, destinationA);

            if (percentA != 0 && percentB != 0)
            {
                return true;
            }
            return false;
        }

        public float getCollisionPercent(Vector2 originA, Vector2 destinationA, Vector2 originB, Vector2 destinationB)
        {
            float collisionPointPercentA = ((destinationB.X - originB.X) * (originA.Y - originB.Y)) - ((destinationB.Y - originB.Y) * (originA.X - originB.X));
            float collisionPointPercentB = ((destinationB.Y - originB.Y) * (destinationA.X - originA.X)) - ((destinationB.X - originB.X) * (destinationA.Y - originA.Y));
            float collisionPointPercent = collisionPointPercentA / collisionPointPercentB;

            return collisionPointPercent;
        }

        public List<CollisionPoint> getCollisionPointOnRectangle(Vector2 originA, Vector2 destinationA, GameObject other){
            List<CollisionPoint> returnList = new List<CollisionPoint>();
            Vector2 corner1 = other.position;
            Vector2 corner2 = other.position;
            corner2.X += other.size.X;
            Vector2 corner3 = other.position + other.size;
            Vector2 corner4 = other.position;
            corner4.Y += other.size.Y;

            if (doesCollide(originA, destinationA, corner1, corner2))
            {
                Vector2 point = getCollisionPoint(originA, destinationA, corner1, corner2);
                CollisionPoint newPoint = new CollisionPoint(originA, point, corner1, corner2);
                returnList.Add(newPoint);
            }
            if (doesCollide(originA, destinationA, corner2, corner3))
            {
                Vector2 point = getCollisionPoint(originA, destinationA, corner2, corner3);
                CollisionPoint newPoint = new CollisionPoint(originA, point, corner3, corner3);
                returnList.Add(newPoint);
            }
            if (doesCollide(originA, destinationA, corner3, corner4))
            {
                Vector2 point = getCollisionPoint(originA, destinationA, corner3, corner4);
                CollisionPoint newPoint = new CollisionPoint(originA, point, corner3, corner4);
                returnList.Add(newPoint);
            }
            if (doesCollide(originA, destinationA, corner4, corner1))
            {
                Vector2 point = getCollisionPoint(originA, destinationA, corner4, corner1);
                CollisionPoint newPoint = new CollisionPoint(originA, point, corner4, corner1);
                returnList.Add(newPoint);
            }

            return returnList;
        }
    }
}
