﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    class CConsole
    {
        private string[] sMessages = new string[10];

        private int ConsoleTimerDef = 60;

        private int ConsoleClearTimer = 120;

        private CConsole()
        {
            for (int i = 0; i < sMessages.Length; i++)
            {
                sMessages[i] = "";
            }
        }

        //singletoning the singleton
        public static CConsole Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CConsole instance = new CConsole();
        }

        private void Update()
        {
            if (ConsoleClearTimer <= 0)
            {
                Print("");
            }
            else
                ConsoleClearTimer--;
        }

        public void Print(String str)
        {
            for (int i = 0; i < sMessages.Length - 1; i++)
            {
                sMessages[i] = sMessages[i+1];
            }

            sMessages[sMessages.Length-1] = str;

            Debug.Print(str);

            ConsoleClearTimer = ConsoleTimerDef;
        }

        public void Render()
        {
            Update();

            for (int i = 0; i < sMessages.Length; i++)
            {
                CSprite.Instance.DrawText(sMessages[i], new Vector2(5, 5 + 16 * i), Color.Black);
            }
        }
    }
}
