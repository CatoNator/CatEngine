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

        public static int iLevelWidth = 3;
        public static int iLevelHeight = 3;

        private FloorTile[,] oFloorTileArray = new FloorTile[iLevelWidth, iLevelHeight];

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

            private Random random = new Random();

            /*
            0-----1
            |     |
            |     |
            2-----3
            */

            public FloorTile(int x, int y, int tileSize)
            {
                iPosition[0] = x;
                iPosition[0] = y;
                iTileSize = tileSize;

                for (int i = 0; i < 4; i++)
                {
                    fCornerHeights[i] = -10.0f + (float)random.NextDouble() * 20.0f;
                }
            }

            public void RenderTile(GraphicsDevice graphicsDevice)
            {
                CRender.Instance.DrawTile(graphicsDevice, iPosition, fCornerHeights, iTileSize);
            }
        }

        private void GenerateLevel()
        {
            for(int i = 0; i < iLevelWidth; i++)
            {
                for (int a = 0; a < iLevelHeight; a++)
                {
                    oFloorTileArray[i, a] = new FloorTile(i, a, 10);
                }
            }
        }

        public void Render(GraphicsDevice graphicsDevice)
        {
            for (int i = 0; i < iLevelWidth; i++)
            {
                for (int a = 0; a < iLevelHeight; a++)
                {
                    if (oFloorTileArray[i, a] != null)
                    {
                        oFloorTileArray[i, a].RenderTile(graphicsDevice);
                    }
                }
            }
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
