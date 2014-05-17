using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Gremlin : Entity
    {
        private static Random random = new Random();

        private int currentFrame, level, elapsedTime, shotElapsedTime = 200, shotCount = 20, health = 400;
        private static double gravity = 1000;
        private double vY;

        public Gremlin(GameWorld world, int x, int y, int level)
            : base(world, (x - 1) * 20, (y - 2) * 20, 60, 60)
        {
            this.level = level;

            elapsedTime = random.Next(500) + 500;
        }

        public void die()
        {
            for(int a = 0; a < 10; a++)
                World.add(new DamageEffect(World, X + Width / 2, Y + Height / 2, 2));
            for (int a = 0; a < 10; a++)
            {
                double dir = (a / 10.0) * 2 * Math.PI;
                World.add(new Explosion(World, X + Width / 2 + 5 * Math.Cos(dir), Y + Height / 2 + 5 * Math.Sin(dir), dir, random.Next(200) + 200));
            }
            World.remove(this);

            Assets.getAssets().death.Play();
        }

        public override bool intersect(Entity e)
        {
            if (e is Bullet && !base.intersect(e))
                return false;

            X += 5;
            Y += 5;
            Width -= 10;
            Height -= 10;
            if (!base.intersect(e))
            {
                X -= 5;
                Y -= 5;
                Width += 10;
                Height += 10;
                return false;
            }
            X -= 5;
            Y -= 5;
            Width += 10;
            Height += 10;

            if (e is Explosion)
                die();
            if (e is Bullet)
            {
                health -= 80;

                if (health <= 0)
                {
                    die();
                }
                else
                {
                    World.add(new DamageEffect(World, e.X, e.Y, 1));
                    World.remove(e);

                    Assets.getAssets().hit.Play();
                }
            }
            else if (e is Player)
                ((Player)e).die();
            else if (e is Enemy)
            {
                World.remove(e);
                World.add(new DamageEffect(World, e.X, e.Y, 0.5));
            }

            return true;
        }

        public override void update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            shotElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            double delta = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            if (health < 400)
                health++;

            if (vY == 0)
                currentFrame = 0;
            else if (vY < 0)
                currentFrame = 1;
            else
                currentFrame = 2;

            if (elapsedTime >= 1500)
            {
                vY = -300;
                elapsedTime -= 1500;
            }
            else
            {
                vY += gravity * delta;
                Y += vY * delta;
            }

            if (shotElapsedTime >= 1500 && level == 1)
            {
                shotCount = 0;
                shotElapsedTime -= 1500;
            }

            if (level == 1)
            {
                if (shotCount < 20)
                {
                    shotCount++;

                    if (shotCount < 14)
                        World.add(new Bullet(World, this, X + Width / 2, Y + Height / 2, X + Width / 2 + Math.Cos((shotCount-2) * Math.PI / 10), Y + Height / 2 - Math.Sin((shotCount-2) * Math.PI / 10)));
                }
            }

            foreach (Entity e in World.getEntities())
            {
                if (e.Removed || !e.intersect(this))
                    continue;

                if (e is Spikes)
                    die();
                else if (e is Block)
                {
                    if (vY < 0)
                    {
                        Y = e.Y + e.Height;
                        vY = 0.01;
                    }
                    else
                    {
                        Y = e.Y - Height;
                        vY = 0;
                    }
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Assets.getAssets().gremlins.draw(spriteBatch, getBounds(), currentFrame, level, 30, 30, 30, 30, false, Color.White);

            Y -= 10;
            Height = 5;
            Assets.getAssets().guys.draw(spriteBatch, getBounds(), 9, 1, 6, 6, 1, 1, false, Color.Black);
            Height = 60;
            Y += 10;

            Y -= 10;
            Height = 5;
            Width = 60 * (health / 400.0);

            Assets.getAssets().guys.draw(spriteBatch, getBounds(), 9, 1, 6, 6, 1, 1, false, Color.Red);
            Width = 60;
            Height = 60;
            Y += 10;
        }
    }
}
