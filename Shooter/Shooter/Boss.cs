using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Boss : Entity
    {
        private static Random random = new Random();
        private List<BossNeck> neck;
        private int elapsedTime = random.Next(1000);

        private int moveElapsedTime;
        private double headRot = Math.PI;
        private const double dist = 150;
        private bool movingCW = true;

        private double origX, origY;

        public Boss(GameWorld world, int x, int y)
            : base(world, (x*20) + dist * Math.Cos(Math.PI), (y*20) - dist * Math.Sin(Math.PI),30,30)
        {
            neck = new List<BossNeck>();

            this.origX = x * 20;
            this.origY = y * 20;

            for (int a = 0; a < 10; a++)
                neck.Add(new BossNeck(world, x * 20, y * 20, a * 15, Math.PI/2 + (Math.PI/2) * Math.Sin((a/10.0) * Math.PI/2)));
        }

        public void die()
        {
            for (int a = 0; a < 5; a++)
                World.add(new DamageEffect(World, X + Width / 2, Y + Height / 2, 2));
            for (int a = 0; a < 10; a++)
            {
                double dir = (a / 10.0) * 2 * Math.PI;
                World.add(new Explosion(World, X + Width / 2 + 5 * Math.Cos(dir), Y + Height / 2 + 5 * Math.Sin(dir), dir, 400));
            }

            World.remove(this);
        }

        public override bool intersect(Entity e)
        {
            if (base.intersect(e))
            {
                if (e is Player)
                    ((Player)e).die();
                else if (e is Explosion)
                    die();

                return true;
            }

            bool intersect = false;

            for(int a = 0; a < 10; a++)
            {
                if (neck[a] != null)
                {
                    if (neck[a].intersect(e))
                    {
                        neck[a] = null;
                        intersect = true;
                    }
                }
            }

            return intersect;
        }

        public override void update(GameTime gameTime)
        {
            moveElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (moveElapsedTime > 30)
            {
                foreach(BossNeck b in neck)
                    if(b != null)
                        b.update();

                headRot += movingCW ? -Math.PI / 59 : Math.PI / 59;

                X = origX + dist * Math.Cos(headRot);
                Y = origY - dist * Math.Sin(headRot);

                if (headRot > Math.PI || headRot < Math.PI / 2)
                    movingCW = !movingCW;

                moveElapsedTime -= 30;
            }

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime >= 1500)
            {
                for (int a = 0; a < 10; a++)
                {
                    World.add(new Enemy(World, World.Game.manager.Player, X, Y, random.NextDouble() * Math.PI + (Math.PI/2), 2));
                    World.add(new Bullet(World, this, X, Y, X + Math.Cos((a / 5.0) * Math.PI), Y + (Math.Sin((a / 5.0) * Math.PI))));
                }

                Assets.getAssets().pew.Play();

                elapsedTime -= 1500;
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            foreach (BossNeck b in neck)
                if(b != null)
                    b.draw(spriteBatch);

            Assets.getAssets().gremlins.draw(spriteBatch, getBounds(), 3, 1, 30, 30, 14, 17, false, Color.White);
        }

        class BossNeck
        {
            private double delta = Math.PI / 60;

            private GameWorld World;
            private readonly double origX, origY, dist;
            private Rectangle2D bounds;

            private double rot;

            public BossNeck(GameWorld world, double origX, double origY, double dist, double startingRot)
            {
                this.World = world;

                this.origX = origX;
                this.origY = origY;
                this.dist = dist;

                Rot = startingRot;
            }

            public void die()
            {
                for (int a = 0; a < 10; a++)
                {
                    double dir = (a / 10.0) * 2 * Math.PI;
                    World.add(new Explosion(World, getBounds2D().X + getBounds2D().Width / 2 + 5 * Math.Cos(dir), getBounds2D().Y + getBounds2D().Height / 2 + 5 * Math.Sin(dir), dir, 1500));
                    if(a%2 == 0)
                        World.add(new DamageEffect(World, getBounds2D().X + getBounds2D().Width / 2 + 5 * Math.Cos(dir), getBounds2D().Y + getBounds2D().Height / 2 + 5 * Math.Sin(dir), 1));
                }
            }

            public double Rot
            {
                get { return rot; }
                set
                {
                    rot = value;

                    if (bounds == null)
                        bounds = new Rectangle2D(0,0,24,24);

                    bounds.setLocation(origX + dist * Math.Cos(rot), origY - dist * Math.Sin(rot));
                }
            }

            public Rectangle2D getBounds2D()
            {
                return bounds;
            }

            public Rectangle getBounds()
            {
                return new Rectangle((int)Math.Round(bounds.X - World.Camera.x), (int)Math.Round(bounds.Y - World.Camera.y), (int)Math.Round(bounds.Width), (int)Math.Round(bounds.Height));
            }

            public bool intersect(Entity e)
            {
                if (!getBounds2D().intersects(e.getBounds2D()))
                    return false;

                if (e is Player)
                {
                    ((Player)e).die();
                    return false;
                }
                else if (e is Enemy)
                {
                    World.remove(e);
                    return false;
                }
                else if (e is Explosion)
                {
                    die();
                    return true;
                }
                else if (e is Bullet)
                {
                    World.remove(e);
                    return false;
                }

                return false;
            }

            public void update()
            {
                Rot += delta;

                if (rot > Math.PI || rot < Math.PI / 2)
                    delta = -delta;
            }

            public void draw(SpriteBatch spriteBatch)
            {
                Assets.getAssets().gremlins.draw(spriteBatch, getBounds(), 4, 1, 30, 30, 12, 12, false, Color.White);
            }
        }
    }
}
