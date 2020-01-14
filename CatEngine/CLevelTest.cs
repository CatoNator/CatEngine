using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine.Content
{
    public class CLevelTest : CContentManager
    {
        private List<String> sPropName = new List<String>();
        private List<String> sPropSprite = new List<String>();
        private List<int> sPropColW = new List<int>();
        private List<int> sPropColH = new List<int>();
        private List<int> sPropHealth = new List<int>();

        //this is the max levelsize in cells per direction. Max levelsize is thus 255x255 cells = 65,025‬ cells
        public const int MAX_LEVELSIZE = 256;
        public static int CELL_SIZE = 400;

        private int activeCellX = 0;
        private int activeCellY = 0;

        //private FloorTile[,] oFloorTileArray = new FloorTile[MAX_LEVELSIZE, MAX_LEVELSIZE];

        private Cell[,] LevelCells = new Cell[MAX_LEVELSIZE, MAX_LEVELSIZE];

        private String[] sLevelTextures = new String[] { "grasstop", "grass_path_side" };

        private CLevelTest()
        {
        }

        //singletoning the singleton
        public static CLevelTest Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CLevelTest instance = new CLevelTest();
        }

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

            //(Point A, Point B, Point C, Point P)
            public bool PointInTriangle(Vector2 point)
            {
                float s1 = C3.Z - C1.Z;
                float s2 = C3.X - C1.X;
                float s3 = C2.Z - C1.Z;
                float s4 = point.Y - C1.Z;

                float w1 = (C1.X * s1 + s4 * s2 - point.X * s1) / (s3 * s2 - (C2.X - C1.X) * s1);
                float w2 = (s4 - w1 * s3) / s1;
                return w1 >= 0 && w2 >= 0 && (w1 + w2) <= 1;
            }

            public float HeightAt(Vector2 point)
            {
                float h = 0.0f;
                float s1 = C3.Z - C1.Z;
                float s2 = C3.X - C1.X;
                float s3 = C2.Z - C1.Z;
                float s4 = point.Y - C1.Z;

                float w1 = (C1.X * s1 + s4 * s2 - point.X * s1) / (s3 * s2 - (C2.X - C1.X) * s1);
                float w2 = (s4 - w1 * s3) / s1;

                //Console.WriteLine("w1 " + w1 + " w2 " + w2);

                float a1 = (C1.Y * (1.0f - w1)) + (C2.Y * w1);
                float a2 = (C1.Y * (1.0f - w2)) + (C3.Y * w2);

                h = (a1 + a2)/2.0f;

                return h;
            }
        }

        private class Cell
        {
            public List<Triangle> Ceilings = new List<Triangle>();
            public List<Triangle> Floors = new List<Triangle>();
            public List<Triangle> Walls = new List<Triangle>();

            public Cell()
            {

            }

            public void RenderCell()
            {
                foreach (Triangle tri in Ceilings)
                {
                    //CRender.Instance.DrawTriangleWireframe(tri.C1, tri.C2, tri.C3, Color.Red);
                    CRender.Instance.DrawTriangleTextured(tri.C1, tri.C2, tri.C3, Color.Red);
                }

                foreach (Triangle tri in Floors)
                {
                    //CRender.Instance.DrawTriangleWireframe(tri.C1, tri.C2, tri.C3, Color.Green);
                    CRender.Instance.DrawTriangleTextured(tri.C1, tri.C2, tri.C3, Color.Green);
                }

                foreach (Triangle tri in Walls)
                {
                    //CRender.Instance.DrawTriangleWireframe(tri.C1, tri.C2, tri.C3, Color.Green);
                    CRender.Instance.DrawTriangleTextured(tri.C1, tri.C2, tri.C3, Color.Blue);
                }
            }

            public float GetFloorHeightAt(float x, float y)
            {
                float Height = 0.0f;

                foreach (Triangle tri in Floors)
                {
                    if (tri.PointInTriangle(new Vector2(x, y)))
                    {
                        Height = tri.HeightAt(new Vector2(x, y));
                        //Console.WriteLine("point is in tri");
                    }
                }

                return Height;
            }
        }

            //rendering loop
        public void Render()
        {
            for (int i = 0; i < MAX_LEVELSIZE; i++)
            {
                for (int a = 0; a < MAX_LEVELSIZE; a++)
                {
                    if (LevelCells[i, a] != null && i == activeCellX && a == activeCellY)
                    {
                        LevelCells[i, a].RenderCell();
                    }
                }
            }
        }

        //public float to return the linearly interpolated height in a tile
        public float GetHeightAt(float x, float y)
        {
            int CellX = (int)(x / CELL_SIZE);
            int CellY = (int)(x / CELL_SIZE);

            float Height = 0.0f;

            if ((CellX >= 0 && CellX < MAX_LEVELSIZE)
                && (CellY >= 0 && CellY < MAX_LEVELSIZE)
                && LevelCells[CellX, CellY] != null)
            {
                Height = LevelCells[CellX, CellY].GetFloorHeightAt(x, y);
            }
            /*else
                Console.WriteLine("collision wasn't in a cell dumbass");*/

            return Height;
        }

        public void UpdateActiveCell(float x, float y)
        {
            activeCellX = (int)(x / CELL_SIZE);
            activeCellY = (int)(y / CELL_SIZE);
        }

        public void LoadPropData()
        {
            CConsole.Instance.Print("Opening prop data");
            string xmlText = System.IO.File.ReadAllText("AssetData/PropData.xml");
            XDocument file = XDocument.Parse(xmlText);

            foreach (XElement element in file.Descendants("prop"))
            {
                sPropName.Add(element.Element("name").Value);
                sPropSprite.Add(element.Element("sprite").Value);
                sPropColW.Add(Int32.Parse(element.Element("col_width").Value));
                sPropColH.Add(Int32.Parse(element.Element("col_height").Value));
                sPropHealth.Add(Int32.Parse(element.Element("health").Value));
            }
        }

        public void LoadTerrainData(string levelName)
        {
            //GenerateLevel();
            //SaveTerrain();

            //we load the collision data
            float scale = 0.5f;

            string vertName = levelName + ".bin";

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
                        Console.WriteLine(iVertices.ToString() + " vertices");

                        //we make vectors out of the values
                        for (int i = 0; i < iVertices; i++)
                        {
                            double val1 = reader.ReadDouble();
                            double val2 = reader.ReadDouble();
                            double val3 = reader.ReadDouble();

                            Vector3 vec = new Vector3(((float)val1 * scale), ((float)val2 * scale), -((float)val3 * scale));

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

                //we loop through the cells
                for (int a = 0; a < MAX_LEVELSIZE; a++)
                {
                    for (int e = 0; e < MAX_LEVELSIZE; e++)
                    {
                        //looping through the corners
                        for (int o = 0; o < 3; o++)
                        {
                            //if one of the corners exists within a cell, we add it to the list
                            if ((fVectors[i + o].X >= a*CELL_SIZE && fVectors[i + o].X <= (a + 1) * CELL_SIZE)
                                && (fVectors[i + o].Z >= e * CELL_SIZE && fVectors[i + o].Z <= (e + 1) * CELL_SIZE))
                            {
                                //if the cell doesn't exist we make it
                                if (LevelCells[a, e] == null)
                                {
                                    LevelCells[a, e] = new Cell();
                                }

                                Vector3 normal = tri.GetNormal();

                                float normalLimit = 0.05f;

                                //REMEMBER TO FIX ME
                                if (normal.Y > normalLimit) //add to ceilings if the normal is pointing down
                                {
                                    if (!LevelCells[a, e].Ceilings.Contains(tri))
                                        LevelCells[a, e].Ceilings.Add(tri);
                                }
                                else if (normal.Y < -normalLimit)//add to floors if the normal is pointing upwards
                                {
                                    //if we haven't indexed the tri here already we add it
                                    if (!LevelCells[a, e].Floors.Contains(tri))
                                        LevelCells[a, e].Floors.Add(tri);
                                }
                                else //if it's neither a ceiling or a floor, it's a wall
                                {
                                    if (!LevelCells[a, e].Walls.Contains(tri))
                                        LevelCells[a, e].Walls.Add(tri);
                                }
                                    
                            }
                        }
                    }
                }
            }
        }

        public void SetTextureArray(string[] textureArray)
        {
            sLevelTextures = textureArray;
        }
    }
}
