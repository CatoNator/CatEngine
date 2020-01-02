using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

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

        public void Render()
        {
            CConsole.Instance.Render();
        }
    }
}
