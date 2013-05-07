using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ParticleGame
{
    class InputListener
    {
        private KeyboardState lastKeyState;
        private MouseState lastMouseState;
        private KeyboardState keyState;
        private MouseState mouseState;
        private Dictionary<string, MouseState> capturedMStates;
        private Dictionary<string, KeyboardState> capturedKStates;

        public InputListener()
        {
            capturedMStates = new Dictionary<string, MouseState>();
            capturedKStates = new Dictionary<string, KeyboardState>();
            lastKeyState = keyState = Keyboard.GetState();
            lastMouseState = mouseState = Mouse.GetState();
        }
        public void Update()
        {
            lastKeyState = keyState;
            lastMouseState = mouseState;
            keyState = Keyboard.GetState();
            mouseState = Mouse.GetState();
        }
        public bool CheckKeypress(Keys key)
        {
            return (keyState.IsKeyDown(key) && lastKeyState.IsKeyUp(key));
        }
        public bool CheckKeyRelease(Keys key)
        {
            return (keyState.IsKeyUp(key) && lastKeyState.IsKeyDown(key));
        }

        /// <summary>
        /// Checks whether the specified mouse button has been pressed between the latest and previous update.
        /// </summary>
        /// <param name="buttonName">The name of the button to check against. Valid names are 'left', 'right' and 'middle'.</param>
        /// <returns>True if the specified mouse button has been pressed, false otherwise, or if the string passed isn't a valid mouse button indicator.</returns>
        public bool CheckMousePress(string buttonName)
        {
            switch (buttonName)
            {
                case "left":
                    return (mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released);
                case "right":
                    return (mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released);
                case "middle":
                    return (mouseState.MiddleButton == ButtonState.Pressed && lastMouseState.MiddleButton == ButtonState.Released);
                default:
                    return false;
            }
        }

        /// <summary>
        /// Checks whether the specified mouse button has been released between the latest and previous update.
        /// </summary>
        /// <param name="buttonName">The name of the button to check against. Valid names are 'left', 'right' and 'middle'.</param>
        /// <returns>True if the specified mouse button has been released, false otherwise, or if the string passed isn't a valid mouse button indicator.</returns>
        public bool CheckMouseRelease(string buttonName)
        {
            switch (buttonName)
            {
                case "left":
                    return (mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed);
                case "right":
                    return (mouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed);
                case "middle":
                    return (mouseState.MiddleButton == ButtonState.Released && lastMouseState.MiddleButton == ButtonState.Pressed);
                default:
                    return false;
            }
        }
        public void CaptureMouseState(string name)
        {
            capturedMStates.Add(name, mouseState);
        }
        public MouseState GetMouseState(string name)
        {
            return capturedMStates[name];
        }
        public void ReleaseMouseState(string name)
        {
            capturedMStates.Remove(name);
        }
    }
}
