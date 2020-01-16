using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CatEngine.Content;

namespace CatEngine
{
    class CConsole
    {
        //private string[] sMessages = new string[10];
        private List<string> sMessages = new List<string>();

        private int ConsoleTimerDef = 240;

        private int ConsoleClearTimer = 240;

        private bool consoleEnabled = true;

        private bool showConsole = false;

        private bool showDebug = false;

        public string debugString = "";
        public float debugValue = 0.0f;

        public string debugString2 = "";
        public float debugValue2 = 0.0f;

        /*private CConsole()
        {
            for (int i = 0; i < sMessages.Length; i++)
            {
                sMessages[i] = "";
            }
        }*/

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
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(CSettings.Instance.kCCShowConsole))
            {
                showConsole = true;
            }
            else if(keyboardState.IsKeyDown(CSettings.Instance.kCCHideConsole))
            {
                showConsole = false;
            }
        }

        public void Print(String str)
        {
            /*for (int i = 0; i < sMessages.Length - 1; i++)
            {
                sMessages[i] = sMessages[i+1];
            }

            sMessages[sMessages.Length-1] = str;*/

            sMessages.Add(str);

            Debug.Print(str);

            //showConsole = true;
            //ConsoleClearTimer = ConsoleTimerDef;
        }

        public void Render()
        {
            Update();

            if (consoleEnabled && showConsole)
            {
                int height = ((5 + 16 * (sMessages.Count) > CSettings.GAME_VIEW_HEIGHT) ? (5 + 16 * (sMessages.Count)) : CSettings.GAME_VIEW_HEIGHT);
                float textOffset = ((5 + 16 * (sMessages.Count) > CSettings.GAME_VIEW_HEIGHT) ? (CSettings.GAME_VIEW_HEIGHT - (5 + 16 * (sMessages.Count))) : 0);

                CSprite.Instance.DrawRect(new Rectangle(0, 0, CSettings.Instance.GAME_VIEW_WIDTH, height), Color.Black * 0.75f);

                for (int i = 0; i < sMessages.Count; i++)
                {
                    CSprite.Instance.DrawText(sMessages[i], new Vector2(5, 5 + 16 * i+textOffset), Color.White);
                }
            }

            if (showDebug)
            {
                DrawDebugInfo();
            }
        }

        private void DrawDebugInfo()
        {
            CSprite.Instance.DrawText(debugString + " " + debugValue + " " + debugString2 + " " + debugValue2, new Vector2(20, 20), Color.White);
        }
    }
}
