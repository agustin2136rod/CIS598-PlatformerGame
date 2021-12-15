/* IScreenFactory.cs
 * Received From: Nathan Bean tutorial
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.StateManagement
{
    /// <summary>
    /// Interface to handle screens
    /// </summary>
    public interface IScreenFactory
    {
        /// <summary>
        /// Create screen and pass in type
        /// </summary>
        /// <param name="screenType">type of screen to create</param>
        /// <returns></returns>
        GameScreen CreateScreen(Type screenType);
    }
}
