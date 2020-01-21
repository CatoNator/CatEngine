using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using CatEngine.Content;

namespace CatEngine
{
    public class CParticleManager
    {
        private Particle[] pParticleList = new Particle[MAX_PARTICLES];
        public const int MAX_PARTICLES = 512;

        private Dictionary<String, ParticleData> dPartDataDict = new Dictionary<string, ParticleData>();
        private CParticleManager()
        {
        }

        //singletoning the singleton
        public static CParticleManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CParticleManager instance = new CParticleManager();
        }

        private struct ParticleData
        {
            public string TextureName;

            public float AngleMin;
            public float AngleMax;
            public float SizeMin;
            public float SizeMax;
            public float AlphaMin;
            public float AlphaMax;

            public Vector3 Speed;
            public float RotationSpeed;
            public float SizeChangeSpeed;
            public float AlphaChangeSpeed;

            public bool bHasGravity;

            public int Life;

            public ParticleData(string tex, float minAngle, float maxAngle, float minSize, float maxSize, float minAlpha, float maxAlpha, Vector3 sp, float rotSp, float sizeSp, float aSp, int lf)
            {
                TextureName = tex;
                AngleMin = minAngle;
                AngleMax = maxAngle;
                SizeMin = minSize;
                SizeMax = maxSize;
                AlphaMin = minAlpha;
                AlphaMax = maxAlpha;

                Speed = sp;
                RotationSpeed = rotSp;
                SizeChangeSpeed = sizeSp;
                AlphaChangeSpeed = aSp;

                bHasGravity = false;

                Life = lf;
            }
        }

        private class Particle
        {
            private string TextureName = "dustcloud";

            private int ID = 0;
            private Vector3 Position;
            private float Angle = 0.0f;
            private float Size = 5.0f;
            private float Alpha = 1.0f;

            private Vector3 Speed;
            private float RotationSpeed = 0.3f;
            private float SizeChangeSpeed = -0.05f;
            private float AlphaChangeSpeed = 0.0f;
            private const float Gravity = 0.07f;

            private bool bHasGravity = false;

            private int Life = 60;

            public Particle()
            {

            }

            public void Spawn (int ind, Vector3 pos, ParticleData partData)
            {
                Random rand = new Random();

                ID = ind;
                Position = pos;
                TextureName = partData.TextureName;
                Speed = partData.Speed;
                Life = partData.Life;
                Angle = partData.AngleMin + (float)(rand.NextDouble() * (partData.AngleMax - partData.AngleMin));
                Size = partData.SizeMin + (float)(rand.NextDouble() * (partData.SizeMax - partData.SizeMin));
                Alpha = partData.AlphaMin + (float)(rand.NextDouble() * (partData.AlphaMax - partData.AlphaMin));
                RotationSpeed = partData.RotationSpeed;
                SizeChangeSpeed = partData.SizeChangeSpeed;
                AlphaChangeSpeed = partData.AlphaChangeSpeed;
                Life = partData.Life;
            }

            public void Update()
            {
                if (bHasGravity)
                    Speed.Y += Gravity;

                Position += Speed;

                Angle += RotationSpeed;

                Size += SizeChangeSpeed;

                Alpha += AlphaChangeSpeed;

                Life--;
                
                if (Life <= 0 || Size <= 0 || Alpha <= 0)
                    CParticleManager.Instance.DestroyParticle(ID);
            }

            public void Render()
            {
                CRender.Instance.DrawBillBoard(Position, new Vector2(Size, Size), new Vector2(Size / 2.0f, Size / 2.0f), Angle*(float)Math.PI/180, Alpha, TextureName);
            }
        }

