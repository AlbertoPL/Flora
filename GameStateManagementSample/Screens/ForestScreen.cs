﻿#region Using Statements
using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class ForestScreen : MiniGameScreen
    {
        #region Fields

        Texture2D background;
        Texture2D seedHealthyTexture;
        Texture2D seedDamagedTexture;
        Texture2D seedMoreDamagedTexture;
        Texture2D treeTexture;

        Texture2D ballNormal;
        Texture2D ballFireGood;
        Texture2D ballTreeGood;
        List<Ball> balls;

        SpriteFont gameFont;

        Bucket bucket;

        SoundEffect treeSound;
        SoundEffect treeSpreadSound;
        SoundEffect shootWaterSound;
        SoundEffect notEnoughWaterSound;
        SoundEffect victorySound;

        TreeObject[,] treeGrid;
        double lastTreeSound = 0;
        int numberOfTrees = 1;
        public bool won = false;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public ForestScreen(GameScreenPosition gameScreenPosition)
        {
            ScreenPosition = gameScreenPosition;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public void LoadContent(ContentManager content, ScreenManager screenManager)
        {
            gameFont = content.Load<SpriteFont>("gamefont");
            background = content.Load<Texture2D>("Background/ForestScreenBackground");
            treeTexture = content.Load<Texture2D>("Sprite/TreeObject");
            seedHealthyTexture = content.Load<Texture2D>("Sprite/SeedObject");
            seedDamagedTexture = content.Load<Texture2D>("Sprite/DamagedSeed");
            seedMoreDamagedTexture = content.Load<Texture2D>("Sprite/MoreDamagedSeed");

            treeSound = content.Load<SoundEffect>("Sound/TreeSpread");
            treeSpreadSound = content.Load<SoundEffect>("Sound/TreeSpread");
            shootWaterSound = content.Load<SoundEffect>("Sound/ShootWater");
            notEnoughWaterSound = content.Load<SoundEffect>("Sound/Error_1");
            victorySound = content.Load<SoundEffect>("Sound/Victory");

            ballNormal = content.Load<Texture2D>("Sprite/NormalWater");
            ballFireGood = content.Load<Texture2D>("Sprite/FireGoodWater");
            ballTreeGood = content.Load<Texture2D>("Sprite/TreeGoodWater");

            // Initialize bucket
            bucket = new Bucket(Resolution.GetMiniGameResolution());
            balls = new List<Ball>();

            int gridWidth = Resolution.GetMiniGameResolution().Width / treeTexture.Width;
            int gridHeight = (int)(Resolution.GetMiniGameResolution().Height * .75 / treeTexture.Height);
            treeGrid = new TreeObject[gridWidth, gridHeight];
            treeGrid[gridWidth / 2 - 1, gridHeight / 2 - 1] = new TreeObject(TreeObjectType.Tree, treeTexture, GetTreeObjectPosition(gridWidth / 2 - 1, gridHeight / 2 - 1));
            Spread(gridWidth / 2 - 1, gridHeight / 2 - 1, false);
            screenManager.Game.ResetElapsedTime();
        }


        /// <summary>
        /// Unload graphics content used by the game.
        /// </summary>
        public void UnloadContent()
        {
        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the state of the game. This method checks the GameScreen.IsActive
        /// property, so the game will stop updating when the pause menu is active,
        /// or if you tab away to a different application.
        /// </summary>
        public void Update(GameTime gameTime)
        {

            for (int i = 0; i < balls.Count; i++)
            {
                balls[i].Update(gameTime);
                if (!balls[i].IsAlive())
                {
                    balls.RemoveAt(i);
                    i--;
                }
            }
            HandleTreeCollisions(gameTime.TotalGameTime.TotalSeconds);

            if (IsActive())
            {
                for (int i = 0; i < balls.Count; i++)
                {
                    balls[i].PaddleCollision(bucket);
                }
            }
            if (IsActive())
                bucket.Update(gameTime);
            UpdateTreeObjects(gameTime);
            removeStoppedDrops();

            if ( numberOfTrees >= Level.TreesToWin )
            {
                OnWin();
            }
        }

        private void HandleTreeCollisions(double gameSeconds)
        {
            numberOfTrees = 0;
            for (int i = 0; i < treeGrid.GetLength(0); i++)
            {
                for (int j = 0; j < treeGrid.GetLength(1); j++)
                {
                    if (IsValidTreeObjectPosition(i, j) && treeGrid[i, j] != null)
                    {
                        if(treeGrid[i, j].Type == TreeObjectType.Tree) numberOfTrees++;
                        HandleBallCollisions(i ,j, gameSeconds);
                    }
                }
            }
        }

        private void HandleBallCollisions(int x, int y, double gameSeconds)
        {
            for (int i = 0; i < balls.Count; i++)
            {
                if (treeGrid[x, y].Type == TreeObjectType.Seed)
                {
                    if (balls[i].CollidesWithObject(treeGrid[x, y].Bounds))
                    {
                        balls[i].LoseHealth();
                        treeGrid[x, y].OnCollision(balls[i].Type);
                        UpdateSeedTexture(treeGrid[x, y]);
                        if (gameSeconds > lastTreeSound && balls[i].Type != WaterType.FireGood)
                        {
                            lastTreeSound = gameSeconds + treeSound.Duration.TotalSeconds;
                            treeSound.Play();
                        }

                        if (!treeGrid[x, y].IsAlive())
                        {
                            treeGrid[x, y] = new TreeObject(TreeObjectType.Tree, treeTexture, GetTreeObjectPosition(x, y));
                            Spread(x, y);
                            balls.RemoveAt(i);
                            i--;
                        }
                    }
                }
            }
        }

        private void UpdateTreeObjects(GameTime gameTime)
        {
            for (int i = 0; i < treeGrid.GetLength(0); i++)
            {
                for (int j = 0; j < treeGrid.GetLength(1); j++)
                {
                    if (IsValidTreeObjectPosition(i, j) && treeGrid[i, j] != null)
                    {
                        
                        treeGrid[i, j].Update(gameTime);
                    }
                }
            }
        }

        private void UpdateSeedTexture(TreeObject treeobj)
        {
            if (Level.MaxTreeHealth == treeobj.GetHealth()) 
            {
                treeobj.SetTexture(seedHealthyTexture);    
            }
            else if (treeobj.GetHealth() <= Level.MaxTreeHealth / 2)
            {
                treeobj.SetTexture(seedMoreDamagedTexture);
            }
            else
            {
                treeobj.SetTexture(seedDamagedTexture);
            }
        }

        private bool IsValidTreeObjectPosition(int x, int y)
        {
            if (y % 2 == 1) //shorter row
                return (x >= 0 && x < treeGrid.GetLength(0) - 1 && y >= 0 && y < treeGrid.GetLength(1));
            else
                return (x >= 0 && x < treeGrid.GetLength(0) && y >= 0 && y < treeGrid.GetLength(1));
        }

        private Vector2 GetTreeObjectPosition(int x, int y)
        {
            if (y % 2 == 1) //shorter row
                return new Vector2(x * treeTexture.Width + (treeTexture.Width / 2), y * treeTexture.Height);
            else
                return new Vector2(x * treeTexture.Width, y * treeTexture.Height);
        }

        private void Spread(int x, int y)
        {
            Spread(x, y, true);
        }

        private void Spread(int x, int y, bool playSound)
        {
            if(playSound)
                treeSpreadSound.Play();
            AddTreeObject(x - 1, y);
            AddTreeObject(x + 1, y);
            if (y % 2 == 0) //longer row
                x--;
            AddTreeObject(x, y - 1);
            AddTreeObject(x, y + 1);
            AddTreeObject(x + 1, y - 1);
            AddTreeObject(x + 1, y + 1);
        }

        private void AddTreeObject(int x, int y)
        {
            if (IsValidTreeObjectPosition(x, y) && treeGrid[x, y] == null)
                treeGrid[x, y] = new TreeObject(TreeObjectType.Seed, seedHealthyTexture, GetTreeObjectPosition(x, y));
        }

        private void removeStoppedDrops()
        {
            for (int ball = 0; ball < balls.Count; ball++)
            {
                if (!balls[ball].IsMoving)
                {
                    balls.RemoveAt(ball);
                    ball--;
                }
            }
        }

        private void OnWin()
        {
            if (won == false)
            {
                MediaPlayer.Stop();
                won = true;
                //System.Console.WriteLine("WINNN");
                victorySound.Play();
                Bucket.ResetWaterCount();
                //screenManager.AddScreen(new GameWinMenuScreen(), ControllingPlayer);
                //Level Cleared
            }
        }

        /// <summary>
        /// Lets the game respond to player input. Unlike the Update method,
        /// this will only be called when the gameplay screen is active.
        /// </summary>
        public void HandleInput(InputState input)
        {
            if (input == null)
                throw new ArgumentNullException("input");

            KeyboardState keyboardState = input.CurrentKeyboardStates[0];
            GamePadState gamePadState = input.CurrentGamePadStates[0];

            bool gamePadDisconnected = !input.CurrentGamePadStates[0].IsConnected &&
                                       input.GamePadWasConnected[0];


            
                if (input.IsNewKeyPress(Keys.Space))
                {
                    if (balls.Count < Level.MaxBalls && bucket.CanShoot())
                    {
                        Ball thisBall = new Ball(GetBallTexture(), Bucket.Type, bucket.GetBounds(), Resolution.GetMiniGameResolution());
                        thisBall.SetInMotion(bucket);
                        balls.Add(thisBall);
                        bucket.DecreaseWaterCount(Ball.MaxHealth);
                        if (!bucket.CanShoot())
                        {
                            Bucket.Type = WaterType.Normal;
                        }
                        shootWaterSound.Play();
                    }
                    else
                    {
                        notEnoughWaterSound.Play();
                    }
                }
            
        }
        private Texture2D GetBallTexture()
        {
            if (Bucket.Type == WaterType.Normal)
            {
                return ballNormal;
            }
            else if (Bucket.Type == WaterType.FireGood)
            {
                return ballFireGood;
            }
            else
            {
                return ballTreeGood;
            }
        }

        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());

            spriteBatch.Draw(background, screen, Color.White);
            DrawTreeObjects(gameTime, spriteBatch);
            for (int i = 0; i < balls.Count; i++)
            {
                if (balls[i].IsMoving || IsActive())
                {
                    balls[i].Draw(spriteBatch, TranslatePosition(balls[i].Position), SpriteScale);
                    balls[i].DrawHealthCount(spriteBatch, TranslatePosition(balls[i].Position), SpriteScale, gameFont);
                }
            }
            if (IsActive()) //Draw bucket if current window is active
            {
                if (bucket.CanShoot() && balls.Count < Level.MaxBalls)
                {
                    Vector2 translatedPosition = TranslatePosition(new Vector2(Bucket.position.X + bucket.GetBounds().Width / 2 - GetBallTexture().Width/2, Bucket.position.Y - GetBallTexture().Height));
                    spriteBatch.Draw(GetBallTexture(), translatedPosition, GetBallTexture().Bounds, Color.White, 0.0f, Vector2.Zero, SpriteScale, SpriteEffects.None, 0.0f);
                }
                Vector2 translatedBucketPosition = TranslatePosition(Bucket.position);
                bucket.Draw(spriteBatch, translatedBucketPosition, SpriteScale);
                bucket.DrawWaterCount(spriteBatch, translatedBucketPosition, SpriteScale, gameFont);
            }

            DrawTreeCount(spriteBatch, TranslatePosition(new Vector2(0, Resolution.GetMiniGameResolution().Height)), SpriteScale, gameFont);
            DrawBallCount(spriteBatch, TranslatePosition(new Vector2(Resolution.GetMiniGameResolution().Width, Resolution.GetMiniGameResolution().Height)), SpriteScale.X * .5f);
            spriteBatch.End();
        }

        private void DrawTreeObjects(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (int i = 0; i < treeGrid.GetLength(0); i++)
            {
                for (int j = 0; j < treeGrid.GetLength(1); j++)
                {
                    if (treeGrid[i, j] != null)
                    {
                        treeGrid[i, j].Draw(spriteBatch, TranslatePosition(GetTreeObjectPosition(i, j)), SpriteScale);
                        treeGrid[i, j].DrawSeedCount(spriteBatch, TranslatePosition(GetTreeObjectPosition(i, j)), SpriteScale, gameFont);
                    }
                }
            }

        }

        private void DrawTreeCount(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale, SpriteFont font)
        {
            Vector2 adjustedPosition = new Vector2(translatedPosition.X, translatedPosition.Y);
            spriteBatch.DrawString(font, "Trees Needed: " + numberOfTrees + "/" + Level.TreesToWin, adjustedPosition, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        private void DrawBallCount(SpriteBatch spriteBatch, Vector2 translatedPostion, float scale)
        {
            float scaledWidth = ballNormal.Width * scale;
            int buffer = (int) ( scaledWidth / 2);
            translatedPostion.X -= ((Level.MaxBalls + 1) * (scaledWidth + buffer)) - (buffer / 2);
            translatedPostion.Y += ( (ballNormal.Height * scale) / 10);

            for (int i = 0; i < Level.MaxBalls; i++)
            {
                translatedPostion.X += (scaledWidth + buffer);
                if (i < Level.MaxBalls - balls.Count)
                {
                    switch (Bucket.Type)
                    {
                        case WaterType.Normal:
                            spriteBatch.Draw(ballNormal, translatedPostion, ballNormal.Bounds, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                            break;
                        case WaterType.FireGood:
                            spriteBatch.Draw(ballFireGood, translatedPostion, ballNormal.Bounds, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                            break;
                        case WaterType.TreeGood:
                            spriteBatch.Draw(ballTreeGood, translatedPostion, ballNormal.Bounds, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                            break;
                    }
                }
                else
                {
                    Color shading = new Color(0, 0, 0, 128);
                    spriteBatch.Draw(ballNormal, translatedPostion, ballNormal.Bounds, shading, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
                }
            }
       }


        #endregion
    }
}

