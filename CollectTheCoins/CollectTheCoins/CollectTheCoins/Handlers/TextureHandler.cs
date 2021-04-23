/* TextureHandler.cs
 * Author: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CollectTheCoins
{
    /// <summary>
    /// Class to handle different textures in the game
    /// </summary>
    public class TextureHandler
    {
        /// <summary>
        /// set up variables
        /// </summary>
        Texture2D texture;
        float currentTime;
        bool doLoop;

        /// <summary>
        /// constructor for the class
        /// </summary>
        /// <param name="texture">the texture </param>
        /// <param name="frameTime">the frametime</param>
        /// <param name="doLoop">whether to loop</param>
        public TextureHandler(Texture2D texture, float frameTime, bool doLoop)
        {
            this.texture = texture;
            this.currentTime = frameTime;
            this.doLoop = doLoop;
        }

        /// <summary>
        /// getter for the texture
        /// </summary>
        public Texture2D Texture { get { return texture; } }

        /// <summary>
        /// getter for the current time
        /// </summary>
        public float CurrentTime { get { return currentTime; } }

        /// <summary>
        /// getter for the bool loop
        /// </summary>
        public bool DoLoop { get { return doLoop; } }

        /// <summary>
        /// getter for the number of frames
        /// </summary>
        public int numberOfFrames {  get { return Texture.Width / Height; } }

        /// <summary>
        /// getter for the width of the texture
        /// </summary>
        public int Width {  get { return Texture.Height; } }

        /// <summary>
        /// getter for the height of the texture
        /// </summary>
        public int Height {  get { return Texture.Height; } }
    }
}