        public void LoadParticleData()
        {
            String partData = "AssetData/PartData.xml";

            /*
            <partdata>
	            <particle name="part_dustcloud"/>
		            <texture name="dust_cloud"/>
		            <angle min="0" max="360"/>
		            <size min="1" max="2.5"/>
		            <alpha min="1" max = "1"/>
		
		            <speed xmin="0" xmax="0" ymin="0" ymax="0" zmin="0" zmax="0"/>
		            <rotspeed value="0.3"/>
		            <sizespeed value="-0.05"/>
		            <alphaspeed value="0.0"/>
	            </particle>
            </partdata>
            */

            if (File.Exists(partData))
            {
                Console.WriteLine("opening partdata");

                XDocument file;
                string xmlText = File.ReadAllText(partData);
                file = XDocument.Parse(xmlText);

                foreach (XElement e in file.Descendants("particle"))
                {
                    string name = e.Attribute("name").Value;

                    Console.WriteLine("particle " + name);

                    string tex = "";
                    float amin = 0;
                    float amax = 0;
                    float anmin = 0;
                    float anmax = 0;
                    float smin = 0;
                    float smax = 0;
                    Vector3 sp = new Vector3(0, 0, 0);
                    float ssp = 0;
                    float asp = 0;
                    float rsp = 0;
                    int life = 0;

                    foreach (XElement d in e.Descendants("texture"))
                    {
                        tex = d.Attribute("name").Value;
                    }

                    foreach (XElement d in e.Descendants("alpha"))
                    {
                        amin = float.Parse(d.Attribute("min").Value);
                        amax = float.Parse(d.Attribute("max").Value);
                    }

                    foreach (XElement d in e.Descendants("angle"))
                    {
                        anmin = float.Parse(d.Attribute("min").Value);
                        anmax = float.Parse(d.Attribute("max").Value);
                    }

                    foreach (XElement d in e.Descendants("size"))
                    {
                        smin = float.Parse(d.Attribute("min").Value);
                        smax = float.Parse(d.Attribute("max").Value);
                    }

                    foreach (XElement d in e.Descendants("speed"))
                    {
                        float x = float.Parse(d.Attribute("xmin").Value);
                        float y = float.Parse(d.Attribute("ymin").Value);
                        float z = float.Parse(d.Attribute("zmin").Value);

                        sp = new Vector3(x, y, z);
                    }

                    foreach (XElement d in e.Descendants("rotspeed"))
                    {
                        rsp = float.Parse(d.Attribute("value").Value);
                    }

                    foreach (XElement d in e.Descendants("sizespeed"))
                    {
                        ssp = float.Parse(d.Attribute("value").Value);
                    }

                    foreach (XElement d in e.Descendants("alphaspeed"))
                    {
                        asp = float.Parse(d.Attribute("value").Value);
                    }

                    foreach (XElement d in e.Descendants("life"))
                    {
                        life = Int32.Parse(d.Attribute("value").Value);
                    }

                    dPartDataDict.Add(name, new ParticleData(tex, anmin, anmax, smin, smax, amin, amax, sp, rsp, ssp, asp, life));
                }
            }
            else
                CConsole.Instance.Print("could not locate partdata!");
        }

        public void CreateParticle(String particleName, Vector3 position)
        {
            //find the first empty slot in the array and put the object there
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                if (pParticleList[i] == null)
                {
                    //making the object!!!!
                    pParticleList[i] = new Particle();

                    //here we make sure the object spawns correctly
                    pParticleList[i].Spawn(i, position, dPartDataDict[particleName]);

                    //debugObjSlot = i;

                    break;
                }
            }
        }

        public void DestroyParticle(int index)
        {
            //making sure we're not deleting a nonexistent object
            if (pParticleList[index] != null)
            {
                //he's dead jim
                pParticleList[index] = null;

                //Console.WriteLine("Removed object with index of " + index);
            }
            else
                CConsole.Instance.Print("Tried to remove a nonexistent particle with index of " + index);
        }

        public void Render()
        {
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                //making sure the instance exists and is active
                if (pParticleList[i] != null)
                    pParticleList[i].Update();
                if (pParticleList[i] != null)
                    pParticleList[i].Render();
            }
        }
    }
}
