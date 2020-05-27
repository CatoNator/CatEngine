using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml.Linq;

namespace CatEngine.SkeletalAnimation
{
    public struct AnimationBone
    {
        public string boneName;

        public float[] Rotations;

        public int[] Images;

        public AnimationBone(string name, float[] rots, int[] images)
        {
            boneName = name;
            Rotations = rots;
            Images = images;
        }
    }

    public class BoneAnimation
    {
        public int animLength = 0;

        List<AnimationBone> Bones;

        public BoneAnimation()
        {
            Bones = new List<AnimationBone>();
        }

        public void LoadAnimation(string path, string animName)
        {
            string animData = path + "/" + animName + ".key";

            if (File.Exists(animData))
            {
                XDocument file;
                string xmlText = File.ReadAllText(animData);
                file = XDocument.Parse(xmlText);

                animLength = Int32.Parse(file.Root.Attribute("frames").Value);

                foreach (XElement bone in file.Descendants("Bone"))
                {
                    System.Globalization.CultureInfo ci = (System.Globalization.CultureInfo)System.Globalization.CultureInfo.CurrentCulture.Clone();
                    ci.NumberFormat.CurrencyDecimalSeparator = ".";

                    string name = bone.Attribute("name").Value;

                    List<float> Rotations = new List<float>();
                    List<int> Images = new List<int>();

                    foreach (XElement f in bone.Elements("Frame"))
                    {
                        Rotations.Add(float.Parse(f.Value, System.Globalization.NumberStyles.Any, ci));
                        Images.Add(Int32.Parse(f.Attribute("txind").Value));
                    }

                    Bones.Add(new AnimationBone(name, Rotations.ToArray(), Images.ToArray()));

                    Console.WriteLine("loaded animation frames for bone " + name);
                }
            }
            else
                CConsole.Instance.Print("Animation data for " + animData + " wasn't found!");
        }

        public List<AnimationBone> GetBones()
        {
            return Bones;
        }
    }
}
