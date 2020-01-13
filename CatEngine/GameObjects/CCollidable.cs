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

            float scale = 0.5f;

            string fileName = "whomp";

            string vertName = fileName + ".bin";

            if (File.Exists(vertName))
            {
                using (FileStream stream = new FileStream(vertName, FileMode.Open))
                {  
                    CConsole.Instance.Print("reading vertex data from file " + vertName);

                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int iVertices = (int)reader.ReadInt32();
                        Console.WriteLine(iVertices.ToString() + " vertices");

                        for (int i = 0; i < iVertices; i++)
                        {
                            double val1 = reader.ReadDouble();
                            double val2 = reader.ReadDouble();
                            double val3 = reader.ReadDouble();

                            Vector3 vec = new Vector3(((float)val1 * scale) + x, ((float)val2 * scale) + z, ((float)val3 * scale) + y);

                            fVectors.Add(vec);
                            Console.WriteLine(vec.ToString());
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("could not find vertdata!");
            }

            for (int i = 0; i < fVectors.Count; i+=3)
            {
                sTriangles.Add(new Triangle(fVectors[i], fVectors[i + 1], fVectors[i + 2]));
            }
        }

        public override void Render()
        {
            foreach (Triangle tri in sTriangles)
            {
                CRender.Instance.DrawTriangleWireframe(tri.C1, tri.C2, tri.C3);
            }
        }
    }
}
