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
        /// handle texture content
        /// </summary>
        public ContentManager ContentManager;

        /// <summary>
        /// constructor for a spike object
        /// </summary>
        /// <param name="content">the content manager</param>
        /// <param name="position">the position of the spike</param>
        public SpikesSprite(ContentManager content, Vector2 position)
        {
            Position = position;
            BoundingRectangle = new BoundingRectangle(Position, 35, 20);
            ContentManager = content;
            LoadContent(content);
        }
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
        /// Draws the animated bat sprite
        /// </summary>
        /// <param name="gameTime">game tme</param>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, Position, Color.White);
#if DEBUG
            //Rectangle rectangle = new Rectangle((int)BoundingRectangle.X, (int)BoundingRectangle.Y, 35, 20);
            //spriteBatch.Draw(pixel, rectangle, Color.White);
#endif
        }
    }
}
