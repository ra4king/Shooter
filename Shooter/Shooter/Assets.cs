using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Shooter
{
    public class Assets
    {
        private static Assets assets;

        private static ContentManager content;

        public SpriteSheet player, gremlins, guys, walls;
        public Texture2D titlescreen, winscreen;
        public SoundEffect boom, death, gethat, hit, jump, launch, oof, pew, splat, startgame;

        private Assets(ContentManager content)
        {
            Assets.content = content;

            player = new SpriteSheet("player");
            gremlins = new SpriteSheet("gremlins");
            guys = new SpriteSheet("guys");
            walls = new SpriteSheet("walls");

            titlescreen = content.Load<Texture2D>("titlescreen");
            winscreen = content.Load<Texture2D>("winscreen2");

            boom = content.Load<SoundEffect>("boom");
            death = content.Load<SoundEffect>("death");
            gethat = content.Load<SoundEffect>("gethat");
            hit = content.Load<SoundEffect>("hit");
            jump = content.Load<SoundEffect>("jump");
            launch = content.Load<SoundEffect>("launch");
            oof = content.Load<SoundEffect>("oof");
            pew = content.Load<SoundEffect>("pew");
            splat = content.Load<SoundEffect>("splat");
            startgame = content.Load<SoundEffect>("startgame");

            assets = this;
        }

        public static Assets init(ContentManager content)
        {
            if (assets != null)
                throw new InvalidOperationException("assets isn't null");

            assets = new Assets(content);
            return assets;
        }

        public static Assets getAssets()
        {
            return assets;
        }

        private String[] chars = {
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
            ".,!?:;\"'+-=/\\< "
        };

        public void drawString(SpriteBatch spriteBatch, string s, int x, int y, int cWidth, int cHeight, Color color)
        {
            if (s == null || s.Equals(""))
                return;

            s = s.ToUpper();

            for (int a = 0; a < s.Length; a++)
            {
                int cy = chars[0].IndexOf(s[a]) == -1 ? 10 : 9;
                if (cy == -1)
                    continue;
                int cx = chars[cy - 9].IndexOf(s[a]);
                if (cx == -1)
                    continue;

                guys.draw(spriteBatch, new Rectangle(x + a*cWidth,y,cWidth,cHeight), cx, cy, 6, 6, 6, 6, false, color);
            }
        }

        public class SpriteSheet
        {
            private Texture2D tex;

            public SpriteSheet(string name)
            {
                tex = content.Load<Texture2D>(name);
            }

            public void draw(SpriteBatch spriteBatch, Rectangle bounds, int frameX, int frameY, int frameWidth, int frameHeight, int swidth, int sheight, bool flip, Color color)
            {
                draw(spriteBatch, bounds, frameX * frameWidth, frameY * frameHeight, swidth, sheight, 0, 0, 0, flip, color);
            }

            public void draw(SpriteBatch spriteBatch, Rectangle bounds, int frameX, int frameY, int frameWidth, int frameHeight, int swidth, int sheight, float rot, int rx, int ry, bool flip, Color color)
            {
                draw(spriteBatch, bounds, frameX * frameWidth, frameY * frameHeight, swidth, sheight, rot, rx, ry, flip, color);
            }

            public void draw(SpriteBatch spriteBatch, Rectangle bounds, int sx, int sy, int swidth, int sheight, float rot, int rx, int ry, bool flip, Color color)
            {
                spriteBatch.Draw(tex, bounds, new Rectangle(sx, sy, swidth, sheight), color, rot, new Vector2(rx, ry), flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0);
            }
        }
    }
}
