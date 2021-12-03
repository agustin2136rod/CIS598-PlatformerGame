/* IScreenFactory.cs
 * Received From: Nathan Bean tutorial
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.StateManagement
{
    public interface IScreenFactory
    {
        GameScreen CreateScreen(Type screenType);
    }
}
