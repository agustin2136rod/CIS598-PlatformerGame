/* CollisionHelper.cs
 * Written By: Nathan Bean
 */
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.Collisions
{
    /// <summary>
    /// Class to assist with collision detection
    /// </summary>
    public static class CollisionHelper
    {
        /// <summary>
        /// Detects a collision between two BoundingCircles
        /// </summary>
        /// <param name="a">first bounding circle</param>
        /// <param name="b">second bounding circle</param>
        /// <returns>true for collision, false otherwise</returns>
        public static bool Collides(BoundingCircle a, BoundingCircle b)
        {
            return Math.Pow(a.Radius + b.Radius, 2) >=
                Math.Pow(a.Center.X - b.Center.X, 2) +
                Math.Pow(a.Center.Y - b.Center.Y, 2);
        }

        /// <summary>
        /// Detects a collision between two BoundingRectangles
        /// </summary>
        /// <param name="a">first rectangle</param>
        /// <param name="b">second rectangle</param>
        /// <returns>true if collision, false otherwise</returns>
        public static bool Collides(BoundingRectangle a, BoundingRectangle b)
        {
            return !(a.Right < b.Left || a.Left > b.Right ||
                     a.Top > b.Bottom || a.Bottom < b.Top);
        }

        /// <summary>
        /// detects a collision between a rectangle and a circle
        /// </summary>
        /// <param name="c">BoundingCirlce</param>
        /// <param name="r">BoundingRectangle</param>
        /// <returns>true if collision, false otherwise</returns>
        public static bool Collides(BoundingCircle c, BoundingRectangle r)
        {
            float nearestX = MathHelper.Clamp(c.Center.X, r.Left, r.Right);
            float nearestY = MathHelper.Clamp(c.Center.Y, r.Top, r.Bottom);
            return Math.Pow(c.Radius, 2) >=
                Math.Pow(c.Center.X - nearestX, 2) +
                Math.Pow(c.Center.Y - nearestY, 2);
        }

        public static bool Collides(BoundingRectangle r, BoundingCircle c)
        {
            return Collides(c, r);
        }
    }
}
