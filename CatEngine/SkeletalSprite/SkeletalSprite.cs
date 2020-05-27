using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.IO;
using Microsoft.Xna.Framework;

namespace CatEngine.SkeletalAnimation
{
    public class SkeletalSprite
    {
        public SkeletalSprite()
        {

        }

        Bone rootBone = null;

        float animFrame = 0;
        int animLength = 0;

        public void LoadSkeleton(string path, string skelName)
        {
            string animData = path + "/" + skelName + ".ske";

            if (File.Exists(animData))
            {
                XDocument file;
                string xmlText = File.ReadAllText(animData);
                file = XDocument.Parse(xmlText);

                //Console.WriteLine(xmlText);

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

                    rootBone = new Bone(name, null, Position, tex, Size, Origin);

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

            bone = new Bone(name, parentBone, Position, tex, Size, Origin);

            Console.WriteLine("created bone " + bone.GetName() + " as child to " + bone.ParentName());

            foreach (XElement b in parent.Elements("Bone"))
            {
                bone.AddChild(LoadBone(bone, b));
            }

            return bone;
        }

        public Bone GetBone(string name)
        {
            Bone bone = null;

            if (rootBone.GetName() != name)
            {
                Console.WriteLine("bone was not root, traversing");
                bone = rootBone.GetChild(name);
            }
            else
                bone = rootBone;

            return bone;
        }

        public void SetAnimation(BoneAnimation anim)
        {
            animLength = anim.animLength;
            
            foreach (AnimationBone aBone in anim.GetBones())
            {
                Bone b = GetBone(aBone.boneName);

                if (b != null)
                {
                    b.SetRotations(aBone.Rotations);
                    b.SetImages(aBone.Images);
                    CConsole.Instance.Print("set rotations for bone "+b.GetName());
                }
                else
                    CConsole.Instance.Print("tried to set rotations for " + aBone.boneName + " but it was null");
            }
        }

        public void UpdateSkeleton(float frame)
        {
            animFrame = frame;

            animFrame %= animLength;

            if (rootBone != null)
                rootBone.UpdateBone(animFrame, animLength);
        }

        public void RenderSkeleton(float scale)
        {
            if (rootBone != null)
                rootBone.RenderBone(scale);
        }
    }
}