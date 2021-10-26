/* BoundingRectangle.cs
 * Written By: Nathan Bean
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.Collisions
{
    /// <summary>
    /// A bounding rectangle for collision detection
    /// </summary>
    public struct BoundingRectangle
    {
        /// <summary>
        /// x position
        /// </summary>
        public float X;

        /// <summary>
        /// y position
        /// </summary>
        public float Y;

        /// <summary>
        /// width of the rectangle
        /// </summary>
        public float Width;

        /// <summary>
        /// height of the rectangle
        /// </summary>
        public float Height;

        /// <summary>
        /// Left side of rectangle
        /// </summary>
        public float Left => X;

        /// <summary>
        /// right side of rectangle
        /// </summary>
        public float Right => X + Width;

        /// <summary>
        /// top of the rectangle
        /// </summary>
        public float Top => Y;

        /// <summary>
        /// Bottome of the rectangle 
        /// </summary>
        public float Bottom => Y + Height;

        /// <summary>
        /// constuctor for a rectangle
        /// </summary>
        /// <param name="x">x position</param>
        /// <param name="y">y position</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="height">height of rectangle</param>
        public BoundingRectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// constructor for a rectangle
        /// </summary>
        /// <param name="position">vector position for top left of rectangle</param>
        /// <param name="width">width of rectangle</param>
        /// <param name="height">height of rectangle</param>
        public BoundingRectangle(Vector2 position, float width, float height)
        {
            X = position.X;
            Y = position.Y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// whether or not two rectangles collided
        /// </summary>
        /// <param name="other">other rectangle</param>
        /// <returns>bool</returns>
        public bool CollidesWith(BoundingRectangle other)
        {
            return CollisionHelper.Collides(this, other);
        }

        /// <summary>
        /// whether or not a rectangle collides with a circle
        /// </summary>
        /// <param name="other">circle</param>
        /// <returns>bool</returns>
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(other, this);
        }
    }
}
