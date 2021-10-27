using CollectTheCoins.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.GameAssets
{
    public class SpikesSprite
    {
        /// <summary>
        /// position of the spikes
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// Variable to get represent the spikes sprite
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// used for debugging purposes
        /// </summary>
        private Texture2D pixel;

        /// <summary>
        /// bounding box for the spikes
        /// </summary>
        public BoundingRectangle BoundingRectangle;

        /// <summary>
        /// loads spikes sprite texture
        /// </summary>
        /// <param name="content">ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/obstacles/spikes");
            BoundingRectangle = new BoundingRectangle(Position, 35, 20);
            pixel = content.Load<Texture2D>("Pixel");
        }

        /// <summary>
        /// Updates the spikes sprite to fly in a pattern
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {
            BoundingRectangle.X = Position.X;
            BoundingRectangle.Y = Position.Y;
        }

        /// <summary>
        /// Draws the animated bat sprite
        /// </summary>
        /// <param name="gameTime">game tme</param>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //TODO: figure out where to draw the spikes. Idea: could be drawn the same way the coins are drawn on top of a block. 
            //spriteBatch.Draw()
            //spriteBatch.Draw(texture, Position, source, Color.White);
#if DEBUG
            Rectangle rectangle = new Rectangle((int)BoundingRectangle.X, (int)BoundingRectangle.Y, 32, 32);
            spriteBatch.Draw(pixel, rectangle, Color.White);
#endif
        }
    }
}
