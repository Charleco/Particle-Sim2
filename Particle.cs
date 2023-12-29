using Microsoft.Xna.Framework;

namespace ParticleSim
{
    public class Particle
    {
        private Vector2 pos;
        private Vector2 vel;
        private readonly World world;
        public Particle(float x, float y, World world)
        {
            this.pos = new Vector2(x, y);
            this.vel = new Vector2(0f, 0f);
            this.world = world;
        }
        public Vector2 GetPos()
        {
            return this.pos;
        }
        public void SetPos(Vector2 pos)
        {
            this.pos = pos;
        }
        public Vector2 GetVel()
        {
            return this.vel;
        }
        public void SetVel(Vector2 vel)
        {
            this.vel = vel;
        }
        public void Grav(float dt)
        {
            this.vel.Y += 5f*dt;
        }
        public void Move(int rowCount, float dt, Particle below)
        {
            if (pos.Y > rowCount-1)
            {
                pos.Y = rowCount-1;
                vel.Y = 0;
            }
            
            else if (below != null)
            {
                vel.Y = 0;
            }
            else
            {
                pos.X += vel.X * dt;
                pos.Y += vel.Y * dt;
            }
        }
    }
}
