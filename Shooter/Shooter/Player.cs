using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    public class Player : Entity
    {
        private Gun gun;

        private int respawnX, respawnY, respawnHatLevel;
        private bool respawnDir = true;

        private int currentFrame = 3;
        private int hatLevel = 1;
        private long elapsedTime, healthElapsedTime;
        private bool isFacingRight = true, isDead, showRespawnMessage;
        private int health = 40, maxHealth = 40;

        private static int gravity = 1000;
        private const float vX = 210;
        private float vY, totalVY;
        private bool isJumping;
        private static float maxVY = 500;

        public Player(GameWorld world, int x, int y) : base(world, x*20, y*20, 20, 40)
        {
            gun = new Gun(this);
            respawnX = x;
            respawnY = y;
        }

        public Gun Gun
        {
            get { return gun; }
        }

        public bool IsFacingRight
        {
            get { return isFacingRight; }
        }

        public void hurt(int damage)
        {
            health -= damage;

            if (hatLevel > 0)
            {
                Rectangle2D hat = new Rectangle2D(X, Y - 30, 20, 10);

                foreach (Entity e in World.getEntities())
                {
                    if (e.getBounds2D().intersects(hat))
                    {
                        while (hatLevel > 0)
                        {
                            hatLevel--;
                            World.add(new Hat(World, X, Y - 5, false));
                        }

                        return;
                    }
                }

                while (hatLevel > 0)
                {
                    hatLevel--;
                    World.add(new Hat(World, X, Y - 30 - 5 * hatLevel, false));
                }
            }
        }

        public float VY
        {
            get { return vY; }
            set { vY = value; }
        }

        public int Health
        {
            get { return health; }
            set { health = value; }
        }

        public bool IsDead
        {
            get { return isDead; }
        }

        public bool ShowRespawnMessage
        {
            get { return showRespawnMessage; }
            set { showRespawnMessage = value; }
        }

        public int HatLevel
        {
            get { return hatLevel; }
            set { hatLevel = value; }
        }

        public void setRespawn(int x, int y, bool dir, int hatLevel)
        {
            respawnX = x;
            respawnY = y;
            respawnDir = dir;
            respawnHatLevel = hatLevel == 0 ? 1 : hatLevel;
        }

        public void die()
        {
            for(  int a = 0; a < 10; a++)
                World.add(new DamageEffect(World, X + Width/2, Y + Height/2,2));

            elapsedTime = 0;

            if(!isDead)
                Stats.deaths++;

            isDead = true;

            Assets.getAssets().death.Play();
        }

        public override bool intersect(Entity e)
        {
            if (!base.intersect(e) || isDead)
                return false;

            if (e is Bullet)
            {
                hurt(3);
                World.remove(e);
                World.add(new DamageEffect(World, e.X, e.Y,1));

                Assets.getAssets().oof.Play();
            }
            else if (e is Explosion)
            {
                die();
            }

            return true;
        }

        public void respawn()
        {
            World.Game.manager.respawn();

            health = maxHealth;
            X = respawnX*20;
            Y = respawnY*20;
            isDead = showRespawnMessage = false;
            hatLevel = respawnHatLevel;
            isFacingRight = true;
            elapsedTime = 0;

            gun.ShotLastFrame = true;
        }

        public override void update(GameTime gameTime)
        {
            float delta = gameTime.ElapsedGameTime.Milliseconds / 1000f;

            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            healthElapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (showRespawnMessage)
            {
                if (Input.isShooting())
                {
                    World.Running = false;
                    respawn();
                }

                return;
            }
            
            if (isDead)
            {
                if (elapsedTime > 1000)
                {
                    showRespawnMessage = true;
                    return;
                }
                else
                    return;
            }

            if (health <= 0)
            {
                die();
                return;
            }

            double oldX = X, oldY = Y;

            if (Input.isMovingRight() > 0)
            {
                X += Input.isMovingRight() * (vX * delta);
                isFacingRight = true;
            }
            
            if (Input.isMovingLeft() > 0)
            {
                X -= Input.isMovingLeft() * (vX * delta);
                isFacingRight = false;
            }
            
            if (Input.isJumping())
            {
                if (!isJumping && totalVY < maxVY)
                {
                    if(vY == 0)
                        Assets.getAssets().jump.Play();

                    float v;
                    if (X != oldX)
                        v = 100 + (float)Math.Abs(oldX - X) * 5;
                    else
                        v = 100;

                    vY -= v;
                    totalVY += v;
                }
            }
            else if (!isJumping)
                isJumping = true;

            if (healthElapsedTime >= 500)
            {
                if (health < maxHealth)
                    health++;

                healthElapsedTime -= 500;
            }

            if (elapsedTime >= 80)
            {
                if (vY != 0)
                    currentFrame = 8;
                else if (oldX == X)
                    currentFrame = 3;
                else
                {
                    currentFrame++;
                    if (currentFrame >= 7)
                        currentFrame = 3;
                }

                elapsedTime -= 80;
            }

            if (gun != null)
                gun.update(gameTime.ElapsedGameTime.Milliseconds);

            vY += gravity * delta;

            if (vY > 700)
                vY = 700;

            Y += vY * delta;

            if (X < 0)
            {
                World.Game.manager.transition(Transition.LEFT);
                return;
            }
            else if (X + Width > World.Width)
            {
                World.Game.manager.transition(Transition.RIGHT);
                return;
            }
            else if (Y < 0)
            {
                World.Game.manager.transition(Transition.UP);
                return;
            }
            else if (Y + Height > World.Height)
            {
                World.Game.manager.transition(Transition.DOWN);
                return;
            }

            foreach (Entity e in World.getEntities())
            {
                if (e.Removed || !e.intersect(this))
                    continue;

                if (e is Hat)
                {
                    if (((Hat)e).IsExtra)
                        World.Game.manager.hatTaken();

                    hatLevel++;
                    World.remove(e);

                    respawnHatLevel = hatLevel;

                    Assets.getAssets().gethat.Play();
                }
                else if (e is Block)
                {
                    double newY = Y;
                    Y = oldY;
                    if (e.intersect(this))
                    {
                        if (X < e.X)
                            X = e.X - Width;
                        else if(X > e.X)
                            X = e.X + e.Width;
                    }
                    Y = newY;

                    double newX = X;
                    if (e.intersect(this))
                    {
                        X = oldX;
                        if (e.intersect(this))
                        {
                            if (Y < e.Y)
                            {
                                Y = e.Y - Height;
                                isJumping = true;
                            }
                            else
                                Y = e.Y + e.Height;

                            if (vY < 0)
                                vY = -0.01f;
                            else
                            {
                                totalVY = vY = 0;
                                isJumping = false;
                            }
                        }
                        X = newX;
                    }
                }
            }
        }
        
        public override void draw(SpriteBatch spriteBatch)
        {
            if (isDead)
                return;

            X -= 6;
            Width = 32;
            Assets.getAssets().player.draw(spriteBatch, getBounds(), currentFrame, hatLevel > 0 ? 0 : 1, 16, 32, 16, 20, !isFacingRight, Color.White);
            Width = 20;
            X += 6;

            for (int a = 1; a < hatLevel; a++)
                Assets.getAssets().player.draw(spriteBatch, new Rectangle(getIntX() - (int)Math.Round(World.Camera.x), (getIntY() - a*2 + (currentFrame == 6 ? 2 : 0)) - (int)Math.Round(World.Camera.y), 20, 10), (isFacingRight ? 0 : 2) * 16 + 3, 33, 10, 5, 0, 0, 0, false, Color.White);

            if (gun != null)
                gun.draw(spriteBatch);
        }
    }
}
