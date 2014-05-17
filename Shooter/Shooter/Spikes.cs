using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Spikes : Block
    {
        public Spikes(GameWorld world, int x, int y, bool flip) : base(world,x,y,3,flip ? 1 : 0)
        {
        }

        public override void intersectPlayer(Player p)
        {
            p.die();
        }

        public override void intersectEnemy(Enemy e)
        {
            World.remove(e);
            World.add(new DamageEffect(World, e.X, e.Y, 2));
        }
    }
}
