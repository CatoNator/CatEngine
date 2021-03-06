﻿using System;
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
        public int GAME_VIEW_WIDTH = 640;
        public const int GAME_VIEW_HEIGHT = 360;

        private int iAspectRatioH = 16;
        private int iAspectRatioV = 9;

        public int iBackBufferWidth = 640;
        public int iBackBufferHeight = 360;

        public int iFovAngle = 45;

        public int iShadowMapSize = 1024;

        /*shadowmap sizes ("shadow quality"):
         * low: 512
         * medium 1024
         * high 2048
        */

        public int iShadowDrawDist = 80;
        public int iModelDrawDist = 160;

        public float fMusicVolume = 1.0f;
        public float fSoundVolume = 1.0f;

        //gameplay input
        public Keys kPTurnLeft = Keys.A;
        public Keys kPTurnRight = Keys.D;
        public Keys kPMoveForward = Keys.W;
        public Keys kPMoveBackward = Keys.S;
        public Keys kPJump = Keys.Space;

        public Keys kGPause = Keys.Escape;

        public Keys kCRotateCamLeft = Keys.Left;
        public Keys kCRotateCamRight = Keys.Right;
        public Keys kCRotateCamUp = Keys.Up;
        public Keys kCRotateCamDown = Keys.Down;

        //gameplay input gamepad
        public Buttons gPJump = Buttons.A;
        
        public Buttons gCRotateCamLeft = Buttons.DPadLeft;
        public Buttons gCRotateCamRight = Buttons.DPadRight;
        public Buttons gCRotateCamUp = Buttons.DPadUp;
        public Buttons gCRotateCamDown = Buttons.DPadDown;

        public Buttons gFire = Buttons.RightTrigger;

        public Buttons gGPause = Buttons.Start;

        public Keys kCCShowConsole = Keys.PageDown;
        public Keys kCCHideConsole = Keys.PageUp;

        public bool bGamepadEnabled = true;

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

        public float GetAspectRatio()
        {
            return (float)iAspectRatioH / (float)iAspectRatioV;
        }
    }
}
