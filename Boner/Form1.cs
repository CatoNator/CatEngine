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
using System.IO;
using Microsoft.Xna.Framework;

namespace Boner
{
    public partial class Form1 : Form
    {
        public int animLength = 0;

        public float currentFrame = 0;

        public float animSpeed = 0;
        
        public Form1()
        {
            InitializeComponent();
            cAnimationPreview1.form = this;
        }

        public static Form1 Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly Form1 instance = new Form1();
        }

        public void UpdateFrame()
        {
            CurrentFrameSelect.Value = (decimal)currentFrame;

            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                if (node.Images != null)
                {
                    if (currentFrame < node.Images.Count)
                        ImageIndBox.Value = node.Images[(int)currentFrame];
                }
                if (node.Rotations != null)
                {
                    if (currentFrame < node.Rotations.Count)
                        RotationBox.Value = (decimal)(node.Rotations[(int)currentFrame]);
                }
            }
        }

        private void AddChildButton_Click(object sender, EventArgs e)
        {
            if (BoneTreeView.SelectedNode != null)
            {
                BoneNode newNode = new BoneNode("newbone", new Vector2(0, 0), new Vector2(0, 0), "", new Vector3(0, 0, 0));
                BoneTreeView.SelectedNode.Nodes.Add(newNode);
                BoneTreeView.SelectedNode.Expand();
            }
        }

        private void RemoveBoneButton_Click(object sender, EventArgs e)
        {
            if (BoneTreeView.SelectedNode != null)
            {
                BoneTreeView.SelectedNode.Parent.Nodes.Remove(BoneTreeView.SelectedNode);
            }
        }

        private void BoneTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (BoneTreeView.SelectedNode != null)
            {
                BoneNode node = (BoneNode)BoneTreeView.SelectedNode;

                BoneNameBox.Text = node.Text;
                BoneSizeXBox.Value = (decimal)node.Size.X;
                BoneSizeYBox.Value = (decimal)node.Size.Y;
                BoneOrigXBox.Value = (decimal)node.Origin.X;
                BoneOrigYBox.Value = (decimal)node.Origin.Y;
                BonePosXBox.Value = (decimal)node.Position.X;
                BonePosYBox.Value = (decimal)node.Position.Y;
                BonePosZBox.Value = (decimal)node.Position.Z;
                BoneTexBox.Text = node.Texture;
                if (node.Images != null)
                {
                    if (currentFrame < node.Images.Count)
                        ImageIndBox.Value = node.Images[(int)currentFrame];
                }
                if (node.Rotations != null)
                {
                    if (currentFrame < node.Rotations.Count)
                        RotationBox.Value = (decimal)(node.Rotations[(int)currentFrame]);
                }
            }
            else
            {
                BoneNameBox.Text = "";
                BoneSizeXBox.Value = 0;
                BoneSizeYBox.Value = 0;
                BoneOrigXBox.Value = 0;
                BoneOrigYBox.Value = 0;
                BonePosXBox.Value = 0;
                BonePosYBox.Value = 0;
                BonePosZBox.Value = 0;
                BoneTexBox.Text = "";
            }
        }

        private void LoadBoneTree(Stream strm)
        {
            //string animData = "player_test.ske";

            //cAnimationPreview1.Load();

            XDocument file;
            byte[] buffer = new byte[strm.Length];
            strm.Read(buffer, 0, buffer.Length);
            string xmlText = Encoding.ASCII.GetString(buffer);
            file = XDocument.Parse(xmlText);

            //Console.WriteLine(xmlText);

            BoneTreeView.Nodes.Clear();

            foreach (XElement e in file.Root.Elements("Bone"))
            {
                System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
                ci.NumberFormat.CurrencyDecimalSeparator = ".";

                string name = e.Attribute("name").Value;

                Vector3 Position = new Vector3(
                    float.Parse(e.Attribute("posx").Value, System.Globalization.NumberStyles.Any, ci),
                    float.Parse(e.Attribute("posy").Value, System.Globalization.NumberStyles.Any, ci),
                    float.Parse(e.Attribute("posz").Value, System.Globalization.NumberStyles.Any, ci));

                Vector2 Size = new Vector2(
                    float.Parse(e.Attribute("sizex").Value, System.Globalization.NumberStyles.Any, ci),
                    float.Parse(e.Attribute("sizey").Value, System.Globalization.NumberStyles.Any, ci));

                Vector2 Origin = new Vector2(
                    float.Parse(e.Attribute("origx").Value, System.Globalization.NumberStyles.Any, ci),
                    float.Parse(e.Attribute("origy").Value, System.Globalization.NumberStyles.Any, ci));

                string tex = e.Attribute("texture").Value;

                BoneNode rootNode = new BoneNode(name, Size, Origin, tex, Position);

                BoneTreeView.Nodes.Add(rootNode);

                Console.WriteLine("created rootbone " + name);

                foreach (XElement b in e.Elements("Bone"))
                {
                    rootNode.Nodes.Add(LoadBone(rootNode, b));
                }
            }
        }

        private BoneNode LoadBone(BoneNode currentNode, XElement parent)
        {
            System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            string name = parent.Attribute("name").Value;

            Vector3 Position = new Vector3(
                float.Parse(parent.Attribute("posx").Value, System.Globalization.NumberStyles.Any, ci),
                float.Parse(parent.Attribute("posy").Value, System.Globalization.NumberStyles.Any, ci),
                float.Parse(parent.Attribute("posz").Value, System.Globalization.NumberStyles.Any, ci));

            Vector2 Size = new Vector2(
                float.Parse(parent.Attribute("sizex").Value, System.Globalization.NumberStyles.Any, ci),
                float.Parse(parent.Attribute("sizey").Value, System.Globalization.NumberStyles.Any, ci));

            Vector2 Origin = new Vector2(
                float.Parse(parent.Attribute("origx").Value, System.Globalization.NumberStyles.Any, ci),
                float.Parse(parent.Attribute("origy").Value, System.Globalization.NumberStyles.Any, ci));

            string tex = parent.Attribute("texture").Value;

            BoneNode newNode = new BoneNode(name, Size, Origin, tex, Position);

            foreach (XElement b in parent.Elements("Bone"))
            {
                newNode.Nodes.Add(LoadBone(newNode, b));
            }

            return newNode;
        }

        private string SaveBoneTree()
        {
            //string skelData = "test_save.ske";

            string xmlText = "<SkeletonStructure></SkeletonStructure>";

            XDocument file = XDocument.Parse(xmlText);

            //Console.WriteLine(xmlText);

            foreach (BoneNode e in BoneTreeView.Nodes)
            {
                XElement element = new XElement("Bone");
                element.Add(new XAttribute("name", e.Name));
                element.Add(new XAttribute("sizex", e.Size.X));
                element.Add(new XAttribute("sizey", e.Size.Y));
                element.Add(new XAttribute("origx", e.Origin.X));
                element.Add(new XAttribute("origy", e.Origin.X));
                element.Add(new XAttribute("texture", e.Texture));
                element.Add(new XAttribute("posx", e.Position.X));
                element.Add(new XAttribute("posy", e.Position.Y));
                element.Add(new XAttribute("posz", e.Position.Z));
                file.Descendants("SkeletonStructure").Single().Add(element);

                foreach (BoneNode b in e.Nodes)
                {
                    XElement bone = SaveBone(b, file);
                    element.Add(bone);
                }
            }

            return file.ToString();
        }

        private XElement SaveBone(BoneNode bone, XDocument file)
        {
            XElement element = new XElement("Bone");
            element.Add(new XAttribute("name", bone.Name));
            element.Add(new XAttribute("sizex", bone.Size.X));
            element.Add(new XAttribute("sizey", bone.Size.Y));
            element.Add(new XAttribute("origx", bone.Origin.X));
            element.Add(new XAttribute("origy", bone.Origin.Y));
            element.Add(new XAttribute("texture", bone.Texture));
            element.Add(new XAttribute("posx", bone.Position.X));
            element.Add(new XAttribute("posy", bone.Position.Y));
            element.Add(new XAttribute("posz", bone.Position.Z));

            foreach (BoneNode b in bone.Nodes)
            {
                XElement nextBone = SaveBone(b, file);
                element.Add(nextBone);
            }

            return element;
        }

        public BoneNode GetBone(string name)
        {
            BoneNode bone = null;

            BoneNode rootNode = ((BoneNode)BoneTreeView.Nodes[0]);

            if (BoneTreeView.Nodes[0].Name != name)
            {
                Console.WriteLine("bone was not root, traversing");
                bone = rootNode.GetChild(name);
            }
            else
                bone = rootNode;

            return bone;
        }

        public void LoadAnimation(Stream strm)
        {
            //string animData = "player_test.ske";

            //cAnimationPreview1.Load();

            XDocument file;
            byte[] buffer = new byte[strm.Length];
            strm.Read(buffer, 0, buffer.Length);
            string xmlText = Encoding.ASCII.GetString(buffer);
            file = XDocument.Parse(xmlText);

            System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

            animLength = Int32.Parse(file.Root.Attribute("frames").Value);
            MaxFramesBox.Value = animLength;
            CurrentFrameSelect.Maximum = animLength;
            currentFrame = 0;

            foreach (XElement bone in file.Descendants("Bone"))
            {
                string name = bone.Attribute("name").Value;

                List<float> Rotations = new List<float>();
                List<int> Images = new List<int>();

                foreach (XElement f in bone.Elements("Frame"))
                {
                    Rotations.Add(float.Parse(f.Value, System.Globalization.NumberStyles.Any, ci));
                    Images.Add(Int32.Parse(f.Attribute("txind").Value));
                }

                GetBone(name).SetAnimation(Rotations, Images);

                Console.WriteLine("loaded animation frames for bone " + name);
            }
        }

        private string SaveAnimation()
        {
            List<BoneNode> nodes = new List<BoneNode>();

            if (BoneTreeView.Nodes.Count > 0)
            {
                {
                    BoneNode rootNode = ((BoneNode)BoneTreeView.Nodes[0]);

                    SaveAnimationBone(rootNode, nodes);
                }
            }

            string xmlText = "<SkeletalAnimation></SkeletalAnimation>";

            XDocument file = XDocument.Parse(xmlText);

            file.Root.Add(new XAttribute("frames", animLength));

            //Console.WriteLine(xmlText);


            foreach (BoneNode e in nodes)
            {
                XElement element = new XElement("Bone");
                element.Add(new XAttribute("name", e.Name));

                if (e.Rotations != null && e.Images != null)
                {
                    for (int i = 0; i < e.Rotations.Count; i++)
                    {
                        XElement frame = new XElement("Frame");
                        frame.Add(new XAttribute("txind", e.Images[i]));
                        frame.SetValue(e.Rotations[i]);
                        element.Add(frame);
                    }
                }

                file.Root.Add(element);
            }

            return file.ToString(); ;
        }

        private void SaveAnimationBone(BoneNode b, List<BoneNode> boneList)
        {

            boneList.Add(b);

            if (b.Nodes.Count > 0)
            {
                foreach (BoneNode e in b.Nodes)
                {
                    SaveAnimationBone(e, boneList);
                }
            }
        }

        private void saveSkeletonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveSkeletonDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream fileStream = SaveSkeletonDialog.OpenFile();
                    Byte[] bytes = Encoding.ASCII.GetBytes(SaveBoneTree());
                    fileStream.Write(bytes, 0, bytes.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception occured\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void loadSkeletonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadSkeletonDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadBoneTree(LoadSkeletonDialog.OpenFile());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception occured\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void loadAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LoadAnimationDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    LoadAnimation(LoadAnimationDialog.OpenFile());
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception occured\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void saveAnimationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SaveAnimationDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream fileStream = SaveAnimationDialog.OpenFile();
                    Byte[] bytes = Encoding.ASCII.GetBytes(SaveAnimation());
                    fileStream.Write(bytes, 0, bytes.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception occured\n\nError message: {ex.Message}\n\n" +
                    $"Details:\n\n{ex.StackTrace}");
                }
            }
        }

        private void MaxFramesBox_ValueChanged(object sender, EventArgs e)
        {
            animLength = (int)MaxFramesBox.Value;
            CurrentFrameSelect.Maximum = MaxFramesBox.Value-1;
            currentFrame = (int)CurrentFrameSelect.Value;

            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                if (node.Images != null)
                {
                    if (currentFrame < node.Images.Count)
                        ImageIndBox.Value = node.Images[(int)currentFrame];
                } 
                if (node.Rotations != null)
                {
                    if (currentFrame < node.Rotations.Count)
                        RotationBox.Value = (decimal)(node.Rotations[(int)currentFrame]);
                }
            }
        }

        private void CurrentFrameSelect_ValueChanged(object sender, EventArgs e)
        {
            currentFrame = (float)(CurrentFrameSelect.Value);

            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                if (node.Images != null)
                {
                    if (currentFrame < node.Images.Count)
                        ImageIndBox.Value = node.Images[(int)currentFrame];
                }
                if (node.Rotations != null)
                {
                    if (currentFrame < node.Rotations.Count)
                        RotationBox.Value = (decimal)(node.Rotations[(int)currentFrame]);
                }
            }
        }

        private void RotationBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                if (node.Rotations != null)
                {
                    if (currentFrame < node.Rotations.Count)
                        node.Rotations[(int)currentFrame] = (float)(RotationBox.Value);
                }
            }
        }

        private void BoneTexBox_TextChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Texture = BoneTexBox.Text;
            }
        }

        private void BoneNameBox_TextChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Name = BoneNameBox.Text;
                node.Text = BoneNameBox.Text;
            }
        }

        private void BoneSizeXBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Size = new Vector2((float)(BoneSizeXBox.Value), node.Size.Y);
            }
        }

        private void BoneSizeYBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Size = new Vector2(node.Size.X, (float)(BoneSizeYBox.Value));
            }
        }

        private void BoneOrigXBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Origin = new Vector2((float)(BoneOrigXBox.Value), node.Origin.Y);
            }
        }

        private void BoneOrigYBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Origin = new Vector2(node.Origin.X, (float)(BoneOrigYBox.Value));
            }
        }

        private void BonePosXBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Position = new Vector3((float)(BonePosXBox.Value), node.Position.Y, node.Position.Z);
            }
        }

        private void BonePosYBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Position = new Vector3(node.Position.X, (float)(BonePosYBox.Value), node.Position.Z);
            }
        }

        private void BonePosZBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                node.Position = new Vector3(node.Position.X, node.Position.Y, (float)(BonePosZBox.Value));
            }
        }

        private void ImageIndBox_ValueChanged(object sender, EventArgs e)
        {
            BoneNode node = (BoneNode)(BoneTreeView.SelectedNode);

            if (node != null)
            {
                if (node.Images != null)
                {
                    if (currentFrame < node.Images.Count)
                        node.Images[(int)currentFrame] = (int)(ImageIndBox.Value);
                }
            }
        }

        private void AnimSpeedBox_ValueChanged(object sender, EventArgs e)
        {
            animSpeed = (float)(AnimSpeedBox.Value);
        }
    }

    public class BoneNode : TreeNode
    {
        public Vector2 Size;
        public Vector2 Origin;
        public string Texture;
        public Vector3 Position;

        public List<float> Rotations;
        public List<int> Images;

        public BoneNode(string Name, Vector2 size, Vector2 orig, string tex, Vector3 pos)
        {
            this.Name = Name;
            this.Text = Name;
            Size = size;
            Origin = orig;
            Texture = tex;
            Position = pos;

            Rotations = new List<float>(); // new float[1];
        }

        public void SetAnimation(List<float> rot, List<int> img)
        {
            Rotations = new List<float>();// new float[rot.Length];
            Images = new List<int>();

            Rotations = rot;
            Images = img;
        }

        public BoneNode GetChild(string name)
        {
            BoneNode childBone = null;

            if (Nodes.Count > 0)
            {
                foreach (BoneNode b in Nodes)
                {
                    if (childBone == null)
                    {
                        if (b.Name == name)
                        {
                            childBone = b;
                            Console.WriteLine("found child");
                        }
                        else
                        {
                            Console.WriteLine("child was not the one; " + Name);
                            childBone = b.GetChild(name);
                        }
                    }
                }
            }

            return childBone;
        }
    }
}
