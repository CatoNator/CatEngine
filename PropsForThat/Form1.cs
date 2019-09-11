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

namespace PropsForThat
{
    public partial class Form1 : Form
    {
        List<Prop> propList = new List<Prop>();

        public Form1()
        {
            InitializeComponent();
            LoadPropData();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private class Prop
        {
            public String Name;
            public String Sprite;
            public int ColW;
            public int ColH;
            public int Health;

            public Prop( String name, String sprite, int colW, int colH, int hp)
            {
                Name = name;
                Sprite = sprite;
                ColW = colW;
                ColH = colH;
                Health = hp;
            }

            public void SetName(String name)
            {
                Name = name;
            }

            public void SetSprite(String name)
            {
                Sprite = name;
            }

            public void SetColW(int colW)
            {
                ColW = colW;
            }

            public void SetColH(int colH)
            {
                ColH = colH;
            }

            public void SetHP(int hp)
            {
                Health = hp;
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = listBox1.SelectedIndex;

            if (listBox1.SelectedIndex >= 0)
                DebugLabel.Text = "spritename " + propList[ind].Sprite;

            if (ind >= 0)
            {
                PropNameBox.Text = propList[ind].Name;
                SpriteNameBox.Text = propList[ind].Sprite;
                ColWBox.Text = propList[ind].ColW.ToString();
                ColHBox.Text = propList[ind].ColH.ToString();
                HPBox.Text = propList[ind].Health.ToString();
            }
            else
            {
                PropNameBox.Text = "";
                SpriteNameBox.Text = "";
                ColWBox.Text = "";
                ColHBox.Text = "";
                HPBox.Text = "";
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddProp();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int ind = listBox1.SelectedIndex;

            /*if (listBox1.SelectedIndex > 0)
                listBox1.SelectedIndex--;
            else
                listBox1.SelectedIndex++;*/

            RemoveProp(ind);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SavePropData();
        }

        private void PropNameBox_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                propList[listBox1.SelectedIndex].SetName(PropNameBox.Text);
                listBox1.Items[listBox1.SelectedIndex] = PropNameBox.Text;
                PropNameBox.SelectionStart = PropNameBox.Text.Length;
            }
        }

        private void SpriteNameBox_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                propList[listBox1.SelectedIndex].SetSprite(SpriteNameBox.Text);
        }

        private void ColWBox_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                propList[listBox1.SelectedIndex].SetColW(Int32.Parse(ColWBox.Text));
        }

        private void ColHBox_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                propList[listBox1.SelectedIndex].SetColH(Int32.Parse(ColHBox.Text));
        }

        private void HPBox_TextChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
                propList[listBox1.SelectedIndex].SetHP(Int32.Parse(HPBox.Text));
        }

        private void LoadPropData()
        {
            string xmlText = System.IO.File.ReadAllText("AssetData/PropData.xml");
            XDocument file = XDocument.Parse(xmlText);

            foreach (XElement element in file.Descendants("prop"))
            {
                String name = element.Element("name").Value;
                String sprite = element.Element("sprite").Value;
                int colW = Int32.Parse(element.Element("col_width").Value);
                int colH = Int32.Parse(element.Element("col_height").Value);
                int hp = Int32.Parse(element.Element("health").Value);

                propList.Add(new Prop(name, sprite, colW, colH, hp));

                listBox1.Items.Add(name);
            }

            Debug.Print(xmlText);
        }

        private void AddProp()
        {
            propList.Add(new Prop("propNewProp", "sprTest", 16, 16, -1));

            listBox1.Items.Add("propNewProp");
        }

        private void RemoveProp(int index)
        {
            if (listBox1.Items.Count > 0)
            {
                propList.RemoveAt(index);

                listBox1.Items.RemoveAt(index);
            }
        }

        private void SavePropData()
        {
            string xmlText = "<propdata></propdata>";

            XDocument file = XDocument.Parse(xmlText);

            foreach (Prop i in propList)
            {
                XElement element = new XElement("prop");
                element.Add(new XElement("name", i.Name));
                element.Add(new XElement("sprite", i.Sprite));
                element.Add(new XElement("col_width", i.ColW));
                element.Add(new XElement("col_width", i.ColH));
                element.Add(new XElement("health", i.Health));
                file.Descendants("propdata").Single().Add(element);
            }

            xmlText = file.ToString();

            File.WriteAllText("AssetData/PropData.xml", xmlText);
        }
    }
}
