using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    enum FireObjectType
    {
        Invincible,
        Normal
    }

    class FireObject
    {
        Texture2D texture;
        float spreadTime = 0.0f;
        int health = 1;

        public FireObjectType Type
        {
            get { return type; }
        }
        FireObjectType type;

        public Circle Bounds
        {
            get { return new Circle(new Vector2(position.X + texture.Width / 2,position.Y + texture.Height / 2), texture.Width / 2); }
        }

        public Vector2 Position
        {
            get { return position; }
        }
        Vector2 position;

        public FireObject(FireObjectType type, Texture2D texture, Vector2 position)
        {
            this.type = type;
            this.texture = texture;
            this.position = position;
        }

        public bool Update(GameTime gameTime)
        {
            spreadTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (spreadTime >= Level.FireSpawnRate)
            {
                spreadTime = 0.0f;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale)
        {
            spriteBatch.Draw(texture, translatedPosition, texture.Bounds, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        public bool OnCollision(WaterType type)
        {
            int damage = 0;
            if (type == WaterType.Normal)
            {
                damage = 1;
            }
            else if (type == WaterType.FireGood)
            {
                damage = 2;
            }
            else if (type == WaterType.TreeGood)
            {
                return true;
            }

            health -= damage;
            return false;
        }

        public bool IsAlive()
        {
            return health > 0;
        }
    }
}
