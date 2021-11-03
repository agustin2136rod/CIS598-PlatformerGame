/* Game1.cs
 * Author: Agustin Rodriguez 
 */
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Content;
using CollectTheCoins.Handlers;
using System;
using System.IO;

namespace CollectTheCoins
{
    /// <summary>
    /// This class deals with the mechanics of starting and running the game
    /// </summary>
    public class Game1 : Game
    {
        //declare all variables that will be used within this game. 
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        Vector2 screenSize = new Vector2(800, 480);
        private Matrix globalTransformation;
        int bufferWidth, bufferHeight;
        private SpriteFont font;
        private int levelIndex = 0;
        private LevelHandler level;
        private bool continuePressed;
        private Texture2D win;
        private Texture2D instructions;
        private Texture2D fail;
        private Texture2D done;
        private Song backgroundMusic;
        private KeyboardState keyboardState;
        private const int numberOfLevels = 4;
        private bool seenInstructions = false;

        /// <summary>
        /// Constructor for the class
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            IsMouseVisible = false;
            //Accelerometer.Initialize();
        }

        /// <summary>
        /// Not used
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// Method to Load the content that is utilized within the game
        /// </summary>
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here

            font = Content.Load<SpriteFont>("arial");
            win = Content.Load<Texture2D>("Winner");
            instructions = Content.Load<Texture2D>("Instructions");
            fail = Content.Load<Texture2D>("fail");
            done = Content.Load<Texture2D>("Done");
            ScalePresentation();
            backgroundMusic = Content.Load<Song>("sounds/BoxCat Games - Epic Song");
            MediaPlayer.Volume -= 0.7f;
            MediaPlayer.Play(backgroundMusic);
            MediaPlayer.IsRepeating = true;
            LoadLevel();
        }

        /// <summary>
        /// Method to scale the presentation window for the game. It adjusts to your screen size and sets it accordingly
        /// </summary>
        public void ScalePresentation()
        {
            bufferWidth = GraphicsDevice.PresentationParameters.BackBufferWidth;
            bufferHeight = GraphicsDevice.PresentationParameters.BackBufferHeight;
            float horScaling = bufferWidth / screenSize.X;
            float verScaling = bufferHeight / screenSize.Y;
            Vector3 screenScalingFactor = new Vector3(horScaling, verScaling, 1);
            globalTransformation = Matrix.CreateScale(screenScalingFactor);
        }

        /// <summary>
        /// Method to update the game as it is being played. 
        /// </summary>
        /// <param name="gameTime">elapsed time for the game</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            HandleInput(gameTime);

            level.Update(gameTime, keyboardState, Window.CurrentOrientation);

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// Method to handle input from the player 
        /// </summary>
        /// <param name="gameTime">elapsed time for the game</param>
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
                level = new LevelHandler(Services, fileStream, levelIndex);
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

        /// <summary>
        /// Method to draw the content on the gameplay screen
        /// </summary>
        /// <param name="gameTime">the elapsed time of the game</param>
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

        /// <summary>
        /// Secondary method of drawing
        /// </summary>
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

            if (level.AtExit && level.Coins.Count == 0 && levelIndex != 3)
            {
                _spriteBatch.Draw(win, center - winSize / 2, Color.White);
                MediaPlayer.Stop();
            }

            if (level.TimeLeft == TimeSpan.Zero && !level.AtExit)
            {
                _spriteBatch.Draw(fail, center - winSize / 2, Color.White);
                MediaPlayer.Stop();
            }

            if (level.AtExit && level.Coins.Count == 0 && levelIndex == 3)
            {
                _spriteBatch.Draw(done, center - winSize / 2, Color.White);
                MediaPlayer.Stop();
            }
        }
    }
}
