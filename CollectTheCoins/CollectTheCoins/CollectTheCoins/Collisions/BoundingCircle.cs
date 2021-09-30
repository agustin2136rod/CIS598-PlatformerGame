/*BoundingCircle.cs
 * Written By: Nathan Bean
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollectTheCoins.Collisions
{
    /// <summary>
    /// A struct written by Nathan Bean representing circular bounds for ball and ghost from demo
    /// </summary>
    public struct BoundingCircle
    {
        /// <summary>
        /// center of the bounding circle
        /// </summary>
        public Vector2 Center;

        /// <summary>
        /// radius of the bounding circle
        /// </summary>
        public float Radius;

        public static implicit operator Rectangle(BoundingCircle circle)
        {
            return new Rectangle()
            {
                X = (int)(circle.Center.X - circle.Radius), 
                Y = (int)(circle.Center.Y - circle.Radius),
                Width = (int)(2 * circle.Radius), 
                Height = (int)(2 * circle.Radius)
            };
        } 

        /// <summary>
        /// Constructs a new bounding circle
        /// </summary>
        /// <param name="center">center of the circle</param>
        /// <param name="radius">Radius of the circle</param>
        public BoundingCircle(Vector2 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        /// <summary>
        /// Tests for a collision between this and another bounding circle
        /// </summary>
        /// <param name="other">the other bounding circle</param>
        /// <returns>true for a collision, false otherwise</returns>
        public bool CollidesWith(Rectangle rectangle)
        {
            Vector2 tempVector = new Vector2(MathHelper.Clamp(Center.X, rectangle.Left, rectangle.Right),
                                    MathHelper.Clamp(Center.Y, rectangle.Top, rectangle.Bottom));

            Vector2 direction = Center - tempVector;
            float distanceSquared = direction.LengthSquared();

            return ((distanceSquared > 0) && (distanceSquared < Radius * Radius));
        }
    }
}
