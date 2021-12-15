/* MediaHandler.cs
 * Written By: Agustin Rodriguez 
 */
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.Handlers
{
    /// <summary>
    /// this class handles the media playing throughout the game
    /// </summary>
    public class MediaHandler
    {
        /// <summary>
        /// Method to play the background song for the game
        /// </summary>
        /// <param name="song"></param>
        public void Play(Song song)
        {
            MediaPlayer.Play(song);
        } 

        /// <summary>
        /// Method to stop the music from playing 
        /// </summary>
        public void Stop()
        {
            MediaPlayer.Stop();
        }

        /// <summary>
        /// Method to set the volume 
        /// </summary>
        /// <param name="volume"></param>
        public void setVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        /// <summary>
        /// Method to set the Media player to repeat the song
        /// </summary>
        public void SetRepeating()
        {
            MediaPlayer.IsRepeating = true;
        }
    }
}
