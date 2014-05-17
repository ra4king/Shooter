using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Jabberwocky : Entity
    {
        private static Random random = new Random();

        private int health = 0, currentFrame = 3, elapsedTime;

        public Jabberwocky(GameWorld world, int x, int y)
            : base(world, (x - 1) * 20, (y - 2) * 20, 60, 60)
        {
        }

        public void die()
        {
            for (int a = 0; a < 10; a++)
                World.add(new DamageEffect(World, X + Width / 2, Y + Height / 2, 2));
            World.remove(this);

            Assets.getAssets().death.Play();
        }

        public override bool intersect(Entity e)
        {
            Y += 10;
            Height -= 10;
            if (!base.intersect(e))
            {
                Y -= 10;
                Height += 10;
                return false;
            }
            Y -= 10;
            Height += 10;

            if (e is Player)
                ((Player)e).die();
            else if (e is Enemy)
            {
                health += 10;

                if (health >= 400)
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
            else if (e is Explosion)
            {
                die();
            }

            return true;
        }

        public override void update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > 1000)
            {
                currentFrame = currentFrame == 3 ? 4 : 3;
                elapsedTime -= 1000;
            }

            if (health > 0)
                health--;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Assets.getAssets().gremlins.draw(spriteBatch, getBounds(), currentFrame, 0, 30, 30, 30, 30, false, Color.White);

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
