using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GameStateManagement
{
    class Bucket
    {
        #region Fields
        public static Vector2 position;
        public Vector2 motion;
        //static float paddleSpeed = 600.0f;


        public static Texture2D texture;

        public static WaterType Type 
        { 
            get { return type; }
            set { type = value; } 
        }
        private static WaterType type = WaterType.Normal;

        public static int waterCount = Level.StartingWaterCount;

        KeyboardState keyboardState;
        GamePadState gamePadState;

        #endregion

        #region Initialize

        /// <summary>
        /// Constructor
        /// </summary>

        public Bucket(Texture2D texture, Rectangle screenBounds)
        {
            Bucket.texture = texture;
            SetInStartPosition(screenBounds);
            Bucket.Type = WaterType.Normal;
            ResetWaterCount();
        }

        public Bucket(Rectangle screenBounds)
        {
            if (texture == null)
                throw new Exception("Need to initialize bucket's texture.");
            SetInStartPosition(screenBounds);
        }

        #endregion

        #region Update and Methods

        public void Update(GameTime gameTime)
        {
            
            keyboardState = Keyboard.GetState();
            gamePadState = GamePad.GetState(PlayerIndex.One);

            if (keyboardState.IsKeyDown(Keys.Left) ||
                gamePadState.IsButtonDown(Buttons.LeftThumbstickLeft) ||
                gamePadState.IsButtonDown(Buttons.DPadLeft))
            {
                motion.X = -1;
            }
            else if (keyboardState.IsKeyDown(Keys.Right) ||
                gamePadState.IsButtonDown(Buttons.LeftThumbstickRight) ||
                gamePadState.IsButtonDown(Buttons.DPadRight))
            {
                motion.X = 1;
            }
            else if (keyboardState.IsKeyUp(Keys.Left) &&
                (!gamePadState.IsConnected ||
                (gamePadState.IsButtonUp(Buttons.LeftThumbstickLeft) &&
                gamePadState.IsButtonUp(Buttons.DPadLeft))) &&
                keyboardState.IsKeyUp(Keys.Right) &&
                (!gamePadState.IsConnected ||
                (gamePadState.IsButtonUp(Buttons.LeftThumbstickRight) &&
                gamePadState.IsButtonUp(Buttons.DPadRight))))
            {
                motion = Vector2.Zero;
            }
            motion.X *= Level.BucketSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            position += motion;

            HandlePositioning();
        } // end update

        public void HandlePositioning()
        {
            if (position.X < 0)
                position.X = 0;

            if (position.X + texture.Width > Resolution.GetMiniGameResolution().Width)
                position.X = Resolution.GetMiniGameResolution().Width - texture.Width;
        }

        public void SetInStartPosition(Rectangle bounds)
        {
            position.X = (bounds.Width - texture.Width) / 2;
            position.Y = bounds.Height - texture.Height - 5;
        } // end SetInStartPosition()

        public void Draw(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale)
        {
            Rectangle dimensions = new Rectangle(0, 0, GetBounds().Width, GetBounds().Height);
            spriteBatch.Draw(texture, translatedPosition, dimensions, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        } // end Draw()

        public void DrawWaterCount(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale, SpriteFont font)
        {
            string waterCountDisplay = waterCount + "/" + Level.MaxWater;
            Vector2 textPosition = font.MeasureString(waterCountDisplay);
            //Position the text to be at the center of the bucket
            Vector2 newPosition = new Vector2(translatedPosition.X + texture.Width / 2 - textPosition.X / 2, translatedPosition.Y + texture.Height / 2 - textPosition.Y / 2);
            spriteBatch.DrawString(font, waterCountDisplay, newPosition, Color.Black, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        } // end GetBounds()

        public Vector2 GetMotion()
        {
            return motion;
        }
        #endregion

        /// <summary>
        /// Handles increasing/decreasing waterCount
        /// </summary>
        /// <param name="waterChange"></param>
        #region Water Count Handlers

        public bool DecreaseWaterCount( int waterChange )
        {
            if (waterCount >= waterChange)
            {
                waterCount -= waterChange;
                return true;
            }

            return false;
            
        } // end DecreaseWaterCount()

        public void IncreaseWaterCount(int waterChange)
        {
            waterCount += waterChange;

            if (waterCount >= Level.MaxWater)
                waterCount = Level.MaxWater;
        } // end IncreaseWaterCount()

        public static void ResetWaterCount()
        {
            waterCount = Level.StartingWaterCount;
        }

        public bool CanShoot()
        {
            return waterCount >= Ball.MaxHealth;
        }

        #endregion
    }
}
