using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using CollectTheCoins.Handlers;
using Platformer2D;
using System;
using System.IO;

namespace CollectTheCoins
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Vector2 screenSize = new Vector2(800, 480);
        private Matrix globalTransformation;
        int bufferWidth, bufferHeight;

        private SpriteFont font;

        private int levelIndex = -1;
        private LevelHandler level;
        private bool continuePressed;
        private Texture2D win;
        private Texture2D instructions;
        private Texture2D fail;
        private Song backgroundMusic;

        private KeyboardState keyboardState;
        private const int numberOfLevels = 1;
        private bool seenInstructions = false;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            IsMouseVisible = false;
            //Accelerometer.Initialize();
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            font = Content.Load<SpriteFont>("arial");
            win = Content.Load<Texture2D>("Winner");
            instructions = Content.Load<Texture2D>("Instructions");
            fail = Content.Load<Texture2D>("fail");
            ScalePresentation();
            backgroundMusic = Content.Load<Song>("sounds/music");
            MediaPlayer.Play(backgroundMusic);
            LoadLevel();
        }

        public void ScalePresentation()
        {
            bufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            bufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            float horScaling = bufferWidth / screenSize.X;
            float verScaling = bufferHeight / screenSize.Y;
            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            HandleInput(gameTime);

            level.Update(gameTime, keyboardState, Window.CurrentOrientation);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        private void HandleInput(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            bool proceed = keyboardState.IsKeyDown(Keys.Space);
            if (proceed) seenInstructions = true;

            if (!continuePressed && proceed)
            {
                if (!level.Player.Alive)
                {
                    level.Start();
                    MediaPlayer.Play(backgroundMusic);
                }
                else if (level.TimeLeft == TimeSpan.Zero)
                {
                    if (level.AtExit)
                    {
                        LoadLevel();
                        MediaPlayer.Play(backgroundMusic);
                    }
                    else
                    {
                        ReloadCurrentLevel();
                    }

                }
            }

            continuePressed = proceed;
        }

        private void LoadLevel()
        {
            levelIndex = (levelIndex + 1) % numberOfLevels;
            if (level != null) level.Dispose();

            string path = string.Format("Content/Levels/{0}.txt", levelIndex);
            using (Stream fileStream = TitleContainer.OpenStream(path))
            {
                level = new LevelHandler(Services, fileStream, levelIndex);
            }
        }

        private void ReloadCurrentLevel()
        {
            --levelIndex;
            LoadLevel();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, globalTransformation);
            level.Draw(gameTime, _spriteBatch);
            DrawHud();
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawHud()
        {
            
            Rectangle title = GraphicsDevice.Viewport.TitleSafeArea;
            Vector2 hudSpot = new Vector2(title.X, title.Y);

            Vector2 center = new Vector2(screenSize.X / 2, screenSize.Y / 2);

            string time = "TIME: " + level.TimeLeft.Minutes.ToString("00") + ":" + level.TimeLeft.Seconds.ToString("00");
            Color colorOfTime = Color.Red;
            _spriteBatch.DrawString(font, time, hudSpot, colorOfTime);
            Vector2 winSize = new Vector2(win.Width, win.Height);
            if (!seenInstructions)
            {
                _spriteBatch.Draw(instructions, center - winSize / 2, Color.White);
            }

            if (level.AtExit && level.Coins.Count == 0)
            {
                _spriteBatch.Draw(win, center - winSize / 2, Color.White);
                MediaPlayer.Stop();
            }

            if (level.TimeLeft == TimeSpan.Zero && !level.AtExit)
            {
                _spriteBatch.Draw(fail, center - winSize / 2, Color.White);
                MediaPlayer.Stop();
            }
        }
    }
}
