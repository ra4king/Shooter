using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Hat : Entity
    {
        private int currentFrame, dir = 1;
        private long timePassed;
        private bool isFalling = true, isExtra;

        public Hat(GameWorld world, double x, double y, bool isExtra)
            : base(world, x, y, 20, 10)
        {
            this.isExtra = isExtra;
        }

        public bool IsExtra
        {
            get { return isExtra; }
        }

        public override void update(GameTime gameTime)
        {
            double oldY = Y;

            if (isFalling)
            {
                timePassed += gameTime.ElapsedGameTime.Milliseconds;

                if (timePassed >= 500)
                {
                    timePassed -= 500;

                    currentFrame += dir;
                    if (currentFrame == 2)
                        dir = -1;
                    if (currentFrame == 0)
                        dir = 1;
                }
            }

            Y += 20 * gameTime.ElapsedGameTime.Milliseconds / 1000f;

            foreach (Entity e in World.getEntities())
            {
                if (e.Removed)
                    continue;
                
                if (e is Block && e.intersect(this))
                {
                    if (Y > e.Y)
                        Y = e.Y + e.Height;
                    else
                    {
                        isFalling = false;
                        Y = e.Y - Height;
                    }
                }
            }

            if(Y != oldY)
                isFalling = true;

            foreach (Entity e in World.getEntities())
                if (e is Block && e.intersect(this))
                {
                    Y += e.Height;
                }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Assets.getAssets().player.draw(spriteBatch, getBounds(), currentFrame * 16 + 3, 33, 10, 5, 0, 0, 0, false, Color.White);
        }
    }
}
