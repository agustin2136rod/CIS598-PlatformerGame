/* PlayerEventIndexEventArgs.cs
 * Received From: Nathan Bean tutorial
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollectTheCoins.Screens
{
    // Custom event argument which includes the index of the player who
    // triggered the event. This is used by the MenuEntry.Selected event.
    public class PlayerEventIndexEventArgs : EventArgs
    {
        public PlayerIndex PlayerIndex { get; }

        public PlayerEventIndexEventArgs(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
