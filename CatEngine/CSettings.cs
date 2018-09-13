﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
{
    public sealed class CSettings
    {
        public int GAME_VIEW_WIDTH = 416;
        public const int GAME_VIEW_HEIGHT = 240;

        private int iAspectRatioH = 16;
        private int iAspectRatioV = 9;

        public int iBackBufferWidth = 640;
        public int iBackBufferHeight = 360;

        public float fMusicVolume = 1.0f;
        public float fSoundVolume = 1.0f;
        
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
