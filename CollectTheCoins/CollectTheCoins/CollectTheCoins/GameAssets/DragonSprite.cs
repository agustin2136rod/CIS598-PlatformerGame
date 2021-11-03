/* DragonSprite.cs
 * Written By: Agustin Rodriguez
 */

using CollectTheCoins.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CollectTheCoins.GameAssets
{
    /// <summary>
    /// Enum to handle the direction the dragon flies in 
    /// </summary>
    public enum DragonDirection
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    /// <summary>
    /// Class to represent a dragon enemy in the game 
    /// </summary>
    public class DragonSprite
    {
        /// <summary>
        /// Variable to represent the dragon sprite
        /// </summary>
        private Texture2D texture;

        /// <summary>
        /// used for debugging purposes
        /// </summary>
        private Texture2D pixel;

        /// <summary>
        /// keep count of the time the dragon flies in each direction
        /// </summary>
        private double directionTimer;

        /// <summary>
        /// keep count animation timer for effects
        /// </summary>
        private double animationTimer;

        /// <summary>
        /// the frame the dragon is currently in 
        /// </summary>
        private short animationFrame = 1;

        /// <summary>
        /// direction of the dragon
        /// </summary>
        public DragonDirection Direction;

        /// <summary>
        /// position of the dragon
        /// </summary>
        public Vector2 Position;

        /// <summary>
        /// bounding box for the dragon
        /// </summary>
        public BoundingRectangle BoundingRectangle;

        /// <summary>
        /// loads dragon sprite texture
        /// </summary>
        /// <param name="content">ContentManager to load with</param>
        public void LoadContent(ContentManager content)
        {
            texture = content.Load<Texture2D>("sprites/obstacles/dragon");
            BoundingRectangle = new BoundingRectangle(Position, 144, 128);
            pixel = content.Load<Texture2D>("Pixel");
        }

        /// <summary>
        /// Updates the dragon sprite to fly in a pattern
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
                    case DragonDirection.Up:
                        Direction = DragonDirection.Down;
                        break;
                    case DragonDirection.Down:
                        Direction = DragonDirection.Right;
                        break;
                    case DragonDirection.Right:
                        Direction = DragonDirection.Left;
                        break;
                    case DragonDirection.Left:
                        Direction = DragonDirection.Up;
                        break;
                }
                directionTimer -= 2.0;

            }
            //Move the dragon in the direction it is flying
            switch (Direction)
            {
                case DragonDirection.Up:
                    //down
                    Position += new Vector2(0, -1) * 60 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case DragonDirection.Down:
                    //right
                    Position += new Vector2(0, 1) * 40 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case DragonDirection.Left:
                    //up
                    Position += new Vector2(-1, 0) * 40 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case DragonDirection.Right:
                    //left
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
