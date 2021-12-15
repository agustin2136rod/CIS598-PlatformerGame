using CollectTheCoins.Collisions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.GameAssets
{
    /// <summary>
    /// Enum to handle the direction the minotaur walks in 
    /// </summary>
    public enum WarriorDirection
    {
        Up = 0,
        Right = 1,
        Down = 2,
        Left = 3,
    }

    /// <summary>
    /// Class to represent a minotaur sprite enemy
    /// </summary>
    public class WarriorSprite
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
        public WarriorDirection Direction;

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
            texture = content.Load<Texture2D>("sprites/obstacles/warrior");
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
            if (Position.X < endPosition.X || Position.X > startPosition.X)
            {
                switch (Direction)
                {
                    case WarriorDirection.Right:
                        Direction = WarriorDirection.Left;
                        break;
                    case WarriorDirection.Left:
                        Direction = WarriorDirection.Right;
                        break;
                }
            }

            //Move the minotaur in the direction it is walking 
            switch (Direction)
            {
                case WarriorDirection.Left:
                    Position += new Vector2(-1, 0) * 80 * (float)gameTime.ElapsedGameTime.TotalSeconds;
                    break;
                case WarriorDirection.Right:
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
        }
    }
}
