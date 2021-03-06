﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine
{
    public sealed class CObjectManager
    {
        public CGameObject[] pGameObjectList;
        public const int MAX_INSTANCES = 256;

        public CLight[] pLightList;
        public const int MAX_LIGHTS = 16;

        public int iGameObjects;

        public Texture2D txTexture;

        public SpriteBatch sbSpriteBatch;

        public GraphicsDeviceManager graphics;

        private CObjectManager()
        {
            this.iGameObjects = 0;

            pGameObjectList = new CGameObject[MAX_INSTANCES];

            pLightList = new CLight[MAX_LIGHTS];
        }

        //singletoning the singleton
        public static CObjectManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CObjectManager instance = new CObjectManager();
        }

        //creates a new instance and returns a reference to that instance
        public CGameObject CreateInstance(Type instanceType, float x, float z, float y)
        {
            CGameObject returnObject = null;

            //int debugObjSlot = 0;

            //find the first empty slot in the array and put the object there
            for (int i = 0; i < MAX_INSTANCES; i++)
            {
                if (pGameObjectList[i] == null)
                {
                    //making the object!!!!
                    try
                    {
                        returnObject = pGameObjectList[i] = (CGameObject)Activator.CreateInstance(instanceType);
                    }
                    catch (Exception e)
                    {
                        try
                        {
                            CConsole.Instance.Print("Could not create entity type of " + instanceType.ToString() + "! " + e.Message);
                        }
                        catch (Exception a)
                        {
                            CConsole.Instance.Print("Entity type was null! " + a.Message);
                        }
                    }

                    if (returnObject != null)
                    {
                        //this is for the function
                        //returnObject = pGameObjectList[i];

                        //here we make sure the object spawns correctly
                        returnObject.Spawn(x, z, y, i);

                        //debugObjSlot = i;
                    }
                    break;
                }
            }

            if (returnObject == null)
                CConsole.Instance.Print("Entity creation failed!");

            return returnObject;
        }

        public CLight CreateLight(int x, int y)
        {
            CLight returnObject = null;

            //int debugObjSlot = 0;

            //find the first empty slot in the array and put the object there
            for (int i = 0; i < MAX_LIGHTS; i++)
            {
                if (pLightList[i] == null)
                {
                    //making the object!!!!
                    pLightList[i] = new CLight(x, y);

                    //this is for the function
                    returnObject = pLightList[i];

                    //debugObjSlot = i;

                    break;
                }
            }

            return returnObject;
        }

        //removing a gameobject at index
        public void DestroyInstance(int index)
        {
            //making sure we're not deleting a nonexistent object
            if (pGameObjectList[index] != null)
            {
                //calling the ondestruction subroutine
                pGameObjectList[index].OnDestruction();

                pGameObjectList[index].Dispose();

                //he's dead jim
                pGameObjectList[index] = null;

                //Console.WriteLine("Removed object with index of " + index);
            }
            else
                CConsole.Instance.Print("Tried to remove a nonexistent object with index of " + index);
        }

        public bool IndexExists(int index)
        {
            return pGameObjectList[index] != null;
        }

        //the gameobject updating loop
        public void Update()
        {
            for (int i = 0; i < MAX_INSTANCES; i++)
            {
                //making sure the instance exists and is active
                if (pGameObjectList[i] != null && pGameObjectList[i].bActive)
                    pGameObjectList[i].Update();
            }

            /*Console.WriteLine("New frame!");
            for (int i = 0; i < MAX_INSTANCES; i++)
            {
                if (pGameObjectList[i] != null)
                    Console.WriteLine("Object at array slot " + i + " with an index of " + pGameObjectList[i].iIndex);
            }*/
        }

        //the rendering loop
        public void Render(string technique)
        {
            //rendering them fuckos
            for (int i = 0; i < MAX_INSTANCES; i++)
            {
                if (pGameObjectList[i] != null)
                {
                    pGameObjectList[i].Render(technique);
                }
            }
        }

        public void Render2D()
        {
            //rendering them fuckos
            for (int i = 0; i < MAX_INSTANCES; i++)
            {
                if (pGameObjectList[i] != null)
                {
                    pGameObjectList[i].Render2D();
                }
            }
        }

        public void RenderLights()
        {
            //rendering them fuckos
            for (int i = 0; i < MAX_LIGHTS; i++)
            {
                if (pLightList[i] != null)
                    pLightList[i].Render();
            }
        }
    }
}
