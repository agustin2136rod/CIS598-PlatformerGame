using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CollectTheCoins
{
    public struct TexturePlayer
    {

        TextureHandler textureHandler;
        private int index;
        private float time;


        public TextureHandler TextureHandler { get { return textureHandler; } }

        public int Index { get { return index; } }

        public Vector2 Origin { get { return new Vector2(textureHandler.Width / 2.0f, textureHandler.Height); } }

        public void Play(TextureHandler textureHandler)
        {
            if (this.TextureHandler == textureHandler)
            {
                return;
            }
            this.textureHandler = textureHandler;
            index = 0;
            time = 0.0f;
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 position, SpriteEffects spriteEffects)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds;

            while (time > textureHandler.CurrentTime)
            {
                time -= textureHandler.CurrentTime;
                if (textureHandler.DoLoop)
                {
                    index = (index + 1) % textureHandler.numberOfFrames;
                } 
                else
                {
                    index = Math.Min(Index + 1, textureHandler.numberOfFrames - 1);
                }
            }
            Rectangle sourceRectangle = new Rectangle(Index * textureHandler.Texture.Height, 0, TextureHandler.Texture.Height, TextureHandler.Texture.Height);

            spriteBatch.Draw(textureHandler.Texture, position, sourceRectangle, Color.White, 0.0f, Origin, 1.0f, spriteEffects, 0.0f);
        }

    }
}
