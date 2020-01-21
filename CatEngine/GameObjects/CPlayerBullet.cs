using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEngine.Content;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    class CPlayerBullet : CGameObject
    {
        private int iLife = 60;

        public void SetProperties(float dir, float sp)
        {
            fDirection = dir;
            fVelocity = sp;
        }

        public override void Update()
        {
            x += distDirX(fVelocity, fDirection);
            y += distDirY(fVelocity, fDirection);

            if (iLife <= 0)
                CObjectManager.Instance.DestroyInstance(iIndex);
            else
                iLife--;
        }

        public override void Render()
        {
            CRender.Instance.DrawBillBoard(new Vector3(x, z, y), new Vector2(2, 2), new Vector2(1, 1), 0, 1, "bullet");
        }
    }
}
