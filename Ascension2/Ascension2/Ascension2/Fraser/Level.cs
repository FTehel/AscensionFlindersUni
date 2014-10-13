using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Ascension2
{
    class Level
    {
        public gridLine[] tilesXPositive;
        public gridLine[] tilesXNegative;
        int level = 1;
        int sizeInGrids;
        float size = 20;
        public int gridSize = 20;
        int childrenNumber = 2;
        Vector2 startingPoint = new Vector2(0,0);

        public Level()
        {
            this.size = this.gridSize * (float)Math.Pow(childrenNumber, level);
            this.sizeInGrids = (int)Math.Pow(childrenNumber, level);
            tilesXNegative = new gridLine[0];
            tilesXPositive = new gridLine[0];
        }

        public int getIndex(int xCoord)
        {
            if (xCoord >= 0)
            {
                return xCoord / sizeInGrids;
            }
            else
            {
                return -((xCoord+1) / sizeInGrids);
            }
        }

        public void fillTile(int xCoord, int yCoord, Texture2D texture)
        {
            int index = getIndex(xCoord);

            if (xCoord >= 0)
            {
                addTileArrayPositive(index);
                
                tilesXPositive[index].fillGrid(xCoord, yCoord, texture);
            }
            else
            {
                addTileArrayNegative(index);
                tilesXNegative[index].fillGrid(xCoord, yCoord, texture);
            }
        }

        public void addTileArrayPositive(int index)
        {
            if (index >= tilesXPositive.Length)
            {
                gridLine[] temp = new gridLine[index + 1];
                for (var i = 0; i < tilesXPositive.Length; i++)
                {
                    temp[i] = tilesXPositive[i];
                }
                tilesXPositive = temp;
            }
            if (tilesXPositive[index] == null) {
                gridLine newArray = new gridLine(level, childrenNumber, size, index * sizeInGrids);
                tilesXPositive[index] = newArray;
            }   
        }

        public void addTileArrayNegative(int index)
        {
            
            if (index >= tilesXNegative.Length)
            {
                gridLine[] temp = new gridLine[index + 1];
                for (var i = 0; i < tilesXNegative.Length; i++)
                {
                    temp[i] = tilesXNegative[i];
                }
                tilesXNegative = temp;
            }
            if (tilesXNegative[index] == null)
            {
                gridLine newArray = new gridLine(level, childrenNumber, size, -((index + 1) * sizeInGrids));
                tilesXNegative[index] = newArray;
            }
            
        }
    }
}
