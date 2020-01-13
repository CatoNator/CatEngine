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
        public static int CELL_SIZE = 10;

        private FloorTile[,] oFloorTileArray = new FloorTile[MAX_LEVELSIZE, MAX_LEVELSIZE];

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
        }

        private class Cell
        {
            public List<Triangle> Floors = new List<Triangle>();
        }

        private class FloorTile
        {
            public int iTileSize;
            public int[] iPosition = new int[2];
            public float[] fCornerHeights = new float[4];

            public float fAverageHeight = 10;

            //the texture indexes for whatever textures the tile needs to render
            //the one visible on tope of the tile
            private int tileTextureIndex = 0;

            //side textures, these ones are only visible in special cases
            private int topWallTextureIndex = 0;
            private int leftWallTextureIndex = 0;

            //these ones are basically always visible
            private int rightWallTextureIndex = 0;
            private int bottomWallTextureIndex = 0;

            /*
            0-----1
            |     |
            |     |
            2-----3
            */

            public FloorTile()
            {

            }

            public void SetProperties(int x, int y, int tileSize, float[] cornerHeights)
            {
                iPosition[0] = x;
                iPosition[1] = y;
                iTileSize = tileSize;

                fCornerHeights = cornerHeights;
            }

            public void SetTextures(int tileTex, int topWallTex, int LWallTex, int RWallTex, int BWallTex)
            {
                tileTextureIndex = tileTex;

                topWallTextureIndex = topWallTex;
                leftWallTextureIndex = LWallTex;

                rightWallTextureIndex = RWallTex;
                bottomWallTextureIndex = BWallTex;
            }

            /*
                 0
              |-----|
            1 |     | 3
              |-----|
                 2
            */


            public void RenderTile(String[] textureArray)
            {
                
            }

            public float GetHeightAt(float x, float y)
            {
                //the values
                float q11 = fCornerHeights[0];
                float q12 = fCornerHeights[2];
                float q21 = fCornerHeights[1];
                float q22 = fCornerHeights[3];

                //corner positions
                float x1 = 0;
                float y1 = 0;
                float x2 = (float)iTileSize;
                float y2 = (float)iTileSize;

                //bilinear interpolation
                float x2x1, y2y1, x2x, y2y, yy1, xx1;
                x2x1 = x2 - x1;
                y2y1 = y2 - y1;
                x2x = x2 - x;
                y2y = y2 - y;
                yy1 = y - y1;
                xx1 = x - x1;
                return 1.0f / (x2x1 * y2y1) * (
                    q11 * x2x * y2y +
                    q21 * xx1 * y2y +
                    q12 * x2x * yy1 +
                    q22 * xx1 * yy1
                );
            }
        }


        public void SetCornerHeigth(int tileX, int tileY, float[] corners)
        {
            if ((tileX >= 0 && tileX < MAX_LEVELSIZE) && (tileY >= 0 && tileY < MAX_LEVELSIZE))
            {
                if (oFloorTileArray[tileX, tileY] == null)
                    oFloorTileArray[tileX, tileY] = new FloorTile();

                oFloorTileArray[tileX, tileY].SetProperties(tileX, tileY, CELL_SIZE, corners);

                float combinedCorners = 0.0f;

                for (int e = 0; e < 4; e++)
                {
                    combinedCorners += corners[e];
                }

                if (combinedCorners <= 0)
                {
                    oFloorTileArray[tileX, tileY] = null;
                }
            }
                
        }

        public void SetTextures(int tileX, int tileY, int tileTex, int topWallTex, int LWallTex, int RWallTex, int BWallTex)
        {
            if ((tileX >= 0 && tileX < MAX_LEVELSIZE) && (tileY >= 0 && tileY < MAX_LEVELSIZE) && oFloorTileArray[tileX, tileY] != null)
                oFloorTileArray[tileX, tileY].SetTextures(tileTex, topWallTex, LWallTex, RWallTex, BWallTex);
        }

            //rendering loop
            public void Render()
        {
            for (int i = 0; i < MAX_LEVELSIZE; i++)
            {
                for (int a = 0; a < MAX_LEVELSIZE; a++)
                {
                    if (oFloorTileArray[i, a] != null)
                    {
                        oFloorTileArray[i, a].RenderTile(sLevelTextures);
                    }
                }
            }
        }

        //public float to return the linearly interpolated height in a tile
        public float GetMapHeightAt(float x, float y)
        {
            float height = 0.0f;

            int tileX = (int)x / CELL_SIZE;
            int tileY = (int)y / CELL_SIZE;

            if ((tileX >= 0 && tileX < MAX_LEVELSIZE)
               && (tileY >= 0 && tileY < MAX_LEVELSIZE)
               && oFloorTileArray[tileX, tileY] != null)
            {
                float xInTile = x % CELL_SIZE;
                float yInTile = y % CELL_SIZE;
                height = oFloorTileArray[tileX, tileY].GetHeightAt(xInTile, yInTile);
            }

            return height;
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

            if (File.Exists(levelName))
            {
                using (FileStream stream = new FileStream(levelName, FileMode.Open))
                {
                    CConsole.Instance.Print("reading level data from file " + levelName);

                    using (BinaryReader reader = new BinaryReader(stream))
                    {
                        //CELL_SIZE = (int)reader.ReadByte();
                        //MAX_LEVELSIZE = (int)reader.ReadByte();
                        //MAX_LEVELSIZE = (int)reader.ReadByte();

                        //padding, "TILEDATABEGIN"
                        for (int i= 0; i < 13; i++)
                        {
                            int padding = reader.ReadByte();
                        }

                        for (int i = 0; i < MAX_LEVELSIZE; i++)
                        {
                            for (int a = 0; a < MAX_LEVELSIZE; a++)
                            {
                                float[] cornerHeights = new float[4];
                                float combinedCorners = 0;

                                for(int e = 0; e < 4; e++)
                                {
                                    cornerHeights[e] = (float)reader.ReadDouble();
                                    combinedCorners += cornerHeights[e];
                                }

                                if (combinedCorners > 0)
                                {
                                    oFloorTileArray[i, a] = new FloorTile();
                                    oFloorTileArray[i, a].SetProperties(i, a, CELL_SIZE, cornerHeights);
                                }
                            }
                        }

                        //more padding, "TEXDATABEGIN"
                        for (int i = 0; i < 12; i++)
                        {
                            int padding = reader.ReadByte();
                        }

                        //texture data
                        for (int i = 0; i < MAX_LEVELSIZE; i++)
                        {
                            for (int a = 0; a < MAX_LEVELSIZE; a++)
                            {
                                if (oFloorTileArray[i, a] != null)
                                {
                                    int iTileTex = reader.ReadByte();
                                    int iTopWallTex = reader.ReadByte();
                                    int iLWallTex = reader.ReadByte();
                                    int iRWallTex = reader.ReadByte();
                                    int iBWallTex = reader.ReadByte();

                                    oFloorTileArray[i, a].SetTextures(iTileTex, iTopWallTex, iLWallTex, iRWallTex, iBWallTex);
                                }
                            }
                        }

                        reader.Close();
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
