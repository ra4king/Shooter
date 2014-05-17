using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    public class GameWorld : Screen
    {
        private Camera camera;
        private List<Entity> entities;
        private List<Action> temp;
        private bool looping, running = true;

        public GameWorld(Shooter game) : base(game)
        {
            entities = new List<Entity>();
            temp = new List<Action>();
            camera = new Camera(0,0);
        }

        public Camera Camera
        {
            get { return camera; }
        }

        public Entity add(Entity e)
        {
            if (looping)
                temp.Add(new Action(e, Action.ADD));
            else
            {
                entities.Add(e);
                e.World = this;
            }

            e.Removed = false;

            return e;
        }

        public Entity get(int idx)
        {
            foreach (Entity e in entities)
            {
                if (idx == 0)
                    return e;
                idx--;
            }

            return null;
        }

        public Entity getAt(double x, double y)
        {
            foreach (Entity e in entities)
            {
                if (e.X == x && e.Y == y)
                    return e;
            }

            return null;
        }

        public Entity getContains(double x, double y)
        {
            foreach (Entity e in entities)
            {
                if (x >= e.X && x <= e.X + e.Width && y >= e.Y && y <= e.Y + e.Height)
                    return e;
            }

            return null;
        }

        public Entity getAt(Vector2 p)
        {
            return getAt(p.X, p.Y);
        }

        public bool remove(Entity e)
        {
            e.Removed = true;

            if (looping)
            {
                temp.Add(new Action(e, Action.REMOVE));
                return true;
            }
            else
                return entities.Remove(e);
        }

        public int size()
        {
            return entities.Count;
        }

        public Boolean contains(Entity e)
        {
            return entities.Contains(e);
        }

        public List<Entity> getEntities()
        {
            return entities;
        }

        public bool Running
        {
            get { return running; }
            set { running = value; }
        }

        public override void update(GameTime gameTime)
        {
            preLoop();

            foreach (Entity e in entities)
                e.update(gameTime);

            postLoop();
        }

        public override void draw(SpriteBatch spriteBatch) {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            preLoop();
            
            foreach(Entity e in entities)
                e.draw(spriteBatch);

            postLoop();

            spriteBatch.End();
        }

        private void preLoop()
        {
            looping = true;
        }

        private void postLoop()
        {
            looping = false;

            foreach (Action a in temp)
            {
                bool b = a.e.Removed;

                switch (a.action)
                {
                    case Action.ADD:
                        add(a.e);
                        break;
                    case Action.REMOVE:
                        remove(a.e);
                        break;
                }

                a.e.Removed = b;
            }

            temp.Clear();
        }

        private class Action
        {
            public const int ADD = 0, REMOVE = 1;

            public Entity e;
            public int action;

            public Action(Entity e, int action)
            {
                this.e = e;
                this.action = action;
            }
        }
    }
}
