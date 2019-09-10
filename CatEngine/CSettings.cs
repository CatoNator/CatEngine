using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CatEngine
{
    public sealed class CSettings
    {
        public int GAME_VIEW_WIDTH = 416;
        public const int GAME_VIEW_HEIGHT = 240;

        private int iAspectRatioH = 16;
        private int iAspectRatioV = 9;

        public int iBackBufferWidth = 1280;
        public int iBackBufferHeight = 720;

        public float fMusicVolume = 1.0f;
        public float fSoundVolume = 1.0f;

        public Keys kPTurnLeft = Keys.A;
        public Keys kPTurnRight = Keys.D;
        public Keys kPMoveForward = Keys.W;
        public Keys kPMoveBackward = Keys.S;
        public Keys kPStrafeLeft = Keys.Q;
        public Keys kPStrafeRight = Keys.E;

        public Keys kPFire = Keys.Space;

        public Keys kPReload = Keys.R;

        private CSettings()
        {
        }

        //singletoning the singleton
        public static CSettings Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CSettings instance = new CSettings();
        }

        public void SetGameViewSize()
        {
            GAME_VIEW_WIDTH = GAME_VIEW_HEIGHT * iAspectRatioH / iAspectRatioV;
        }

        public void SetBackbufferSize(int bufferHeight)
        {
            iBackBufferHeight = bufferHeight;

            iBackBufferWidth = iBackBufferHeight * iAspectRatioH / iAspectRatioV;
        }
    }
}
