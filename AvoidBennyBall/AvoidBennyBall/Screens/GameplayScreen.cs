using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using AvoidBennyBall.StateManagement;
using AvoidBennyBall;
using AvoidBennyBall.Collisions;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace AvoidBennyBall.Screens
{
    // This screen implements the actual game logic. It is just a
    // placeholder to get the idea across: you'll probably want to
    // put some more interesting gameplay in here!
    public class GameplayScreen : GameScreen
    {

        //set up all variables
        private Vector2 ballPosition;
        private Vector2 ballVelocity;
        private Texture2D ballTexture;
        private SlimeGhostSprite slimeGhost;
        private SpriteFont spriteFont;
        private BoundingCircle bounding;

        private int timesHit = 0;

        private SoundEffect slimeHit;
        private Song backgroundMusic;




        private ContentManager _content;
        private SpriteFont _gameFont;

        private Vector2 _playerPosition = new Vector2(100, 100);
        private Vector2 _enemyPosition = new Vector2(100, 100);

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public GameplayScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            //instructions for the game
            _pauseAction = new InputAction(
                new[] { Buttons.Start, Buttons.Back },
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _gameFont = _content.Load<SpriteFont>("gamefont");

            slimeGhost = new SlimeGhostSprite();
            ballTexture = _content.Load<Texture2D>("ball");
            slimeGhost.LoadContent(_content);
            spriteFont = _content.Load<SpriteFont>("arial");
            backgroundMusic = _content.Load<Song>("music");
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(backgroundMusic);
            slimeHit = _content.Load<SoundEffect>("Hit_Hurt5");

            // A real game would probably have more content than this sample, so
            // it would take longer to load. We simulate that by delaying for a
            // while, giving you a chance to admire the beautiful loading screen.
            Thread.Sleep(1000);

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
            startGame();
        }

        private void startGame()
        {
            ballPosition = new Vector2(ScreenManager.GraphicsDevice.Viewport.Width / 2, ScreenManager.GraphicsDevice.Viewport.Height / 2);
            bounding = new BoundingCircle(ballPosition, 64);
            Random random = new Random();
            ballVelocity = new Vector2((float)random.NextDouble(), (float)random.NextDouble());
            ballVelocity.Normalize();
            ballVelocity *= 1500;
            slimeGhost.Reset();
            MediaPlayer.Play(backgroundMusic);
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }

        public override void Unload()
        {
            _content.Unload();
        }

        // This method checks the GameScreen.IsActive property, so the game will
        // stop updating when the pause menu is active, or if you tab away to a different application.
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                _pauseAlpha = Math.Min(_pauseAlpha + 1f / 32, 1);
            else
                _pauseAlpha = Math.Max(_pauseAlpha - 1f / 32, 0);

            if (IsActive)
            {
                // Apply some random jitter to make the enemy move around.
                const float randomization = 10;

                _enemyPosition.X += (float)(_random.NextDouble() - 0.5) * randomization;
                _enemyPosition.Y += (float)(_random.NextDouble() - 0.5) * randomization;

                // Apply a stabilizing force to stop the enemy moving off the screen.
                var targetPosition = new Vector2(
                    ScreenManager.GraphicsDevice.Viewport.Width / 2 - _gameFont.MeasureString("Insert Gameplay Here").X / 2,
                    200);

                _enemyPosition = Vector2.Lerp(_enemyPosition, targetPosition, 0.05f);

                // This game isn't very fun! You could probably improve
                // it by inserting something more interesting in this space :-)
            }
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            var keyboardState = input.CurrentKeyboardStates[playerIndex];
            var gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected && input.GamePadWasConnected[playerIndex];

            PlayerIndex player;
            if (_pauseAction.Occurred(input, ControllingPlayer, out player) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else
            {
                // TODO: Add your update logic here
                slimeGhost.Update(gameTime);
                slimeGhost.Color = Color.White;
                if (slimeGhost.Bounds.CollidesWith(bounding))
                {
                    slimeGhost.Color = Color.Black;
                    slimeHit.Play();
                    timesHit++;
                    
                }

                //code receieved from Nathan Bean to implement the ball moving across the screen from HelloGame Demo
                ballPosition += (float)gameTime.ElapsedGameTime.TotalSeconds * ballVelocity;
                bounding.Center = ballPosition;
                if (ballPosition.X < ScreenManager.GraphicsDevice.Viewport.X || ballPosition.X > ScreenManager.GraphicsDevice.Viewport.Width - 64)
                {
                    ballVelocity.X *= -1;
                }
                if (ballPosition.Y < ScreenManager.GraphicsDevice.Viewport.Y || ballPosition.Y > ScreenManager.GraphicsDevice.Viewport.Height - 64)
                {
                    ballVelocity.Y *= -1;
                }
                
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);

            // Our player and enemy are both actually just text strings.
            var spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            slimeGhost.Draw(gameTime, spriteBatch);
            spriteBatch.Draw(ballTexture, ballPosition, Color.White);
            spriteBatch.DrawString(spriteFont, "Total times hit: " + timesHit, new Vector2(2, 2), Color.Gold);

            

            spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }
    }
}
