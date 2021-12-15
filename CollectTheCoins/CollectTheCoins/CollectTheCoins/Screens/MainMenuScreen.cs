/* MainMenuScreen.cs
 * Received From: Nathan Bean tutorial
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using CollectTheCoins.StateManagement;
using Microsoft.Xna.Framework.Graphics;

namespace CollectTheCoins.Screens
{
    // The main menu screen is the first thing displayed when the game starts up.
    public class MainMenuScreen : MenuScreen
    {
        GameServiceContainer _services;

        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="gameService">the services</param>
        public MainMenuScreen(GameServiceContainer gameService) : base("Main Menu")
        {
            _services = gameService;

            var playGameMenuEntry = new MenuEntry("Play Game");
            var RemarksMenuEntry = new MenuEntry("Remarks");
            var exitMenuEntry = new MenuEntry("Exit");

            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            RemarksMenuEntry.Selected += RemarksMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;

            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(RemarksMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        /// <summary>
        /// Method for when play game is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayGameMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(_services));
        }

        /// <summary>
        /// Method when remarks menu entry is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemarksMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the player wants to exit
        /// </summary>
        /// <param name="playerIndex"></param>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit?";
            var confirmExitMessageBox = new MessageBoxScreen(message, false);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }

        /// <summary>
        /// Method for when the player accepts exit confirmation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmExitMessageBoxAccepted(object sender, PlayerEventIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }
    }
}
