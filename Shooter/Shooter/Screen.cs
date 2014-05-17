using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    public abstract class Screen
    {
        private Shooter game;

        public Screen(Shooter game)
        {
            this.game = game;
        }

        public Shooter Game
        {
            get { return game; }
        }

        public int Width
        {
            get { return Game.GraphicsDevice.Viewport.Width; }
        }

        public int Height
        {
            get { return Game.GraphicsDevice.Viewport.Height; }
        }

        public abstract void update(GameTime gameTime);

        public abstract void draw(SpriteBatch spriteBatch);
    }
}
