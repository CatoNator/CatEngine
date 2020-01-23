using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEngine.Content;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    class CCheckpoint : CGameObject
    {

        public override void InstanceSpawn()
        {
            hitCylinder = new CylinderCollider(new Vector3(x, 0, y), 200, 5);
        }

        public override void Update()
        {
            CGameObject playerCollision = CollisionCylinder(typeof(CPlayer));

            if (playerCollision != null)
            {
                CAudioManager.Instance.PlaySound("menuselect1");
                CObjectManager.Instance.DestroyInstance(iIndex);
            }
        }

        public override void Render()
        {
            if (CDebug.Instance.ShowHitBoxes)
                CRender.Instance.DrawHitBox(hitCylinder.Position, hitCylinder.Height, hitCylinder.Radius);
        }
    }
}
