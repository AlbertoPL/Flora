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
    class DialogueBoxScreen : GameScreen
    {
        #region Fields

        string message;
        Texture2D dialogBoxTexture;

        List<String> listOfMessages;
        int messageNum = 0;
        int screenNum;
        InstructionScreen prevScreen;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor automatically includes the standard "A=ok, B=cancel"
        /// usage text prompt.
        /// </summary>
        public DialogueBoxScreen(InstructionScreen prevScreen, List<String> message, int screenNumber, bool includeUsageText)
            : this(message, includeUsageText)
        { screenNum = screenNumber; this.prevScreen = prevScreen; }


        /// <summary>
        /// Constructor lets the caller specify whether to include the standard
        /// "A=ok, B=cancel" usage text prompt.
        /// </summary>
        public DialogueBoxScreen(List<String> message, bool includeUsageText)
        {
            listOfMessages = message;
            const string usageText = "\nSpace, Enter = next page";

            if (includeUsageText)
                this.message = listOfMessages[messageNum] + usageText;
            else
                this.message = listOfMessages[messageNum];

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

            dialogBoxTexture = content.Load<Texture2D>("DialogueBox");
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
                if (messageNum + 1 < listOfMessages.Count)
                {
                    messageNum++;
                    this.message = listOfMessages[messageNum];
                }
                else
                {
                    ScreenManager.RemoveScreen(prevScreen);
                    ScreenManager.AddScreen(new InstructionScreen(++screenNum), PlayerIndex.One);
                    ExitScreen();
                }
            }
            else if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
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

            textPosition.X = viewport.Width / 2 - dialogBoxTexture.Width / 2 + dialogBoxTexture.Width * .01f;
            textPosition.Y = viewport.Height * .75f + dialogBoxTexture.Height * .1f;

            // The background includes a border somewhat larger than the text itself.

            Rectangle backgroundRectangle = new Rectangle((int)(textPosition.X - dialogBoxTexture.Width * .01f),
                                                          (int)(textPosition.Y - dialogBoxTexture.Height * .1f),
                                                          dialogBoxTexture.Width,
                                                          dialogBoxTexture.Height);

            // Fade the popup alpha during transitions.
            Color color = Color.White * TransitionAlpha;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());

            // Draw the background rectangle.
            spriteBatch.Draw(dialogBoxTexture, backgroundRectangle, color);

            // Draw the message box text.
            spriteBatch.DrawString(font, message, textPosition, color);

            spriteBatch.End();
        }


        #endregion
    }
}