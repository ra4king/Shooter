using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Shooter
{
    public class Shooter : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;

        private Screen currentScreen;

        private SpriteBatch spriteBatch;
        public LevelManager manager;
        long lastTime, lastTime2;
        int updates, UPS;
        int frames, FPS;

        private Texture2D background;

        public Shooter()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;

            graphics.SynchronizeWithVerticalRetrace = true;
        }

        protected override void Initialize()
        {
            base.Initialize();

            Assets.init(Content);

            lastTime = lastTime2 = DateTime.Now.Ticks;

            background = Content.Load<Texture2D>("background");

            manager = new LevelManager(this, Content);

            setScreen(new TitleScreen(this));
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public void setScreen(Screen screen)
        {
            currentScreen = screen;
        }

        public Screen getScreen()
        {
            return currentScreen;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);

            if (currentScreen != null)
                currentScreen.update(gameTime);

            updates++;

            if (DateTime.Now.Ticks - lastTime2 >= 1e7)
            {
                UPS = updates;
                updates = 0;
                lastTime2 += (long)1e7;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0,0,640,480), Color.White);
            spriteBatch.End();

            frames++;

            if (DateTime.Now.Ticks - lastTime >= 1e7)
            {
                FPS = frames;
                frames = 0;
                lastTime += (long)1e7;
            }

            if (currentScreen != null)
                currentScreen.draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            
            if (manager.Player != null && manager.Player.ShowRespawnMessage)
                Assets.getAssets().drawString(spriteBatch, "PRESS X TO RESPAWN", getScreen().Width / 2 - 18 * 6, getScreen().Height / 2 - 6, 12, 12, Color.White);

            spriteBatch.End();
        }
    }
}
