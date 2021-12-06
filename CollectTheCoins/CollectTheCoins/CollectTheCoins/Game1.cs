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
using CollectTheCoins.StateManagement;
using CollectTheCoins.Screens;

namespace CollectTheCoins
{
    /// <summary>
    /// This class deals with the mechanics of starting and running the game
    /// </summary>
    public class Game1 : Game
    {
        //declare all variables that will be used within this game. 
        private GraphicsDeviceManager _graphics;
        private readonly ScreenManager _screenManager;


        /// <summary>
        /// Constructor for the class
        /// </summary>
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            Content.RootDirectory = "Content";
            _graphics.SupportedOrientations = DisplayOrientation.LandscapeLeft | DisplayOrientation.LandscapeRight;
            IsMouseVisible = true;

            var screenFactory = new ScreenFactory();
            Services.AddService(typeof(IScreenFactory), screenFactory);

            _screenManager = new ScreenManager(this);
            Components.Add(_screenManager);

            AddInitializeScreens();
        }

        private void AddInitializeScreens()
        {
            _screenManager.AddScreen(new BackgroundScreen(), null);
            _screenManager.AddScreen(new MainMenuScreen(Services), null);
        }

        /// <summary>
        /// Not used
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// Method to Load the content that is utilized within the game
        /// </summary>
        protected override void LoadContent() { }

        /// <summary>
        /// Method to update the game as it is being played. 
        /// </summary>
        /// <param name="gameTime">elapsed time for the game</param>
        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        /// <summary>
        /// Method to draw the content on the gameplay screen
        /// </summary>
        /// <param name="gameTime">the elapsed time of the game</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }

        
    }
}
