#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
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
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class CreditsScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D backgroundTexture;
        Texture2D gentlemanCorn;
        List<String> listOfProgrammers = new List<string>();
        List<String> listOfArtists = new List<string>();
        List<String> listOfDesigners = new List<string>();
        List<String> listOfProducers = new List<string>();
        List<String> listOfSoundArtists = new List<string>();

        Vector2 position;
        Vector2 designersPosition;
        Vector2 programmersPosition;
        Vector2 artistsPosition;
        Vector2 soundPosition;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public CreditsScreen()
        {
            MediaPlayer.Stop(); 
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            backgroundTexture = content.Load<Texture2D>("Background/CreditsBackground");
            gentlemanCorn = content.Load<Texture2D>("Sprite/gentleman Corn");

            //Producers
            listOfProducers.Add("PRODUCER");
            //listOfProducers.Add("");
            listOfProducers.Add("Oliver Holmes");
            listOfProducers.Add("");

            //Designers
            listOfDesigners.Add("DESIGNERS");
            //listOfDesigners.Add("");
            listOfDesigners.Add("Kelvin Ho");
            listOfDesigners.Add("Michael Sarfati");
            listOfDesigners.Add("William Stone");
            listOfDesigners.Add("Ryan Torres");
            listOfDesigners.Add("Colin Wheelock");
            listOfDesigners.Add("");

            //Programmers
            listOfProgrammers.Add("PROGRAMMERS");
            //listOfProgrammers.Add("");
            listOfProgrammers.Add("Michael Cappe");
            listOfProgrammers.Add("Richard Creencia");
            listOfProgrammers.Add("Sen Hirano");
            listOfProgrammers.Add("Lillian Huang");
            listOfProgrammers.Add("Alejandro Larragueta");
            listOfProgrammers.Add("Alberto Pareja-Lecaros");
            listOfProgrammers.Add("Mason Phillips");
            listOfProgrammers.Add("");

            //Artists
            listOfArtists.Add("Artists");
            //listOfArtists.Add("");
            listOfArtists.Add("Oliver Holmes");
            listOfArtists.Add("Rachel Sala");
            listOfArtists.Add("");

            //Sound Artists
            listOfSoundArtists.Add("Sound");
           // listOfSoundArtists.Add("");
            listOfSoundArtists.Add("Bryan Ploof");
            listOfSoundArtists.Add("");

            position = new Vector2(gentlemanCorn.Width, Resolution.GetVirtualResolution().Height);
            designersPosition = new Vector2(gentlemanCorn.Width, position.Y + (ScreenManager.Font.LineSpacing * listOfProducers.Count));
            programmersPosition = new Vector2(gentlemanCorn.Width, designersPosition.Y + (ScreenManager.Font.LineSpacing * listOfDesigners.Count));
            artistsPosition = new Vector2(gentlemanCorn.Width, programmersPosition.Y + (ScreenManager.Font.LineSpacing * listOfProgrammers.Count));
            soundPosition = new Vector2(gentlemanCorn.Width, artistsPosition.Y + (ScreenManager.Font.LineSpacing * listOfArtists.Count));

            Song mainMenuSong = content.Load<Song>("Sound/MainMenuSong");

            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = .75f; //Otherwise the sound will drive us nuts while testing
                MediaPlayer.Play(mainMenuSong);
            }
            catch { }
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
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
                ScreenManager.AddScreen(new MainMenuScreen(), PlayerIndex.One);
                ExitScreen();
            }
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            position.Y--;
            designersPosition.Y--;
            programmersPosition.Y--;
            artistsPosition.Y--;
            soundPosition.Y--;
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            Rectangle viewport = Resolution.GetVirtualResolution();
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            
            spriteBatch.Draw(backgroundTexture, fullscreen, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));
            spriteBatch.Draw(gentlemanCorn, gentlemanCorn.Bounds, new Color(TransitionAlpha, TransitionAlpha, TransitionAlpha));

            //Producers
            for (int i = 0; i < listOfProducers.Count; i++)
            {
                spriteBatch.DrawString(ScreenManager.Font, listOfProducers[i], new Vector2(position.X, position.Y + (ScreenManager.Font.LineSpacing * i)), Color.DarkKhaki);
            }

            //Designers
            for (int i = 0; i < listOfDesigners.Count; i++)
            {
                spriteBatch.DrawString(ScreenManager.Font, listOfDesigners[i], new Vector2(designersPosition.X, designersPosition.Y + (ScreenManager.Font.LineSpacing * i)), Color.CadetBlue);
            }

            //Programmers
            for (int i = 0; i < listOfProgrammers.Count; i++)
            {
                spriteBatch.DrawString(ScreenManager.Font, listOfProgrammers[i], new Vector2(programmersPosition.X, programmersPosition.Y + (ScreenManager.Font.LineSpacing * i)), Color.ForestGreen);
            }

            //Artists
            for (int i = 0; i < listOfArtists.Count; i++)
            {
                spriteBatch.DrawString(ScreenManager.Font, listOfArtists[i], new Vector2(artistsPosition.X, artistsPosition.Y + (ScreenManager.Font.LineSpacing * i)), Color.IndianRed);
            }

            //Sound
            for (int i = 0; i < listOfSoundArtists.Count; i++)
            {
                spriteBatch.DrawString(ScreenManager.Font, listOfSoundArtists[i], new Vector2(soundPosition.X, soundPosition.Y + (ScreenManager.Font.LineSpacing * i)), Color.LightGray);
            }



            spriteBatch.End();
        }

       
        #endregion
    }
}
