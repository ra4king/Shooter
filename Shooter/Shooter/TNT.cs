using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class TNT : Block
    {
        private bool blownUp;

        public TNT(GameWorld world, int x, int y) : base(world,x,y,1,0)
        {
        }

        public override bool intersect(Entity e)
        {
            if (!base.intersect(e) || blownUp)
                return false;

            if (e is Explosion)
            {
                blowUp();
                World.remove(this);
            }

            return true;
        }

        public void blowUp()
        {
            World.remove(this);

            Random random = new Random();
            for (int i = 0; i < 16; i++)
            {
                double dir = i * Math.PI * 2 / 8.0;
                double dist = (i / 8) + 1;
                World.add(new Explosion(World, X + Math.Cos(dist) * 10, Y + Math.Sin(dist) * 10, dir, random.Next(200) + 200));
            }

            blownUp = true;

            Assets.getAssets().boom.Play();
        }

        public override void intersectBullet(Bullet b)
        {
            base.intersectBullet(b);

            blowUp();
        }
    }
}
