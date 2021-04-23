/* CoinHandler.cs
 * Author: Agustin Rodriguez 
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using CollectTheCoins.Collisions;
using CollectTheCoins.Handlers;

namespace CollectTheCoins
{
    /// <summary>
    /// class to represent a coin on the game
    /// </summary>
    public class CoinHandler
    {
        //variables to use in the class
        private Texture2D texture;
        private Vector2 origin;
        public readonly Color Color = Color.Yellow;
        private Vector2 basePosition;
        private float bounce;
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;
        LevelHandler level;

        /// <summary>
        /// getter for the level
        /// </summary>
        public LevelHandler Level { get { return level; } }
        
        /// <summary>
        /// constructor for the class 
        /// </summary>
        /// <param name="level">the current level</param>
        /// <param name="position">the position of the coin</param>
        public CoinHandler(LevelHandler level, Vector2 position)
        {
            basePosition = position;
            this.level = level;
            LoadContent();
        }

        /// <summary>
        /// Method to load the content for the coins
        /// </summary>
        public void LoadContent()
        {
            texture = level.Content.Load<Texture2D>("sprites/coins/coins");
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
        }

        /// <summary>
        /// method to update each coin
        /// </summary>
        /// <param name="gameTime">the elapsed game time</param>
        public void Update(GameTime gameTime)
        {
            float bounceHeight = 0.18f;
            float bounceRate = 3.0f;
            float bounceSync = -0.75f;

            double time = gameTime.TotalGameTime.TotalSeconds * bounceRate + Position.X * bounceSync;
            bounce = (float)Math.Sin(time) * bounceHeight * texture.Height;
        }

        /// <summary>
        /// Getter for the bounding circle around a coin for collision purposes
        /// </summary>
        public BoundingCircle BoundingCircle
        {
            get { return new BoundingCircle(Position, Block.Width / 0.8f); }
        }

        /// <summary>
        /// Position of the coin
        /// </summary>
        public Vector2 Position { get { return basePosition + new Vector2(58.0f, bounce); } }

        /// <summary>
        /// method to draw each coin on the game screen
        /// </summary>
        /// <param name="gameTime">elapsed game time</param>
        /// <param name="spriteBatch">the spritebatch</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }
            var source = new Rectangle(animationFrame * 16, 0, 16, 16);
            spriteBatch.Draw(texture, Position, source, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }
    }
}
