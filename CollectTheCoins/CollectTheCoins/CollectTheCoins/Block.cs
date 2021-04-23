/* Block.cs
 * Author: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CollectTheCoins
{
    /// <summary>
    /// enum for the collision of the blocks
    /// </summary>
    public enum BlockCollision
    {
        Passable = 0, 
        Impassable = 1, 
        Platform = 2,
    }
    
    /// <summary>
    /// struct to represent a block
    /// </summary>
    public struct Block
    {
        //variables for texture and collisions
        public Texture2D Texture;
        public BlockCollision Collision;

        public const int Width = 40;
        public const int Height = 32;

        /// <summary>
        /// size of the block is consistent
        /// </summary>
        public static readonly Vector2 Size = new Vector2(Width, Height);

        /// <summary>
        /// constructor for the block
        /// </summary>
        /// <param name="texture">which Texture the block is</param>
        /// <param name="collision">the collision type for the block</param>
        public Block(Texture2D texture, BlockCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }
}
