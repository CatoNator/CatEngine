using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using CatEngine.Content;
using CatEngine.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CatEngine.UI
{
    class CPauseMenu
    {
        private List<string> sMenuItems = new List<string>();
        private List<string> sMenuCommands = new List<string>();
        private int iSelectedItem = 0;

        private int iPreviousInput = 0;

        private CPauseMenu()
        {
            sMenuItems.Add("toggle CDebug.ShowTerrainDebug");
            sMenuItems.Add("toggle CDebug.ShowHitBoxes");
            sMenuItems.Add("toggle CDebug.DrawShadowMap");

            sMenuCommands.Add("DebugToggleGround");
            sMenuCommands.Add("DebugToggleHitboxes");
            sMenuCommands.Add("DebugToggleShadowMap");
        }

        //singletoning the singleton
        public static CPauseMenu Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CPauseMenu instance = new CPauseMenu();
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

                MethodInfo method = typeof(CPauseMenu).GetMethod(sMenuCommands[iSelectedItem]);

                if (method != null)
                    method.Invoke(this, new List<string>().ToArray());
            }

            iPreviousInput = iInput;
        }

        public void DebugToggleGround()
        {
            CDebug.Instance.ShowTerrainDebug = !CDebug.Instance.ShowTerrainDebug;
        }

        public void DebugToggleHitboxes()
        {
            CDebug.Instance.ShowHitBoxes = !CDebug.Instance.ShowHitBoxes;
        }

        public void DebugToggleShadowMap()
        {
            CDebug.Instance.DrawShadowMap = !CDebug.Instance.DrawShadowMap;
        }

        public void Render()
        {
            CSprite.Instance.DrawRect(new Rectangle(0, 0, CSettings.Instance.GAME_VIEW_WIDTH, CSettings.GAME_VIEW_HEIGHT), Color.Black * 0.5f);
            CSprite.Instance.DrawText("PAUSED", new Vector2(10, 10), Color.White);

            for (int i = 0; i < sMenuItems.Count; i++)
            {
                if (iSelectedItem == i)
                    CSprite.Instance.DrawText(sMenuItems[i], new Vector2(10, 30 + 16 * i), Color.Red);
                else
                    CSprite.Instance.DrawText(sMenuItems[i], new Vector2(10, 30 + 16 * i), Color.White);
            }
        }
    }
}
