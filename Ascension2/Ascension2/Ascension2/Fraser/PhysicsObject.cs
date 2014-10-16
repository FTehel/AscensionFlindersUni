using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ascension2.Fraser;

namespace Ascension2.Fraser
{
    class PhysicsObject : GameObject
    {

        Boolean collided = false;
        Collision[] currentCollisions = new Collision[0];

        public Collision getCollision(GameObject other)
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

                Vector2 newPosition;

                if (isBetweenValues(thisMaxX, otherMaxX, otherMinX) || isBetweenValues(thisMinX, otherMaxX, otherMinX))
                {
                    if (isBetweenValues(thisMaxY, otherMaxY, otherMinY) || isBetweenValues(thisMinY, otherMaxY, otherMinY))
                    {
                        newPosition = getCollisionPosition(position, other.position);
                        if(other.GetType() == typeof(gridSpace)){
                            gridSpace grid = (gridSpace)other;
                            if (grid.level != 0){
                                return getCollisionOnChildren(grid);
                            }
                            else
                            {
                                return new Collision(newPosition, other);
                            }
                        }
                        else
                        {
                            return new Collision(newPosition, other);
                        }
                    }
                }
            }
            return null;
        }

        public Vector2 getCollisionPosition(Vector2 thisPos, Vector2 otherPos)
        {
            Vector2 returnPos = new Vector2(0,0);
            returnPos.Y = thisPos.Y > otherPos.Y? position.Y:position.Y + size.Y;
            returnPos.X = thisPos.X > otherPos.X ? position.X : position.X + size.X;

            return returnPos;
        }

        public void addCollision(Collision item){
            Collision[] temp = new Collision[currentCollisions.Length];
            for(var i = 0;i < currentCollisions.Length;i++){
                temp[i] = currentCollisions[i];
            }
            temp[currentCollisions.Length] = item;
            currentCollisions = temp;
        }

        public Collision getCollisionOnChildren(gridSpace thisSpace){
            for(int i = 0;i < thisSpace.children.GetLength(0);i ++){
                for (int j = 0; j < thisSpace.children.GetLength(1); j++)
                {
                    Collision thisCollision = getCollision(thisSpace.children[i,j]);
                    if (thisCollision != null)
                    {
                        return thisCollision;
                    }
                }
            }
            return null;
        }

        public Collision getCollisionForGridLine(gridLine line){
            for (var i = 0; i < line.grids.Length; i++)
            {
                return getCollision(line.grids[i]);
            }

            return null;
        }

        public Collision getCollisionForLevel(Level otherLevel){

            for (var i = 0; i < otherLevel.tilesXPositive.Length; i++)
            {
                var otherCol = getCollisionForGridLine(otherLevel.tilesXPositive[i]);
            }
            for (var i = 0; i < otherLevel.tilesXNegative.Length; i++)
            {
                var otherCol = getCollisionForGridLine(otherLevel.tilesXNegative[i]);
            }

            return null;
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

        public void Update(GameTime gameTime, Level thisLevel){
            getCollisionEnter();
        }
    }
}
