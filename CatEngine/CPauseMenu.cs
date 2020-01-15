using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
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
    }
}
