using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PrimitiveBuddy;
namespace ParticleSim
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Primitive _prim;
        private World world;
        private Vector2 MousePos;
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
            MousePos = new (_graphics.GraphicsDevice.Viewport.Width / 2, _graphics.GraphicsDevice.Viewport.Height / 2);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new(GraphicsDevice);
            _prim = new(_graphics.GraphicsDevice, _spriteBatch);
        }

        protected override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            MousePos.X = mouseState.X;
            MousePos.Y = mouseState.Y;
            var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
            world.PartMove(dt);
            world.GridUpdate();
            world.GridBalance();
            System.Diagnostics.Debug.WriteLine(MousePos+ " "+ MousePos.X / world.scale + " "+ MousePos.Y / world.scale);
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                world.PartAdd((int)MousePos.X/world.scale, (int)MousePos.Y/world.scale);
            }
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
