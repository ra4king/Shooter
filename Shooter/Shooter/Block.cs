using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    class Block : Entity
    {
        private int imageX, imageY;

        public Block(GameWorld world, int x, int y) : this(world,x,y,0,0)
        {
        }

        public Block(GameWorld world, int x, int y, int imageX, int imageY)
            : base(world, x * 20, y * 20, 20, 20)
        {
            this.imageX = imageX;
            this.imageY = imageY;
        }

        public int ImageX
        {
            get { return imageX; }
            set { imageX = value; }
        }

        public int ImageY
        {
            get { return imageY; }
            set { imageY = value; }
        }

        public override bool intersect(Entity e)
        {
            if (!base.intersect(e))
                return false;

            if(e is Bullet)
                intersectBullet((Bullet)e);

            if (e is Player)
                intersectPlayer((Player)e);

            if (e is Enemy)
                intersectEnemy((Enemy)e);

            return true;
        }

        public virtual void intersectBullet(Bullet b)
        {
            World.remove(b);
        }

        public virtual void intersectPlayer(Player p)
        {
        }

        public virtual void intersectEnemy(Enemy e)
        {
        }

        public override void update(GameTime gameTime)
        {}

        public override void draw(SpriteBatch spriteBatch)
        {
            Assets.getAssets().walls.draw(spriteBatch, getBounds(), imageX, imageY, 10, 10, 10, 10, false, Color.White);
        }
    }
}
