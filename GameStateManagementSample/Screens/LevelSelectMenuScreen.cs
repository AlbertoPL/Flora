#region File Description
//-----------------------------------------------------------------------------
// OptionsMenuScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using Microsoft.Xna.Framework;
using System;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// The options screen is brought up over the top of the main menu
    /// screen, and gives the user a chance to configure the game
    /// in various hopefully useful ways.
    /// </summary>
    class LevelSelectMenuScreen : MenuScreen
    {
        #region Fields

        MenuEntry level1;
        MenuEntry level2;
        MenuEntry level3;
        MenuEntry level4;
        MenuEntry level5;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public LevelSelectMenuScreen()
            : base("")
        {
            // Create our menu entries.
            level1 = new MenuEntry("Level 1");
            level2 = new MenuEntry("Level 2");
            level3 = new MenuEntry("Level 3");
            level4 = new MenuEntry("Level 4");
            level5 = new MenuEntry("Level 5");

            MenuEntry back = new MenuEntry("Back");

            // Hook up menu event handlers.
            level1.Selected += LevelOneMenuEntrySelected;
            level2.Selected += LevelTwoMenuEntrySelected;
            level3.Selected += LevelThreeMenuEntrySelected;
            level4.Selected += LevelFourMenuEntrySelected;
            level5.Selected += LevelFiveMenuEntrySelected;
            back.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(level1);
            MenuEntries.Add(level2);
            MenuEntries.Add(level3);
            MenuEntries.Add(level4);
            MenuEntries.Add(level5);
            MenuEntries.Add(back);
        }

        #endregion

        #region Handle Input


        /// <summary>
        /// Event handler for when the Level One menu entry is selected.
        /// </summary>
        void LevelOneMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LevelLoadingScreen.Load(ScreenManager, true, e.PlayerIndex, 1, new GameplayScreen(1));
        }

        /// <summary>
        /// Event handler for when the Level One menu entry is selected.
        /// </summary>
        void LevelTwoMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LevelLoadingScreen.Load(ScreenManager, true, e.PlayerIndex, 2, new GameplayScreen(2));
        }

        /// <summary>
        /// Event handler for when the Level One menu entry is selected.
        /// </summary>
        void LevelThreeMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LevelLoadingScreen.Load(ScreenManager, true, e.PlayerIndex, 3, new GameplayScreen(3));
        }

        /// <summary>
        /// Event handler for when the Level One menu entry is selected.
        /// </summary>
        void LevelFourMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LevelLoadingScreen.Load(ScreenManager, true, e.PlayerIndex, 4, new GameplayScreen(4));
        }

        /// <summary>
        /// Event handler for when the Level One menu entry is selected.
        /// </summary>
        void LevelFiveMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LevelLoadingScreen.Load(ScreenManager, true, e.PlayerIndex, 5, new GameplayScreen(5));
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
            Vector2 position = new Vector2(0f, 300f);

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