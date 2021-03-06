﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
{
    class CDebug
    {
        public bool ShowTerrainDebug = false;

        public bool ShowDebugValues = false;

        public bool ShowHitBoxes = false;

        public bool DrawShadowMap = false;

        private CDebug()
        {
        }

        //singletoning the singleton
        public static CDebug Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CDebug instance = new CDebug();
        }
    }
}
