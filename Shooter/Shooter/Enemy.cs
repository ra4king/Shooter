using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Enemy : Entity
    {
        private static Random random = new Random();

        private Player player;
        private bool isFacingRight, isFlying = true;
        private long timePassed;

        private const int gravity = 1000, friction = 100;
        private double vX, vY;

        public Enemy(GameWorld world, Player player, double x, double y, double direction, double power)
            : base(world, x, y, 12, 12)
        {
            this.player = player;

            timePassed = random.Next(1000);

            if (direction == -1)
                return;

            double speed = power * (random.Next(100) + 250);
            this.vX = speed * Math.Cos(direction);
            this.vY = speed * Math.Sin(-direction);
        }

        public double VX
        {
            get { return vX; }
            set { vX = value; }
        }

        public override bool intersect(Entity e)
        {
            if (!base.intersect(e))
                return false;

            if (e is Bullet)
            {
                World.remove(this);
                World.remove(e);
                World.add(new DamageEffect(World, e.X, e.Y, 2));
            }
            else if (e is Explosion)
            {
                World.remove(this);
            }

            return true;
        }

        public override void update(GameTime gameTime)
        {
            isFacingRight = player.X - X >= 0;

            double delta = gameTime.ElapsedGameTime.Milliseconds / 1000.0;

            double oldX = X, oldY = Y;

            vY += gravity * delta;
            Y += vY * delta;

            vX += vX > 0 ? - friction * delta : vX < 0 ? friction * delta : 0;
            X += vX * delta;
            
            foreach (Entity e in World.getEntities())
            {
                if (e.Removed || !e.intersect(this))
                    continue;

                if (e is Block)
                {
                    double newY = Y;
                    Y = oldY;
                    if (e.intersect(this))
                    {
                        X = oldX;

                        if (X < e.X)
                            X = e.X - Width - 1;
                        else
                            X = e.X + e.Width + 1;

                        vX = 0;
                    }
                    Y = newY;

                    double newX = X;
                    X = oldX;
                    if (e.intersect(this))
                    {
                        if (Y < e.Y)
                        {
                            Y = e.Y - Height;

                            if (isFlying)
                            {
                                isFlying = false;

                                foreach (Entity en in World.getEntities())
                                    if (en != this && en is Enemy && e.intersect(this))
                                        en.X += Math.Abs(vX) / vX * 10;

                                vX = 0;
                            }
                        }
                        else
                            Y = oldY;

                        vY = 0;
                    }
                    X = newX;
                }
            }

            if (!isFlying && !player.IsDead)
            {
                timePassed += gameTime.ElapsedGameTime.Milliseconds;

                long time = 1800;
                if (timePassed >= time)
                {
                    timePassed -= time;
                    World.add(new Bullet(World, this, X + Width/2, Y + Height / 2, player.X + player.Width / 2, player.Y + player.Height / 2));
                }
            }

            if (X < 0 || X > World.Width)
                World.remove(this);

            if (Y < 0 || Y > World.Height)
                World.remove(this);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            Assets.getAssets().guys.draw(spriteBatch, getBounds(), 0, 2, 6, 6, 6, 6, !isFacingRight, Color.White);
        }
    }
}
