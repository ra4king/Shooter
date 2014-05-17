using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Shooter
{
    public class LevelManager
    {
        private Shooter shooter;
        private GameWorld world;
        private Player player;
        private uint[] map;
        private int imageWidth, imageHeight;
        private int currentLevelX, currentLevelY;

        //the hats are at hat3x1, hat3x2, hat2x5, hat1x6, hat5x7, hat9x0
        private bool[,] hats = new bool[10, 8];

        public LevelManager(Shooter shooter, ContentManager content)
        {
            this.shooter = shooter;

            Texture2D image = content.Load<Texture2D>("levels");

            imageWidth = image.Width;
            imageHeight = image.Height;

            map = new uint[imageWidth * imageHeight];

            image.GetData<uint>(map);

            for (int a = 0; a < map.Length; a++)
                map[a] = ((map[a] & 0x000000FF) << 24) | ((map[a] & 0x0000FF00) << 8) | ((map[a] & 0x0000FF0000) >> 8) | ((map[a] & 0xFF000000) >> 24);
        }

        public Player Player
        {
            get { return player; }
        }

        public void reset()
        {
            player = null;

            shooter.setScreen(load(0 ,0));
        }

        public void hatTaken()
        {
            hats[currentLevelX, currentLevelY] = true;
            Console.WriteLine("Hat taken at: " + currentLevelX + "," + currentLevelY);
        }

        public GameWorld load(int levelX, int levelY)
        {
            if (levelX == 3 && levelY == 11)
            {
                shooter.setScreen(new WinScreen(shooter));
                return null;
            }

            currentLevelX = levelX;
            currentLevelY = levelY;

            world = new GameWorld(shooter);

            bool addPlayer = false;

            for (int y = 0; y < 24; y++)
            {
                for (int x = 0; x < 32; x++)
                {
                    uint p = map[(levelY * 24 + y - levelY) * imageWidth + levelX * 32 + x - levelX];
                    switch (p)
                    {
                        case 0xFFFFFFFF:
                            world.add(new Block(world, x, y));
                            break;
                        case 0xFF00FFFF:
                            world.add(new MovableBlock(world, x, y));
                            break;
                        case 0xFFFF00FF:
                            world.add(new TNT(world, x, y));
                            break;
                        case 0xFF0000FF:
                            world.add(new Spikes(world, x, y, map[(levelY * 24 + y - levelY + 1) * imageWidth + levelX * 32 + x - levelX] == 0x000000FF));
                            break;
                        case 0xB7B7B7FF:
                            world.add(new TransparentBlock(world, x, y));
                            break;
                        case 0xFF5050FF:
                            world.add(new ConveyorBelt(world, x, y, true));
                            break;
                        case 0xFF5051FF:
                            world.add(new ConveyorBelt(world, x, y, false));
                            break;
                        case 0x383838FF:
                            world.add(new Door(world, x, y));
                            break;
                        case 0xA3FFFFFF:
                            world.add(new ExplodingBlock(world, x, y));
                            break;
                        case 0x83FFFFFF:
                            world.add(new Boss(world, x, y));
                            break;
                        case 0x80FFFFFF:
                            world.add(new Gremlin(world, x, y, 0));
                            break;
                        case 0x81FFFFFF:
                            world.add(new Gremlin(world, x, y, 1));
                            break;
                        case 0x82FFFFFF:
                            world.add(new Jabberwocky(world, x, y));
                            break;
                        case 0xFFADF8FF:
                            if (hats[currentLevelX,currentLevelY])
                                break;
                            world.add(new Hat(world, x * 20, y * 20, true));
                            break;
                        case 0x0000FFFF:
                            if (player != null)
                            {
                                player.setRespawn(x, y-1, player.IsFacingRight, player.HatLevel);
                                continue;
                            }

                            player = new Player(world, x, y-1);
                            addPlayer = true;
                            break;
                        case 0x00FFFFFF:
                            world.add(new Enemy(world, player, x*20 + 10, y*20 + 8, -1, 1));
                            break;
                        default:
                            if ((p & 0x00FFFFFF) == 0x00FF00FF && (p & 0xFF0000FF) > 256)
                            {
                                world.add(new Sign(world, x, y, (p >> 24) & 0xFF));
                            }
                            break;
                    }
                }
            }

            if(addPlayer && player != null)
                world.add(player);

            return world;
        }

        public void respawn()
        {
            world.remove(player);
            shooter.setScreen(load(currentLevelX, currentLevelY));
            world.add(player);
        }

        public void transition(int direction)
        {
            try
            {
                switch (direction)
                {
                    case Transition.RIGHT:
                        shooter.setScreen(new Transition(shooter, world, load(currentLevelX + 1, currentLevelY), direction, player));
                        break;
                    case Transition.LEFT:
                        shooter.setScreen(new Transition(shooter, world, load(currentLevelX - 1, currentLevelY), direction, player));
                        break;
                    case Transition.DOWN:
                        shooter.setScreen(new Transition(shooter, world, load(currentLevelX, currentLevelY + 1), direction, player));
                        break;
                    case Transition.UP:
                        shooter.setScreen(new Transition(shooter, world, load(currentLevelX, currentLevelY - 1), direction, player));
                        break;
                    default:
                        return;
                }
            }
            catch(Exception exc)
            {
                Console.WriteLine(exc);
                return;
            }
        }
    }
}
