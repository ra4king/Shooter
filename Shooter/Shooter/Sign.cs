using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    class Sign : Block
    {
        private bool hasSeen;
        private uint id;

        public Sign(GameWorld world, int x, int y, uint id)
            : base(world, x, y, id == 6 ? 5 : id == 15 ? 6 : 4, 0)
        {
            this.id = id;
        }

        public override bool intersect(Entity e)
        {
            if (!base.intersect(e))
                return false;

            if (e is Player)
            {
                if ((!hasSeen && (id == 1 || id == 6 || id == 15)) || Input.isPointingUp())
                {
                    World.Game.setScreen(new SignReadScreen(World, id));
                    hasSeen = true;

                    if (id == 6 || id == 15)
                        World.remove(this);
                }

                return false;
            }
            else if (e is Enemy)
                return false;

            return true;
        }

        public override void update(GameTime gameTime)
        {
            base.update(gameTime);

            if ((World.Game.manager.Player.Gun.Level > 0 && id == 6) || (World.Game.manager.Player.Gun.Level > 1 && id == 15))
                World.remove(this);
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            if ((World.Game.manager.Player.Gun.Level > 0 && id == 6) || (World.Game.manager.Player.Gun.Level > 1 && id == 15))
                return;
            else
                base.draw(spriteBatch);
        }
    }
}
