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
        private List<Triangle> Floors = new List<Triangle>();
        private List<Triangle> Walls = new List<Triangle>();
        private List<Triangle> Ceilings = new List<Triangle>();

        private float fScale = 10.0f;

        public Vector3 Speed = new Vector3(0, 0, 0);
        public Vector3 RotationSpeed = new Vector3(0, 0, 0);

        public Vector3 prevPos = new Vector3(0, 0, 0);

        public Vector3 Rotation = new Vector3(0, 0, 0);

        private string sModelName = "hexatri";

        private class Triangle
        {
            public Vector3 C1;
            public Vector3 C2;
            public Vector3 C3;

            public bool isActive = false;

            public Triangle(Vector3 V1, Vector3 V2, Vector3 V3)
            {
                C1 = V1;
                C2 = V2;
                C3 = V3;
            }

            public Vector3 GetNormal()
            {
                //subtract the vectors
                Vector3 ab = C1 - C3;
                Vector3 cb = C2 - C3;

                ab.Normalize();
                cb.Normalize();
                //get a vector perpendicular to those two edges
                return Vector3.Cross(ab, cb);
            }

            public bool PointInTriangle(Vector2 point)
            {
                float w1 = ((C2.Z - C3.Z) * (point.X - C3.X) + (C3.X - C2.X) * (point.Y - C3.Z)) / ((C2.Z - C3.Z) * (C1.X - C3.X) + (C3.X - C2.X) * (C1.Z - C3.Z));

                float w2 = ((C3.Z - C1.Z) * (point.X - C3.X) + (C1.X - C3.X) * (point.Y - C3.Z)) / ((C2.Z - C3.Z) * (C1.X - C3.X) + (C3.X - C2.X) * (C1.Z - C3.Z));

                return w1 >= 0 && w2 >= 0 && (w1 + w2) <= 1;
            }

            public float HeightAt(Vector2 point)
            {
                float h = 0.0f;
                float w1 = ((C2.Z - C3.Z) * (point.X - C3.X) + (C3.X - C2.X) * (point.Y - C3.Z)) / ((C2.Z - C3.Z) * (C1.X - C3.X) + (C3.X - C2.X) * (C1.Z - C3.Z));

                float w2 = ((C3.Z - C1.Z) * (point.X - C3.X) + (C1.X - C3.X) * (point.Y - C3.Z)) / ((C2.Z - C3.Z) * (C1.X - C3.X) + (C3.X - C2.X) * (C1.Z - C3.Z));

                float w3 = 1 - w1 - w2;

                h = (w1 * C1.Y) + (w2 * C2.Y) + (w3 * C3.Y);

                return h;
            }

            private bool PointInCustomTri(Vector2 TC1, Vector2 TC2, Vector2 TC3, Vector2 point)
            {
                float w1 = ((TC2.Y - TC3.Y) * (point.X - TC3.X) + (TC3.X - TC2.X) * (point.Y - TC3.Y)) / ((TC2.Y - TC3.Y) * (TC1.X - TC3.X) + (TC3.X - TC2.X) * (TC1.Y - TC3.Y));

                float w2 = ((TC3.Y - TC1.Y) * (point.X - TC3.X) + (TC1.X - TC3.X) * (point.Y - TC3.Y)) / ((TC2.Y - TC3.Y) * (TC1.X - TC3.X) + (TC3.X - TC2.X) * (TC1.Y - TC3.Y));

                return w1 >= 0 && w2 >= 0 && (w1 + w2) <= 1;
            }

            public bool PointInWall(Vector3 point, float rad, float h)
            {
                bool inWall = false;

                int normalDir = (Math.Abs(GetNormal().X) > Math.Abs(GetNormal().Z) ? 1 : 0);

                float origX = Math.Min(Math.Min(C1.X, C2.X), C3.X);
                float origY = Math.Min(Math.Min(C1.Y, C2.Y), C3.Y);
                float origZ = Math.Min(Math.Min(C1.Z, C2.Z), C3.Z);

                float width = Math.Max(Math.Max(C1.X, C2.X), C3.X) - origX;
                float height = Math.Max(Math.Max(C1.Y, C2.Y), C3.Y) - origY;
                float length = Math.Max(Math.Max(C1.Z, C2.Z), C3.Z) - origZ;

                List<Vector2> points = new List<Vector2>();

                Vector2 CA = new Vector2(0, 0);
                Vector2 CB = new Vector2(0, 0);
                Vector2 CC = new Vector2(0, 0);
                Vector2 CD = new Vector2(0, 0);

                List<bool> trueCorners = new List<bool>();

                if ((point.Z + h >= origY && point.Z + h <= origY + height)
                    || (point.Z >= origY && point.Z <= origY + height))
                {
                    if (normalDir == 0)
                    {
                        CA = new Vector2(origX, origZ - rad);
                        CB = new Vector2(origX, origZ + rad);
                        CC = new Vector2(origX + width, origZ + length + rad);
                        CD = new Vector2(origX + width, origZ + length - rad);
                    }
                    else if (normalDir == 1)
                    {
                        //the points
                        CA = new Vector2(origX - rad, origZ);
                        CB = new Vector2(origX + rad, origZ);
                        CC = new Vector2(origX + width + rad, origZ + length);
                        CD = new Vector2(origX + width - rad, origZ + length);
                    }

                    if (PointInCustomTri(CA, CC, CB, new Vector2(point.X, point.Y)) || PointInCustomTri(CB, CC, CD, new Vector2(point.X, point.Y)))
                        inWall = true;
                }

                return inWall;
            }

            public void UpdateTri(Vector3 Speed)
            {
                C1 = new Vector3(C1.X + Speed.X, C1.Y + Speed.Z, C1.Z + Speed.Y);
                C2 = new Vector3(C2.X + Speed.X, C2.Y + Speed.Z, C2.Z + Speed.Y);
                C3 = new Vector3(C3.X + Speed.X, C3.Y + Speed.Z, C3.Z + Speed.Y);
            }
        }

        public override void InstanceSpawn()
        {
            TempLoadCollision("hexatri");
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
            UpdateTriangles();
        }

        private void UpdateTriangles()
        {
            foreach (Triangle tri in Ceilings)
            {
                tri.UpdateTri(Speed);
            }

            foreach (Triangle tri in Floors)
            {
                tri.UpdateTri(Speed);
            }

            foreach (Triangle tri in Walls)
            {
                tri.UpdateTri(Speed);
            }
        }

        //there is absolutely no way this can be loaded here, we'd need to load the collision for every single instance of a prop! should be a pointer to somewhere instead
        private void TempLoadCollision(string collisionName)
        {
            string vertName = "AssetData/Props/"+collisionName+"/"+collisionName+".bin";

            List<Vector3> fVectors = new List<Vector3>();

            //checkie
            if (File.Exists(vertName))
            {
                //open sesame
                using (FileStream stream = new FileStream(vertName, FileMode.Open))
                {
                    CConsole.Instance.Print("reading vertex data from file " + vertName);

                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        int iVertices = (int)reader.ReadInt32();
                        CConsole.Instance.Print("CCollidable: loaded " + iVertices.ToString() + " vertices of collision data");

                        //we make vectors out of the values
                        for (int i = 0; i < iVertices; i++)
                        {
                            double val1 = reader.ReadDouble();
                            double val2 = reader.ReadDouble();
                            double val3 = reader.ReadDouble();

                            Vector3 vec = new Vector3(x+((float)val1 * fScale), z+((float)val2 * fScale), y+((float)val3 * fScale));

                            fVectors.Add(vec);
                            //Console.WriteLine(vec.ToString());
                        }
                    }
                }
            }
            else //fuck
            {
                Console.WriteLine("could not find vertdata!");
            }

            //now the fuck
            for (int i = 0; i < fVectors.Count; i += 3)
            {
                //we make a triangle
                Triangle tri = new Triangle(fVectors[i], fVectors[i + 1], fVectors[i + 2]);

                //the triangle is sorted by its normal. 
                Vector3 normal = tri.GetNormal();

                float normalLimit = 0.05f;

                if (normal.Y < -normalLimit) //add to ceilings if the normal is pointing down
                {
                    if (!Ceilings.Contains(tri))
                        Ceilings.Add(tri);
                }
                else if (normal.Y > normalLimit)//add to floors if the normal is pointing upwards
                {
                    //if we haven't indexed the tri here already we add it
                    if (!Floors.Contains(tri))
                        Floors.Add(tri);
                }
                else //if it's neither a ceiling or a floor, it's a wall
                {
                    if (!Walls.Contains(tri))
                        Walls.Add(tri);
                }
            }
        }

        public float GetHeightAt(float x, float y)
        {
            float h = 0.0f;

            foreach (Triangle tri in Floors)
            {
                if (tri.PointInTriangle(new Vector2(x, y)))
                    h = tri.HeightAt(new Vector2(x, y));
            }

            return h;
        }

        public Vector3 PointInWall(Vector3 position, float rad, float height)
        {
            bool InWall = false;

            Vector3 snap = new Vector3(0, 0, 0);

            //Console.WriteLine("checking for collision in cell");

            foreach (Triangle tri in Walls)
            {
                if (tri.PointInWall(new Vector3(position.X, position.Z, position.Y), rad, height))
                {
                    InWall = true;
                    snap = tri.GetNormal();
                    //Console.WriteLine("point " + position.X + " " + position.Z + " in wall");
                }
            }

            return snap;//InWall;
        }

        public override void Render()
        {
            CRender.Instance.DrawSimpleModel(sModelName, new Vector3(x, z, y), Rotation, fScale);

            if (CDebug.Instance.ShowTerrainDebug)
                RenderCollision();
        }

        private void RenderCollision()
        {
            foreach (Triangle tri in Ceilings)
            {
                CRender.Instance.DrawTriangleWireframe(tri.C1, tri.C2, tri.C3, Color.Red);
                //CRender.Instance.DrawTriangleTextured(tri.C1, tri.C2, tri.C3, Color.Red);
            }

            foreach (Triangle tri in Floors)
            {
                if (tri.isActive)
                {
                    CRender.Instance.DrawTriangleTextured(tri.C1, tri.C2, tri.C3, Color.Yellow);
                    Triangle t_tri = tri;

                    tri.isActive = false;
                }
                else
                {
                    CRender.Instance.DrawTriangleWireframe(tri.C1, tri.C2, tri.C3, Color.Black);
                    //CRender.Instance.DrawTriangleTextured(tri.C1, tri.C2, tri.C3, Color.Green);
                }
            }

            foreach (Triangle tri in Walls)
            {
                CRender.Instance.DrawTriangleWireframe(tri.C1, tri.C2, tri.C3, Color.Blue);
                //CRender.Instance.DrawTriangleTextured(tri.C1, tri.C2, tri.C3, Color.Blue);
            }
        }
    }
}
