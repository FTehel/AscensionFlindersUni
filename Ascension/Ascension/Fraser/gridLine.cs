using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ascension
{
    class gridLine
    {
        public gridSpace[] grids;
        public int level;
        public int sizeInGrids;
        public float size;
        public int childrenNumber;
        public int thisXCoordinate;

        public gridLine(int newLevel, int newChildrenNumber, float newSize, int newCoordinate)
        {
            this.level = newLevel;
            this.childrenNumber = newChildrenNumber;
            this.size = newSize;
            this.sizeInGrids = (int)Math.Pow((double)newChildrenNumber, (double)newLevel);
            this.thisXCoordinate = newCoordinate;

            grids = new gridSpace[0];
        }

        public void fillGrid(int xCoord, int yCoord, Texture2D texture){
            int index = yCoord / sizeInGrids;
            if (index >= grids.Length || grids[index] == null)
            {
                int coordMult = (int)Math.Pow((double)childrenNumber, (double)level);
                Vector2 newCoordinate = new Vector2(thisXCoordinate, index * coordMult);
                gridSpace newGrid = new gridSpace(level, size, getPosition(xCoord, yCoord), newCoordinate, childrenNumber, null);
                addGrid(index, newGrid);
            }
            grids[index].fillGrid(xCoord, yCoord, texture);
        }

        public Vector2 getPosition(int xCoord, int yCoord){
            int xIndex = xCoord / sizeInGrids;
            int yIndex = yCoord / sizeInGrids;

            float x = xIndex * size;
            float y = yIndex * size;

            return new Vector2(x, y);
        }

        public void addGrid(int yCoord, gridSpace newGrid)
        {
            if (yCoord >= grids.Length)
            {
                gridSpace[] temp = new gridSpace[yCoord + 1];
                for (var i = 0; i < grids.Length; i++)
                {
                    temp[i] = grids[i];
                }
                grids = temp;
            }
            grids[yCoord] = newGrid;
        }
    }
}
