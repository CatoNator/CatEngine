using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MonoGame.Forms;

namespace BullSheet
{
    public partial class Form1 : Form
    {
        private List<Sprite> spriteList = new List<Sprite>();

        private int iImg = 0;

        public Form1()
        {
            InitializeComponent();
            LoadTextures();
            LoadSpriteData();
            //SpritePreview.Load();
        }

        private class Sprite
        {
            public String Name;
            public String TextureSheet;
            public int Left;
            public int Top;
            public int Width;
            public int Height;
            public int Images;
            public int XOrig;
            public int YOrig;

            public Sprite(String name, String tex, int left, int top, int w, int h, int img, int xorig, int yorig)
            {
                Name = name;
                TextureSheet = tex;
                Left = left;
                Top = top;
                Width = w;
                Height = h;
                Images = img;
                XOrig = xorig;
                YOrig = yorig;
            }
        }

        private void SpriteListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ind = SpriteListBox.SelectedIndex;

            iImg = 0;
            ImgLabel.Text = iImg.ToString();

            if (ind >= 0)
            {
                SpriteNameBox.Text = spriteList[ind].Name;
                SpriteTexBox.SelectedIndex = SpriteTexBox.Items.IndexOf(spriteList[ind].TextureSheet);
                SpriteLeftBox.Text = spriteList[ind].Left.ToString();
                SpriteTopBox.Text = spriteList[ind].Top.ToString();
                SpriteWidthBox.Text = spriteList[ind].Width.ToString();
                SpriteHeightBox.Text = spriteList[ind].Height.ToString();
                SpriteImgBox.Text = spriteList[ind].Images.ToString();
                SpriteXorigBox.Text = spriteList[ind].XOrig.ToString();
                SpriteYorigBox.Text = spriteList[ind].YOrig.ToString();
                UpdateSprite();
            }
            else
            {
                SpriteNameBox.Text = "";
                SpriteTexBox.SelectedIndex = -1;
                SpriteLeftBox.Text = "";
                SpriteTopBox.Text = "";
                SpriteWidthBox.Text = "";
                SpriteHeightBox.Text = "";
                SpriteImgBox.Text = "";
                SpriteXorigBox.Text = "";
                SpriteYorigBox.Text = "";
            }
        }

