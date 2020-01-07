using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using CatEngine.Content;

namespace CatEngine
{
    class CProp : CGameObject
    {
        private string sSpriteName = "sprTest";

        private int iDir = 0;

        private int iHealth = -1;

        private int iColWidth = 0;
        private int iColHeight = 0;

        public override void InstanceSpawn()
        {
            rCollisionRectangle = new Rectangle(0, 0, 16, 16);
        }

        public void SetProperties(float x, float z, float y, string spriteName, int dir, int colWidth, int colHeight, int health)
        {
            
            this.sSpriteName = spriteName;
            this.iColWidth = colWidth;
            this.iColHeight = colHeight;
            this.iHealth = health;
            this.iDir = dir;

            if (iDir%2 == 0)
                rCollisionRectangle = new Rectangle(0, 0, colWidth, colHeight);
            else
                rCollisionRectangle = new Rectangle(0, 0, colHeight, colWidth);
        }

        public override void Render2D()
        {
            UpdateCollision();

            CSprite.Instance.DrawRect(rCollisionRectangle, Color.Gray);

            CSprite.Instance.Render(sSpriteName, x, y+iColHeight, 0, false, -degToRad((float)iDir * 90.0f), 1.0f, Color.White);
        }
    }
}
