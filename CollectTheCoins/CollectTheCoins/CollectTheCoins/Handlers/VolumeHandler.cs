/* VolumeHandler.cs
 * Written By: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.Handlers
{
    /// <summary>
    /// This class represents the volume of the background music in the game
    /// </summary>
    public class VolumeHandler
    {
        /// <summary>
        /// variable to store the volume of the game
        /// </summary>
        private float volume;

        /// <summary>
        /// constructor that initiates the volume of the game
        /// </summary>
        /// <param name="vol"></param>
        public VolumeHandler(float vol)
        {
            volume = vol;
        }

        /// <summary>
        /// setter for the volume of the game
        /// </summary>
        public float Volume { get { return volume; } }

        /// <summary>
        /// method to increment the volume of the game
        /// </summary>
        public void IncrementVolume() 
        {
            if (volume >= 1.0f)
            {
                volume = 0;
            }
            else
            {
                volume += 0.1f;
            }
        }
    }
}
