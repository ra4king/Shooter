using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    class ExplodingBlock : TNT
    {
        public ExplodingBlock(GameWorld world, int x, int y)
            : base(world, x, y)
        {
            ImageX = 7;
        }

        public override void intersectBullet(Bullet b)
        {
            World.remove(b);
        }
    }
}
