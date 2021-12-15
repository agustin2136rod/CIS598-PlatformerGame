/* OptionsMenuScreen.cs
 * Received From: Nathan Bean tutorial
 */
using System;
using System.Collections.Generic;
using System.Text;

namespace CollectTheCoins.Screens
{
    // The options screen is brought up over the top of the main menu
    // screen, and gives the user a chance to configure the game
    // in various hopefully useful ways.
    public class OptionsMenuScreen : MenuScreen
    {
        //set up private variables 
        private readonly MenuEntry _creator;
        private readonly MenuEntry _languageMenuEntry;
        private readonly MenuEntry _repo;

        private static readonly string Language = "C#";
        private static readonly string Repo = "https://github.com/agustin2136rod/CIS598-PlatformerGame";

        /// <summary>
        /// constructor for the class
        /// </summary>
        public OptionsMenuScreen() : base("Remarks")
        {
            _creator = new MenuEntry(string.Empty);
            _languageMenuEntry = new MenuEntry(string.Empty);
            _repo = new MenuEntry(string.Empty);

            SetMenuEntryText();

            var back = new MenuEntry("Back");

            back.Selected += OnCancel;

            MenuEntries.Add(_creator);
            MenuEntries.Add(_languageMenuEntry);
            MenuEntries.Add(_repo);
            MenuEntries.Add(back);
        }

        // Fills in the latest values for the options screen menu text.
        private void SetMenuEntryText()
        {
            _creator.Text = $"Game Creator: Agustin Rodriguez";
            _languageMenuEntry.Text = $"Game Programming Language: {Language}";
            _repo.Text = $"GitHub Repo: {Repo}";
        }
    }
}
