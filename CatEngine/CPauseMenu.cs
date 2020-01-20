using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEngine.Content;
using Microsoft.Xna.Framework;

namespace CatEngine.UI
{
    class CPauseMenu
    {
        private CPauseMenu()
        {
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

        public void Render()
        {
            CSprite.Instance.DrawRect(new Rectangle(0, 0, CSettings.Instance.GAME_VIEW_WIDTH, CSettings.GAME_VIEW_HEIGHT), Color.Black * 0.5f);
            CSprite.Instance.DrawText("PAUSED", new Vector2(10, 10), Color.White);
        }
    }
}
