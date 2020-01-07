﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    public class CParticleManager
    {
        private Particle[] pParticleList = new Particle[MAX_PARTICLES];
        public const int MAX_PARTICLES = 512;

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

        private class Particle
        {
            private int ID = 0;
            private Vector3 Position;
            private Vector3 Speed;

            private int Life = 60;

            public Particle()
            {

            }

            public void Spawn (Vector3 pos, Vector3 spd, int lifeTime, int ind)
            {
                ID = ind;
                Position = pos;
                Speed = spd;
            }

            public void Update()
            {
                Position += Speed;

                if (Life > 0)
                    Life--;
                else
                    CParticleManager.Instance.DestroyParticle(ID);
            }

            public void Render()
            {
                CRender.Instance.DrawBillBoard(Position, new Vector2(5, 5), new Vector2(2.5f, 2.5f), "grasstop");
            }
        }

        public void CreateParticle(Vector3 position, Vector3 speed)
        {
            //find the first empty slot in the array and put the object there
            for (int i = 0; i < MAX_PARTICLES; i++)
            {
                if (pParticleList[i] == null)
                {
                    //making the object!!!!
                    pParticleList[i] = new Particle();

                    //here we make sure the object spawns correctly
                    pParticleList[i].Spawn(position, speed, 240, i);

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
