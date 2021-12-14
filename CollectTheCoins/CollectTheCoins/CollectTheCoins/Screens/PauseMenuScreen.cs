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
        private VolumeHandler _volume;
        private GameServiceContainer _services;
        private readonly MenuEntry _volumeMenuEntry;
        private const string instructionsForGame = "Use the Arrow or 'WASD' keys to move \nthe player around. " +
            "\nPressing Up, 'W', or 'Space' key will make the \nplayer jump. The objective is to " +
            "\ncollect all the coins, avoid the obstacles, \nand reach the exit sign before time expires.\n";

        public PauseMenuScreen(GameServiceContainer gameService, VolumeHandler volume) : base("Paused")
        {
            _services = gameService;
            _volume = volume;

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

        private void QuitGameMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message, false);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        private void SeeInstructionsGameMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            var confirmSeenInstructionsMessageBox = new MessageBoxScreen(instructionsForGame, true);

            confirmSeenInstructionsMessageBox.Accepted += OnCancel;

            ScreenManager.AddScreen(confirmSeenInstructionsMessageBox, ControllingPlayer);
        }

        private void RestartGameMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            const string message = "Are you sure you want to restart this game?";
            var confirmRestartMessageBox = new MessageBoxScreen(message, false);

            confirmRestartMessageBox.Accepted += ConfirmRestartMessageBoxAccepted;

            ScreenManager.AddScreen(confirmRestartMessageBox, ControllingPlayer);
        }

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

        private void ChangeVolumeMenuEntrySelected(object sender, PlayerEventIndexEventArgs e)
        {
            _volume.IncrementVolume();
            SetVolumeEntryText();
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerEventIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen(_services));
        }

        private void ConfirmRestartMessageBoxAccepted(object sender, PlayerEventIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, PlayerIndex.One, new GameplayScreen(_services));
        }
    }
}
