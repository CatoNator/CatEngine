using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CatEngine.Content
{
    public class CColliderManager : CContentManager
    {
        public Dictionary<string, ObjectCollider> dColliderDict = new Dictionary<string, ObjectCollider>();

        private CColliderManager()
        {
        }

        //singletoning the singleton
        public static CColliderManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CColliderManager instance = new CColliderManager();
        }

        public void LoadCollider(string path, string colliderName)
        {
            ObjectCollider col = new ObjectCollider();
            col.LoadCollider(path, colliderName);

            dColliderDict.Add(colliderName, col);
        }

        public float GetFloorHeightAt(string colliderName, float x, float y, float z)
        {
            float height = 0.0f;

            try
            {
                height = dColliderDict[colliderName].GetFloorHeightAt(x, y, z);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("collider " + colliderName + " not loaded! " + e.Message);
            }

            return height;
        }

        public Vector3 PointInWall(string colliderName, Vector3 colliderPos, float scale, Vector3 point, float height, float rad)
        {
            Vector3 snap = new Vector3(0, 0, 0);

            try
            {
                //dColliderDict[colliderName].UpdateCollider(colliderPos, scale);
                snap = dColliderDict[colliderName].PointInWall(point, rad, height, scale);
                //dColliderDict[colliderName].UpdateCollider(-colliderPos, 1/scale);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("collider " + colliderName + " not loaded! " + e.Message);
            }

            return snap;
        }

        public void UpdateCollider(string colliderName, Vector3 pos, float scale)
        {
            try
            {
                //dColliderDict[colliderName].UpdateCollider(pos, scale);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("collider " + colliderName + " not loaded! " + e.Message);
            }
        }

        public void RenderCollider(string colliderName, Vector3 pos, float scale)
        {
            try
            {
                dColliderDict[colliderName].RenderCollider(pos, scale);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("collider " + colliderName + " not loaded! " + e.Message);
            }
        }


    }

    class Triangle
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

        public void UpdateTri(Vector3 Pos, float Scale)
        {
            C1 = Pos + C1 * Scale;
            C2 = Pos + C2 * Scale;
            C3 = Pos + C3 * Scale; 
        }
    }

    public class ObjectCollider
    {
        private List<Triangle> Ceilings = new List<Triangle>();
        private List<Triangle> Floors = new List<Triangle>();
        private List<Triangle> Walls = new List<Triangle>();

        const float fCollisionBufferSize = 2.0f;

        float fScale = 1.0f;

        public ObjectCollider()
        {

        }

        public void LoadCollider(string path, string colliderName)
        {
            string vertName = path + "/" + colliderName + ".bin";

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
                        CConsole.Instance.Print("CColliderManager: loaded " + iVertices.ToString() + " vertices of collision data");

                        //we make vectors out of the values
                        for (int i = 0; i < iVertices; i++)
                        {
                            double val1 = reader.ReadDouble();
                            double val2 = reader.ReadDouble();
                            double val3 = reader.ReadDouble();

                            Vector3 vec = new Vector3(((float)val1 * fScale), ((float)val2 * fScale), ((float)val3 * fScale));

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

        public void UpdateCollider(Vector3 pos, float scale)
        {
            foreach (Triangle tri in Ceilings)
            {
                tri.UpdateTri(pos, scale);
            }

            foreach (Triangle tri in Floors)
            {
                tri.UpdateTri(pos, scale);
            }

            foreach (Triangle tri in Walls)
            {
                tri.UpdateTri(pos, scale);
            }
        }

        public void RenderCollider(Vector3 pos, float scale)
        {
            foreach (Triangle tri in Ceilings)
            {
                CRender.Instance.DrawTriangleWireframe(pos + tri.C1 * scale, pos + tri.C2 * scale, pos + tri.C3 * scale, Color.Red);
            }

            foreach (Triangle tri in Floors)
            {
                CRender.Instance.DrawTriangleWireframe(pos + tri.C1 * scale, pos + tri.C2 * scale, pos + tri.C3 * scale, Color.Yellow);
            }

            foreach (Triangle tri in Walls)
            {
                CRender.Instance.DrawTriangleWireframe(pos + tri.C1 * scale, pos + tri.C2 * scale, pos + tri.C3 * scale, Color.Blue);
            }
        }

        public float GetFloorHeightAt(float x, float y, float z)
        {
            float Height = 0.0f;

            //make a list of possible floors the player could stand on
            List<Triangle> possibleTris = new List<Triangle>();
            List<float> possibleHeights = new List<float>();

            foreach (Triangle tri in Floors)
            {
                if (tri.PointInTriangle(new Vector2(x, y)))
                {
                    possibleHeights.Add(tri.HeightAt(new Vector2(x, y)));
                    possibleTris.Add(tri);
                    Console.WriteLine("point is in tri");
                }
            }

            //we loop through the floor candidates and select the one below the player and closes to the player
            if (possibleHeights.Count > 0)
            {
                //remove the ones above the player
                float min = z;

                //foreach (float f in possibleHeights)
                for (int i = 0; i < possibleHeights.Count; i++)
                {
                    float f = possibleHeights[i];
                    float diff = z - f;

                    //Console.WriteLine(f + " " + z + " " + diff);

                    if (diff >= -fCollisionBufferSize && diff < min) //
                    {
                        Height = f;
                        min = diff;
                    }
                }

                //Height = min;
            }

            return Height;
        }

        public Vector3 PointInWall(Vector3 position, float rad, float height, float scale)
        {
            Vector3 snap = new Vector3(0, 0, 0);

            //Console.WriteLine("checking for collision in cell");

            foreach (Triangle tri in Walls)
            {
                if (tri.PointInWall(new Vector3(position.X, position.Z, position.Y), rad, height))
                {
                    snap = tri.GetNormal();
                    //Console.WriteLine("point " + position.X + " " + position.Z + " in wall");
                }
            }

            return snap;//InWall;
        }

        public void Unload()
        {
            Floors.Clear();
            Ceilings.Clear();
            Walls.Clear();
        }
    }
}
