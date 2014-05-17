using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Transition : Screen
    {
        public const int UP = -2, DOWN = 2, RIGHT = 1, LEFT = -1;

        private GameWorld oldWorld, newWorld;
        private Player player;
        private int vx, vy;

        public Transition(Shooter shooter, GameWorld oldWorld, GameWorld newWorld, int direction, Player player)
            : base(shooter)
        {
            this.oldWorld = oldWorld;
            this.newWorld = newWorld;
            this.player = player;

            switch (direction)
            {
                case RIGHT: vx = 1; break;
                case LEFT: vx = -1; break;
                case DOWN: vy = 1; break;
                case UP: vy = -1; break;
            }

            newWorld.Camera.x = 32 * 20 * -vx;
            newWorld.Camera.y = 24 * 20 * -vy;
        }
        
        public override void update(GameTime gameTime)
        {
            int speed = 20;

            oldWorld.Camera.x += speed * vx;
            oldWorld.Camera.y += speed * vy;

            newWorld.Camera.x += speed * vx;
            newWorld.Camera.y += speed * vy;

            if (Math.Abs(oldWorld.Camera.x) >= 32 * 20 || Math.Abs(oldWorld.Camera.y) >= 24 * 20)
            {
                oldWorld.remove(player);
                Game.setScreen(newWorld);
                newWorld.add(player);

                newWorld.Camera.x = 0;
                newWorld.Camera.y = 0;

                if (vx == 1)
                    player.X -= 32 * 20 - player.Width;
                if (vx == -1)
                    player.X += 32 * 20 - player.Width;
                if (vy == 1)
                    player.Y -= 24 * 20 - player.Height;
                if (vy == -1)
                    player.Y += 24 * 20 - player.Height;

                player.setRespawn(player.getIntX()/20, player.getIntY()/20, player.IsFacingRight, player.HatLevel);
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            oldWorld.draw(spriteBatch);
            newWorld.draw(spriteBatch);
        }
    }
}
