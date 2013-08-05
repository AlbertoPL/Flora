using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameStateManagement
{
    enum WaterType
    {
        Normal, 
        FireGood,
        TreeGood,
        Rainbow
    }
    class Drop
    {
        const int NormalWaterValue = 1;
        const int FireGoodWaterValue = 1;
        const int TreeGoodWaterValue = 1;
        int RainbowWaterValue = Level.MaxWater;
        float speed;
        Texture2D texture;
        Rectangle bounds;
       
        public WaterType Type
        {
            get { return type; }
        }
        WaterType type;

        public Vector2 Position
        {
            get { return position; }
        }
        Vector2 position = Vector2.Zero;

        public Drop(WaterType waterType, Texture2D texture)
        {
            Random rand = new Random();
            speed = rand.Next(300, 500);
            position.X = rand.Next(1, Resolution.GetMiniGameResolution().Width - texture.Width);
            this.type = waterType;
            this.texture = texture;
            bounds = new Rectangle(0, 0, texture.Width, texture.Height);
        }

        public void Update(GameTime gameTime)
        {
            position.Y += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale)
        {
            //spriteBatch.Draw(texture, position, Resolution.GetVirtualResolution(), Color.White);
            spriteBatch.Draw(texture, translatedPosition, bounds, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        public Rectangle GetBounds()
        {
            return new Rectangle(
            (int)position.X,
            (int)position.Y,
            texture.Width,
            texture.Height);
        } // end GetBounds()

        public int GetDropValue()
        {
            if (type == WaterType.Normal)
                return NormalWaterValue;
            else if (type == WaterType.FireGood)
                return FireGoodWaterValue;
            else if (type == WaterType.TreeGood)
                return TreeGoodWaterValue;
            else// if (type == WaterType.Rainbow)
                return RainbowWaterValue;
        }
    }


}
