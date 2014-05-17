using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    class SignReadScreen : Screen
    {
        private static string[][] signs = new string[][] {
            new string[] {
                "READING",
                "", 
                "PRESS UP TO READ SIGNS"
            },
            new string[] {
                "JUMPING",
                "", 
                "PRESS Z TO JUMP",
                "YOU CAN JUMP HIGHER BY",
                "GETTING A RUNNING START",
                "OR HOLDING DOWN Z",
            },
            new string[] {
                "PROGRESSING",
                "", 
                "LEAVE A ROOM THROUGH ANY",
                "EXIT TO CONTINUE YOUR",
                "ADVENTURE",
            },
            new string[] {
                "DYING",
                "", 
                "IF YOU DIE, YOU RESTART",
                "AT THE BEGINNING OF THE",
                "CURRENT ROOM",
            },
            new string[] {
                "DODGING",
                "", 
                "THE GUNNERS DON'T LIKE YOU",
                "AND SHOOT AT YOU.",
                "IT WOULD BE WISE TO STAY AWAY",
            },    
            new string[] {
                "THE LAUNCHER",
                "", 
                "AS YOU PICK UP THE LAUNCHER,",
                "YOU REALIZE IT'S NOT YOUR",
                "AVERAGE LAUNCHER.",
                "",
                "PRESS UP AND DOWN TO AIM",
                "PRESS X TO FIRE THE LAUNCHER",
            },      
            new string[] {
                "JONESING",
                "", 
                "DON'T FORGET YOUR FEDORA!",
            },
            new string[] {
                "EXPLODING",
                "", 
                "TNT BLOCKS ARE HIGHLY",
                "EXPLOSIVE, AND WILL",
                "REACT POORLY TO BEING",
                "SHOT.",
            },              
            new string[] {
                "PUSHING",
                "", 
                "THE CAMARADERIE BOX IS",
                "SOMETHING SOMETHING",
                "",
                "IT'S FROM PORTAL.",
            },              
            new string[] {
                "BATTLING",
                "", 
                "THE GREMLIN IS LARGE",
                "AND IN YOUR WAY.",
                "OVERHEAT IT TO DESTROY",
                "IT AND CLAIM YOUR PRIZE",
            },      
            new string[] {
                "EVADING",
                "", 
                "THE GUNNERS SHOTS WILL",
                "PASS THROUGH GLASS.",
                "YOU, HOWEVER, WILL NOT",
            },         
            new string[] {
                "SWEATING",
                "", 
                "THESE SLIGHTLY MORE",
                "SOPHISTICATED GREMLINS",
                "HAVE LEARNED A NEW",
                "TRICK",
            },
            new string[] {
                "CONVEYING",
                "", 
                "TIME TO BURN OFF SOME",
                "FAT AND HAVE FUN WHILE",
                "DOING IT!",
            },          
            new string[] {
                "BOSSFIGHTING",
                "", 
                "BEHIND THIS DOOR, MEGAN",
                "AWAITS! WHO IS MEGAN?",
                "ARE YOU MEGAN?",
            },            
            new string[] {
                "THE NEW LAUNCHER",
                "",
                "WELL, THIS IS BAD."
            },               
            new string[] {
                "FEEDING",
                "",
                "THE JABBERWOCKY IS",
                "HUNGRY, AND WILL EAT",
                "WAY MORE THAN IT SHOULD",
                "",
                "PLEASE DO NOT FEED!",
            },               
            new string[] {
                "HOVERING",
                "",
                "THE RECOIL ON THE NEW",
                "LAUNCHER SURE IS",
                "POWERFUL!",
            },
            new string[] {
                "FLYING",
                "",
                "SERIOUSLY, THE RECOIL",
                "IS OUT OF THIS WORLD!",
            },             
            new string[] {
                "WINNING",
                "",
                "YOUR FINAL CHALLENGE",
                "IS RIGHT DOWN THIS",
                "HALLWAY.",
            }, 
            new string[] {
                "FRESHERERST",
                "",
                "BIG ADAM, GIANT SISTER.",
                "IT IS KNOWN BY MANY NAMES",
                "BUT JUDITH 4HRPG BLUEBERRY.",
                "",
                "FISSION MAILED!",
            }
        };

        private GameWorld world;
        private uint id;
        private const int SCREEN_WIDTH = 640, SCREEN_HEIGHT = 480, BLOCK_SIZE = 12;

        public SignReadScreen(GameWorld world, uint id)
            : base(world.Game)
        {
            this.world = world;
            this.id = id-1;
        }

        public override void update(GameTime gameTime)
        {
            if (Input.isShooting())
            {
                Game.setScreen(world);
                Game.manager.Player.Gun.ShotLastFrame = true;

                switch (id)
                {
                    case 5:
                        Game.manager.Player.Gun.Level = 1;
                        break;
                    case 14:
                        Game.manager.Player.Gun.Level = 2;
                        break;
                }
            }
        }

        public override void draw(SpriteBatch spriteBatch)
        {
            world.draw(spriteBatch);

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);

            string[] sign = signs[id];

            int width = maxWidth(sign);
            int height = sign.Length + 3;

            int left = SCREEN_WIDTH / 2 - ((width + 2) * BLOCK_SIZE) / 2;
            int top = SCREEN_HEIGHT / 2 - ((height + 2) * BLOCK_SIZE) / 2;

            Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left, top, BLOCK_SIZE, BLOCK_SIZE), 0, 11, 6, 6, 6, 6, false, Color.White);

            for (int a = 1; a <= width; a++)
                Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left + a * BLOCK_SIZE, top, BLOCK_SIZE, BLOCK_SIZE), 1, 11, 6, 6, 6, 6, false, Color.White);

            Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left + (width + 1) * BLOCK_SIZE, top, BLOCK_SIZE, BLOCK_SIZE), 2, 11, 6, 6, 6, 6, false, Color.White);

            for(int a = 1; a <= sign.Length; a++)
            {
                Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left, top + a * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 0, 12, 6, 6, 6, 6, false, Color.White);

                for (int b = 0; b < width; b++)
                    Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left + (b + 1) * BLOCK_SIZE, top + a * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 1, 12, 6, 6, 6, 6, false, Color.White);

                Assets.getAssets().drawString(spriteBatch, sign[a-1], left + BLOCK_SIZE, top + a * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE, Color.White);

                Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left + (width + 1) * BLOCK_SIZE, top + a * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 2, 12, 6, 6, 6, 6, false, Color.White);
            }

            Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left, top + (height + 1) * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 0, 13, 6, 6, 6, 6, false, Color.White);

            for (int a = 1; a <= width; a++)
                Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left + a * BLOCK_SIZE, top + (height + 1) * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 1, 13, 6, 6, 6, 6, false, Color.White);

            Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left + (width + 1) * BLOCK_SIZE, top + (height + 1) * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 2, 13, 6, 6, 6, 6, false, Color.White);

            for (int a = height - 2; a < height + 1; a++)
            {
                Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left, top + a * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 0, 12, 6, 6, 6, 6, false, Color.White);

                for (int b = 0; b < width; b++)
                    Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left + (b + 1) * BLOCK_SIZE, top + a * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 1, 12, 6, 6, 6, 6, false, Color.White);

                Assets.getAssets().guys.draw(spriteBatch, new Rectangle(left + (width + 1) * BLOCK_SIZE, top + a * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE), 2, 12, 6, 6, 6, 6, false, Color.White);
            }

            string l = "PRESS 'X' ";
            Assets.getAssets().drawString(spriteBatch, l, (left + (width + 1) * BLOCK_SIZE) - (l.Length - 1) * BLOCK_SIZE, top + height * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE, Color.White);

            spriteBatch.End();
        }

        private int maxWidth(string[] message)
        {
            if(message.Length == 0)
                return 0;

            int max = message[0].Length;
            foreach (string m in message)
            {
                if (m.Length > max)
                    max = m.Length;
            }

            return max;
        }
    }
}
