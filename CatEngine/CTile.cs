using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
{
    public class CTile
    {
        int x;
        int y;
        int left;
        int top;

        public CTile ()
        {
        }

        public void Render()
        {
            CSprite.Instance.RenderTile(x, y, left, top);
        }
    }
}
