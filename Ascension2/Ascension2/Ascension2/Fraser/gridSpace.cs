﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;


namespace Ascension2
{
    public class gridSpace : GameObject
    {
        public Vector2 thisCoordinate = new Vector2(0,0);
        public gridSpace[,] children;
        public int level;
        public int sizeInGrids;
       //public Vector2 size;
        public int childrenNumber;
        //public Vector2 position;
        public Texture2D texture;
        public gridSpace parent;
        public Boolean isFilled = false;
        public Boolean hasChildren = false;

        public gridSpace(int newLevel, Vector2 newSize, Vector2 newPosition, Vector2 newCoordinate, int thisChildrenNumber, gridSpace parent){
            

            this.level = newLevel;
            this.size = newSize;
            this.position = newPosition;
            this.childrenNumber = thisChildrenNumber;
            this.parent = parent;
            this.thisCoordinate = newCoordinate;
            this.hasCollision = true;

            this.setSizeInCoordinates();
            this.children = new gridSpace[thisChildrenNumber, thisChildrenNumber];

            /*if (newLevel != 0)
            {
                this.children = new gridSpace[thisChildrenNumber, thisChildrenNumber];
                float childSize = this.size.X / thisChildrenNumber;
                var childLevel = level - 1;
                Vector2 childPos = position;

                int xCoordinate = (int)Math.Pow((double)childrenNumber, (double)childLevel);
                int yCoordinate = (int)Math.Pow((double)childrenNumber, (double)childLevel);

                for (int i = 0; i < thisChildrenNumber; i++)
                {
                    for (int j = 0; j < thisChildrenNumber; j++)
                        {
                            Vector2 newCoordinates = new Vector2(this.thisCoordinate.X + (xCoordinate * j), this.thisCoordinate.Y + (yCoordinate * i));
                            children[j,i] = new gridSpace(childLevel, new Vector2(childSize, childSize), childPos, newCoordinates, thisChildrenNumber, this);
                            childPos.X += childSize;
                        }
                    childPos.Y += childSize;
                    childPos.X = position.X;
                }
            }*/
        }

        public void createChildren(){
            hasChildren = true;
            float childSize = this.size.X / childrenNumber;
            var childLevel = level - 1;
            Vector2 childPos = position;

            int xCoordinate = (int)Math.Pow((double)childrenNumber, (double)childLevel);
            int yCoordinate = (int)Math.Pow((double)childrenNumber, (double)childLevel);

            for (int i = 0; i < childrenNumber; i++)
            {
                for (int j = 0; j < childrenNumber; j++)
                    {
                        Vector2 newCoordinates = new Vector2(this.thisCoordinate.X + (xCoordinate * j), this.thisCoordinate.Y + (yCoordinate * i));
                        children[j,i] = new gridSpace(childLevel, new Vector2(childSize, childSize), childPos, newCoordinates, childrenNumber, this);
                        childPos.X += childSize;
                    }
                childPos.Y += childSize;
                childPos.X = position.X;
            }
        }

        public gridSpace getChild(int xCoordinate, int yCoordinate)
        {
            if (level != 0) {
                int multiplier = (int)Math.Pow(childrenNumber, level-1);
                int indexY = (yCoordinate - (int)thisCoordinate.Y)/multiplier;
                int indexX;
                if (xCoordinate >= 0){
                    indexX = (xCoordinate - (int)thisCoordinate.X)/multiplier;
                }
                else
                {
                    indexX = (xCoordinate - (int)thisCoordinate.X)/multiplier;
                }
                if (!hasChildren)
                {
                    createChildren();
                }
                
                return children[indexX, indexY].getChild(xCoordinate,yCoordinate);
            }
            else
            {
                return this;
            }
        }

        public void fillGrid(int xCoord, int yCoord, Texture2D newTexture)
        {
            if (level > 0 && !hasChildren)
            {
                createChildren();
            }
            gridSpace child = getChild(xCoord, yCoord);
            child.setTexture(newTexture);
        }

        public void setSizeInCoordinates()
        {
            double returnInt = Math.Pow((double)childrenNumber, (double)level);
            sizeInGrids = (int)returnInt;
        }

        public void setTexture(Texture2D newTexture){
            texture = newTexture;
            isFilled = true;
            fillParents();
        }

        public void fillParents()
        {
            if (parent != null) { 
                parent.isFilled = true;
                parent.fillParents();
            }
        }

        public override string  ToString()
        {
            return "children: " + childrenNumber + " isFilled: " + isFilled + " coordinates " + thisCoordinate + " size " +
                size + " level: " + level + " texture " + " position " + position;
        }
    }
}
