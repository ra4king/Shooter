using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    class TransparentBlock : Block
    {
        public TransparentBlock(GameWorld world, int x, int y)
            : base(world, x, y, 1, 1)
        {
        }

        public override void intersectBullet(Bullet b)
        {
        }
    }
}
