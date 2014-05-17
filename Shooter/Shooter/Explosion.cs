using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Explosion : Entity
    {
        private static Random random = new Random();
        private long elapsedTime, frameTime;
        private int currentColor, currentFrame;
        private double vX, vY;
        private int life;
        
        public Explosion(GameWorld world, double x, double y, double dir, int life) : base(world,x,y,12,12)
        {
            this.vX = Math.Cos(dir) * 60 + (random.NextDouble() - random.NextDouble()) * 15;
            this.vY = Math.Sin(dir) * 60 + (random.NextDouble() - random.NextDouble()) * 15;
            this.life = life;

            currentColor = 4 + random.Next(3);
        }

        public override void update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            frameTime += gameTime.ElapsedGameTime.Milliseconds;
            
            double delta = gameTime.ElapsedGameTime.Milliseconds/1000.0;

            X += vX * delta;
            Y += vY * delta;

            if (elapsedTime >= life)
                World.remove(this);

            if (frameTime >= 50)
            {
                currentFrame++;
                if (currentFrame > 7)
                    currentFrame = 0;

                frameTime -= 50;
            }

            if (elapsedTime >= 100 && elapsedTime <= life * 0.7)
            {
                foreach (Entity e in World.getEntities())
                {
                    if (e.Removed)
                        continue;

                    e.intersect(this);
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Assets.getAssets().guys.draw(spriteBatch, getBounds(), currentFrame, currentColor, 6, 6, 6, 6, false, Color.White);
        }
    }
}
