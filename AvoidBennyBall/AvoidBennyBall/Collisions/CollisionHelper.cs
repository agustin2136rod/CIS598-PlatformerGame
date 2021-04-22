/*CollisionHelper.cs
 * Written By: Nathan Bean
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace AvoidBennyBall.Collisions
{
    /// <summary>
    /// Class written by Nathan Bean in Demo to detect a collision between two BoundingCircles
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
    }
}
