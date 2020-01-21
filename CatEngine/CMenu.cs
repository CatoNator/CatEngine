using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEngine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using CatEngine.Input;
using System.IO;

namespace CatEngine.UI
{
    class CMenu
    {
        private List<string> sMenuItems = new List<string>();
        private int iSelectedItem = 0;

        private int iPreviousInput = 0;

        private CMenu()
        {
            string path = "AssetData/Levels";

            string[] dirs = Directory.GetDirectories(path);

            foreach (string s in dirs)
            {
                sMenuItems.Add(s.Split('\\')[1]);
            }
        }

        //singletoning the singleton
        public static CMenu Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CMenu instance = new CMenu();
        }

        public void Update()
        {
            GamePadState gamepadState = GamePad.GetState(PlayerIndex.One);

            int iInput = (int)gamepadState.ThumbSticks.Left.Y;

            if (iInput != iPreviousInput)
            {
                if (iInput != 0)
                    CAudioManager.Instance.PlaySound("menucursor");

                if (iInput > 0 && iSelectedItem > 0)
                    iSelectedItem--;
                else if (iInput < 0 && iSelectedItem < sMenuItems.Count - 1)
                    iSelectedItem++;
            }

            if (CInputManager.ButtonPressed(CSettings.Instance.gPJump))
            {
                CAudioManager.Instance.PlaySound("menuselect1");

                //CLoadingScreen.Instance.PrepareLevelData(sMenuItems[iSelectedItem].Split('\\')[1]);
                CGame.Instance.InitiateFadeLevel(sMenuItems[iSelectedItem]);
                //bLanded = false;
            }

            iPreviousInput = iInput;
        }

        public void Render()
        {
            for (int i = 0; i < sMenuItems.Count; i++)
            {
                if (iSelectedItem == i)
                    CSprite.Instance.DrawText(sMenuItems[i], new Vector2(10, 10 + 16 * i), Color.Red);
                else
                    CSprite.Instance.DrawText(sMenuItems[i], new Vector2(10, 10+16*i), Color.White);
            }
        }
    }
}
