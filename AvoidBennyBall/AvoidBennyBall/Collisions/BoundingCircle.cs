/*BoundingCircle.cs
 * Written By: Nathan Bean
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace AvoidBennyBall.Collisions
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
        public bool CollidesWith(BoundingCircle other)
        {
            return CollisionHelper.Collides(this, other);
        }
    }
}
