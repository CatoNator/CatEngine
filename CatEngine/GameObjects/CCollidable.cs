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
        //floor heights
        /*
        0---1
        |   |
        |   |
        2---3
        */
        public float[] fFloorHeights = new float[4];
        //ceiling heights
        public float[] fCeilHeights = new float[4];

        private List<Triangle> sTriangles = new List<Triangle>();
        private List<Vector3> fVectors = new List<Vector3>();

        public Vector2 fSize = new Vector2(5, 5);

        private struct Triangle
        {
            public Vector3 C1;
            public Vector3 C2;
            public Vector3 C3;

            public Triangle(Vector3 V1, Vector3 V2, Vector3 V3)
            {
                C1 = V1;
                C2 = V2;
                C3 = V3;
            }
        }

        public override void InstanceSpawn()
        {
            for (int i = 0; i < 4; i++)
            {
                fFloorHeights[i] = z + 2.5f;
                fCeilHeights[i] = z - 2.5f;
            }
        }

        public override void Render()
        {
            
        }
    }
}
