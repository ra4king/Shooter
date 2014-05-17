using System;
using Microsoft.Xna.Framework;

namespace Shooter
{
    class ConveyorBelt : Block
    {
        private long elapsedTime;
        
        public ConveyorBelt(GameWorld world, int x, int y, bool facingLeft)
            : base(world, x, y, 0, facingLeft ? 2 : 3)
        {
        }

        public override bool intersect(Entity e)
        {
            if (!base.intersect(e))
                return false;

            if (e is Enemy)
            {
                ((Enemy)e).VX = ImageX == 2 ? -80 : 80;
            }
            else if (e is Gremlin)
            {
                double speed = 2;
                if (ImageY == 2)
                    e.X += speed;
                else
                    e.X -= speed;
            }

            return true;
        }

        public override void intersectPlayer(Player p)
        {
            base.intersectPlayer(p);

            if (ImageY == 2)
                p.X += 1.5;
            else
                p.X -= 2;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > 100)
            {
                if (ImageY == 2)
                    ImageX++;
                else
                    ImageX--;

                if (ImageX > 3)
                    ImageX = 0;
                if (ImageX < 0)
                    ImageX = 3;

                elapsedTime -= 20;
            }
        }
    }
}
