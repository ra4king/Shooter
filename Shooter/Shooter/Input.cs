using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Shooter
{
    class Input
    {
        public static bool isShooting()
        {
            return Keyboard.GetState().IsKeyDown(Keys.X) || (GamePad.GetState(PlayerIndex.One).IsConnected && (GamePad.GetState(PlayerIndex.One).Triggers.Right >= 0.1 || GamePad.GetState(PlayerIndex.One).Buttons.X == ButtonState.Pressed));
        }

        public static bool isJumping()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Z) || (GamePad.GetState(PlayerIndex.One).IsConnected && GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed);
        }

        public static float isMovingRight()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Right) ? 1 : (GamePad.GetState(PlayerIndex.One).IsConnected ? ((GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed ? 1 : GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)) : 0);
        }

        public static float isMovingLeft()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Left) ? 1 : (GamePad.GetState(PlayerIndex.One).IsConnected ? ((GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed ? 1 : -GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.X)) : 0);
        }

        public static bool isPointingUp()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Up) || (GamePad.GetState(PlayerIndex.One).IsConnected && (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y > 0.1));
        }

        public static bool isPointingDown()
        {
            return Keyboard.GetState().IsKeyDown(Keys.Down) || (GamePad.GetState(PlayerIndex.One).IsConnected && (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).ThumbSticks.Right.Y < -0.1));
        }
    }
}