        private void SpriteNameBox_TextChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                spriteList[SpriteListBox.SelectedIndex].Name = SpriteNameBox.Text;
                SpriteListBox.Items[SpriteListBox.SelectedIndex] = SpriteNameBox.Text;
                SpriteNameBox.SelectionStart = SpriteNameBox.Text.Length;
                UpdateSprite();
            }
        }

        private void SpriteTexBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                spriteList[SpriteListBox.SelectedIndex].TextureSheet = SpriteTexBox.Items[SpriteTexBox.SelectedIndex].ToString();
                UpdateSprite();
            }
        }

        private void SpriteLeftBox_TextChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                if (SpriteLeftBox.Text.Length > 0)
                    spriteList[SpriteListBox.SelectedIndex].Left = Int32.Parse(SpriteLeftBox.Text);
                else
                    spriteList[SpriteListBox.SelectedIndex].Left = 0;

                UpdateSprite();
            }
        }

        private void SpriteTopBox_TextChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                if (SpriteTopBox.Text.Length > 0)
                    spriteList[SpriteListBox.SelectedIndex].Top = Int32.Parse(SpriteTopBox.Text);
                else
                    spriteList[SpriteListBox.SelectedIndex].Top = 0;

                UpdateSprite();
            }
        }

        private void SpriteWidthBox_TextChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                if (SpriteWidthBox.Text.Length > 0)
                    spriteList[SpriteListBox.SelectedIndex].Width = Int32.Parse(SpriteWidthBox.Text);
                else
                    spriteList[SpriteListBox.SelectedIndex].Width = 0;

                UpdateSprite();
            }
        }

        private void SpriteHeightBox_TextChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                if (SpriteHeightBox.Text.Length > 0)
                    spriteList[SpriteListBox.SelectedIndex].Height = Int32.Parse(SpriteHeightBox.Text);
                else
                    spriteList[SpriteListBox.SelectedIndex].Height = 0;

                UpdateSprite();
            }
        }

        private void SpriteImgBox_TextChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                if (SpriteImgBox.Text.Length > 0)
                    spriteList[SpriteListBox.SelectedIndex].Images = Int32.Parse(SpriteImgBox.Text);
                else
                    spriteList[SpriteListBox.SelectedIndex].Images = 0;

                UpdateSprite();
            }
        }

        private void SpriteXorigBox_TextChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                if (SpriteXorigBox.Text.Length > 0)
                    spriteList[SpriteListBox.SelectedIndex].XOrig = Int32.Parse(SpriteXorigBox.Text);
                else
                    spriteList[SpriteListBox.SelectedIndex].XOrig = 0;

                UpdateSprite();
            }
        }

        private void SpriteYorigBox_TextChanged(object sender, EventArgs e)
        {
            if (SpriteListBox.SelectedIndex >= 0)
            {
                if (SpriteYorigBox.Text.Length > 0)
                    spriteList[SpriteListBox.SelectedIndex].YOrig = Int32.Parse(SpriteYorigBox.Text);
                else
                    spriteList[SpriteListBox.SelectedIndex].YOrig = 0;

                UpdateSprite();
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            AddSprite();
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            int ind = SpriteListBox.SelectedIndex;

            RemoveSprite(ind);
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveSpriteData();
        }

        private void PrevImgButton_Click(object sender, EventArgs e)
        {
            if (iImg > 0)
                iImg--;

            ImgLabel.Text = iImg.ToString();

            UpdateSprite();
        }

        private void NextImgButton_Click(object sender, EventArgs e)
        {
            if (iImg < spriteList[SpriteListBox.SelectedIndex].Images)
                iImg++;

            ImgLabel.Text = iImg.ToString();

            UpdateSprite();
        }

        private void LoadSpriteData()
        {
            string xmlText = System.IO.File.ReadAllText("AssetData/SpriteData.xml");
            XDocument file = XDocument.Parse(xmlText);

            foreach (XElement element in file.Descendants("sprite"))
            {
                String name = element.Element("name").Value;
                String tex = element.Element("texture").Value;
                int l = Int32.Parse(element.Element("left").Value);
                int t = Int32.Parse(element.Element("top").Value);
                int w = Int32.Parse(element.Element("width").Value);
                int h = Int32.Parse(element.Element("height").Value);
                int img = Int32.Parse(element.Element("images").Value);
                int xo = Int32.Parse(element.Element("xorig").Value);
                int yo = Int32.Parse(element.Element("yorig").Value);

                spriteList.Add(new Sprite(name, tex, l, t, w, h, img, xo, yo));

                SpriteListBox.Items.Add(name);
            }

            Debug.Print(xmlText);
        }

        private void SaveSpriteData()
        {
            string xmlText = "<spritedata></spritedata>";

            XDocument file = XDocument.Parse(xmlText);

            foreach (Sprite i in spriteList)
            {
                XElement element = new XElement("sprite");
                element.Add(new XElement("name", i.Name));
                element.Add(new XElement("texture", i.TextureSheet));
                element.Add(new XElement("left", i.Left));
                element.Add(new XElement("top", i.Top));
                element.Add(new XElement("width", i.Width));
                element.Add(new XElement("height", i.Height));
                element.Add(new XElement("images", i.Images));
                element.Add(new XElement("xorig", i.XOrig));
                element.Add(new XElement("yorig", i.YOrig));
                file.Descendants("spritedata").Single().Add(element);
            }

            xmlText = file.ToString();

            System.IO.File.WriteAllText("AssetData/SpriteData.xml", xmlText);
        }

        private void AddSprite()
        {
            spriteList.Add(new Sprite("sprite" + spriteList.Count(), "Player", 0, 0, 16, 16, 0, 0, 0));

            SpriteListBox.Items.Add("sprite" + spriteList.Count());
        }

        private void RemoveSprite(int index)
        {
            if (SpriteListBox.Items.Count > 0 && index >= 0)
            {
                spriteList.RemoveAt(index);

                SpriteListBox.Items.RemoveAt(index);
            }
        }

        private void LoadTextures()
        {
            SpriteTexBox.Items.Add("Player");
            SpriteTexBox.Items.Add("Enemy");
            SpriteTexBox.Items.Add("Props");
            SpriteTexBox.Items.Add("Weapons");
        }

        private void UpdateSprite()
        {
            TextureSheetPreview.SetSprite(spriteList[SpriteListBox.SelectedIndex].TextureSheet, spriteList[SpriteListBox.SelectedIndex].Left,
                spriteList[SpriteListBox.SelectedIndex].Top, spriteList[SpriteListBox.SelectedIndex].Width, spriteList[SpriteListBox.SelectedIndex].Height,
                spriteList[SpriteListBox.SelectedIndex].Images, spriteList[SpriteListBox.SelectedIndex].XOrig, spriteList[SpriteListBox.SelectedIndex].YOrig, iImg);

            SpritePreview.SetSprite(spriteList[SpriteListBox.SelectedIndex].TextureSheet, spriteList[SpriteListBox.SelectedIndex].Left,
                spriteList[SpriteListBox.SelectedIndex].Top, spriteList[SpriteListBox.SelectedIndex].Width, spriteList[SpriteListBox.SelectedIndex].Height,
                spriteList[SpriteListBox.SelectedIndex].Images, spriteList[SpriteListBox.SelectedIndex].XOrig, spriteList[SpriteListBox.SelectedIndex].YOrig, iImg);
        }
    }
}
