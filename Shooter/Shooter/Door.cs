using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shooter
{
    class Door : Block
    {
        public Door(GameWorld world, int x, int y)
            : base(world, x, y, 4, 1)
        {
        }

        public override bool intersect(Entity e)
        {
            if (!base.intersect(e))
                return false;

            if (e is Player)
                return false;

            return false;
        }
    }
}
