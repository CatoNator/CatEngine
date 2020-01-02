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

namespace CatEngine
{
    public class CLevel : CContentManager
    {
        private List<String> sPropName = new List<String>();
        private List<String> sPropSprite = new List<String>();
        private List<int> sPropColW = new List<int>();
        private List<int> sPropColH = new List<int>();
        private List<int> sPropHealth = new List<int>();

        public const int MAX_LEVELSIZE = 256;
        public int iLevelWidth = 4;
        public int iLevelHeight = 4;
        public int iTileSize = 10;

        private FloorTile[,] oFloorTileArray = new FloorTile[MAX_LEVELSIZE, MAX_LEVELSIZE];

        private String[] sLevelTextures = new String[] { "grasstop", "grass_path_side" };

        //public GraphicsDevice graphicsDevice;

        /*<name>PropCopCar</name>
		<sprite>sprCopCar</sprite>
		<col_width>97</col_width>
		<col_height>44</col_height>
		<health>-1</health>*/

        private CLevel()
        {
        }

        //singletoning the singleton
        public static CLevel Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CLevel instance = new CLevel();
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
                //CRender.Instance.DrawTile(graphicsDevice, iPosition, fCornerHeights, iTileSize);

                CRender.Instance.DrawRectangle(new Vector3(iPosition[0] * iTileSize, fCornerHeights[2], iPosition[1] * iTileSize + iTileSize),
                    new Vector3(iPosition[0] * iTileSize + iTileSize, fCornerHeights[3], iPosition[1] * iTileSize + iTileSize),
                    new Vector3(iPosition[0] * iTileSize, fCornerHeights[0], iPosition[1] * iTileSize),
                    new Vector3(iPosition[0] * iTileSize + iTileSize, fCornerHeights[1], iPosition[1] * iTileSize), textureArray[tileTextureIndex], false);

                int iLevelHeight = CLevel.Instance.iLevelHeight;
                int iLevelWidth = CLevel.Instance.iLevelWidth;
                FloorTile[,] oFloorTileArray = CLevel.Instance.oFloorTileArray;

                int i = iPosition[0];
                int a = iPosition[1];

                //rendering the wall below us
                if (a >= 0 && a < iLevelHeight - 1)
                {
                    FloorTile currentTile = oFloorTileArray[i, a];
                    FloorTile belowTile = oFloorTileArray[i, a + 1];

                    CRender.Instance.DrawRectangle(new Vector3(i * iTileSize, belowTile.fCornerHeights[0], (a + 1) * iTileSize),
                        new Vector3((i + 1) * iTileSize, belowTile.fCornerHeights[1], (a + 1) * iTileSize),
                        new Vector3(i * iTileSize, currentTile.fCornerHeights[2], (a + 1) * iTileSize),
                        new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[3], (a + 1) * iTileSize), textureArray[bottomWallTextureIndex], false);
                }

