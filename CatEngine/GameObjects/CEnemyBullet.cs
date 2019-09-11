using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine
{
    class CEnemyBullet : CGameObject
    {
        private int timer = 60;

        private float fAimDirection = 0.0f;

        public override void InstanceSpawn()
        {
        }

        public override void Update()
        {
            fHorSpeed = (float)distDirX(5, fAimDirection);
            fVerSpeed = (float)distDirY(5, fAimDirection);

            x += fHorSpeed;
            y += fVerSpeed;

            if (timer < 0)
                CObjectManager.Instance.DestroyInstance(iIndex);
            else
                timer--;
        }

        public void SetAimdir(float dir)
        {
            fAimDirection = dir;
        }

        public override void Render()
        {
            //CSprite.Instance.DrawRect(rCollisionRectangle, Color.Green);

            CSprite.Instance.Render("sprEnemyBullet", x, y, 1, false, -degToRad(fAimDirection), 1.0f, Color.White);
        }
    }
}
