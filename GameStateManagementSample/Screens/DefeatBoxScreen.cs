#region File Description
//-----------------------------------------------------------------------------
// MessageBoxScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

#endregion

namespace GameStateManagement
{
    /// <summary>
    /// A popup message box screen, used to display "are you sure?"
    /// confirmation messages.
    /// </summary>
    class DefeatBoxScreen : GameScreen
    {
        #region Fields

        Texture2D dialogueBoxTexture;

        const String message = "Press space or enter to retry.";

        GameplayScreen gameplayScreen;

        int currentLevel;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public DefeatBoxScreen(GameplayScreen gameplayScreen, int currentLevel)
            : this(false)
        { 
            this.gameplayScreen = gameplayScreen;
            this.currentLevel = currentLevel;
        }


        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public DefeatBoxScreen(bool includeUsageText)
        {
            
            //const string usageText = "\nA button, Space, Enter = next page";
            IsPopup = true;

            TransitionOnTime = TimeSpan.FromSeconds(0.2);
            TransitionOffTime = TimeSpan.FromSeconds(0.2);
        }


        /// <summary>
        /// Loads graphics content for this screen. This uses the shared ContentManager
        /// provided by the Game class, so the content will remain loaded forever.
        /// Whenever a subsequent MessageBoxScreen tries to load this same content,
        /// it will just get back another reference to the already loaded data.
        /// </summary>
        public override void LoadContent()
        {
            ContentManager content = ScreenManager.Game.Content;

            dialogueBoxTexture = content.Load<Texture2D>("DialogueBox");
        }


        #endregion

        #region Handle Input


        /// <summary>
        /// Responds to user input, accepting or cancelling the message box.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            

            // We pass in our ControllingPlayer, which may either be null (to
            // accept input from any player) or a specific index. If we pass a null
            // controlling player, the InputState helper returns to us which player
            // actually provided the input. We pass that through to our Accepted and
            // Cancelled events, so they can tell which player triggered them.
            KeyboardState keyboardState = input.CurrentKeyboardStates[1];
            GamePadState gamePadState = input.CurrentGamePadStates[1];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[1];

            if (input.IsNewKeyPress(Keys.Space) || input.IsNewKeyPress(Keys.Enter) || input.IsNewButtonPress(Buttons.A))
            {
                ScreenManager.RemoveScreen(gameplayScreen);
                LevelLoadingScreen.Load(ScreenManager, true, null, currentLevel, new GameplayScreen(currentLevel));
                ExitScreen();     
            }
        }


        #endregion

        #region Draw


        /// <summary>
        /// Draws the message box.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.DialogFont;

            // Center the message text in the viewport.
            Rectangle viewport = Resolution.GetVirtualResolution();
            Vector2 viewportSize = new Vector2(viewport.Width, viewport.Height);
            Vector2 textSize = font.MeasureString(message);
            Vector2 textPosition = (viewportSize - textSize) / 2;

            textPosition.X = viewport.Width / 2 - dialogueBoxTexture.Width / 2 + dialogueBoxTexture.Width * .01f;
            textPosition.Y = viewport.Height * .75f + dialogueBoxTexture.Height * .1f;

            // The background includes a border somewhat larger than the text itself.

            Rectangle backgroundRectangle = new Rectangle((int)(textPosition.X - dialogueBoxTexture.Width * .01f),
                                                          (int)(textPosition.Y - dialogueBoxTexture.Height * .1f),
                                                          dialogueBoxTexture.Width,
                                                          dialogueBoxTexture.Height);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());

            // Draw the background rectangle.
            spriteBatch.Draw(dialogueBoxTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, message, textPosition, color);

            spriteBatch.End();
        }


        #endregion
    }
}