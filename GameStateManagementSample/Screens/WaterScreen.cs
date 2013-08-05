﻿
#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
#endregion

namespace GameStateManagement
{
    /// <summary>
    /// This screen implements the actual game logic. It is just a
    /// placeholder to get the idea across: you'll probably want to
    /// put some more interesting gameplay in here!
    /// </summary>
    class WaterScreen : MiniGameScreen
    {
        #region Fields

        Texture2D background;
        SpriteFont gameFont;
        Texture2D normalWater;
        Texture2D fireGoodWater;
        Texture2D treeGoodWater;
        Texture2D rainbowWater;

        SoundEffect waterCollectSound;
        SoundEffect specialWaterCollectSound;
        SoundEffect rainbowWaterCollectSound;
        SoundEffect bucketFullSound;

        Texture2D normalBucketTexture;
        Texture2D fireGoodBucketTexture;
        Texture2D treeGoodBucketTexture;
        Bucket bucket;
        
        List<Drop> drops = new List<Drop>();
        
        float dropTime = 0.0f;

        Vector2 playerPosition = new Vector2(0, 0);
        Vector2 enemyPosition = new Vector2(20, 20);

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public WaterScreen(GameScreenPosition gameScreenPosition)
        {
            ScreenPosition = gameScreenPosition;
        }


        /// <summary>
        /// Load graphics content for the game.
        /// </summary>
        public void LoadContent(ContentManager content, ScreenManager screenManager)
        {
            gameFont = content.Load<SpriteFont>("gamefont");
            background = content.Load<Texture2D>("Background/WaterScreenBackGround");
            normalWater = content.Load<Texture2D>("Sprite/NormalWater");
            fireGoodWater = content.Load<Texture2D>("Sprite/FireGoodWater");
            treeGoodWater = content.Load<Texture2D>("Sprite/TreeGoodWater");
            rainbowWater = content.Load<Texture2D>("Sprite/RainbowWater");

            waterCollectSound = content.Load<SoundEffect>("Sound/WaterCollect");
            specialWaterCollectSound = content.Load<SoundEffect>("Sound/SpecialWaterCollect");
            rainbowWaterCollectSound = content.Load<SoundEffect>("Sound/RainbowDropCollect");
            bucketFullSound = content.Load<SoundEffect>("Sound/BucketFull");

            // Initialize bucket
            normalBucketTexture = content.Load<Texture2D>("Sprite/NormalBucket");
            fireGoodBucketTexture = content.Load<Texture2D>("Sprite/FireGoodBucket");
            treeGoodBucketTexture = content.Load<Texture2D>("Sprite/TreeGoodBucket");
            bucket = new Bucket(normalBucketTexture, Resolution.GetMiniGameResolution());

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
            UpdateBucketTextureAndType();
            if(IsActive())
                bucket.Update(gameTime);

            for (int thisDrop = 0; thisDrop < drops.Count; thisDrop++)
            {
                drops[thisDrop].Update(gameTime);

                // check for intersecting w/ player
                if (IsActive() && drops[thisDrop].GetBounds().Intersects(bucket.GetBounds()) )
                {
                    if (drops[thisDrop].Type == WaterType.Rainbow)
                        rainbowWaterCollectSound.Play();
                    else if ((Bucket.Type != WaterType.Normal && drops[thisDrop].Type == WaterType.Normal) || (Bucket.Type != WaterType.FireGood && drops[thisDrop].Type == WaterType.FireGood) || (Bucket.Type != WaterType.TreeGood && drops[thisDrop].Type == WaterType.TreeGood))
                    {
                        specialWaterCollectSound.Play();
                    }
                    else if (Bucket.waterCount >= Level.MaxWater)
                        bucketFullSound.Play();
                    else
                        waterCollectSound.Play();
                    
                    if (drops[thisDrop].Type != WaterType.Rainbow)
                    {
                        Bucket.Type = drops[thisDrop].Type;
                    }

                    bucket.IncreaseWaterCount(drops[thisDrop].GetDropValue());
                    drops.RemoveAt(thisDrop);
                    thisDrop--;
                }
                else if (TranslatePosition(drops[thisDrop].Position).Y + (drops[thisDrop].GetBounds().Height * screenScale.Y) >= TranslatePosition(new Vector2(0, Resolution.GetMiniGameResolution().Height)).Y)
                {
                    drops.RemoveAt(thisDrop);
                    thisDrop--;
                }
            }

            dropTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (dropTime >= Level.WaterDropRate)
            {
                Random rand = new Random((int)System.Diagnostics.Stopwatch.GetTimestamp());
                int dropChance = rand.Next(1, 25);
                if (dropChance >= 1 && dropChance <= 3)
                {
                    int chance = rand.Next(1, 11);
                    if (chance == 1)
                    {
                        drops.Add(new Drop(WaterType.Rainbow, rainbowWater));
                    }
                    else if (chance > 6 && Bucket.Type != WaterType.FireGood)
                    {
                        drops.Add(new Drop(WaterType.FireGood, fireGoodWater));
                    }
                    else if (chance <= 6 && Bucket.Type != WaterType.TreeGood)
                    {
                        drops.Add(new Drop(WaterType.TreeGood, treeGoodWater));
                    }
                    else
                    {
                        drops.Add(new Drop(WaterType.Normal, normalWater));
                    }
                }
                else if (dropChance >= 4 && dropChance <= 6)
                {
                    drops.Add(new Drop(WaterType.Normal, normalWater));
                }
                else
                {
                    switch (Bucket.Type)
                    {
                        case WaterType.Normal:
                            drops.Add(new Drop(WaterType.Normal, normalWater));
                            break;
                        case WaterType.FireGood:
                            drops.Add(new Drop(WaterType.FireGood, fireGoodWater));
                            break;
                        case WaterType.TreeGood:
                            drops.Add(new Drop(WaterType.TreeGood, treeGoodWater));
                            break;
                        case WaterType.Rainbow:
                            drops.Add(new Drop(WaterType.Normal, normalWater));
                            break;
                    }

                }
                
                dropTime = 0.0f;
            }
        }

        public void UpdateBucketTextureAndType()
        {
            if (Bucket.Type == WaterType.Normal)
            {
                Bucket.texture = normalBucketTexture;
            }
            else if (Bucket.Type == WaterType.FireGood)
            {
                Bucket.texture = fireGoodBucketTexture;
            }
            else if (Bucket.Type == WaterType.TreeGood)
            {
                Bucket.texture = treeGoodBucketTexture;
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
        }


        /// <summary>
        /// Draws the gameplay screen.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Resolution.getTransformationMatrix());
          
            spriteBatch.Draw(background, screen, Color.White);
            if (IsActive()) //Draw bucket if current window is active
            {
                Vector2 translatedBucketPosition = TranslatePosition(Bucket.position);
                bucket.Draw(spriteBatch, translatedBucketPosition, SpriteScale);
                bucket.DrawWaterCount(spriteBatch, translatedBucketPosition, SpriteScale, gameFont);
            }
            foreach (Drop drop in drops)
            {
                Vector2 translatedPosition = TranslatePosition(drop.Position);
                drop.Draw(spriteBatch, translatedPosition, SpriteScale);
            }
            spriteBatch.End();
        }




        #endregion
    }
}
