using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GameStateManagement
{
    enum GameScreenPosition
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    class MiniGameScreen
    {
        public static Vector2 maxScreenDimensions = new Vector2(Resolution.GetMiniGameResolution().Width, Resolution.GetMiniGameResolution().Height);
        public static Vector2 centerOfScreen = new Vector2(Resolution.GetVirtualResolution().Width / 2, Resolution.GetVirtualResolution().Height / 2);
        public static Vector2 maxScreenScale = new Vector2(1.0f, 1.0f);
        public static Vector2 sideScreenDimensions = new Vector2(Resolution.GetMiniGameResolution().Width * .6f, Resolution.GetMiniGameResolution().Height * .7f);
        public static Vector2 sideScreenScale = new Vector2(sideScreenDimensions.X / (float)maxScreenDimensions.X, sideScreenDimensions.Y/ (float)maxScreenDimensions.Y);
        public static Rectangle mainScreen = new Rectangle((int)(centerOfScreen.X - maxScreenDimensions.X / 2), (int)(centerOfScreen.Y - maxScreenDimensions.Y / 2), (int)maxScreenDimensions.X, (int)maxScreenDimensions.Y);
        public static Rectangle leftScreen = new Rectangle(mainScreen.Left - (int)(sideScreenDimensions.X * .8f), (int)(centerOfScreen.Y - sideScreenDimensions.Y / 2), (int)sideScreenDimensions.X, (int)sideScreenDimensions.Y);
        public static Rectangle rightScreen = new Rectangle(mainScreen.Right - (int)(sideScreenDimensions.X * .2f), (int)(centerOfScreen.Y - sideScreenDimensions.Y / 2), (int)sideScreenDimensions.X, (int)sideScreenDimensions.Y);
        protected Vector2 screenScale;
        protected Rectangle screen;

        public GameScreenPosition ScreenPosition
        {
            get { return screenPosition; }
            set 
            { 
                screenPosition = value;
                UpdateScreen();
            }
        }
        GameScreenPosition screenPosition = GameScreenPosition.Center;

        public Vector2 SpriteScale
        {
            get { return screenScale / maxScreenScale; }
        }


        public bool IsActive()
        {
            return ScreenPosition == GameScreenPosition.Center;
        }

        private void UpdateScreenScale()
        {
            if (ScreenPosition == GameScreenPosition.Center)
                screenScale = maxScreenScale;
            else //If a side screen
                screenScale = sideScreenScale;
        }

        private void UpdateScreen()
        {
            UpdateScreenScale();
            if (ScreenPosition == GameScreenPosition.Center)
                screen = mainScreen;
            else if (ScreenPosition == GameScreenPosition.Left)
                screen = leftScreen;
            else
                screen = rightScreen;
        }

        protected Vector2 TranslatePosition(Vector2 position)
        {
            position = new Vector2(position.X * screenScale.X, position.Y * screenScale.Y);
            position += new Vector2(screen.Left, screen.Top);
            return position;
        }
    }
}
