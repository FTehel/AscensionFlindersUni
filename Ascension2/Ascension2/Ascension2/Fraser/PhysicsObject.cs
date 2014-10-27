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
        int checks = 0;

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

        public Collision[] addToCollisionList(Collision[] list, Collision item)
        {
            Collision[] temp = new Collision[list.Length + 1];
            for (int i = 0; i < list.Length; i++)
            {
                temp[i] = list[i];
            }
            temp[list.Length] = item;
            return temp;
        }

        public CollisionPoint[] addToCollisionPointList(CollisionPoint[] list, CollisionPoint item)
        {
            CollisionPoint[] temp = new CollisionPoint[list.Length + 1];
            for (int i = 0; i < list.Length; i++)
            {
                temp[i] = list[i];
            }
            temp[list.Length] = item;
            return temp;
        }

        public CollisionPoint[] joinCollisionPointList(CollisionPoint[] list, CollisionPoint[] list2)
        {
            CollisionPoint[] temp = new CollisionPoint[list.Length + list2.Length];
            for (int i = 0; i < list.Length; i++)
            {
                temp[i] = list[i];
            }
            for (int i = list.Length; i < temp.Length; i++)
            {
                temp[i] = list2[i - list.Length];
            }
            return temp;
        }

        public Collision[] getCollision(Vector2 currentPos, Vector2 newPos, GameObject other, Collision[] collisions)
        {
            if (other != null && other.hasCollision)
            {
                Boolean filled = true;
                Boolean isGrid = false;
                if (other.GetType() == typeof(gridSpace))
                {
                    gridSpace grid = (gridSpace)other;
                    filled = grid.isFilled;
                    isGrid = true;
                }
                if (!isGrid || filled)
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
                            if (other.GetType() == typeof(gridSpace))
                            {
                                gridSpace grid = (gridSpace)other;
                                if (grid.level != 0)
                                {
                                    collisions = getCollisionOnChildren(currentPos, newPos, grid, collisions);
                                }
                                else
                                {
                                    collisions = addToCollisionList(collisions, new Collision(other));
                                }
                            }
                            else
                            {
                                collisions = addToCollisionList(collisions, new Collision(other));
                            }
                        }
                    }
                }
            }
            return collisions;
        }

        public CollisionPoint[] getCollisionPoints(Vector2 currentPos, GameObject other, CollisionPoint[] collisions, Vector2 newVelocity)
        {
            //List<Collision> newCollisions = collisions;
            if (other != null)
            {
                if (other.hasCollision)
                {
                    Boolean filled = true;
                    Boolean isGrid = false;
                    if (other.GetType() == typeof(gridSpace))
                    {
                        gridSpace grid = (gridSpace)other;
                        filled = grid.isFilled;
                        isGrid = true;
                    }

                    if (!isGrid || filled)
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



                        int currentMaxX = (int)currentPos.X + (int)size.X;
                        int currentMinX = (int)currentPos.X;
                        int currentMaxY = (int)currentPos.Y + (int)size.Y;
                        int currentMinY = (int)currentPos.Y;

                        Console.WriteLine(checks);
                        checks++;

                        CollisionPoint[] collisionsA = getCollisionPointOnRectangle(corner1, corner1Projection, other);
                        CollisionPoint[] collisionsB = getCollisionPointOnRectangle(corner2, corner2Projection, other);
                        CollisionPoint[] collisionsC = getCollisionPointOnRectangle(corner3, corner3Projection, other);
                        CollisionPoint[] collisionsD = getCollisionPointOnRectangle(corner4, corner4Projection, other);

                        collisionsA = joinCollisionPointList(collisionsA, collisionsB);
                        collisionsA = joinCollisionPointList(collisionsA, collisionsC);
                        collisionsA = joinCollisionPointList(collisionsA, collisionsD);

                        if (collisionsA.Length > 0)
                        {
                            CollisionPoint[] points = collisionsA;
                            if (isGrid)
                            {
                                gridSpace grid = (gridSpace)other;
                                if (grid.level > 0)
                                {
                                    Console.WriteLine("Should not go to children");
                                    //collisions = getCollisionOnChildren(currentPos, newPos, grid, collisions, newVelocity);
                                }
                                else
                                {
                                    for (int i = 0; i < points.Length; i++)
                                    {
                                        collisions = addToCollisionPointList(collisions, points[i]);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0; i < points.Length; i++)
                                {
                                    collisions = addToCollisionPointList(collisions, points[i]);
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

        public Collision[] getCollisionOnChildren(Vector2 currentPos, Vector2 newPos, gridSpace thisSpace, Collision[] collisions){
            for(int i = 0;i < thisSpace.children.GetLength(0);i ++){
                for (int j = 0; j < thisSpace.children.GetLength(1); j++)
                {
                    collisions = getCollision(currentPos, newPos, thisSpace.children[i,j], collisions);
                }
            }
            return collisions;
        }

        public Collision[] getCollisionForGridLine(Vector2 currentPos, Vector2 newPos, gridLine line, Collision[] collisions){
            for (int i = 0; i < line.grids.Length; i++)
            {
                collisions = getCollision(currentPos, newPos, line.grids[i], collisions);
                //i = line.grids.Length;
            }
            return collisions;
        }

        public void getCollisionForLevel(Level otherLevel, GameTime gameTime){
            if (velocity.Length() > 0)
            {
                Vector2 newPos = (velocity * (float)gameTime.ElapsedGameTime.TotalSeconds) + position;
                Vector2 newVelocity = velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
                Vector2 currentPos = position;
                Collision[] collisions = new Collision[0];
                currentCollisions = new Collision[0];
                for (int i = 0; i < otherLevel.tilesXPositive.Length; i++)
                {
                    if (otherLevel.tilesXPositive[i] != null)
                    {
                        collisions = getCollisionForGridLine(currentPos, newPos, otherLevel.tilesXPositive[i], collisions);
                        //i = otherLevel.tilesXPositive.Length;
                    }
                }
                for (int i = 0; i < otherLevel.tilesXNegative.Length; i++)
                {
                    if (otherLevel.tilesXNegative[i] != null)
                    {
                        collisions = getCollisionForGridLine(currentPos, newPos, otherLevel.tilesXNegative[i], collisions);
                        //i = otherLevel.tilesXNegative.Length;
                    }
                }
                currentCollisions = collisions;
            }
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
            checks = 0;
            velocity = currentVelocity;
            getCollisionForLevel(thisLevel, gameTime);
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
            
            float deltaTime = (float)time.TotalGameTime.TotalSeconds;

            newVelocity *= deltaTime;

            Boolean down = true;
            Boolean up = true;
            Boolean left = true;
            Boolean right = true;

            CollisionPoint[] points = new CollisionPoint[0];
            for (int i = 0; i < currentCollisions.Length; i++)
            {
                Vector2 otherPos = currentCollisions[i].other.position;
                Vector2 otherSize = currentCollisions[i].other.size;
                if (otherPos.X == position.X + size.X)
                {
                    right = false;
                }
                if (otherPos.X + otherSize.X == position.X)
                {
                    left = false;
                }
                if (otherPos.Y == position.Y + size.Y)
                {
                    up = false;
                }
                if (otherPos.Y + otherSize.Y == position.Y)
                {
                    down = false;
                }
                points = getCollisionPoints(position, currentCollisions[i].other, points, newVelocity);
            }

                if (deltaTime != 0)
                {
                    Vector2 nVelocity = velocity;
                    nVelocity.Normalize();
                    float dot = 0;
                    float speed = velocity.Length() * deltaTime;

                    CollisionPoint shortest = getShortestCollision(points);
                    Vector2 side = shortest.otherSide;
                    Vector2 collisionOverlap = shortest.origin + (velocity * deltaTime) - shortest.collision;
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
                    newVelocity = (shortest.collision - shortest.origin) + (sideVelocity * dot * collisionOverlapLength);
                    newVelocity /= deltaTime;
                }
                if (!left && newVelocity.X < 0)
                {
                    newVelocity.X = 0;
                }
                if (!right && newVelocity.X > 0)
                {
                    newVelocity.X = 0;
                }
                if (!up && newVelocity.Y > 0)
                {
                    newVelocity.Y = 0;
                }
                if (!down && newVelocity.Y < 0)
                {
                    newVelocity.Y = 0;
                }
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

        public CollisionPoint getShortestCollision(CollisionPoint[] collisions)
        {
            float shortestDistance = 0;
            int shortestIndex = -1;
            for (int i = 0; i < collisions.Length; i++)
            {
                if (shortestIndex == -1 || shortestDistance > collisions[i].length)
                {
                    shortestDistance = collisions[i].length;
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

            if (percentA >= 0 && percentA <= 1 && percentB >= 0 && percentB <= 1)
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

            if (percentA >= 0 && percentA <= 1 && percentB >= 0 && percentB <= 1)
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
            if (collisionPointPercentB == 0)
            {
                return 0;
            }

            return collisionPointPercent;
        }

        public CollisionPoint[] getCollisionPointOnRectangle(Vector2 originA, Vector2 destinationA, GameObject other){
            CollisionPoint[] returnList = new CollisionPoint[0];
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
                returnList = addToCollisionPointList(returnList, newPoint);
            }
            if (doesCollide(originA, destinationA, corner2, corner3))
            {
                Vector2 point = getCollisionPoint(originA, destinationA, corner2, corner3);
                CollisionPoint newPoint = new CollisionPoint(originA, point, corner2, corner3);
                returnList = addToCollisionPointList(returnList, newPoint);
            }
            if (doesCollide(originA, destinationA, corner3, corner4))
            {
                Vector2 point = getCollisionPoint(originA, destinationA, corner3, corner4);
                CollisionPoint newPoint = new CollisionPoint(originA, point, corner3, corner4);
                returnList = addToCollisionPointList(returnList, newPoint);
            }
            if (doesCollide(originA, destinationA, corner4, corner1))
            {
                Vector2 point = getCollisionPoint(originA, destinationA, corner4, corner1);
                CollisionPoint newPoint = new CollisionPoint(originA, point, corner4, corner1);
                returnList = addToCollisionPointList(returnList, newPoint);
            }

            return returnList;
        }
    }
}
