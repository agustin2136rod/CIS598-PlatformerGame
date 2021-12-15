/* PauseMenuScreen.cs
 * Received From: Nathan Bean tutorial 
 */
using System;
using System.Collections.Generic;
using System.Text;
using CollectTheCoins.Handlers;
using CollectTheCoins.StateManagement;
using Microsoft.Xna.Framework;

namespace CollectTheCoins.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class PauseMenuScreen : MenuScreen
    {
        //private variables used in the class
        private VolumeHandler _volume;
        private GameServiceContainer _services;
        private MediaHandler _media;
        private readonly MenuEntry _volumeMenuEntry;
        private const string instructionsForGame = "Use the Arrow or 'WASD' keys to move \nthe player around. " +
            "\nPressing Up, 'W', or 'Space' key will make the \nplayer jump. The objective is to " +
            "\ncollect all the coins, avoid the obstacles, \nand reach the exit sign before time expires.\n";

        /// <summary>
        /// Constructor for the class
        /// </summary>
        /// <param name="gameService">the services</param>
        /// <param name="volume">volume handler for the game</param>
        /// <param name="media">media handler for the game</param>
        public PauseMenuScreen(GameServiceContainer gameService, VolumeHandler volume, MediaHandler media) : base("Paused")
        {
            _services = gameService;
            _volume = volume;
            _media = media;

            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");
            _volumeMenuEntry = new MenuEntry(string.Empty);
            var restartGameMenuEntry = new MenuEntry("Restart Game");
            var seeInstructionsGameMenuEntry = new MenuEntry("Instructions");

            SetVolumeEntryText();

            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            _volumeMenuEntry.Selected += ChangeVolumeMenuEntrySelected;
            restartGameMenuEntry.Selected += RestartGameMenuEntrySelected;
            seeInstructionsGameMenuEntry.Selected += SeeInstructionsGameMenuEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(seeInstructionsGameMenuEntry);
            MenuEntries.Add(_volumeMenuEntry);
            MenuEntries.Add(restartGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        /// <summary>
        /// Method for when 'quit game' is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void QuitGameMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message, false);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Method for when 'see instructions' menu entry is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SeeInstructionsGameMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            var confirmSeenInstructionsMessageBox = new MessageBoxScreen(instructionsForGame, true);

            confirmSeenInstructionsMessageBox.Accepted += OnCancel;

            ScreenManager.AddScreen(confirmSeenInstructionsMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Method for when 'restart game' menu entry is selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestartGameMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            const string message = "Are you sure you want to restart this game?";
            var confirmRestartMessageBox = new MessageBoxScreen(message, false);

            confirmRestartMessageBox.Accepted += ConfirmRestartMessageBoxAccepted;

            ScreenManager.AddScreen(confirmRestartMessageBox, ControllingPlayer);
        }

        /// <summary>
        /// Method to change the volume of the game
        /// </summary>
        private void SetVolumeEntryText()
        {
            if (_volume.Volume <= 1.0f)
            {
                _volumeMenuEntry.Text = $"Volume: {_volume.Volume}f";
            }
            else
            {
                _volumeMenuEntry.Text = $"Volume: 1.0f";
            }
        }

        /// <summary>
        /// Method for 'change volume' menu entry selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeVolumeMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            _volume.IncrementVolume();
            SetVolumeEntryText();
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerEventIndexEventArgs e)
        {
            _media.Stop();
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(_services));
        }

        /// <summary>
        /// Messagebox to ask for confirmation if the player wants to restart the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConfirmRestartMessageBoxAccepted(object sender, PlayerEventIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, new GameplayScreen(_services));
        }
    }
}
