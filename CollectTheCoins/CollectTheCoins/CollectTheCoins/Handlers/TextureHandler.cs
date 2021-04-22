using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace CollectTheCoins
{
    public class TextureHandler
    {
        Texture2D texture;
        float currentTime;
        bool doLoop;

        public TextureHandler(Texture2D texture, float frameTime, bool doLoop)
        {
            this.texture = texture;
            this.currentTime = frameTime;
            this.doLoop = doLoop;
        }

        public Texture2D Texture { get { return texture; } }

        public float CurrentTime { get { return currentTime; } }

        public bool DoLoop { get { return doLoop; } }

        public int numberOfFrames {  get { return Texture.Width / Height; } }

        public int Width {  get { return Texture.Height; } }

        public int Height {  get { return Texture.Height; } }
    }
}
