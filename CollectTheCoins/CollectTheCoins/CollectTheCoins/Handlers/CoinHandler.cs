using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using CollectTheCoins.Collisions;
using CollectTheCoins.Handlers;

namespace CollectTheCoins
{
    public class CoinHandler
    {
        private Texture2D texture;
        private Vector2 origin;
        public readonly Color Color = Color.Yellow;
        private Vector2 basePosition;
        private float bounce;
        private const float ANIMATION_SPEED = 0.1f;
        private double animationTimer;
        private int animationFrame;

        
        LevelHandler level;
        public LevelHandler Level { get { return level; } }
        

        public CoinHandler(LevelHandler level, Vector2 position)
        {
            basePosition = position;
            this.level = level;
            LoadContent();
        }

        public void LoadContent()
        {
            texture = level.Content.Load<Texture2D>("sprites/coins/coins");
            origin = new Vector2(texture.Width / 2.0f, texture.Height / 2.0f);
            //collectedSound = Level.Content.Load<SoundEffect>("Sounds/GemCollected");
        }

        public void Update(GameTime gameTime)
        {
            float bounceHeight = 0.18f;
            float bounceRate = 3.0f;
            float bounceSync = -0.75f;

            double time = gameTime.TotalGameTime.TotalSeconds * bounceRate + Position.X * bounceSync;
            bounce = (float)Math.Sin(time) * bounceHeight * texture.Height;
        }

        public BoundingCircle BoundingCircle
        {
            get { return new BoundingCircle(Position + new Vector2(8, 8), 8); }
        }

        public Vector2 Position { get { return basePosition + new Vector2(0.0f, bounce); } }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            animationTimer += gameTime.ElapsedGameTime.TotalSeconds;
            if (animationTimer > ANIMATION_SPEED)
            {
                animationFrame++;
                if (animationFrame > 7) animationFrame = 0;
                animationTimer -= ANIMATION_SPEED;
            }
            var source = new Rectangle(animationFrame * 16, 0, 16, 16);
            spriteBatch.Draw(texture, Position, source, Color, 0.0f, origin, 1.0f, SpriteEffects.None, 0.0f);
        }


    }
}
