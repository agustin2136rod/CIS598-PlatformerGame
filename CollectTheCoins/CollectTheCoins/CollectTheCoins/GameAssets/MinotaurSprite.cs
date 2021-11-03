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
        /// bounding box for the minotaur
        /// </summary>
        public BoundingRectangle BoundingRectangle;

        /// <summary>
        /// loads minotaur sprite texture
        /// </summary>
        /// <param name="content">ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/obstacles/minotaur");
            BoundingRectangle = new BoundingRectangle(Position, 48, 64);
            pixel = content.Load<Texture2D>("Pixel");
        }

        /// <summary>
        /// Updates the minotaur sprite to walk in a given direction
        /// </summary>
        /// <param name="gameTime">game time</param>
        public void Update(GameTime gameTime)
        {
            //update the direction timer
            directionTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //switch directions every two seconds
            if (directionTimer > 2.0)
            {
                switch (Direction)
                {
                    case MinotaurDirection.Up:
                        Direction = MinotaurDirection.Down;
                        break;
                    case MinotaurDirection.Down:
                        Direction = MinotaurDirection.Right;
                        break;
                    case MinotaurDirection.Right:
                        Direction = MinotaurDirection.Left;
                        break;
                    case MinotaurDirection.Left:
                        Direction = MinotaurDirection.Up;
                        break;
                }
                directionTimer -= 2.0;

            }
            //Move the dragon in the direction it is flying
            switch (Direction)
            {
                case MinotaurDirection.Up:
                    Position += new Vector2(0, -1) * 40 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case MinotaurDirection.Down:
                    Position += new Vector2(0, 1) * 40 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case MinotaurDirection.Left:
                    Position += new Vector2(-1, 0) * 40 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case MinotaurDirection.Right:
                    Position += new Vector2(1, 0) * 40 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }

            //update dragon bounding box
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
            var source = new Rectangle(animationFrame * 144, (int)Direction * 128, 144, 128);
            spriteBatch.Draw(texture, Position, source, Color.White);
#if DEBUG
            //Rectangle rectangle = new Rectangle((int)BoundingRectangle.X, (int)BoundingRectangle.Y, 144, 128);
            //spriteBatch.Draw(pixel, rectangle, Color.White);
#endif
        }

    }
}
