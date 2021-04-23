using System;
using System.Collections.Generic;
using System.Text;
using CollectTheCoins.Handlers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CollectTheCoins
{
    public class Player
    {
        private TextureHandler idleAnimation;
        private TextureHandler runAnimation;
        private TextureHandler jumpAnimation;
        private TextureHandler celebrateAnimation;
        private SpriteEffects flip = SpriteEffects.None;
        private SoundEffect jumpSound;
        private TexturePlayer spritePlayer;
        LevelHandler level;
        private bool alive;
        private Vector2 position;
        private Vector2 velocity;
        private bool onGround;
        private Rectangle bounds;

        // Constants for controlling horizontal movement
        private const float MoveAcceleration = 13000.0f;
        private const float MaxMoveSpeed = 1750.0f;
        private const float GroundDragFactor = 0.48f;
        private const float AirDragFactor = 0.58f;

        // Constants for controlling vertical movement
        private const float MaxJumpTime = 0.35f;
        private const float JumpLaunchVelocity = -3500.0f;
        private const float GravityAcceleration = 3400.0f;
        private const float MaxFallSpeed = 550.0f;
        private const float JumpControlPower = 0.14f;

        // Input configuration
        private const float MoveStickScale = 1.0f;
        private const float AccelerometerScale = 1.5f;
        private const Buttons JumpButton = Buttons.A;

        private float movement;
        private bool jumping;
        private bool hasJumped;
        private float jumpTime;
        private float oldBottom;


        public LevelHandler Level { get { return level; } }

        public bool Alive { get { return alive; } }

        public Vector2 Position { get { return position; } }

        public Vector2 Velocity {  get { return velocity; } }

        public bool OnGround { get { return onGround; } }

        public Rectangle BoundingRectangle
        {
            get
            {
                int left = (int)Math.Round(Position.X - spritePlayer.Origin.X) + bounds.X;
                int top = (int)Math.Round(Position.Y - spritePlayer.Origin.Y) + bounds.Y;

                return new Rectangle(left, top, bounds.Width, bounds.Height);
            }
        }

        public Player(LevelHandler level, Vector2 position)
        {
            this.level = level;
            LoadContent();
            Reset(position);
        }

        public void LoadContent()
        {
            idleAnimation = new TextureHandler(Level.Content.Load<Texture2D>("sprites/player/idle"), 0.1f, true);
            runAnimation = new TextureHandler(Level.Content.Load<Texture2D>("sprites/player/run"), 0.1f, true);
            jumpAnimation = new TextureHandler(Level.Content.Load<Texture2D>("sprites/player/jump"), 0.1f, false);
            celebrateAnimation = new TextureHandler(Level.Content.Load<Texture2D>("sprites/player/celeb"), 0.1f, false);
            jumpSound = Level.Content.Load<SoundEffect>("sounds/jumpSound");

            int width = (int)(idleAnimation.Width * 0.4);
            int left = (idleAnimation.Width - width) / 2;
            int height = (int)(idleAnimation.Height * 0.8);
            int top = idleAnimation.Height - height;
            bounds = new Rectangle(left, top, width, height);

            
        }

        public void Reset(Vector2 position)
        {
            this.position = position;
            velocity = Vector2.Zero;
            alive = true;
            spritePlayer.Play(idleAnimation);
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState, DisplayOrientation orientation)
        {
            GetInput(keyboardState, orientation);

            ApplyPhysics(gameTime);

            if (Alive && OnGround)
            {
                if (Math.Abs(Velocity.X) - 0.02f > 0)
                {
                    spritePlayer.Play(runAnimation);
                }
                else
                {
                    spritePlayer.Play(idleAnimation);
                }
            }

            movement = 0.0f;
            jumping = false;
        }

        public void GetInput(KeyboardState keyboardState, DisplayOrientation orientation)
        {
            if (Math.Abs(movement) < 0.5f)
            {
                movement = 0.0f;
            }


            if (keyboardState.IsKeyDown(Keys.Left) || keyboardState.IsKeyDown(Keys.A))
            {
                movement = -1.0f;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) || keyboardState.IsKeyDown(Keys.D))
            {
                movement = 1.0f;
            }

            jumping = keyboardState.IsKeyDown(Keys.Space) || keyboardState.IsKeyDown(Keys.Up) || keyboardState.IsKeyDown(Keys.W);
        }

        public void ApplyPhysics(GameTime gameTime)
        {
            float timeElapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Vector2 oldPosition = Position;

            velocity.X += movement * MoveAcceleration * timeElapsed;
            velocity.Y = MathHelper.Clamp(velocity.Y + GravityAcceleration * timeElapsed, -MaxFallSpeed, MaxFallSpeed);
            velocity.Y = Jump(velocity.Y, gameTime);

            if (OnGround) velocity.X *= GroundDragFactor;
            else velocity.X *= AirDragFactor;

            velocity.X = MathHelper.Clamp(velocity.X, -MaxMoveSpeed, MaxMoveSpeed);

            position += velocity * timeElapsed;
            position = new Vector2((float)Math.Round(Position.X), (float)Math.Round(Position.Y));

            HandleCollision();

            if (Position.X == oldPosition.X) velocity.X = 0;
            if (Position.Y == oldPosition.Y) velocity.Y = 0;
        }

        private float Jump(float velocityInY, GameTime gameTime)
        {
            if (jumping)
            {
                //start jump
                if ((!hasJumped && OnGround) || jumpTime > 0.0f)
                {
                    if (jumpTime == 0.0f)
                        jumpSound.Play();

                    jumpTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    spritePlayer.Play(jumpAnimation);
                }

                //ascending jump
                if (0.0f < jumpTime && jumpTime <= MaxJumpTime)
                {
                    // Fully override the vertical velocity with a power curve that gives players more control over the top of the jump
                    velocityInY = JumpLaunchVelocity * (1.0f - (float)Math.Pow(jumpTime / MaxJumpTime, JumpControlPower));
                }
                else
                {
                    // Reached the apex of the jump
                    jumpTime = 0.0f;
                }
            }
            else
            {
                jumpTime = 0.0f;
            }
            hasJumped = jumping;
            return velocityInY;
        }

        private void HandleCollision()
        {
            Rectangle bounds = BoundingRectangle;
            int leftBlock = (int)Math.Floor((float)bounds.Left / Block.Width);
            int rightBlock = (int)Math.Ceiling(((float)bounds.Right / Block.Width)) - 1;
            int topBlock = (int)Math.Floor((float)bounds.Top / Block.Height);
            int bottomBlock = (int)Math.Ceiling(((float)bounds.Bottom / Block.Height)) - 1;

            onGround = false;

            for (int y = topBlock; y <= bottomBlock; ++y)
            {
                for (int x = leftBlock; x <= rightBlock; ++x)
                {
                    // If this tile is collidable,
                    BlockCollision collision = Level.GetCollision(x, y);
                    if (collision != BlockCollision.Passable)
                    {
                        // Determine collision depth (with direction) and magnitude.
                        Rectangle blockBounds = Level.GetBounds(x, y);
                        Vector2 depth = RectangleExtensionHandler.GetIntersectionDepth(bounds, blockBounds);
                        if (depth != Vector2.Zero)
                        {
                            float absDepthX = Math.Abs(depth.X);
                            float absDepthY = Math.Abs(depth.Y);

                            // Resolve the collision along the shallow axis.
                            if (absDepthY < absDepthX || collision == BlockCollision.Platform)
                            {
                                // If we crossed the top of a tile, we are on the ground.
                                if (oldBottom <= blockBounds.Top)
                                    onGround = true;

                                // Ignore platforms, unless we are on the ground.
                                if (collision == BlockCollision.Impassable || OnGround)
                                {
                                    // Resolve the collision along the Y axis.
                                    position = new Vector2(Position.X, Position.Y + depth.Y);

                                    // Perform further collisions with the new bounds.
                                    bounds = BoundingRectangle;
                                }
                            }
                            else if (collision == BlockCollision.Impassable) // Ignore platforms.
                            {
                                // Resolve the collision along the X axis.
                                position = new Vector2(Position.X + depth.X, Position.Y);

                                // Perform further collisions with the new bounds.
                                bounds = BoundingRectangle;
                            }
                        }
                    }
                }
            }
            oldBottom = bounds.Bottom;
        }

        public void ReachedExit()
        {
            spritePlayer.Play(celebrateAnimation);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (Velocity.X > 0) flip = SpriteEffects.FlipHorizontally;
            else if (Velocity.X < 0) flip = SpriteEffects.None;

            spritePlayer.Draw(gameTime, spriteBatch, Position, flip);
        }
    }
}
