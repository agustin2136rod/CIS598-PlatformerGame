/* InputState.cs
 * Adapted from the MonoGame port of the original XNA GameStateExample 
 * https://github.com/tomizechsterson/game-state-management-monogame
 * Received From: Nathan Bean Tutorial 
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CollectTheCoins.StateManagement
{
    /// <summary>
    /// Helper for reading input from keyboard, gamepad, and touch input. This class 
    /// tracks both the current and previous state of the input devices, and implements 
    /// query methods for high level input actions such as "move up through the menu"
    /// or "pause the game".
    /// </summary> 
    public class InputState
    {
        private const int MaxInputs = 1;
        public readonly KeyboardState[] CurrentKeyboardStates;
        private readonly KeyboardState[] _lastKeyboardStates;

        /// <summary>
        /// Constructs a new InputState
        /// </summary>
        public InputState()
        {
            CurrentKeyboardStates = new KeyboardState[MaxInputs];
            _lastKeyboardStates = new KeyboardState[MaxInputs];
        }

        // Reads the latest user input state.
        public void Update()
        {
            for (int i = 0; i < MaxInputs; i++)
            {
                _lastKeyboardStates[i] = CurrentKeyboardStates[i];
                CurrentKeyboardStates[i] = Keyboard.GetState();
            }
        }

        /// <summary>
        /// Helper for checking if a key was pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary>
        public bool IsKeyPressed(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return CurrentKeyboardStates[i].IsKeyDown(key);
            }

            // Accept input from the player.
            return IsKeyPressed(key, PlayerIndex.One, out playerIndex);
        }


        /// <summary>
        /// Helper for checking if a key was newly pressed during this update. The
        /// controllingPlayer parameter specifies which player to read input for.
        /// If this is null, it will accept input from any player. When a keypress
        /// is detected, the output playerIndex reports which player pressed it.
        /// </summary> 
        public bool IsNewKeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex playerIndex)
        {
            if (controllingPlayer.HasValue)
            {
                // Read input from the specified player.
                playerIndex = controllingPlayer.Value;

                int i = (int)playerIndex;

                return (CurrentKeyboardStates[i].IsKeyDown(key) &&
                        _lastKeyboardStates[i].IsKeyUp(key));
            }

            // Accept input from any player.
            return IsNewKeyPress(key, PlayerIndex.One, out playerIndex);
        }
    }
}
