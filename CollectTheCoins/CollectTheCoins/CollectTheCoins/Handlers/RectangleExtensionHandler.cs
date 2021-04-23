/* RectangleExtensionHandler.cs
 * Author: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace CollectTheCoins.Handlers
{
    /// <summary>
    /// class to assist collisions of a rectangle
    /// </summary>
    public static class RectangleExtensionHandler
    {
        /// <summary>
        /// Method to get the intersection depth as a vector
        /// </summary>
        /// <param name="rectA">the first rectangle</param>
        /// <param name="rectB">the second rectangle</param>
        /// <returns>the vector of the intersection depth</returns>
        public static Vector2 GetIntersectionDepth(this Rectangle rectA, Rectangle rectB)
        {
            float halfWidthA = rectA.Width / 2.0f;
            float halfHeightA = rectA.Height / 2.0f;
            float halfWidthB = rectB.Width / 2.0f;
            float halfHeightB = rectB.Height / 2.0f;

            // centers
            Vector2 centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
            Vector2 centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

            // current and minimum-non-intersecting distances between centers
            float distanceX = centerA.X - centerB.X;
            float distanceY = centerA.Y - centerB.Y;
            float minDistanceX = halfWidthA + halfWidthB;
            float minDistanceY = halfHeightA + halfHeightB;

            if (Math.Abs(distanceX) >= minDistanceX || Math.Abs(distanceY) >= minDistanceY)
                return Vector2.Zero;


            float depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
            float depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;
            return new Vector2(depthX, depthY);
        }

        /// <summary>
        /// Method to get the center of bottom of rectangle
        /// </summary>
        /// <param name="rectangle">the rectangle</param>
        /// <returns>the center of the bottom</returns>
        public static Vector2 GetBottomCenter(this Rectangle rectangle)
        {
            return new Vector2(rectangle.X + rectangle.Width / 2.0f, rectangle.Bottom);
        }
    }
}
