using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class WinScreen : Screen
    {
        private int elapsedTime;

        public WinScreen(Shooter shooter)
            : base(shooter)
        {
            Stats.endTime = DateTime.Now.Ticks / 10000;
        }

        public override void update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (elapsedTime > 1000)
            {
                if (Input.isShooting())
                    Game.manager.reset();
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            spriteBatch.Draw(Assets.getAssets().winscreen, new Rectangle(0, -100, Width, Height), Color.White);

            int yOffset = 20;

            Assets.getAssets().drawString(spriteBatch, "TIME:", Width / 2 - (5 * 12) - 10, Height / 2 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, (Stats.endTime - Stats.startTime) / 60000 + ":" + ((Stats.endTime - Stats.startTime) / 1000) % 60, Width / 2 + 10, Height / 2 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, "+" + Stats.timePoints(), Width / 2 + 10 + 7 * 12, Height / 2 + yOffset, 12, 12, Color.White);

            Assets.getAssets().drawString(spriteBatch, "DEATHS:", Width / 2 - (7 * 12) - 10, Height / 2 + 15 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, Stats.deaths + "", Width / 2 + 10, Height / 2 + 15 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, "+" + Stats.deathPoints(), Width / 2 + 10 + 7 * 12, Height / 2 + 15 + yOffset, 12, 12, Color.White);

            Assets.getAssets().drawString(spriteBatch, "FEDORAS:", Width / 2 - (8 * 12) - 10, Height / 2 + 30 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, Game.manager.Player.HatLevel + "", Width / 2 + 10, Height / 2 + 30 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, "+" + Stats.fedoraPoints(Game.manager.Player.HatLevel), Width / 2 + 10 + 7 * 12, Height / 2 + 30 + yOffset, 12, 12, Color.White);

            Assets.getAssets().drawString(spriteBatch, "SHOTS FIRED:", Width / 2 - (12 * 12) - 10, Height / 2 + 45 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, Stats.shotsFired + "", Width / 2 + 10, Height / 2 + 45 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, "+" + Stats.shotsFiredPoints(), Width / 2 + 10 + 7 * 12, Height / 2 + 45 + yOffset, 12, 12, Color.White);

            Assets.getAssets().drawString(spriteBatch, "FINAL SCORE:", Width / 2 - (12 * 12) - 10, Height / 2 + 75 + yOffset, 12, 12, Color.White);
            Assets.getAssets().drawString(spriteBatch, (Stats.timePoints() + Stats.deathPoints() + Stats.fedoraPoints(Game.manager.Player.HatLevel) + Stats.shotsFiredPoints()) + "", Width / 2 + 10, Height / 2 + 75 + yOffset, 12, 12, Color.White);

            if(elapsedTime > 1000 && DateTime.Now.Ticks/10000000%2 == 0)
                Assets.getAssets().drawString(spriteBatch, "PRESS X TO RESET THE GAME", Width / 2 - (25 * 6), Height / 2 + 175, 12, 12, Color.White);

            spriteBatch.End();
        }
    }
}
