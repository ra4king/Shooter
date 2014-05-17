using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Bullet : Entity
    {
        private Entity origin;
        private double vx, vy;

        public Bullet(GameWorld world, Entity origin, double x, double y, double px, double py) : base(world,x,y,3,3)
        {
            this.origin = origin;

            double speed = 200;
            vx = speed * (px - x) / Math.Sqrt((px - x) * (px - x) + (py - y) * (py - y));
            vy = speed * (py - y) / Math.Sqrt((px - x) * (px - x) + (py - y) * (py - y));

            Assets.getAssets().pew.Play();
        }

        public double vX
        {
            get { return vx; }
        }

        public double vY
        {
            get { return vy; }
        }

        public override void update(GameTime gameTime)
        {
            float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;
            X += vx * delta;
            Y += vy * delta;

            if (X < 0 || X >= World.Width || Y < 0)
                World.remove(this);

            foreach (Entity e in World.getEntities())
            {
                if (e.Removed)
                    continue;

                if (e == origin || (e is Enemy && e.getBounds2D().intersects(origin.getBounds2D())))
                    continue;

                if (e.intersect(this) && Removed)
                    break;
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Assets.getAssets().guys.draw(spriteBatch, getBounds(), 9, 1, 6, 6, 1, 1, false, Color.Yellow);
        }
    }
}
