#region File Description
//-----------------------------------------------------------------------------
// GameplayScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class GameplayScreen : GameScreen
    {
        #region Fields

        WaterScreen waterScreen;
        FireScreen fireScreen;
        ForestScreen forestScreen;

        //Bucket player;

        ContentManager content;
        SpriteFont gameFont;
        Texture2D background;

        MiniGameScreen left;
        MiniGameScreen center;
        MiniGameScreen right;

        float pauseAlpha;
        int currentLevel;
        float currentWinLoseDelay;
        const float maxWlnLoseDelay = 2000;
        bool isWonOrLost;
        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public GameplayScreen(int pCurrentLevel)
        {
            MediaPlayer.Stop();
            TransitionOnTime = TimeSpan.FromSeconds(1.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
            waterScreen = new WaterScreen(GameScreenPosition.Center);
            fireScreen = new FireScreen(GameScreenPosition.Left);
            forestScreen = new ForestScreen(GameScreenPosition.Right);
            currentLevel = pCurrentLevel;
            left = fireScreen;
            center = waterScreen;
            right = forestScreen;
            currentWinLoseDelay = 0;
            isWonOrLost = false;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

            gameFont = content.Load<SpriteFont>("gamefont");
            background = content.Load<Texture2D>("Background/GameplayScreenBackground");
            Level.LoadContent(content, ScreenManager);
            Level.changeLevel(currentLevel);
            waterScreen.LoadContent(content, ScreenManager);
            fireScreen.LoadContent(content, ScreenManager);
            forestScreen.LoadContent(content, ScreenManager);

            Song gameplaySong = content.Load<Song>("Sound/GameplaySong");

            try
            {
                MediaPlayer.IsRepeating = true;
                MediaPlayer.Volume = .75f; //Otherwise the sound will drive us nuts while testing
                MediaPlayer.Play(gameplaySong);
            }
            catch { }
            Thread.Sleep(750);
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


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);

            // Gradually fade in or out depending on whether we are covered by the pause screen.
            if (coveredByOtherScreen)
                pauseAlpha = Math.Min(pauseAlpha + 1f / 32, 1);
            else
            {
                if(isWonOrLost)
                    currentWinLoseDelay += gameTime.ElapsedGameTime.Milliseconds;
                pauseAlpha = Math.Max(pauseAlpha - 1f / 32, 0);
                if (!isWonOrLost)
                {
                    waterScreen.Update(gameTime);
                    fireScreen.Update(gameTime);
                    forestScreen.Update(gameTime);
                }
                if ((fireScreen.lost || forestScreen.won) && !isWonOrLost)
                {
                    isWonOrLost = true;
                }
                if (fireScreen.lost && currentWinLoseDelay >= maxWlnLoseDelay)
                {
                    isWonOrLost = false;
                    currentWinLoseDelay = 0;
                    ScreenManager.AddScreen(new DefeatScreen(this, currentLevel), ControllingPlayer);
                    MediaPlayer.Stop();
                }
                if (forestScreen.won && currentWinLoseDelay >= maxWlnLoseDelay)
                {
                    isWonOrLost = false;
                    currentWinLoseDelay = 0;
                    forestScreen.won = false;
                    ScreenManager.AddScreen(new VictoryScreen(currentLevel + 1), ControllingPlayer);
                    ExitScreen();
                }
            }

        }


        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public override void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            // Look up inputs for the active player profile.
            int playerIndex = (int)PlayerIndex.One;

            KeyboardState keyboardState = input.CurrentKeyboardStates[playerIndex];
            GamePadState gamePadState = input.CurrentGamePadStates[playerIndex];

            // The game pauses either if the user presses the pause button, or if
            // they unplug the active gamepad. This requires us to keep track of
            // whether a gamepad was ever plugged in, because we don't want to pause
            // on PC if they are playing with a keyboard and have no gamepad at all!
            bool gamePadDisconnected = !gamePadState.IsConnected &&
                                       input.GamePadWasConnected[playerIndex];

            if (input.IsPauseGame(ControllingPlayer) || gamePadDisconnected)
            {
                ScreenManager.AddScreen(new PauseMenuScreen(), ControllingPlayer);
            }
            else if (input.IsNewKeyPress(Keys.A) || input.IsNewButtonPress(Buttons.X))
            {    
                ChangeScreenPositions(right, left, center);
            }
            else if ( input.IsNewKeyPress( Keys.D ) || input.IsNewButtonPress( Buttons.Y ) )
            {

                ChangeScreenPositions( center, right, left );
            }
            else
            {
                if ( waterScreen.IsActive() )
                    waterScreen.HandleInput( input );
                else if ( fireScreen.IsActive() )
                    fireScreen.HandleInput( input );
                else if ( forestScreen.IsActive() )
                    forestScreen.HandleInput( input );
            }
        }

        private void ChangeScreenPositions(MiniGameScreen toBeLeft, MiniGameScreen toBeCenter, MiniGameScreen toBeRight)
        {


            toBeLeft.ScreenPosition = GameScreenPosition.Left;
            left = toBeLeft;
            toBeCenter.ScreenPosition = GameScreenPosition.Center;
            center = toBeCenter;
            toBeRight.ScreenPosition = GameScreenPosition.Right;
            right = toBeRight;
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            // This game has a blue background. Why? Because!
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0, 0);

            // Our player and enemy are both actually just text strings.
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
            spriteBatch.Draw(background, Resolution.GetVirtualResolution(), Color.White);
            spriteBatch.End();

            DrawTheScreens(gameTime, spriteBatch);



            // If the game is transitioning on or off, fade it out to black.
            if (TransitionPosition > 0 || pauseAlpha > 0)
            {
                float alpha = MathHelper.Lerp(1f - TransitionAlpha, 1f, pauseAlpha / 2);

                ScreenManager.FadeBackBufferToBlack(alpha);
            }
        }

        private void DrawTheScreens(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //Draws inactive ones first so that the active one is on top of them
            if (!waterScreen.IsActive())
                waterScreen.Draw(gameTime, spriteBatch);
            if (!fireScreen.IsActive())
                fireScreen.Draw(gameTime, spriteBatch);
            if (!forestScreen.IsActive())
                forestScreen.Draw(gameTime, spriteBatch);

            //Draw the active one
            if (waterScreen.IsActive())
                waterScreen.Draw(gameTime, spriteBatch);
            else if (fireScreen.IsActive())
                fireScreen.Draw(gameTime, spriteBatch);
            else if (forestScreen.IsActive())
                forestScreen.Draw(gameTime, spriteBatch);

        }


        #endregion
    }
}