/* TexturePlayer.cs
 * Author: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CollectTheCoins
{
    /// <summary>
    /// Classes to draw the animations while playing the game
    /// </summary>
    public struct TexturePlayer
    {
        //private variables
        TextureHandler textureHandler;
        private int index;
        private float time;

        /// <summary>
        /// getter for the TextureHandler
        /// </summary>
        public TextureHandler TextureHandler { get { return textureHandler; } }

        /// <summary>
        /// getter for the index
        /// </summary>
        public int Index { get { return index; } }

        /// <summary>
        /// Getter for the Origin
        /// </summary>
        public Vector2 Origin { get { return new Vector2(textureHandler.Width / 2.0f, textureHandler.Height); } }

        /// <summary>
        /// Method to play an animation
        /// </summary>
        /// <param name="textureHandler">Animation handler</param>
        public void Play(TextureHandler textureHandler)
        {
            if (this.TextureHandler == textureHandler)
            {
                return;
            }
            this.textureHandler = textureHandler;
            index = 0;
            time = 0.0f;
        }

        /// <summary>
        /// Method to draw on the gamescreen
        /// </summary>
        /// <param name="gameTime">elapsed game time</param>
        /// <param name="spriteBatch">the spritebatch</param>
        /// <param name="position">the position to draw at</param>
        /// <param name="spriteEffects">the sprite effects</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (time > textureHandler.CurrentTime)
            {
                time -= textureHandler.CurrentTime;
                if (textureHandler.DoLoop)
                {
                    index = (index + 1) % textureHandler.numberOfFrames;
                } 
                else
                {
                    index = Math.Min(Index + 1, textureHandler.numberOfFrames - 1);
                }
            }
            Rectangle sourceRectangle = new Rectangle(Index * textureHandler.Texture.Height, 0, TextureHandler.Texture.Height, TextureHandler.Texture.Height);

            spriteBatch.Draw(textureHandler.Texture, position, sourceRectangle, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);
        }

    }
}
