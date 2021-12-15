/* MenuEntry.cs
 * Received From: Nathan Bean tutorial 
 */
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CollectTheCoins.StateManagement;

namespace CollectTheCoins.Screens
{
    public class MenuEntry
    {
        private string _text;
        private float _selectionFade;    // Entries transition out of the selection effect when they are deselected
        private Vector2 _position;    // This is set by the MenuScreen each frame in Update

        /// <summary>
        /// Text of the entry 
        /// </summary>
        public string Text
        {
            private get => _text;
            set => _text = value;
        }

        /// <summary>
        /// Position of the text
        /// </summary>
        public Vector2 Position
        {
            get => _position;
            set => _position = value;
        }

        public event EventHandler<PlayerEventIndexEventArgs> Selected;

        /// <summary>
        /// Method when an entry is selected
        /// </summary>
        /// <param name="playerIndex"></param>
        protected internal virtual void OnSelectEntry(PlayerIndex playerIndex)
        {
            Selected?.Invoke(this, new PlayerEventIndexEventArgs(playerIndex));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="text">text of the menu entry </param>
        public MenuEntry(string text)
        {
            _text = text;
        }

        /// <summary>
        /// Method to update the menu entries
        /// </summary>
        /// <param name="screen">Menu screen</param>
        /// <param name="isSelected">if the entry is selected</param>
        /// <param name="gameTime">game time progressions</param>
        public virtual void Update(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            // When the menu selection changes, entries gradually fade between
            // their selected and deselected appearance, rather than instantly
            // popping to the new state.
            float fadeSpeed = (float)gameTime.ElapsedGameTime.TotalSeconds * 4;

            if (isSelected)
                _selectionFade = Math.Min(_selectionFade + fadeSpeed, 1);
            else
                _selectionFade = Math.Max(_selectionFade - fadeSpeed, 0);
        }


        // This can be overridden to customize the appearance.
        public virtual void Draw(MenuScreen screen, bool isSelected, GameTime gameTime)
        {
            var color = isSelected ? Color.Yellow : Color.White;

            // Pulsate the size of the selected menu entry.
            double time = gameTime.TotalGameTime.TotalSeconds;
            float pulsate = (float)Math.Sin(time * 6) + 1;
            float scale = 1 + pulsate * 0.05f * _selectionFade;

            // Modify the alpha to fade text out during transitions.
            color *= screen.TransitionAlpha;

            // Draw text, centered on the middle of each line.
            var screenManager = screen.ScreenManager;
            var spriteBatch = screenManager.SpriteBatch;
            var font = screenManager.Font;

            var origin = new Vector2(0, font.LineSpacing / 2);

            spriteBatch.DrawString(font, _text, _position, color, 0,
                origin, scale, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Method to get the height of the screen
        /// </summary>
        /// <param name="screen">the screen</param>
        /// <returns>int height </returns>
        public virtual int GetHeight(MenuScreen screen)
        {
            return screen.ScreenManager.Font.LineSpacing;
        }

        /// <summary>
        /// Method to get the width of the screen
        /// </summary>
        /// <param name="screen">the screen</param>
        /// <returns>int width </returns>
        public virtual int GetWidth(MenuScreen screen)
        {
            return (int)screen.ScreenManager.Font.MeasureString(Text).X;
        }
    }
}
