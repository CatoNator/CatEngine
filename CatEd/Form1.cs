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

namespace CatEd
{
    public partial class CatEdMainForm : Form
    {
        public CatEdMainForm()
        {
            InitializeComponent();
        }

        private void LoadLevel()
        {
            string xmlText = System.IO.File.ReadAllText("AssetData/PropData.xml");
            XDocument file = XDocument.Parse(xmlText);

            int a = 0;

            foreach (XElement element in file.Descendants("wall"))
            {
                a++;
                int x = Int32.Parse(element.Element("x").Value);
                int y = Int32.Parse(element.Element("y").Value);
                int xscale = Int32.Parse(element.Element("xscale").Value);
                int yscale = Int32.Parse(element.Element("yscale").Value);

                //CEntityManager.AddWall(x, y, xscale, yscale);

                WallListBox.Items.Add("Wall " + a);
            }

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

        private void EnemyTab_Click(object sender, EventArgs e)
        {

        }

        private void TextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label1_Click(object sender, EventArgs e)
        {

        }

        private void TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Label2_Click(object sender, EventArgs e)
        {

        }

        private void GroupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void Label3_Click(object sender, EventArgs e)
        {

        }

        private void TextBox3_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
