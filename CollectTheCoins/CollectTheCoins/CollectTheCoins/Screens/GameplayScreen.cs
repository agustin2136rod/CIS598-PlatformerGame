/* GameplayScreen.cs
 * Written By: Agustin Rodriguez
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CollectTheCoins.StateManagement;
using CollectTheCoins.Handlers;
using System.IO;
using Microsoft.Xna.Framework.Media;

namespace CollectTheCoins.Screens
{
    public class GameplayScreen : GameScreen
    {
        private ContentManager _content;
        private SpriteFont _gameFont;
        private GameServiceContainer _services;
        private LevelHandler level;
        private Song backgroundMusic;
        private Texture2D win;
        private Texture2D fail;
        private Texture2D done;
        private Texture2D died;
        private Vector2 screenSize = new Vector2(800, 480);
        private Matrix globalTransformation;
        private int bufferWidth, bufferHeight;
        private SpriteBatch _spriteBatch;
        private bool seenInstructions = false;
        private bool continuePressed;
        private Texture2D instructions;
        private KeyboardState keyboardState;
        private int levelIndex = -1;
        private const int numberOfLevels = 11;
        private VolumeHandler gameVolume;
        private MediaHandler mediaHandler;
        

        private readonly Random _random = new Random();

        private float _pauseAlpha;
        private readonly InputAction _pauseAction;

        public GameplayScreen(GameServiceContainer gameService)
        {
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            _services = gameService;

            _pauseAction = new InputAction(
                new[] { Keys.Back, Keys.Escape }, true);
        }

        // Load graphics content for the game
        public override void Activate()
        {
            if (_content == null)
                _content = new ContentManager(ScreenManager.Game.Services, "Content");

            _spriteBatch = new SpriteBatch(ScreenManager.GraphicsDevice);

            _gameFont = _content.Load<SpriteFont>("arial");
            win = _content.Load<Texture2D>("Winner");
            instructions = _content.Load<Texture2D>("Instructions");
            fail = _content.Load<Texture2D>("fail");
            done = _content.Load<Texture2D>("Done");
            died = _content.Load<Texture2D>("died");
            ScalePresentation();
            backgroundMusic = _content.Load<Song>("sounds/BoxCat Games - Epic Song");
            gameVolume = new VolumeHandler(0.7f);
            mediaHandler = new MediaHandler();
            mediaHandler.setVolume(gameVolume.Volume);
            mediaHandler.Play(backgroundMusic);
            mediaHandler.SetRepeating();
            LoadLevel();

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }

        /// <summary>
        /// Method to scale the presentation window for the game. It adjusts to your screen size and sets it accordingly
        /// </summary>
        public void ScalePresentation()
        {
            bufferWidth = ScreenManager.GraphicsDevice.PresentationParameters.BackBufferWidth;
            bufferHeight = ScreenManager.GraphicsDevice.PresentationParameters.BackBufferHeight;
            float horScaling = bufferWidth / screenSize.X;
            float verScaling = bufferHeight / screenSize.Y;
            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);
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
            level.Update(gameTime, keyboardState, gameVolume);
            mediaHandler.setVolume(gameVolume.Volume);

            base.Update(gameTime, otherScreenHasFocus, false);
        }

        /// <summary>
        /// Method to load each level 
        /// </summary>
        private void LoadLevel()
        {
            levelIndex = (levelIndex + 1) % numberOfLevels;
            if (level != null) level.Dispose();

            string path = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(path))
            {
                level = new LevelHandler(_services, fileStream, levelIndex);
            }
        }

        /// <summary>
        /// Method to reload a level
        /// </summary>
        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadLevel();
        }

        // Unlike the Update method, this will only be called when the gameplay screen is active.
        public override void HandleInput(GameTime gameTime, InputState input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            // Look up inputs for the active player profile.
            int playerIndex = (int)ControllingPlayer.Value;

            keyboardState = input.CurrentKeyboardStates[playerIndex];

            bool proceed = keyboardState.IsKeyDown(Keys.Space);
            if (proceed) 
            {
                seenInstructions = true;
                if (!level.TimerRunning)
                {
                    level.StartLevelTimer();
                }
            }

            PlayerIndex player;

            if (_pauseAction.Occurred(input, ControllingPlayer, out player))
            {
                ScreenManager.AddScreen(new PauseMenuScreen(_services, gameVolume, mediaHandler), ControllingPlayer);
                level.PauseLevelTimer();
            }
            else
            {
                if (level.IsTimePaused)
                {
                    level.ResumeLevelTimer();
                    mediaHandler.setVolume(gameVolume.Volume);
                }
                if (!continuePressed && proceed)
                {
                    if (!level.Player.Alive)
                    {
                        level.Start();
                        level.StartLevelTimer();
                        mediaHandler.setVolume(gameVolume.Volume);
                        mediaHandler.Play(backgroundMusic);
                    }
                    else if (level.TimeLeft == TimeSpan.Zero)
                    {
                        if (level.AtExit)
                        {
                            LoadLevel();
                            level.StartLevelTimer();
                            mediaHandler.setVolume(gameVolume.Volume);
                            mediaHandler.Play(backgroundMusic);
                        }
                        else
                        {
                            ReloadCurrentLevel();
                            level.StartLevelTimer();
                            mediaHandler.setVolume(gameVolume.Volume);
                            mediaHandler.Play(backgroundMusic);
                        }
                    }
                }
                continuePressed = proceed;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0, 0);
           

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, globalTransformation);
            level.Draw(gameTime, _spriteBatch);
            DrawHud();
            _spriteBatch.End();

            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || _pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, _pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        /// <summary>
        /// Secondary method of drawing
        /// </summary>
        private void DrawHud()
        {

            Rectangle title = ScreenManager.GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudSpot = new Vector2(title.X, title.Y);
            Vector2 levelLocation = new Vector2(hudSpot.X + 150, title.Y);

            Vector2 center = new Vector2(screenSize.X / 2, screenSize.Y / 2);
            
            if (level.TimerRunning)
            {
                string time = "TIME: " + level.TimeLeft.Minutes.ToString("00") + ":" + level.TimeLeft.Seconds.ToString("00");
                string displayLevel = "LEVEL: " + (levelIndex + 1).ToString();
                Color colorOfFont = Color.Red;
                _spriteBatch.DrawString(_gameFont, time, hudSpot, colorOfFont);
                _spriteBatch.DrawString(_gameFont, displayLevel, levelLocation, colorOfFont);
            }
            
           
            Vector2 winSize = new Vector2(win.Width, win.Height);
            if (!seenInstructions)
            {
                _spriteBatch.Draw(instructions, center - winSize / 2, Color.White);
            }

            if (level.AtExit && level.Coins.Count == 0 && levelIndex != 3)
            {
                _spriteBatch.Draw(win, center - winSize / 2, Color.White);
                mediaHandler.Stop();
            }

            if (level.TimeLeft == TimeSpan.Zero && !level.AtExit)
            {
                _spriteBatch.Draw(fail, center - winSize / 2, Color.White);
                mediaHandler.Stop();
            }

            if (level.EnemyCollidedWithCharacter && level.TimeLeft == TimeSpan.Zero && !level.AtExit)
            {
                _spriteBatch.Draw(died, center - winSize / 2, Color.White);
                mediaHandler.Stop();
            }

            if (level.AtExit && level.Coins.Count == 0 && levelIndex == 3)
            {
                _spriteBatch.Draw(done, center - winSize / 2, Color.White);
                mediaHandler.Stop();
            }
        }
    }
}
