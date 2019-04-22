using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    public class CWall : CGameObject
    {
        public override void InstanceSpawn()
        {
            rCollisionRectangle = new Rectangle(0, 0, 16, 8);

            UpdateCollision();
        }

        public override void OnDestruction()
        {
            CGameManager.Instance.iScore += 10;
        }

        public override void Render()
        {
            UpdateCollision();

            CSprite.Instance.DrawRect(rCollisionRectangle, Color.Gray);

            //sprSprite.Render(x, y, 0.0f, 1.0f, myColor);
        }
    }
}
