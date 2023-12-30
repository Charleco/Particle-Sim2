using Microsoft.Xna.Framework;
using PrimitiveBuddy;
using System;

namespace ParticleSim
{
    public class World
    {
        private Particle[,] grid
        { get; set; }
        private readonly int colCount;
        private readonly int rowCount;
        public readonly int scale;
        public World(int scale, int width, int height)
        {
            colCount = width / scale;
            rowCount = height/scale;
            this.scale = scale;
            grid = new Particle[rowCount, colCount];
            this.PopGrid();
        }
       
        public void PopGrid()
        {
            Random rand = new();
            for(int y=1; y <rowCount-1; y++)
            {
                for(int x = 1; x<colCount-1; x++)
                {
                    if (rand.NextDouble() > 0.5)
                        grid[y,x] = new Particle(x, y, this);
                }
            }
        }
        public void PartAdd(int x, int y)
        {
            grid[y, x] = new Particle(x, y, this);
        }
        public void GridUpdate()
        {
            for(int y = 0;y<rowCount;y++)
            {
                for(int x = 0; x<colCount;x++)
                {
                    if (grid[y,x]!= null)
                    {
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
        }
 
        public void GridBalance()
        {
            for(int y = rowCount-2;y>=1;y--)
            {
                for(int x = colCount-2;x>=1;x--)
                {
                    if (grid[y, x] != null && grid[y - 1,x] == null && grid[y + 1, x] != null && SafeMove(x,y))
                    {
                        int leftCol = ColHeight(x - 1);
                        int rightCol = ColHeight(x + 1);
                        int thisCol = ColHeight(x);
                        if (leftCol < thisCol && leftCol < rightCol && grid[y, x - 1] == null && thisCol - leftCol > 1)
                        {
                            MoveParticle(x, y, x - 1, y);
                        }
                        else if (rightCol < thisCol && rightCol < leftCol && grid[y, x + 1] == null && thisCol - rightCol > 1)
                        {
                            MoveParticle(x, y, x + 1, y);
                        }
                        else if (rightCol == leftCol && leftCol < thisCol && thisCol - leftCol > 1)
                        {
                            Random rand = new();
                            if (rand.NextDouble() > 0.5 && grid[y,x - 1] == null)
                            {
                                MoveParticle(x, y, x - 1, y);
                            }
                            else if (grid[y,x + 1] == null)
                            {
                                MoveParticle(x, y, x + 1, y);
                            }
                        }   
                    }
                }
            }
        }
        public int ColHeight(int x)
        {
            int count = 0;
            for (int y = 1; y < rowCount - 1; y++)
            {
                if (grid[y, x] != null)
                {
                    count++;
                }
                /*
                else
                    break;
                */
            }
            return count;
        }
        public Boolean SafeMove(int x, int yPos)
        {
            int count = 0;
            for (int y = rowCount - 1; y > 1; y--)
            {
                if (grid[y, x] != null)
                {
                    count++;
                }
                else
                    break;
            }
            return count == Math.Abs(yPos - rowCount);
        }
        public void MoveParticle(int x, int y, int newX, int newY)
        {
            Particle temp = grid[y, x];
            grid[y, x] = null;
            grid[newY, newX] = temp;
            temp.SetPos(new Vector2(newX,newY));
        } 
        public void PartMove(float dt)
        {
            for(int y = 1; y < rowCount-1; y++)
            {
                for(int x = 1; x < colCount; x++)
                {
                    if (grid[y, x] != null)
                    {
                        grid[y, x].Grav(dt);
                        grid[y, x].Move(rowCount, dt, grid[y+1,x]);
                    }
                }
            }
        }
        public void PartDraw(Primitive prim)
        {
            Rectangle rect = new(0, 0, scale, scale);
            for (int y = 0; y < rowCount; y++)
            {
                for (int x = 0; x < colCount; x++)
                {
                    if (grid[y, x] != null)
                    {
                        rect.X = x * scale;
                        rect.Y = y * scale;
                        prim.Rectangle(rect, Color.White);
                    }
                }
            }
        }
       
    }
}
