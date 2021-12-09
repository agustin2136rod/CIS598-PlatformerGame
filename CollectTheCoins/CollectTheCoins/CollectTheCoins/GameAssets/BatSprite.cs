/* BatSprite.cs
 * Written By Nathan Bean
 */ 
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using CollectTheCoins.Collisions;

namespace CollectTheCoins.GameAssets
{
    /// <summary>
    /// Enum to handle the direction the bat flies in 
    /// </summary>
    public enum BatDirection
    {
        Down = 0, 
        Right = 1, 
        Up = 2, 
        Left = 3,
    }
    
    /// <summary>
    /// A class representing a bat sprite to act as an obstacle for the player
    /// </summary>
    public class BatSprite
    {
        /// <summary>
        /// Variable to represent the bat sprite
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// used for debugging purposes
        /// </summary>
        private Texture2D pixel;

        /// <summary>
        /// keep count of the time the bat flies in each direction
        /// </summary>
        private double directionTimer;

        /// <summary>
        /// keep count animation timer for effects
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// the frame the bat is currently in 
        /// </summary>
        private short animationFrame = 1;

        /// <summary>
        /// direction of the bat
        /// </summary>
        public BatDirection Direction;

        /// <summary>
        /// position of the bat
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// bounding box for the bat
        /// </summary>
        public BoundingRectangle BoundingRectangle;

        /// <summary>
        /// loads bat sprite texture
        /// </summary>
        /// <param name="content">ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/obstacles/bat");
            BoundingRectangle = new BoundingRectangle(Position, 32, 32);
            pixel = content.Load<Texture2D>("Pixel");
        }

        /// <summary>
        /// Updates the bat sprite to fly in a pattern
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
                    case BatDirection.Up:
                        Direction = BatDirection.Down;
                        break;
                    case BatDirection.Down:
                        Direction = BatDirection.Right;
                        break;
                    case BatDirection.Right:
                        Direction = BatDirection.Left;
                        break;
                    case BatDirection.Left:
                        Direction = BatDirection.Up;
                        break;
                }
                directionTimer -= 2.0;

            }
            //Move the bat in the direction it is flying
            switch (Direction)
            {
                case BatDirection.Up:
                    Position += new Vector2(0, -1) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case BatDirection.Down:
                    Position += new Vector2(0, 1) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case BatDirection.Left:
                    Position += new Vector2(-1, 0) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case BatDirection.Right:
                    Position += new Vector2(1, 0) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
            }
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
            //update animation timer
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;

            //update animation frame
            if (animationTimer > 0.3)
            {
                animationFrame++;
                if (animationFrame > 3)
                {
                    animationFrame = 1;
                }
                animationTimer -= 0.3;
            }

            //draw the sprite
            var source = new Rectangle(animationFrame * 32, (int)Direction * 32, 32, 32);
            spriteBatch.Draw(texture, Position, source, Color.White);
#if DEBUG
            //Rectangle rectangle = new Rectangle((int) BoundingRectangle.X, (int) BoundingRectangle.Y, 32, 32);
            //spriteBatch.Draw(pixel, rectangle, Color.White);
#endif
        }
    }
}
