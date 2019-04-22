using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine
{
    public sealed class CHud
    {
        private CHud()
        {
        }

        //singletoning the singleton
        public static CHud Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CHud instance = new CHud();
        }

        public void Update()
        {
        }

        public void Render(SpriteBatch spriteBatch, SpriteFont font)
        {
            spriteBatch.DrawString(font, CGameManager.Instance.iScore.ToString(), new Vector2(10, 220), Color.White);
            spriteBatch.DrawString(font, CGameManager.Instance.iLives.ToString(), new Vector2(400, 220), Color.White);
        }
    }
}
