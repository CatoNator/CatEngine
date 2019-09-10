using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    class CLevel : CGameObject
    {
        private List<String> sPropName = new List<String>();
        private List<String> sPropSprite = new List<String>();
        private List<int> sPropColW = new List<int>();
        private List<int> sPropColH = new List<int>();
        private List<int> sPropHealth = new List<int>();

        /*<name>PropCopCar</name>
		<sprite>sprCopCar</sprite>
		<col_width>97</col_width>
		<col_height>44</col_height>
		<health>-1</health>*/

        public override void InstanceSpawn()
        {
            LoadPropData();
            LoadLevelData("LevelTest.xml");
        }

        private void LoadPropData()
        {
            Debug.WriteLine("Opening prop data");
            string xmlText = System.IO.File.ReadAllText("AssetData/PropData.xml");
            Debug.WriteLine("Text file loaded");
            XDocument file = XDocument.Parse(xmlText);
            Debug.WriteLine("XML parsed");

            foreach (XElement element in file.Descendants("prop"))
            {
                sPropName.Add(element.Element("name").Value);
                sPropSprite.Add(element.Element("sprite").Value);
                sPropColW.Add(Int32.Parse(element.Element("col_width").Value));
                sPropColH.Add(Int32.Parse(element.Element("col_height").Value));
                sPropHealth.Add(Int32.Parse(element.Element("health").Value));
            }
        }

        private void LoadLevelData(string fileName)
        {
            Debug.WriteLine("Opening level data");
            string xmlText = System.IO.File.ReadAllText("AssetData/"+fileName);
            Debug.WriteLine("Text file loaded");
            XDocument file = XDocument.Parse(xmlText);
            Debug.WriteLine("XML parsed");

            //loading walls
            foreach (XElement element in file.Descendants("wall"))
            {
                int x = Int32.Parse(element.Element("x").Value);
                int y = Int32.Parse(element.Element("y").Value);
                int xscale = Int32.Parse(element.Element("xscale").Value);
                int yscale = Int32.Parse(element.Element("yscale").Value);

                CWall wall = (CWall)CObjectManager.Instance.CreateInstance(typeof(CWall), x, y);
                wall.SetScale(xscale, yscale);
            }

            foreach (XElement element in file.Descendants("prop"))
            {
                String name = element.Element("name").Value;
                int x = Int32.Parse(element.Element("x").Value);
                int y = Int32.Parse(element.Element("y").Value);
                int dir = Int32.Parse(element.Element("dir").Value);

                int propInd = sPropName.IndexOf(name);

                String sprite = sPropSprite[propInd];
                int colW = sPropColW[propInd];
                int colH = sPropColH[propInd];
                int health = sPropHealth[propInd];

                Debug.Print("spritename " + sprite);

                CProp prop = (CProp)CObjectManager.Instance.CreateInstance(typeof(CProp), x, y);
                prop.SetProperties((float)x, (float)y, sprite, dir, colW, colH, health);
            }
        }
    }
}
