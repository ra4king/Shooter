using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class TitleScreen : Screen
    {
        private int yOffset = 1440-480;

        public TitleScreen(Shooter shooter)
            : base(shooter)
        {
        }

        public override void update(GameTime gameTime)
        {
            if (yOffset == 0)
            {
                if (Input.isShooting())
                {
                    Game.manager.reset();
                    Stats.startTime = DateTime.Now.Ticks / 10000;
                    Assets.getAssets().startgame.Play();
                }
            }
            else
                yOffset -= 5;
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            spriteBatch.Draw(Assets.getAssets().titlescreen, new Rectangle(0, -yOffset, 640, 1440), Color.White);

            if (yOffset == 0 && DateTime.Now.Ticks / 10000000 % 2 == 0)
                Assets.getAssets().drawString(spriteBatch, "PRESS X TO START", Width / 2 - (16 * 6), Height / 2 + 70, 12, 12, Color.White);

            Assets.getAssets().drawString(spriteBatch, "Made by Roi Atalla", 5, Height - 20, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, "Original game by Markus \"Notch\" Persson", 5, Height - 8, 6, 6, Color.White);

            spriteBatch.End();
        }
    }
}
