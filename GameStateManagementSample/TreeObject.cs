﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace GameStateManagement
{
    enum TreeObjectType
    {
        Tree,
        Seed
    }

    class TreeObject
    {
        Texture2D texture;

        int health = Level.MaxTreeHealth;

        public TreeObjectType Type
        {
            get { return type; }
        }
        TreeObjectType type;

        public Circle Bounds
        {
            get { return new Circle(new Vector2(position.X + texture.Width / 2, position.Y + texture.Height / 2), texture.Width / 2); }
        }

        public Vector2 Position
        {
            get { return position; }
        }
        Vector2 position;

        public TreeObject(TreeObjectType type, Texture2D texture, Vector2 position)
        {
            this.type = type;
            this.texture = texture;
            this.position = position;
        }

        public bool Update(GameTime gameTime)
        {
            return false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale)
        {
            spriteBatch.Draw(texture, translatedPosition, texture.Bounds, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        public void OnCollision(WaterType type)
        {

            if (type == WaterType.Normal)
            {
                health -= 1;
            }
            else if (type == WaterType.FireGood)
            {
                health = Level.MaxTreeHealth;
            }
            else if (type == WaterType.TreeGood)
            {
                health -= 2;
            }

        }

        public bool IsAlive()
        {
            return health > 0;
        }

        public int GetHealth()
        {
            return health;
        }

        public void SetTexture(Texture2D texture)
        {
            this.texture = texture;
        }

        public void DrawSeedCount(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale, SpriteFont font)
        {
            spriteBatch.DrawString(font, "", translatedPosition, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
    }
}
