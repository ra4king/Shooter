using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class DamageEffect : Entity
    {
        private static Random random = new Random();

        private List<Particle> particles;
        private const int GRAVITY = 100;

        public DamageEffect(GameWorld world, double x, double y, double power) : base(world,x,y)
        {
            particles = new List<Particle>();
            for (int a = 0; a < 10; a++)
                particles.Add(new Particle(world, x, y, random.NextDouble() * 2 * Math.PI, random.NextDouble() * 30*power + 20*power));
        }

        public override void update(GameTime gameTime)
        {
            List<Particle> toRemove = new List<Particle>();

            foreach (Particle p in particles)
                if (p.update(gameTime))
                    toRemove.Add(p);

            foreach (Particle p in toRemove)
                particles.Remove(p);

            toRemove.Clear();

            if (particles.Count == 0)
                World.remove(this);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            foreach (Particle p in particles)
                p.draw(spriteBatch);
        }

        private class Particle
        {
            private GameWorld world;
            private Rectangle2D bounds;
            private Vector2 velocity;

            public Particle(GameWorld world, double x, double y, double angle, double speed)
            {
                this.world = world;

                bounds = new Rectangle2D(x, y, 2, 2);

                velocity = new Vector2((float)(speed * Math.Cos(angle)), (float)(speed * Math.Sin(angle)));
            }

            public bool update(GameTime gameTime)
            {
                float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;

                bounds.X += velocity.X * delta;
                bounds.Y += velocity.Y * delta;

                velocity.Y += GRAVITY * delta;

                foreach (Entity e in world.getEntities())
                {
                    if (e.Removed)
                        continue;

                    if (e is Block && e.getBounds2D().intersects(bounds))
                    {
                        if (Math.Abs(velocity.Y) < 2)
                            return true;

                        velocity.Y = -velocity.Y/3;
                    }
                }

                if (bounds.Y >= world.Height)
                    return true;

                return false;
            }

            public void draw(SpriteBatch spriteBatch)
            {
                Assets.getAssets().guys.draw(spriteBatch, new Rectangle((int)Math.Round(bounds.X-world.Camera.x), (int)Math.Round(bounds.Y-world.Camera.y), (int)bounds.Width, (int)bounds.Height), 7, 1, 6, 6, 1, 1, false, Color.White);
            }
        }
    }
}
