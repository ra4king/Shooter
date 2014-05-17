using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    public abstract class Entity
    {
        private GameWorld world;
        private Dictionary<string, Texture2D> textures;
        private Rectangle bounds;
        private Rectangle2D bounds2D;
        private bool removed;

        public Entity(GameWorld world) : this(world,0,0) {}

        public Entity(GameWorld world, double x, double y) : this(world, x, y, 0, 0) { }

        public Entity(GameWorld world, double x, double y, double width, double height)
        {
            this.world = world;
            textures = new Dictionary<string, Texture2D>();
            bounds2D = new Rectangle2D(x, y, width, height);
        }

        public GameWorld World
        {
            get { return world; }
            set {
                if (value == null)
                    throw new NullReferenceException();
                world = value;
            }
        }

        public virtual bool intersect(Entity e)
        {
            return getBounds2D().intersects(e.getBounds2D());
        }

        public virtual bool Removed
        {
            get { return removed; }
            set { removed = value; }
        }

        public double X
        {
            get { return bounds2D.X; }
            set { bounds2D.X = value; }
        }

        public int getIntX()
        {
            return (int)Math.Round(X);
        }

        public double ScreenX
        {
            get { return X - world.Camera.x; }
        }

        public double Y
        {
            get { return bounds2D.Y; }
            set { bounds2D.Y = value; }
        }

        public int getIntY()
        {
            return (int)Math.Round(Y);
        }

        public double ScreenY
        {
            get { return Y - world.Camera.y; }
        }

        public double Width
        {
            get { return bounds2D.Width; }
            set { bounds2D.Width = value; }
        }

        public int getIntWidth()
        {
            return (int)Math.Round(Width);
        }

        public double Height
        {
            get { return bounds2D.Height ; }
            set { bounds2D.Height = value; }
        }

        public int getIntHeight()
        {
            return (int)Math.Round(Height);
        }

        public void add(Vector2 v)
        {
            X += v.X;
            Y += v.Y;
        }

        public void addTexture(string name)
        {
            textures.Add(name, world.Game.Content.Load<Texture2D>(name));
        }

        public Texture2D getTexture(string name)
        {
            return textures[name];
        }

        public bool containsTexture(string name)
        {
            return textures.ContainsKey(name);
        }

        public Texture2D removeTexture(string name)
        {
            Texture2D t = textures[name];
            textures.Remove(name);
            return t;
        }

        public Rectangle getBounds()
        {
            if (bounds == null)
                bounds = new Rectangle();
            bounds.X = (int)Math.Round(X - world.Camera.x);
            bounds.Y = (int)Math.Round(Y - world.Camera.y);
            bounds.Width = (int)Math.Round(Width);
            bounds.Height = (int)Math.Round(Height);
            return bounds;
        }

        public Rectangle2D getBounds2D()
        {
            return bounds2D;
        }

        public void setLocation(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void setSize(float width, float height)
        {
            Width = width;
            Height = height;
        }

        public abstract void update(GameTime gameTime);

        public abstract void draw(SpriteBatch spriteBatch);
    }
}
