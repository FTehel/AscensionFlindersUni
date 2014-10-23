using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Ascension2
{
    public class Floor
    {
        int height = 0;
        public int[] walls = new int[0];

        public Floor below;
        public Floor above;

        public int yCoordinate = 0;

        public Floor(int start, int end, int newHeight, Vector2 roomSize, Floor newBelow, int currentHeight)
        {
            this.addWall(start);
            this.addWall(end);
            this.height = newHeight;
            this.generateRooms(roomSize);
            this.below = newBelow;
            this.yCoordinate = currentHeight;
        }

        public void addWall(int location){
            int[] temp = new int[walls.Length + 1];
            int j = 0;
            Boolean placed = false;
            for (int i = 0; i < temp.Length; i++)
            {
                if (walls.Length == 0)
                {
                    temp[j] = location;
                    
                }
                else if(!placed){
                    if (i != 0)
                    {
                        int last = walls[i - 1];
                        if (last < location)
                        {
                            temp[j] = location;
                            placed = true;
                        }
                        else
                        {
                            temp[j] = walls[i];
                        }
                    }
                    else
                    {
                        int next = walls[i];
                        if (next > location)
                        {
                            temp[j] = location;
                            i--;
                            placed = true;
                        }
                        else{
                            temp[j] = walls[i];
                        }
                    }
                }
                else
                {
                    temp[j] = walls[i - 1];
                }
                j++;
            }
            walls = temp;
        }

        
        
        public void generateRooms(Vector2 size)
        {
            Random rand = new Random();
            int i = walls[0];
            int end = walls[1];
            while (end - i >= (int)size.X)
            {
                int width = rand.Next((int)size.X, (int)size.Y);
                
                    if (end - (i + width) < size.X)
                    {
                        if (end - i >= (int)size.X * 2)
                        {
                            width = rand.Next((int)size.X, end - i - (int)size.X);

                        }
                        else
                        {
                            width = end - i;
                        }
                    }
                
                i += width;
                addWall (i);
            }
        }
            
    }
}
