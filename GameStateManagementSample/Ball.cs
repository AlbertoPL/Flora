﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    class Ball
    {
        Vector2 motion = Vector2.Zero;

        public static int MaxHealth = 5;
        public int Health
        {
            get { return health; }
        }
        int health = MaxHealth;

        public bool IsMoving
        {
            get { return motion != Vector2.Zero; }
        }

        public Rectangle RectangleBounds
        {
            get { return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height); }
        }

        public Circle CircleBounds
        {
            get { return new Circle(new Vector2(RectangleBounds.Center.X, RectangleBounds.Center.Y), texture.Width/2); }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        Vector2 position;

        private WaterType type;
        public WaterType Type
        {
            get { return type; }
        }

        //float ballSpeed = 400.0f;

        public Texture2D texture;
        Rectangle screenBounds;

        public Ball(Texture2D texture, WaterType type, Rectangle startPosition, Rectangle screenBounds)
        {
            this.texture = texture;
            this.screenBounds = screenBounds;
            SetInStartPosition(startPosition);
            this.type = type;
        }

        public void Update(GameTime gameTime)
        {
            if(IsMoving)
                motion.Normalize();
            position += motion * Level.BallSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if(IsMoving)
                CheckWallCollision();
        }

        private void CheckWallCollision()
        {
            if (position.X < 0)
            {
                position.X = 0;
                motion.X *= -1;
                HandleHorizontalMovement();
            }
            else if (position.X + texture.Width > screenBounds.Width)
            {
                position.X = screenBounds.Width - texture.Width;
                motion.X *= -1;
                HandleHorizontalMovement();
            }
            if (position.Y < 0)
            {
                position.Y = 0;
                motion.Y *= -1;
            }
            else if (position.Y > screenBounds.Height)
            {
                OnDeath();
            }
        }

        private void HandleHorizontalMovement()
        {
            float angleToVary = 10; // Will check if angle is within +-angleToVary degrees from the horizontal
            angleToVary *= .5f;
            float angle = MathHelper.ToDegrees((float)Math.Atan2(motion.Y, motion.X)); //In degrees
            if (angle < 0) angle += 360.0f;
            if ((angle > (180.0f - angleToVary) && angle < (180.0f + angleToVary)) || angle > (360.0f - angleToVary) || angle < angleToVary)
            {
                float angleOffset = 20;
                if ((motion.Y > 0 && motion.X > 0) || (motion.Y <= 0 && motion.X < 0)) //If moving down-right or up-left, subtract the angle
                    angleOffset *= -1;
                angle += angleOffset;
                angle = MathHelper.ToRadians(angle);
                int ySign = (motion.Y == 0) ? -1 : Math.Sign(motion.Y);
                motion = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle) * ySign);
            }
        }

        public void SetInStartPosition(Rectangle paddleLocation)
        {
            health = MaxHealth;
            motion = Vector2.Zero;
            position.Y = paddleLocation.Y - texture.Height;
            position.X = paddleLocation.X + (paddleLocation.Width / 2) - (texture.Width / 2);
        }

        public void OnDeath()
        {
            health = 0;
        }

        public void SetInMotion(Bucket bucket)
        {
            if (!IsMoving)
            {
                float angle;
                if (bucket.GetMotion().X == 0)
                {
                    motion = new Vector2(.2f, -1);
                    angle = 80.0f;
                }
                else if (bucket.GetMotion().X > 0)
                {
                    angle = (float)new Random().NextDouble() * 20 + 30; //30-50 degrees
                }
                else
                {
                    angle = (float)new Random().NextDouble() * 20 + 120; //120-140 degrees
                }
                angle = MathHelper.ToRadians(angle);
                motion = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            }
        }

        public bool OffBottom()
        {
            if (position.Y > screenBounds.Height)
                return true;
            return false;
        }

        public void PaddleCollision(Bucket bucket)
        {
            Rectangle bucketBounds = bucket.GetBounds();
            if (bucketBounds.Intersects(RectangleBounds))
            {
                if (position.Y < bucketBounds.Top - texture.Height / 2) //If clearly hitting the bucket from above
                    position.Y = bucketBounds.Top - texture.Height;
                else if (position.X > bucketBounds.Left) //If hitting the bucket's right side
                    position.X = bucketBounds.Left + bucketBounds.Width;
                else if (position.X < bucketBounds.Right)//If hitting the bucket's left side
                    position.X = bucketBounds.Left - texture.Width;
                if (IsMoving)
                {
                    if (RectangleBounds.X >= bucketBounds.X + bucketBounds.Width * .95f)
                        motion = new Vector2(1, -.75f);
                    else if (RectangleBounds.X <= bucketBounds.X + bucketBounds.Width * .05f)
                        motion = new Vector2(-1, -.75f);
                    else
                        motion = new Vector2(motion.X + bucket.motion.X/10, -motion.Y);
                }
            }
        }

        public bool CollidesWithObject(Circle objectBounds)
        {
            Circle bounds = CircleBounds;
            if (objectBounds.Intersects(bounds))
            {
                Vector2 ballToObject = bounds.Center - objectBounds.Center;
                Vector2 ballToObjectN = ballToObject;
                ballToObjectN.Normalize();
                
                Vector2 velDiffProjected = ballToObjectN * Vector2.Dot(motion, ballToObjectN);
                motion -= velDiffProjected;

                //Move the ball out of the object
                float distanceToMove = objectBounds.Radius + bounds.Radius - ballToObject.Length();
                ballToObject.Normalize();
                ballToObject *= distanceToMove;
                position += ballToObject;

                return true;
            }
            return false;
        }

        public bool CollidesWithFireObject(FireObject theFire)
        {
            if (type != WaterType.FireGood)
                return CollidesWithObject(theFire.Bounds);
            else 
                return (theFire.Bounds.Intersects(CircleBounds));
        }

        public void LoseHealth()
        {
            health--;
        }

        public bool IsAlive()
        {
            return health > 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale)
        {
            spriteBatch.Draw(texture, translatedPosition, texture.Bounds, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }

        public void DrawHealthCount(SpriteBatch spriteBatch, Vector2 translatedPosition, Vector2 scale, SpriteFont font)
        {
            spriteBatch.DrawString(font, "", translatedPosition, Color.White, 0.0f, Vector2.Zero, scale, SpriteEffects.None, 0.0f);
        }
    }
}