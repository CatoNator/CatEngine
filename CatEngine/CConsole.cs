using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CatEngine
{
    class CConsole
    {
        private CConsole()
        {
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

        public void Print(String str)
        {
            Debug.Print(str);
        }
    }
}
