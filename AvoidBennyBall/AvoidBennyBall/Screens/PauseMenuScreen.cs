using AvoidBennyBall.StateManagement;
using AvoidBennyBall.Screens;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace AvoidBennyBall.Screens
{
    // The pause menu comes up over the top of the game,
    // giving the player options to resume or quit.
    public class PauseMenuScreen : MenuScreen
    {

        public PauseMenuScreen() : base("Paused")
        {
            var resumeGameMenuEntry = new MenuEntry("Resume Game");
            var quitGameMenuEntry = new MenuEntry("Quit Game");
            var restartGameMenuEntry = new MenuEntry("Restart Game");
            var adjustSoundEffectsEntry = new MenuEntry("Increase Sound Effects Volume");
            var adjustVolumeEntry = new MenuEntry("Increase Game Volume");


            resumeGameMenuEntry.Selected += OnCancel;
            quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;
            restartGameMenuEntry.Selected += RestartGameMenuEntrySelected;
            adjustSoundEffectsEntry.Selected += AdjustSoundEffectsEntrySelected;
            adjustVolumeEntry.Selected += AdjustVolumeEntrySelected;

            MenuEntries.Add(resumeGameMenuEntry);
            MenuEntries.Add(adjustVolumeEntry);
            MenuEntries.Add(adjustSoundEffectsEntry);
            MenuEntries.Add(restartGameMenuEntry);
            MenuEntries.Add(quitGameMenuEntry);
        }

        private void AdjustSoundEffectsEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            SoundEffect.MasterVolume++;
        }

        private void AdjustVolumeEntrySelected(object sender, PlayerIndexEventArgs e)
        {

            MediaPlayer.Volume++;
        }

        private void RestartGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to restart this game?";
            var confirmRestartMessageBox = new MessageBoxScreen(message);

            confirmRestartMessageBox.Accepted += ConfirmRestartMessageBoxAccepted;

            ScreenManager.AddScreen(confirmRestartMessageBox, ControllingPlayer);
        }

        private void ConfirmRestartMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GameplayScreen(), new CountDownScreen());
        }


        private void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";
            var confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        // This uses the loading screen to transition from the game back to the main menu screen.
        private void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new MainMenuScreen());
        }
    }
}
