using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Ascension
{

    class proceduralGenerator
    {
        int towerWidth = 80;
        int floorHeight = 10;
        int towerStart = -80;

        Vector2 roomWidth = new Vector2(10, 30);

        Floor startFloor;
        Floor lastFloor;
        Floor current;

        int currentHeight = 0;

        float generateDistance = 1.5f;

        int floors = 0;

        void fillGrid(int x, int y, Texture2D texture, Level levelToFill){
            levelToFill.fillTile(x, y, texture);
        }

        public void createLine(int startX, int startY, int endX, int endY, Texture2D texture, Level levelToFill)
        {
            Vector2 start = new Vector2(startX, startY);
            Vector2 end = new Vector2(endX, endY);
            Vector2 velocity = end - start;

            if (velocity.X > 0){
                velocity.X = 1;
            }
            else if (velocity.X < 0){
                velocity.X = -1;
            }
            if (velocity.Y > 0){
                velocity.Y = 1;
            }
            else if (velocity.Y < 0){
                velocity.Y = -1;
            }

            int i = startX;
            while (!start.Equals(end))
            {
                fillGrid((int)start.X, (int)start.Y, texture, levelToFill);
                start += velocity;
            }
        }

        public void createBlock(int startX, int startY, int endX, int endY, Texture2D texture, Level levelToFill)
        {
            int currentX = startX;
            while (currentX != endX)
            {
                createLine(currentX, startY, currentX, endY, texture, levelToFill);
                if (currentX < endX)
                {
                    currentX++;
                }
                else
                {
                    currentX--;
                }
            }
        }

        public void createFloor(Texture2D texture, Level levelToFill)
        {
            
            Floor newFloor = new Floor(towerStart, towerStart + towerWidth, floorHeight, roomWidth, current, currentHeight);
            if (current != null)
            {
                current.above = newFloor;
            }
            current = newFloor;
            createFloorAndCeiling(towerStart, currentHeight, towerStart + towerWidth, currentHeight + floorHeight, texture, levelToFill);
            for (int i = 0; i < current.walls.Length; i++)
            {
                createLine(current.walls[i], currentHeight, current.walls[i], currentHeight + floorHeight, texture, levelToFill);
            }
            currentHeight += floorHeight;
            floors++;
            if (floors % 10 == 0)
            {
                Console.WriteLine("There are " + floors + " floors");
            }
        }

        public void createFloorAndCeiling(int startX, int startY, int endX, int endY, Texture2D texture, Level levelToFill)
        {
            createLine(startX, startY, endX, startY, texture, levelToFill);
            createLine(startX, endY, endX, endY, texture, levelToFill);
        }

        public int gridToDistance(int grids, int gridSize)
        {
            return grids * gridSize;
        }

        public Boolean isHeightAboveScreen(int gridSize, int cameraHeight)
        {
            int screenHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            int currentHeightInDistance = gridToDistance(currentHeight, gridSize);
            int heightToScreen = currentHeightInDistance - cameraHeight;
            if (heightToScreen <= screenHeight * generateDistance)
            {
                return true;
            }
            return false;
        }

        public void generate(int gridSize, int cameraHeight, Texture2D texture, Level levelToFill){
            if(isHeightAboveScreen(gridSize, cameraHeight)){
                createFloor(texture, levelToFill);
            }
        }
    }
}
