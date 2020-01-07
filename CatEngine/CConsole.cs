using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using CatEngine.Content;

namespace CatEngine
{
    class CConsole
    {
        private string[] sMessages = new string[10];

        private int ConsoleTimerDef = 240;

        private int ConsoleClearTimer = 240;

        private bool showConsole = true;

        private bool showDebug = false;

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
                showConsole = false;
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

            //Debug.Print(str);

            showConsole = true;
            ConsoleClearTimer = ConsoleTimerDef;
        }

        public void Render()
        {
            Update();

            if (showConsole)
            {
                CSprite.Instance.DrawRect(new Rectangle(0, 0, CSettings.Instance.GAME_VIEW_WIDTH, 5 + 16 * (sMessages.Length)), Color.Black * 0.75f);

                for (int i = 0; i < sMessages.Length; i++)
                {
                    CSprite.Instance.DrawText(sMessages[i], new Vector2(5, 5 + 16 * i), Color.White);
                }
            }

            if (showDebug)
            {
                DrawDebugInfo();
            }
        }

        private void DrawDebugInfo()
        {
            Vector3 test = CRender.Instance.SunDebug();
            CSprite.Instance.DrawText("sun orientation " + test.X + " " + test.Y + " " + test.Z, new Vector2(20, 20), Color.White);
        }
    }
}
