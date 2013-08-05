﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GameStateManagement
{
    static class Level
    {
        public static void LoadContent(ContentManager content, ScreenManager screenManager)
        {
            fireTexture = content.Load<Texture2D>("Sprite/FireObject");
            invincFireTexture = content.Load<Texture2D>("Sprite/InvincFireObject");
        }
        public static int numberOfLevels = 5;
        private static Texture2D invincFireTexture;
        private static Texture2D fireTexture;
        static float waterDropRate;
        public static float WaterDropRate
        { 
            get { return waterDropRate; } 
        }
        static float fireSpawnRate;
        public static float FireSpawnRate
        {
            get { return fireSpawnRate; }
        }
        static FireObject[,] fireGrid;
        public static FireObject[,] FireGrid
        {
            get { return fireGrid; }
        }
        static int maxTreeHealth;
        public static int MaxTreeHealth
        {
            get { return maxTreeHealth; }
        }
        static float bucketSpeed;
        public static float BucketSpeed
        {
            get { return bucketSpeed; }
        }
        static float ballSpeed;
        public static float BallSpeed
        {
            get { return ballSpeed; }
        }
        static int treesToWin;
        public static int TreesToWin
        {
            get { return treesToWin; }
        }
        static int maxBalls;
        public static int MaxBalls
        {
            get { return maxBalls; }
        }
        static int maxWater;
        public static int MaxWater
        {
            get { return maxWater; }
        }
        static int startingWaterCount;
        public static int StartingWaterCount
        {
            get { return startingWaterCount; }
        }


        public static void changeLevel(int level)
        {
            int gridWidth = (int)Resolution.GetMiniGameResolution().Width / fireTexture.Width;
            int gridHeight = (int)(Resolution.GetMiniGameResolution().Height * .75 / fireTexture.Height);
            fireGrid = new FireObject[gridWidth, gridHeight];

            switch (level)
            {
                    
                case 1:
                    waterDropRate = 0.5f;
                    fireSpawnRate = 16.5f;
                    maxTreeHealth = 3;
                    bucketSpeed = 600.0f;
                    ballSpeed = 600.0f;
                    treesToWin = 6;
                    AddFireSpawner(gridWidth / 2 - 1, gridHeight / 2 - 1);
                    maxBalls = 2;
                    startingWaterCount = 0;
                    maxWater = 50;
                    break;
                case 2:
                    waterDropRate = 0.5f;
                    fireSpawnRate = 14.5f;
                    maxTreeHealth = 4;
                    bucketSpeed = 600.0f;
                    ballSpeed = 600.0f;
                    treesToWin = 8;
                    AddFireSpawner(gridWidth / 2 - 1, gridHeight / 2 - 1);
                    maxBalls = 2;
                    startingWaterCount = 10;
                    maxWater = 50;
                    break;
                case 3:
                    waterDropRate = 0.5f;
                    fireSpawnRate = 13f;
                    maxTreeHealth = 5;
                    bucketSpeed = 600.0f;
                    ballSpeed = 600.0f;
                    treesToWin = 10;
                    AddFireSpawner(gridWidth / 2 - 1, gridHeight / 2 - 1);
                    maxBalls = 3;
                    startingWaterCount = 15;
                    maxWater = 75;
                    break;
                case 4:
                    waterDropRate = 0.5f;
                    fireSpawnRate = 12.5f;
                    maxTreeHealth = 5;
                    bucketSpeed = 600.0f;
                    ballSpeed = 600.0f;
                    treesToWin = 12;
                    AddFireSpawner(gridWidth / 2 - 1, gridHeight / 2 - 1);
                    maxBalls = 4;
                    startingWaterCount = 20;
                    maxWater = 100;
                    break;
                case 5:
                    waterDropRate = 0.5f;
                    fireSpawnRate = 10.0f;
                    maxTreeHealth = 7;
                    bucketSpeed = 600.0f;
                    ballSpeed = 600.0f;
                    treesToWin = 15;
                    AddFireSpawner(gridWidth / 2 - 1, gridHeight / 2 - 1);
                    maxBalls = 5;
                    startingWaterCount = 40;
                    maxWater = 100;
                    break;
                default:
                    throw new Exception("Level number invalid.");
            }
        }
        private static Vector2 GetFireObjectPosition(int x, int y)
        {
            if (y % 2 == 1) //shorter row
                return new Vector2(x * fireTexture.Width + (fireTexture.Width / 2), y * fireTexture.Height);
            else
                return new Vector2(x * fireTexture.Width, y * fireTexture.Height);
        }

        private static void AddFireSpawner(int x, int y)
        {
            fireGrid[x, y] = new FireObject(FireObjectType.Invincible, invincFireTexture, GetFireObjectPosition(x, y));
        }
    }
}