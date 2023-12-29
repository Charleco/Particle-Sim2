using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PrimitiveBuddy;
namespace ParticleSim
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Primitive _prim;
        private World world;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            int winWidth = GraphicsDevice.Viewport.Width;
            int winHeight = GraphicsDevice.Viewport.Height;
            world = new(10,winWidth,winHeight);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new(GraphicsDevice);
            _prim = new(_graphics.GraphicsDevice, _spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            world.PartMove(dt);
            world.GridUpdate();
            world.GridBalance();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            world.PartDraw(_prim);
            _spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
