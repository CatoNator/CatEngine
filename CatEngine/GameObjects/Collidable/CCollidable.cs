using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using CatEngine.Content;

namespace CatEngine
{
    class CCollidable : CGameObject
    {
private float fScale = 10.0f;

        public Vector3 Speed = new Vector3(0, 0, 0);
        public Vector3 RotationSpeed = new Vector3(0, 0, 0);

        public Vector3 prevPos = new Vector3(0, 0, 0);

        public Vector3 Rotation = new Vector3(0, 0, 0);

        private Vector3 playervec = new Vector3(0, 0, 0);

        private string sModelName = "hexatri";

        public override void InstanceSpawn()
        {
            //TempLoadCollision("hexatri");
        }

        public override void Update()
        {
            //x += Speed.X;
            //y += Speed.Y;
            //z += Speed.Z;
            /*rot += 0.01f;
            rot %= (float)Math.PI * 2;
            x = 5 + distDirX(dist, rot);
            z = 10 -distDirY(dist, rot);*/

            //Speed = new Vector3(x - prevPos.X, y - prevPos.Y, z - prevPos.Z);

            prevPos = new Vector3(x, y, z);
            //CColliderManager.Instance.UpdateCollider(sModelName, new Vector3(0, 0, 0), fScale);
        }

        public float GetHeightAt(float x, float y, float z)
        {
            return 0;//return CColliderManager.Instance.GetFloorHeightAt(sModelName, x, y, z);

            /*float h = 0.0f;

            foreach (Triangle tri in Floors)
            {
                if (tri.PointInTriangle(new Vector2(x, y)))
                    h = tri.HeightAt(new Vector2(x, y));
            }

            return h;*/
        }

        public Vector3 PointInWall(Vector3 position, float rad, float height)
        {
            return new Vector3(0, 0, 0);//return CColliderManager.Instance.PointInWall(sModelName, new Vector3(x, z, y), fScale, position, height, rad);
        }

        public override void Render()
        {
            Vector3 posVec = new Vector3(x, z, y);

            //CRender.Instance.DrawSimpleModel(sModelName, posVec, Rotation, fScale);

            if (CDebug.Instance.ShowTerrainDebug)
                CColliderManager.Instance.RenderCollider(sModelName, new Vector3(x, z, y), fScale);
        }
    }
}
