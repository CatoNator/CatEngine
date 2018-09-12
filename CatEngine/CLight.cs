using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    public class CLight
    {
        public float x;
        public float y;

        public CLight(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void Render()
        {
            CSprite.Instance.Render("sprLight", x, y, 0, false, 0.0f, 1.0f, Color.White);
        }
    }
}
