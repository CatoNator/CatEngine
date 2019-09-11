using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
{
    public class CTileManager
    {
        private CTileManager()
        {
        }

        //singletoning the singleton
        public static CTileManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CTileManager instance = new CTileManager();
        }

        public void AddTile()
        {

        }

        public void RenderTiles()
        {

        }
    }
}