                //and the wall to the right of us
                if (i >= 0 && i < iLevelWidth - 1)
                {
                    FloorTile currentTile = oFloorTileArray[i, a];
                    FloorTile nextTile = oFloorTileArray[i + 1, a];

                    CRender.Instance.DrawRectangle(new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[1], a * iTileSize),
                        new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[3], (a + 1) * iTileSize),
                        new Vector3((i + 1) * iTileSize, nextTile.fCornerHeights[0], a * iTileSize),
                        new Vector3((i + 1) * iTileSize, nextTile.fCornerHeights[2], (a + 1) * iTileSize), textureArray[rightWallTextureIndex], false);
                }

                //if we're on the leftmost row, let's render a top wall
                if (i == 0)
                {
                    FloorTile currentTile = oFloorTileArray[i, a];
                    CRender.Instance.DrawRectangle(new Vector3(i, 0, a * iTileSize),
                        new Vector3(i, 0, (a + 1) * iTileSize),
                        new Vector3(i, currentTile.fCornerHeights[0], a * iTileSize),
                        new Vector3(i, currentTile.fCornerHeights[2], (a + 1) * iTileSize), textureArray[leftWallTextureIndex], true);
                }

                //if we're on the top row, let's draw a top wall
                if (a == 0)
                {
                    FloorTile currentTile = oFloorTileArray[i, a];
                    CRender.Instance.DrawRectangle(new Vector3(i * iTileSize, currentTile.fCornerHeights[0], a),
                        new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[1], a),
                        new Vector3(i * iTileSize, 0, a),
                        new Vector3((i + 1) * iTileSize, 0, a), textureArray[topWallTextureIndex], false);
                }

                //if we're on the rightmost row, let's render a right wall
                if (i == iLevelWidth - 1)
                {
                    FloorTile currentTile = oFloorTileArray[i, a];
                    CRender.Instance.DrawRectangle(new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[1], a * iTileSize),
                        new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[3], (a + 1) * iTileSize),
                        new Vector3((i + 1) * iTileSize, 0, a * iTileSize),
                        new Vector3((i + 1) * iTileSize, 0, (a + 1) * iTileSize), textureArray[rightWallTextureIndex], false);
                }

                //if we're on the bottom row, let's draw a bottom wall
                if (a == iLevelHeight - 1)
                {
                    FloorTile currentTile = oFloorTileArray[i, a];
                    CRender.Instance.DrawRectangle(new Vector3(i * iTileSize, 0, (a + 1) * iTileSize),
                        new Vector3((i + 1) * iTileSize, 0, (a + 1) * iTileSize),
                        new Vector3(i * iTileSize, currentTile.fCornerHeights[2], (a + 1) * iTileSize),
                        new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[3], (a + 1) * iTileSize), textureArray[bottomWallTextureIndex], true);
                }
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

        //testing
        public void GenerateLevel()
        {
            //creating tiles
            for (int i = 0; i < iLevelWidth; i++)
            {
                for (int a = 0; a < iLevelHeight; a++)
                {
                    oFloorTileArray[i, a] = new FloorTile();
                    oFloorTileArray[i, a].SetProperties(i, a, iTileSize, new float[] { 5.0f, 5.0f, 7.0f, 8.0f });
                }
            }
        }

        //also testing
        private void TestSaveLevel()
        {
            //this is where we save the level to a binary file as debug
            using (FileStream stream = new FileStream("test.bin", FileMode.Create))
            {
                using (BinaryWriter writer = new BinaryWriter(stream))
                {
                    writer.Write((byte)iTileSize);
                    writer.Write((byte)iLevelWidth);
                    writer.Write((byte)iLevelHeight);

                    //padding
                    writer.Write((byte)84); //T
                    writer.Write((byte)73); //I
                    writer.Write((byte)76); //L
                    writer.Write((byte)69); //E
                    writer.Write((byte)68); //D
                    writer.Write((byte)65); //A
                    writer.Write((byte)84); //T
                    writer.Write((byte)65); //A
                    writer.Write((byte)66); //B
                    writer.Write((byte)69); //E
                    writer.Write((byte)71); //G
                    writer.Write((byte)73); //I
                    writer.Write((byte)78); //N

                    for (int i = 0; i < iLevelWidth; i++)
                    {
                        for (int a = 0; a < iLevelHeight; a++)
                        {
                            //these need to be floats
                            writer.Write((double)oFloorTileArray[i, a].fCornerHeights[0]);
                            writer.Write((double)oFloorTileArray[i, a].fCornerHeights[1]);
                            writer.Write((double)oFloorTileArray[i, a].fCornerHeights[2]);
                            writer.Write((double)oFloorTileArray[i, a].fCornerHeights[3]);
                        }
                    }

                    writer.Write((byte)84); //T
                    writer.Write((byte)69); //E
                    writer.Write((byte)88); //X
                    writer.Write((byte)68); //D
                    writer.Write((byte)65); //A
                    writer.Write((byte)84); //T
                    writer.Write((byte)65); //A
                    writer.Write((byte)66); //B
                    writer.Write((byte)69); //E
                    writer.Write((byte)71); //G
                    writer.Write((byte)73); //I
                    writer.Write((byte)78); //N

                    //texture data
                    for (int i = 0; i < iLevelWidth; i++)
                    {
                        for (int a = 0; a < iLevelHeight; a++)
                        {
                            //tile
                            writer.Write((byte)1);
                            //top wall
                            writer.Write((byte)0);
                            //left wall
                            writer.Write((byte)0);
                            //right wall
                            writer.Write((byte)0);
                            //bottom wall
                            writer.Write((byte)0);
                        }
                    }

                    writer.Close();
                }
            }
        }

        //rendering loop
        public void Render()
        {
            for (int i = 0; i < iLevelWidth; i++)
            {
                for (int a = 0; a < iLevelHeight; a++)
                {
                    if (oFloorTileArray[i, a] != null)
                    {
                        oFloorTileArray[i, a].RenderTile(sLevelTextures);

                        //let's render the bottom wall
                        
                    }
                }
            }
        }

        //public float to return the linearly interpolated height in a tile
        public float GetMapHeightAt(float x, float y)
        {
            float height = 0.0f;

            int tileX = (int)x / iTileSize;
            int tileY = (int)y / iTileSize;

            if ((tileX >= 0 && tileX < oFloorTileArray.GetLength(0))
               && (tileY >= 0 && tileY < oFloorTileArray.GetLength(1))
               && oFloorTileArray[tileX, tileY] != null)
            {
                float xInTile = x % iTileSize;
                float yInTile = y % iTileSize;
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
            //TestSaveLevel();

            if (File.Exists(levelName))
            {
                using (FileStream stream = new FileStream(levelName, FileMode.Open))
                {
                    CConsole.Instance.Print("reading level data from file " + levelName);

                    using (BinaryReader reader= new BinaryReader(stream))
                    {
                        iTileSize = reader.ReadByte();
                        iLevelWidth = reader.ReadByte();
                        iLevelWidth = reader.ReadByte();

                        //padding, "TILEDATABEGIN"
                        for (int i= 0; i < 13; i++)
                        {
                            int padding = reader.ReadByte();
                        }

                        for (int i = 0; i < iLevelWidth; i++)
                        {
                            for (int a = 0; a < iLevelHeight; a++)
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
                                    oFloorTileArray[i, a].SetProperties(i, a, iTileSize, cornerHeights);
                                }
                            }
                        }

                        //more padding, "TEXDATABEGIN"
                        for (int i = 0; i < 12; i++)
                        {
                            int padding = reader.ReadByte();
                        }

                        //texture data
                        for (int i = 0; i < iLevelWidth; i++)
                        {
                            for (int a = 0; a < iLevelHeight; a++)
                            {
                                int iTileTex = reader.ReadByte();
                                int iTopWallTex = reader.ReadByte();
                                int iLWallTex = reader.ReadByte();
                                int iRWallTex = reader.ReadByte();
                                int iBWallTex = reader.ReadByte();

                                oFloorTileArray[i, a].SetTextures(iTileTex, iTopWallTex, iLWallTex, iRWallTex, iBWallTex);
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
