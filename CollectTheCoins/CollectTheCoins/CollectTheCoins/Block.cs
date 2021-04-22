using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CollectTheCoins
{
    public enum BlockCollision
    {
        Passable = 0, 
        Impassable = 1, 
        Platform = 2,
    }
    
    public struct Block
    {
        public Texture2D Texture;
        public BlockCollision Collision;

        public const int Width = 40;
        public const int Height = 32;

        public static readonly Vector2 Size = new Vector2(Width, Height);

        public Block(Texture2D texture, BlockCollision collision)
        {
            Texture = texture;
            Collision = collision;
        }
    }
}
