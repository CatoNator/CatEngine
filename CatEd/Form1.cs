using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Diagnostics;
using System.IO;
using CatEngine.Content;

namespace CatEd
{
    public partial class CatEdMainForm : Form
    {
        public int SelX = 0;
        public int SelY = 0;
        public int SelW = 1;
        public int SelH = 1;

        public float[] fCorners = new float[4];

        public string sLevelName = "Test";

        private List<string> textureList;

        public CatEdMainForm()
        {
            InitializeComponent();

            cLevelView1.sLevelName = sLevelName;
            cLevelView1.Form = this;

            for (int i = 0; i < 4; i++)
            {
                fCorners[i] = 1.0f;
            }

            TileSelXBox.Text = SelX.ToString();
            TileSelYBox.Text = SelY.ToString();
            TileSelWBox.Text = SelW.ToString();
            TileSelHBox.Text = SelH.ToString();

            TileTLTextBox.Text = fCorners[0].ToString();
            TileTRTextBox.Text = fCorners[1].ToString();
            TileBLTextBox.Text = fCorners[2].ToString();
            TileBRTextBox.Text = fCorners[3].ToString();

            LevelWBox.Text = CLevel.Instance.iLevelWidth.ToString();
            LevelHBox.Text = CLevel.Instance.iLevelHeight.ToString();
        }

        public void RefreshTextureList(List<string> txList)
        {
            textureList = txList;

            TileTTexBox.Items.AddRange(textureList.ToArray());
            TileBTexBox.Items.AddRange(textureList.ToArray());
            TileLTexBox.Items.AddRange(textureList.ToArray());
            TileRTexBox.Items.AddRange(textureList.ToArray());
            TileWTexBox.Items.AddRange(textureList.ToArray());
        }

        private void LoadLevel()
        {
            string xmlText = System.IO.File.ReadAllText("AssetData/PropData.xml");
            XDocument file = XDocument.Parse(xmlText);

            int a = 0;

            foreach (XElement element in file.Descendants("prop"))
            {
                String name = element.Element("name").Value;
                int x = Int32.Parse(element.Element("x").Value);
                int y = Int32.Parse(element.Element("y").Value);
                int dir = Int32.Parse(element.Element("dir").Value);

                //CEntityManager.AddProp(name, x, y, dir);

                PropListBox.Items.Add(name);
            }

            foreach (XElement element in file.Descendants("enemy"))
            {
                String type = element.Element("type").Value;
                int x = Int32.Parse(element.Element("x").Value);
                int y = Int32.Parse(element.Element("y").Value);
                int dir = Int32.Parse(element.Element("dir").Value);

                //CEntityManager.AddPEnemy(type, x, y, dir);

                EnemyListBox.Items.Add(type);
            }

            foreach (XElement element in file.Descendants("item"))
            {
                String type = element.Element("type").Value;
                int x = Int32.Parse(element.Element("x").Value);
                int y = Int32.Parse(element.Element("y").Value);

                //CEntityManager.AddItem(type, x, y, dir);

                ItemListBox.Items.Add(type);
            }

            /*
            player spawn data:
            x (int)
            y (int)
            dir (tbd) possibly same as prop but idk yet
            */

            Debug.Print(xmlText);
        }

        public float BiLinearInterp(float x, float y, float q11, float q12, float q21, float q22)
        {
            //corner positions
            float x1 = 0;
            float y1 = 0;
            float x2 = (float)SelW;
            float y2 = (float)SelH;

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

        private void SetCornerHeights()
        {
            for (int i = 0; i < SelW; i++)
            {
                for (int a = 0; a < SelH; a++)
                {
                    float[] arr = new float[4];
                    arr[0] = BiLinearInterp(i, a, fCorners[0], fCorners[1], fCorners[2], fCorners[3]);
                    arr[1] = BiLinearInterp(i+1, a, fCorners[0], fCorners[1], fCorners[2], fCorners[3]);
                    arr[2] = BiLinearInterp(i, a+1, fCorners[0], fCorners[1], fCorners[2], fCorners[3]);
                    arr[3] = BiLinearInterp(i+1, a+1, fCorners[0], fCorners[1], fCorners[2], fCorners[3]);

                    CLevel.Instance.SetCornerHeigth(SelX+i, SelY+a, arr);

                    CLevel.Instance.SetTextures(SelX + i, SelY + i, 1, 1, 1, 1, 1);
                }
            }
        }

        private void TileSetButton_Click(object sender, EventArgs e)
        {
            SetCornerHeights();
        }

        private void TileSelXBox_TextChanged(object sender, EventArgs e)
        {
            if (TileSelXBox.Text.Length > 0)
                SelX = Int32.Parse(TileSelXBox.Text);
            else
                SelX = 0;
            Debug.Print("sel x set to " + SelX);
        }

        private void TileSelYBox_TextChanged(object sender, EventArgs e)
        {
            if (TileSelYBox.Text.Length > 0)
                SelY = Int32.Parse(TileSelYBox.Text);
            else
                SelY = 0;
            Debug.Print("sel y set to " + SelY);
        }

        private void TileSelWBox_TextChanged(object sender, EventArgs e)
        {
            if (TileSelWBox.Text.Length > 0)
                SelW = Int32.Parse(TileSelWBox.Text);
            else
                SelW = 0;
            Debug.Print("sel w set to " + SelW);
        }

        private void TileSelHBox_TextChanged(object sender, EventArgs e)
        {
            if (TileSelHBox.Text.Length > 0)
                SelH = Int32.Parse(TileSelHBox.Text);
            else
                SelH = 0;
            Debug.Print("sel H set to " + SelH);
        }

        private void TileTLTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TileTLTextBox.Text.Length > 0)
                fCorners[0] = float.Parse(TileTLTextBox.Text);
            else
                fCorners[0] = 0.0f;
            Debug.Print("fcorner 0 set to " + fCorners[0]);
        }

        private void TileTRTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TileTRTextBox.Text.Length > 0)
                fCorners[1] = float.Parse(TileTRTextBox.Text);
            else
                fCorners[1] = 0.0f;
            Debug.Print("fcorner 1 set to " + fCorners[1]);
        }

        private void TileBLTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TileBLTextBox.Text.Length > 0)
                fCorners[2] = float.Parse(TileBLTextBox.Text);
            else
                fCorners[2] = 0.0f;
            Debug.Print("fcorner 2 set to " + fCorners[2]);
        }

        private void TileBRTextBox_TextChanged(object sender, EventArgs e)
        {
            if (TileBRTextBox.Text.Length > 0)
                fCorners[3] = float.Parse(TileBRTextBox.Text);
            else
                fCorners[3] = 0.0f;
            Debug.Print("fcorner 3 set to " + fCorners[3]);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            CLevel.Instance.SaveTerrain(sLevelName);
            Debug.Print("saved");
        }
    }
}
