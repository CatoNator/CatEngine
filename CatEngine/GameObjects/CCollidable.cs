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

        private List<float> fVertexes = new List<float>();

        public Vector2 fSize = new Vector2(5, 5);

        public override void InstanceSpawn()
        {
            for (int i = 0; i < 4; i++)
            {
                fFloorHeights[i] = z + 2.5f;
                fCeilHeights[i] = z - 2.5f;
            }

            string fileName = "cube.bin";

            if (File.Exists(fileName))
            {
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    CConsole.Instance.Print("reading model data from file " + fileName);

                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int iVertices = (int)reader.ReadInt32();
                        Console.WriteLine(iVertices.ToString() + " vertices");

                        for (int i = 0; i < iVertices; i++)
                        {
                            double val = reader.ReadDouble();
                            Console.WriteLine(val.ToString());
                            fVertexes.Add((float)val);
                        }
                    }
                }
            }


        }

        public override void Render()
        {
            List<Vector3> arr = new List<Vector3>();

            for (int i = 0; i < fVertexes.Count(); i += 3)
            {
                if (i + 2 < fVertexes.Count)
                {
                    Vector3 val = new Vector3(x + fVertexes[i], z + fVertexes[i + 1], y + fVertexes[i + 2]);

                    //Console.WriteLine("added vector3 to arr " + val.ToString());
                    arr.Add(val);
                }
            }

            for (int i = 0; i < arr.Count(); i++)
            {
                if (i + 3 < arr.Count)
                {
                    CRender.Instance.DrawTriangleWireframe(arr[i], arr[i + 1], arr[i + 2]);
                }
            }
        }
    }
}
