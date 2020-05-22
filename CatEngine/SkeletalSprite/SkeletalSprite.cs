using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using CatEngine.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine.SkeletalAnimation
{
    public class SkeletalSprite
    {
        public SkeletalSprite()
        {

        }
        
        private class Bone
        {
            String boneName = "";

            SkeletalSprite parentSprite;

            Vector3 Position;
            Vector3 Size;
            float[] Rotations;

            Bone parentBone = null;
            List<Bone> childBones = new List<Bone>();

            private Matrix positionMatrix;
            private Matrix rotationMatrix;

            public Bone(Bone parent, Vector3 pos, float[] rot, string name)
            {
                Position = pos;
                Rotations = rot;
                boneName = name;

                parentBone = parent;
            }

            public string GetName()
            {
                return boneName;
            }

                public string ParentName()
            {
                string name = "";

                if (parentBone != null)
                    name = parentBone.GetName();

                return name;
            }

            public void AddChild(Bone bone)
            {
                childBones.Add(bone);
            }

            public void UpdateBone(int currentFrame)
            {
                if (parentBone != null)
                {
                    positionMatrix = parentBone.GetPositionMatrix() * Matrix.CreateTranslation(Position);
                    rotationMatrix = parentBone.GetRotationMatrix() * Matrix.CreateRotationY(Rotations[currentFrame]);
                }
                else
                {
                    positionMatrix = Matrix.CreateTranslation(Position);
                    rotationMatrix = Matrix.CreateRotationY(Rotations[currentFrame]);
                }

                if (childBones.Count > 0)
                {
                    foreach (Bone b in childBones)
                    {
                        UpdateBone(currentFrame);
                    }
                }
            }

            public void RenderBone()
            {
                Matrix transformMatrix = rotationMatrix * positionMatrix;
                
                CRender.Instance.RenderBone("BasicColorDrawing", "p_body_stand", 0, transformMatrix);
                
                if (childBones.Count > 0)
                {
                    foreach (Bone b in childBones)
                    {
                        RenderBone();
                    }
                }
            }

            public Matrix GetPositionMatrix()
            {
                return positionMatrix;
            }

            public Matrix GetRotationMatrix()
            {
                return rotationMatrix;
            }
        }

        Bone rootBone = null;

        int animFrame = 0;
        int animLength = 0;

        public void LoadAnimation(string animName)
        {
            string animData = "AssetData/SkeletalAnimation/" + animName;

            if (File.Exists(animData))
            {
                XDocument file;
                string xmlText = File.ReadAllText(animData);
                file = XDocument.Parse(xmlText);

                //Console.WriteLine(xmlText);

                animLength = Int32.Parse(file.Root.Attribute("frames").Value);

                foreach (XElement e in file.Root.Elements("Bone"))
                {
                    string name = e.Attribute("name").Value;

                    List<float> Rotations = new List<float>();

                    foreach (XElement rot in e.Elements("Rotations"))
                    {
                        foreach (XElement f in e.Elements("Frame"))
                        {
                            Rotations.Add(float.Parse(f.Value));
                        }
                    }

                    Vector3 Position = new Vector3(
                        float.Parse(e.Attribute("posx").Value),
                        float.Parse(e.Attribute("posy").Value),
                        float.Parse(e.Attribute("posz").Value));

                    rootBone = new Bone(null, Position, Rotations.ToArray(), name);

                    Console.WriteLine("created rootbone " + name);

                    foreach (XElement b in e.Elements("Bone"))
                    {
                        rootBone.AddChild(LoadBone(rootBone, b));
                    }
                }
            }
            else
                CConsole.Instance.Print("Animation data for " + animData + " wasn't found!");
        }

        private Bone LoadBone(Bone parentBone, XElement parent)
        {
            Bone bone;

            string name = parent.Attribute("name").Value;

            List<float> Rotations = new List<float>();

            foreach (XElement rot in parent.Elements("Rotations"))
            {
                foreach (XElement f in parent.Elements("Frame"))
                {
                    Rotations.Add(float.Parse(f.Value));
                }
            }

            Vector3 Position = new Vector3(
                float.Parse(parent.Attribute("posx").Value),
                float.Parse(parent.Attribute("posy").Value),
                float.Parse(parent.Attribute("posz").Value));

            bone = new Bone(parentBone, Position, Rotations.ToArray(), name);

            Console.WriteLine("created bone " + bone.GetName() + " as child to " + bone.ParentName());

            foreach (XElement b in parent.Descendants("Bone"))
            {
                bone.AddChild(LoadBone(bone, b));
            }

            return bone;
        }

        public void UpdateSkeleton(int frame)
        {
            animFrame = frame;
            
            if (animFrame > animLength)
                animFrame %= animLength;
            
            if (rootBone != null)
                rootBone.UpdateBone(animFrame);
        }

        public void RenderSkeleton()
        {
            if (rootBone != null)
                rootBone.RenderBone();
        }
    }
}