/* InputAction.cs
 * Written By: Microsoft Xna example received from Nathan Bean tutorial
 */ 
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CollectTheCoins.StateManagement
{
    /// <summary>
    /// Defines an action that is designated by some set of buttons and/or keys.
    /// </summary> 
    /// <remarks>
    /// The way actions work is that you define a set of buttons and keys that trigger the action. You can
    /// then evaluate the action against an InputState which will test to see if any of the buttons or keys
    /// are pressed by a player. You can also set a flag that indicates if the action only occurs once when
    /// the buttons/keys are first pressed or whether the action should occur each frame.
    /// 
    /// Using this InputAction class means that you can configure new actions based on keys and buttons
    /// without having to directly modify the InputState type. This means more customization by your games
    /// without having to change the core classes of Game State Management.  It can also be used for custom 
    /// input mappings
    /// </remarks>
    public class InputAction
    {
        private readonly Keys[] _keys;
        private readonly bool _firstPressOnly;

        // These delegate types map to the methods on InputState. We use these to simplify the Occurred method
        // by allowing us to map the appropriate delegates and invoke them, rather than having two separate code paths.
        private delegate bool KeyPress(Keys key, PlayerIndex? controllingPlayer, out PlayerIndex player);

        /// <summary>
        /// Constructs a new InputMapping, binding the suppled triggering input options to the action
        /// </summary>
        /// <param name="triggerButtons">The buttons that trigger this action</param>
        /// <param name="triggerKeys">The keys that trigger this action</param>
        /// <param name="firstPressOnly">If this action only triggers on the initial key/button press</param>
        public InputAction(Keys[] triggerKeys, bool firstPressOnly)
        {
            // Store the buttons and keys. If the arrays are null, we create a 0 length array so we don't
            // have to do null checks in the Occurred method
            _keys = triggerKeys != null ? triggerKeys.Clone() as Keys[] : new Keys[0];
            _firstPressOnly = firstPressOnly;
        }

        /// <summary>
        /// Determines if he action has occured. If playerToTest is null, the player parameter will be the player that performed the action
        /// </summary> 
        /// <param name="stateToTest">The InputState object to test</param>
        /// <param name="playerToTest">If not null, specifies the player (0-3) whose input should be tested</param>
        /// <param name="player">The player (0-3) who triggered the action</param>
        public bool Occurred(InputState stateToTest, PlayerIndex? playerToTest, out PlayerIndex player)
        {
            // Figure out which delegate methods to map from the state which takes care of our "firstPressOnly" logic
            KeyPress keyTest;

            if (_firstPressOnly)
            {
                keyTest = stateToTest.IsNewKeyPress;
            }
            else
            {
                keyTest = stateToTest.IsKeyPressed;
            }

            // Now we simply need to invoke the appropriate methods for each button and key in our collections
            foreach (var key in _keys)
            {
                if (keyTest(key, playerToTest, out player))
                    return true;
            }

            // If we got here, the action is not matched
            player = PlayerIndex.One;
            return false;
        }
    }
}
