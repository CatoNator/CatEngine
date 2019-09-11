using System;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CatEngine
{
    class CLoadingScreen
    {
        private struct Command<T>
        {
            public T Instance;
            public String MethodName;
            public List<String> Params;

            public Command(T instance, String method, List<String> paramList)
            {
                Instance = instance;
                MethodName = method;
                Params = paramList;
            }
        }

        private List<Command<CContentManager>> sCommands = new List<Command<CContentManager>>();
        private int iExecutedFunctions = 0;

        public bool hasFinishedLoading = false;

        double dir = 0.0;

        private CLoadingScreen()
        {
            /*CSprite.Instance.dTextureDict.Add("Player", Content.Load<Texture2D>("Player"));
            CSprite.Instance.dTextureDict.Add("Enemy", Content.Load<Texture2D>("Enemy"));
            CSprite.Instance.dTextureDict.Add("Props", Content.Load<Texture2D>("Props"));
            CSprite.Instance.dTextureDict.Add("Lights", Content.Load<Texture2D>("Lights"));*/

            sCommands.Add(new Command<CContentManager>(CSprite.Instance, "AllocateSprites", new List<string>()));
            sCommands.Add(new Command<CContentManager>(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Player" }));
            sCommands.Add(new Command<CContentManager>(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Enemy" }));
            sCommands.Add(new Command<CContentManager>(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Props" }));
            sCommands.Add(new Command<CContentManager>(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Lights" }));
            sCommands.Add(new Command<CContentManager>(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Tiles" }));
            sCommands.Add(new Command<CContentManager>(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Weapons" }));
            sCommands.Add(new Command<CContentManager>(CAudioManager.Instance, "LoadSound", new List<string>() { "normalshot.wav" }));
            sCommands.Add(new Command<CContentManager>(CAudioManager.Instance, "LoadSound", new List<string>() { "rapidfireshot.wav" }));
            sCommands.Add(new Command<CContentManager>(CAudioManager.Instance, "LoadSong", new List<string>() { "dreamland.it" }));
            sCommands.Add(new Command<CContentManager>(CLevel.Instance, "LoadPropData", new List<string>()));
            sCommands.Add(new Command<CContentManager>(CLevel.Instance, "LoadLevelData", new List<string>() { "LevelTest.xml" }));

            Load();
        }

        //singletoning the singleton
        public static CLoadingScreen Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CLoadingScreen instance = new CLoadingScreen();
        }

        public void Update()
        {
        }

        private void Load()
        {
            Thread loaderThread = new Thread(new ThreadStart(LoadData));
            loaderThread.Start();
        }

        public void Render()
        {
            dir += Math.PI/60;
            //dir %= 2*Math.PI;

            float length = 160.0f * ((float)iExecutedFunctions / (float)sCommands.Count());

            CSprite.Instance.DrawRect(new Rectangle(10, 10, 160, 16), Color.Gray);
            CSprite.Instance.DrawRect(new Rectangle(10, 10, (int)length+1, 16), Color.White);

            //CSprite.Instance.Render("sprTest", 160+64, 30, 0, false, (float)dir, 1.0f, Color.White);
        }

        private void LoadData()
        {
            foreach (Command<CContentManager> i in sCommands)
            {
                //load data;
                
                MethodInfo method = i.Instance.GetType().GetMethod(i.MethodName);
                method.Invoke(i.Instance, i.Params.ToArray());

                Interlocked.Increment(ref iExecutedFunctions);
                Debug.Print("performed command " + iExecutedFunctions + ", " + i.MethodName);
            }

            CAudioManager.Instance.PlaySong("dreamland.it");

            hasFinishedLoading = true;
        }
    }
}
