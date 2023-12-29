using Microsoft.VisualBasic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PrimitiveBuddy;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ParticleSim
{
    public class World
    {
        private Particle[,] grid
        { get; set; }
        private int colCount;
        private int rowCount;
        private int scale;
        public World(int scale, int width, int height)
        {
            colCount = width / scale;
            rowCount = height/scale;
            this.scale = scale;
            grid = new Particle[rowCount, colCount];
            this.popGrid();
            /*
            grid[7, 3] = new Particle(7, 3, this);
            grid[3, 7] = new Particle(3, 7, this);
            grid[3, 8] = new Particle(3, 8, this);
            grid[3, 10] = new Particle(10, 3, this);
            grid[3, 5] = new Particle(5, 3, this);
            */
        }
       
        public void popGrid()
        {
            Random rand = new Random();
            for(int y=1; y <rowCount-1; y++)
            {
                for(int x = 1; x<colCount-1; x++)
                {
                    if (rand.NextDouble() > 0.5)
                        grid[y,x] = new Particle(x, y, this);
                }
            }
        }
        public void gridUpdate()
        {
            int partCount = 0;
            for(int y = 0;y<rowCount;y++)
            {
                for(int x = 0; x<colCount;x++)
                {
                    if (grid[y,x]!= null)
                    {
                        partCount++;
                        int newRow = (int)(grid[y, x].GetPos().Y);
                        int newCol = (int)(grid[y, x].GetPos().X);
                        if ((newRow != y || newCol != x) && grid[newRow, newCol] == null)
                        {
                            grid[newRow, newCol] = grid[y, x];
                            grid[y,x] = null;
                        }

                    }
                }
            }
            Console.WriteLine(partCount);
        }
        public int colHeight(int x)
        {
            int count = 0;
            for (int y = 1; y <rowCount-1;y++)
            {
                if (grid[y, x] != null)
                {
                    count++;
                }
            }
                return count;
        }
        public Boolean safeMove(int x, int yPos)
        {
            int count = 0;
            for (int y = rowCount-1; y > 1; y--)
            {
                if (grid[y, x] != null)
                {
                    count++;
                }
                else
                    break;
            }
            //System.Diagnostics.Debug.WriteLine("count "+count+" ypos: "+yPos+ " test "+ (yPos - rowCount));
            return count==Math.Abs(yPos-rowCount);
        }
    
        public void gridBalance()
        {
            for(int y = rowCount-2;y>=1;y--)
            {
                for(int x = colCount-2;x>=1;x--)
                {
                    if (grid[y, x] != null && grid[y - 1,x] == null && grid[y + 1, x] != null && safeMove(x,y))
                    {
                        int leftCol = colHeight(x - 1);
                        int rightCol = colHeight(x + 1);
                        int thisCol = colHeight(x);
                        //System.Diagnostics.Debug.WriteLine("left: " + leftCol + " right: " + rightCol + " this: " + thisCol);
                        if (leftCol < thisCol && leftCol < rightCol && grid[y, x - 1] == null && thisCol - leftCol > 1)
                        {
                            moveParticle(x, y, x - 1, y);
                        }
                        else if (rightCol < thisCol && rightCol < leftCol && grid[y, x + 1] == null && thisCol - rightCol > 1)
                        {
                            moveParticle(x, y, x + 1, y);
                        }
                        else if (rightCol == leftCol && leftCol < thisCol && thisCol - leftCol > 1)
                        {
                            Random rand = new Random();
                            if (rand.NextDouble() > 0.5 && grid[y,x - 1] == null)
                            {
                                moveParticle(x, y, x - 1, y);
                            }
                            else if (grid[y,x + 1] == null)
                            {
                                moveParticle(x, y, x + 1, y);
                            }
                        }   
                    }
                }
            }
        }
        public void moveParticle(int x, int y, int newX, int newY)
        {
            Particle temp = grid[y, x];
            grid[y, x] = null;
            grid[newY, newX] = temp;
            temp.SetPos(new Vector2(newX,newY));
        } 
        public void partMove(float dt)
        {
            for(int y = 1; y < rowCount-1; y++)
            {
                for(int x = 1; x < colCount; x++)
                {
                    if (grid[y, x] != null)
                    {
                        grid[y, x].grav(dt);
                        grid[y, x].move(rowCount, dt, grid[y+1,x]);
                    }
                }
            }
        }
        public void partDraw(Primitive prim)
        {
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    if (grid[y, x] != null)
                    {
                        prim.Rectangle(new Microsoft.Xna.Framework.Rectangle(x * scale, y * scale, scale, scale), Microsoft.Xna.Framework.Color.White);
                    }
                }
            }
        }
       
    }
}
