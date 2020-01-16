using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
{
    class CGame
    {
        public int iNatsas = 0;

        public int iPlayerHealth = 5;
        public int iMaxPlayerHealth = 5;

        private CGame()
        {
        }

        //singletoning the singleton
        public static CGame Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CGame instance = new CGame();
        }
    }
}
