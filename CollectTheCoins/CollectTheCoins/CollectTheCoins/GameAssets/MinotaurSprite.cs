/* MinotaurSprite.cs
 * Written By: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;
using CollectTheCoins.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CollectTheCoins.GameAssets
{
    /// <summary>
    /// Enum to handle the direction the minotaur walks in 
    /// </summary>
    public enum MinotaurDirection
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    /// <summary>
    /// Class to represent a minotaur sprite enemy
    /// </summary>
    public class MinotaurSprite
    {
        /// <summary>
        /// Variable to represent the minotaur sprite
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// used for debugging purposes
        /// </summary>
        private Texture2D pixel;

        /// <summary>
        /// keep count of the time the minotaur walks in each direction
        /// </summary>
        private double directionTimer;

        /// <summary>
        /// keep count animation timer for effects
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// the frame the minotaur is currently in 
        /// </summary>
        private short animationFrame = 1;

        /// <summary>
        /// direction of the Minotaur
        /// </summary>
        public MinotaurDirection Direction;

        /// <summary>
        /// position of the minotaur
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// start at exit block 
        /// </summary>
        public Vector2 startPosition;

        /// <summary>
        /// end at exit block 
        /// </summary>
        public Vector2 endPosition; 

        /// <summary>
        /// bounding box for the minotaur
        /// </summary>
        public BoundingRectangle BoundingRectangle;

        /// <summary>
        /// loads minotaur sprite texture
        /// </summary>
        /// <param name="content">ContentManager to load with</param>
        public void LoadContent(ContentManager content, Vector2 start, Vector2 end)
        {
            texture = content.Load<Texture2D>("sprites/obstacles/minotaur");
            BoundingRectangle = new BoundingRectangle(Position, 48, 64);
            pixel = content.Load<Texture2D>("Pixel");
            startPosition = start;
            endPosition = end;
        }

        /// <summary>
        /// Updates the minotaur sprite to walk in a given direction
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {
            //switch directions if minotaur is at the end of the bounds 
            if (Position.X <= endPosition.X || Position.X >= startPosition.X)
            {
                switch (Direction)
                {
                    case MinotaurDirection.Right:
                        Direction = MinotaurDirection.Left;
                        break;
                    case MinotaurDirection.Left:
                        Direction = MinotaurDirection.Right;
                        break;
                }
            }

            //Move the minotaur in the direction it is walking 
            switch (Direction)
            {
                case MinotaurDirection.Left:
                    Position += new Vector2(-1, 0) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case MinotaurDirection.Right:
                    Position += new Vector2(1, 0) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }

            //update minotaur bounding box
            BoundingRectangle.X = Position.X;
            BoundingRectangle.Y = Position.Y;
        }

        /// <summary>
        /// Draws the animated dragon sprite
        /// </summary>
        /// <param name="gameTime">game tme</param>
        /// <param name="spriteBatch">SpriteBatch to draw with</param>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //update animation frame
            if (animationTimer > 0.3)
            {
                animationFrame++;
                if (animationFrame > 2)
                {
                    animationFrame = 1;
                }
                animationTimer -= 0.3;
            }

            //draw the sprite
            var source = new Rectangle(animationFrame * 48, (int)Direction * 64, 48, 64);
            spriteBatch.Draw(texture, Position, source, Color.White);
#if DEBUG
            Rectangle rectangle = new Rectangle((int)BoundingRectangle.X, (int)BoundingRectangle.Y, 48, 64);
            spriteBatch.Draw(pixel, rectangle, Color.White);
#endif
        }

    }
}
