#region File Description
//-----------------------------------------------------------------------------
// MainMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
using System;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The main menu screen is the first thing displayed when the game starts up.
    /// </summary>
    class MainMenuScreen : MenuScreen
    {

        #region Fields
        ContentManager content;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor fills in the menu contents.
        /// </summary>
        public MainMenuScreen()
            : base("")
        {
            //Stop previously playing music
            MediaPlayer.Stop();

            // Create our menu entries.
            MenuEntry playGameMenuEntry = new MenuEntry("Play Game");
            MenuEntry levelSelectMenuEntry = new MenuEntry("Level Select");
            MenuEntry optionsMenuEntry = new MenuEntry("Options");
            MenuEntry creditsMenuEntry = new MenuEntry("Credits");
            MenuEntry exitMenuEntry = new MenuEntry("Exit");
            MenuEntry instructionMenuEntry = new MenuEntry("Instructions");

            // Hook up menu event handlers.
            playGameMenuEntry.Selected += PlayGameMenuEntrySelected;
            levelSelectMenuEntry.Selected += LevelSelectMenuEntrySelected;
            optionsMenuEntry.Selected += OptionsMenuEntrySelected;
            creditsMenuEntry.Selected += CreditsMenuEntrySelected;
            exitMenuEntry.Selected += OnCancel;
            instructionMenuEntry.Selected += InstructionMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(playGameMenuEntry);
            MenuEntries.Add(instructionMenuEntry);
            MenuEntries.Add(levelSelectMenuEntry);
            MenuEntries.Add(creditsMenuEntry);
            MenuEntries.Add(exitMenuEntry);
        }

        #endregion

        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            base.LoadContent();
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            Song mainMenuSong = content.Load<Song>("Sound/MainMenuSong");

            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = .75f; //Otherwise the sound will drive us nuts while testing
                MediaPlayer.Play(mainMenuSong);
            }
            catch { }

            // once the load has finished, we use ResetElapsedTime to tell the game's
            // timing mechanism that we have just finished a very long frame, and that
            // it should not try to catch up.
            ScreenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void PlayGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LevelLoadingScreen.Load(ScreenManager, true, e.PlayerIndex, 1, new GameplayScreen(1));
        }


        /// <summary>
        /// Event handler for when the Options menu entry is selected.
        /// </summary>
        void OptionsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new OptionsMenuScreen(), e.PlayerIndex);
        }

        void InstructionMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            
            ScreenManager.RemoveScreen(this);
            LoadingScreen.Load(ScreenManager, false, e.PlayerIndex, new InstructionScreen(1));
            this.ExitScreen();
        }

        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void LevelSelectMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new LevelSelectMenuScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// Event handler for when the Elf menu entry is selected.
        /// </summary>
        void CreditsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new CreditsScreen(), e.PlayerIndex);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit the game?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Allows the screen the chance to position the menu entries. By default
        /// all menu entries are lined up in a vertical list, centered on the screen.
        /// </summary>
        protected override void UpdateMenuEntryLocations()
        {
            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            // start at Y = 175; each X value is generated per entry
            Vector2 position = new Vector2(0f, 400f);

            // update each menu entry's location in turn
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                // each entry is to be centered horizontally
                position.X = Resolution.GetVirtualResolution().Width / 10;

                if (ScreenState == ScreenState.TransitionOn)
                    position.X -= transitionOffset * 256;
                else
                    position.X += transitionOffset * 512;

                // set the entry's position
                menuEntry.Position = position;

                // move down for the next entry the size of this entry
                position.Y += menuEntry.GetHeight(this);
            }
        }

        #endregion
    }
}
