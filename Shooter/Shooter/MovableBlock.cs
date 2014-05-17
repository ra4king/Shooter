using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    class MovableBlock : Block
    {
        public MovableBlock(GameWorld world, int x, int y)
            : base(world, x, y, 2, 0)
        {
        }

        public override void intersectBullet(Bullet b)
        {
            base.intersectBullet(b);

            if (Math.Abs(b.vX) > Math.Abs(b.vY))
            {
                double oldX = X;
                X += 20 * (int)(Math.Abs(b.vX) / b.vX);

                foreach (Entity e in World.getEntities())
                {
                    if (e.Removed)
                        continue;

                    if (e != this && !(e is Bullet) && e.getBounds2D().intersects(getBounds2D()))
                    {
                        X = oldX;
                        break;
                    }
                }
            }
            else
            {
                double oldY = Y;

                Y += 20 * (int)(Math.Abs(b.vY) / b.vY);

                foreach (Entity e in World.getEntities())
                {
                    if (e.Removed)
                        continue;

                    if (e != this && !(e is Bullet) && e.getBounds2D().intersects(getBounds2D()))
                    {
                        Y = oldY;
                        break;
                    }
                }
            }
        }
    }
}
