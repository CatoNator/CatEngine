using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine
{
    class CLevel : CContentManager
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

            //size 4 array for 4 walls. Tuple holds 
            public Tuple<float, float>[] tNeighboringCornerHeights = new Tuple<float, float>[4];

            /*
            0-----1
            |     |
            |     |
            2-----3
            */

            public FloorTile(int x, int y, int tileSize)
            {
                iPosition[0] = x;
                iPosition[1] = y;
                iTileSize = tileSize;

                Random random = new Random((x+y)*tileSize);

                fAverageHeight = 10.0f + (float)random.NextDouble() * 5.0f;

                for (int i = 0; i < 4; i++)
                {
                    fCornerHeights[i] = fAverageHeight -2.5f + (float)random.NextDouble() * 5.0f;
                }

                //fCornerHeights[2] = -fCornerHeights[1];
            }

            /*
                 0
              |-----|
            1 |     | 3
              |-----|
                 2
            */
            public void GenerateWallMap(FloorTile[,] oFloorTileArray)
            {
                for (int i = 0; i < 4; i++)
                {
                    int tileX = iPosition[0];
                    int tileY = iPosition[1];

                    if (i == 0)
                    {
                        tileX = iPosition[0];
                        tileY = iPosition[1]-1;
                    }
                    else if (i == 1)
                    {
                        tileX = iPosition[0] - 1;
                        tileY = iPosition[1];
                    }
                    else if (i == 2)
                    {
                        tileX = iPosition[0] + 1;
                        tileY = iPosition[1];
                    }
                    else if (i == 3)
                    {
                        tileX = iPosition[0];
                        tileY = iPosition[1] + 1;
                    }

                    if ((tileX >= 0 && tileX < oFloorTileArray.GetLength(0))
                       && (tileY >= 0 && tileY < oFloorTileArray.GetLength(1))
                       && oFloorTileArray[tileX, tileY] != null)
                    {
                        SetNeighborHeight(i, oFloorTileArray[tileX, tileY]);
                    }
                }
            }

            private void SetNeighborHeight(int wallIndex, FloorTile otherTile)
            {
                //fuck
                if (wallIndex == 0)
                {
                    tNeighboringCornerHeights[wallIndex] = new Tuple<float, float>(otherTile.fCornerHeights[0], otherTile.fCornerHeights[1]);
                }
                else if(wallIndex == 1)
                {
                    tNeighboringCornerHeights[wallIndex] = new Tuple<float, float>(otherTile.fCornerHeights[0], otherTile.fCornerHeights[2]);
                }
                else if (wallIndex == 2)
                {
                    tNeighboringCornerHeights[wallIndex] = new Tuple<float, float>(otherTile.fCornerHeights[2], otherTile.fCornerHeights[3]);
                }
                else if (wallIndex == 3)
                {
                    tNeighboringCornerHeights[wallIndex] = new Tuple<float, float>(otherTile.fCornerHeights[1], otherTile.fCornerHeights[3]);
                }
            }

            public void RenderTile(GraphicsDevice graphicsDevice)
            {
                CRender.Instance.DrawTile(graphicsDevice, iPosition, fCornerHeights, iTileSize);
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

        //we make the level
        private void GenerateLevel()
        {
            //creating tiles
            for (int i = 0; i < iLevelWidth; i++)
            {
                for (int a = 0; a < iLevelHeight; a++)
                {
                    oFloorTileArray[i, a] = new FloorTile(i, a, iTileSize);
                }
            }

            //mapping the walls
            for (int i = 0; i < iLevelWidth; i++)
            {
                for (int a = 0; a < iLevelHeight; a++)
                {
                    oFloorTileArray[i, a].GenerateWallMap(oFloorTileArray);
                }
            }
        }

        //rendering loop
        public void Render(GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < iLevelWidth; i++)
            {
                for (int a = 0; a < iLevelHeight; a++)
                {
                    if (oFloorTileArray[i, a] != null)
                    {
                        oFloorTileArray[i, a].RenderTile(graphicsDevice);

                        //let's render the bottom wall
                        if (a >= 0 && a < iLevelHeight - 1)
                        {
                            FloorTile currentTile = oFloorTileArray[i, a];
                            FloorTile belowTile = oFloorTileArray[i, a + 1];
                            CRender.Instance.DrawRectangle(graphicsDevice,
                                new Vector3(i*iTileSize, currentTile.fCornerHeights[2], (a + 1)*iTileSize),
                                new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[3], (a + 1) * iTileSize),
                                new Vector3(i * iTileSize, belowTile.fCornerHeights[0], (a + 1) * iTileSize),
                                new Vector3((i + 1) * iTileSize, belowTile.fCornerHeights[1], (a + 1) * iTileSize), Color.SandyBrown);
                        }

                        //and the wall to the right of us
                        if (i >= 0 && i < iLevelWidth - 1)
                        {
                            FloorTile currentTile = oFloorTileArray[i, a];
                            FloorTile nextTile = oFloorTileArray[i + 1, a];
                            CRender.Instance.DrawRectangle(graphicsDevice,
                                new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[1], a * iTileSize),
                                new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[3], (a + 1) * iTileSize),
                                new Vector3((i + 1) * iTileSize, nextTile.fCornerHeights[0], a * iTileSize),
                                new Vector3((i + 1) * iTileSize, nextTile.fCornerHeights[2], (a + 1) * iTileSize), Color.SandyBrown);
                        }

                        //if we're on the leftmost row, let's render a top wall
                        if (i == 0)
                        {
                            FloorTile currentTile = oFloorTileArray[i, a];
                            CRender.Instance.DrawRectangle(graphicsDevice,
                                new Vector3(i, 0, a * iTileSize),
                                new Vector3(i, 0, (a + 1) * iTileSize),
                                new Vector3(i, currentTile.fCornerHeights[0], a * iTileSize),
                                new Vector3(i, currentTile.fCornerHeights[2], (a + 1) * iTileSize), Color.SandyBrown);
                        }

                        //if we're on the top row, let's draw a top wall
                        if (a == 0)
                        {
                            FloorTile currentTile = oFloorTileArray[i, a];
                            CRender.Instance.DrawRectangle(graphicsDevice,
                                new Vector3(i * iTileSize, 0, a),
                                new Vector3((i + 1) * iTileSize, 0, a),
                                new Vector3(i * iTileSize, currentTile.fCornerHeights[0], a),
                                new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[1], a), Color.SandyBrown);
                        }

                        //if we're on the rightmost row, let's render a right wall
                        if (i == iLevelWidth - 1)
                        {
                            FloorTile currentTile = oFloorTileArray[i, a];
                            CRender.Instance.DrawRectangle(graphicsDevice,
                                new Vector3((i+1) * iTileSize, currentTile.fCornerHeights[1], a * iTileSize),
                                new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[3], (a + 1) * iTileSize),
                                new Vector3((i + 1) * iTileSize, 0, a * iTileSize),
                                new Vector3((i + 1) * iTileSize, 0, (a + 1) * iTileSize), Color.SandyBrown);
                        }

                        //if we're on the bottom row, let's draw a bottom wall
                        if (a == iLevelHeight - 1)
                        {
                            FloorTile currentTile = oFloorTileArray[i, a];
                            CRender.Instance.DrawRectangle(graphicsDevice,
                                new Vector3(i * iTileSize, currentTile.fCornerHeights[2], (a + 1) * iTileSize),
                                new Vector3((i + 1) * iTileSize, currentTile.fCornerHeights[3], (a + 1) * iTileSize),
                                new Vector3(i * iTileSize, 0, (a + 1) * iTileSize),
                                new Vector3((i + 1) * iTileSize, 0, (a + 1) * iTileSize), Color.SandyBrown);
                        }
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

        public void LoadLevelData(string fileName)
        {
            //debug
            GenerateLevel();

            CConsole.Instance.Print("Opening level data");
            string xmlText = System.IO.File.ReadAllText("AssetData/"+fileName);
            XDocument file = XDocument.Parse(xmlText);

            //loading walls
            foreach (XElement element in file.Descendants("wall"))
            {
                int x = Int32.Parse(element.Element("x").Value);
                int z = 0;
                int y = Int32.Parse(element.Element("y").Value);
                int xscale = Int32.Parse(element.Element("xscale").Value);
                int yscale = Int32.Parse(element.Element("yscale").Value);

                CWall wall = (CWall)CObjectManager.Instance.CreateInstance(typeof(CWall), x, z, y);
                wall.SetScale(xscale, yscale);
            }

            foreach (XElement element in file.Descendants("prop"))
            {
                String name = element.Element("name").Value;
                int x = Int32.Parse(element.Element("x").Value);
                int z = 0;
                int y = Int32.Parse(element.Element("y").Value);
                int dir = Int32.Parse(element.Element("dir").Value);

                int propInd = sPropName.IndexOf(name);

                String sprite = sPropSprite[propInd];
                int colW = sPropColW[propInd];
                int colH = sPropColH[propInd];
                int health = sPropHealth[propInd];

                CProp prop = (CProp)CObjectManager.Instance.CreateInstance(typeof(CProp), x, z, y);
                prop.SetProperties((float)x, (float)z, (float)y, sprite, dir, colW, colH, health);
            }
        }
    }
}
