using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    public class Gun
    {
        private Player player;
        private int direction;
        private bool shotLastFrame;
        private int level;

        public Gun(Player player)
        {
            this.player = player;
        }

        public bool ShotLastFrame
        {
            get { return shotLastFrame; }
            set { shotLastFrame = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }

        public void update(int deltaTime)
        {
            if (level == 0)
                return;

            if (Input.isPointingUp())
                direction = 0;
            else if (Input.isPointingDown())
                direction = 2;
            else
                direction = 1;

            shoot();
        }

        private int count;

        public void shoot()
        {
            if (player.IsDead)
                return;

            if (Level == 1)
            {
                if (!shotLastFrame && Input.isShooting())
                {
                    shotLastFrame = true;
                    player.World.add(new Enemy(player.World, player, player.X + (player.IsFacingRight ? player.Width - 6 : -6), player.Y + player.Height / 2 - 10, getShootingAngle(), 1.5));
                    Stats.shotsFired++;
                    Assets.getAssets().launch.Play();
                }
                else if (!Input.isShooting())
                {
                    shotLastFrame = false;
                }
            }
            else
            {
                count++;

                if (count % 2 != 0)
                    return;

                if (!shotLastFrame && Input.isShooting())
                {
                    player.World.add(new Enemy(player.World, player, player.X + (player.IsFacingRight ? player.Width - 6 : -6), player.Y + player.Height / 2 - 10, getShootingAngle(), 1.5));

                    Stats.shotsFired++;

                    Assets.getAssets().launch.Play();
                    
                    if (direction == 2)
                    {
                        player.Y -= 5;
                        player.VY = 0;
                    }
                    else
                        player.X += player.IsFacingRight ? -1 : 1;
                }
                else if (!Input.isShooting())
                {
                    shotLastFrame = false;
                }
            }
        }

        private double getShootingAngle()
        {
            switch (direction)
            {
                case 0:
                    return player.IsFacingRight ? Math.PI / 4 : 3 * Math.PI / 4;
                case 1:
                    return player.IsFacingRight ? 0 : Math.PI;
                case 2:
                    return player.IsFacingRight ? 7 * Math.PI / 4 : 5 * Math.PI / 4;
            }

            return 0;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            if (level == 0)
                return;

            player.X -= 6;
            player.Width = 32;
            Assets.getAssets().player.draw(spriteBatch, player.getBounds(), direction, Level == 1 ? 0 : 2, 16, 32, 16, 20, !player.IsFacingRight, Color.White);
            player.Width = 20;
            player.X += 6;
        }
    }
}
