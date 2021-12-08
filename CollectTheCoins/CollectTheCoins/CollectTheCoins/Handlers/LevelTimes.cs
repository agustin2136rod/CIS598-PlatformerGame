/* LevelTimes.cs
 * Written By: Agustin Rodriguez 
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.Handlers
{
    /// <summary>
    /// this class represents the time allotted to complete each level
    /// </summary>
    public class LevelTimes
    {
        /// <summary>
        /// array that holds the times for each level
        /// </summary>
        private int[] timesForLevels = new int[] { 45, 40, 50, 47, 55, 43, 37, 30, 41, 35, 60 };

        /// <summary>
        /// method to return the times for each level
        /// </summary>
        public int[] TimesForLevels { get { return timesForLevels; } }
    }
}
