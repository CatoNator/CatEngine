using System;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CatEngine
{
    class CLoadingScreen
    {
        private struct Command
        {
            public CContentManager Instance;
            public String MethodName;
            public List<String> Params;

            public Command(CContentManager instance, String method, List<String> paramList)
            {
                Instance = instance;
                MethodName = method;
                Params = paramList;
            }
        }

        private List<Command> sCommands = new List<Command>();
        private int iExecutedFunctions = 0;

        public bool hasFinishedLoading = false;

        double dir = 0.0;

        private CLoadingScreen()
        {
            QueueLoadCommand(CSprite.Instance, "AllocateSprites", new List<string>());
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Player" });
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Enemy" });
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Props" });
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Lights" });
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Tiles" });
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Weapons" });
            QueueLoadCommand(CAudioManager.Instance, "LoadSound", new List<string>() { "normalshot.wav" });
            QueueLoadCommand(CAudioManager.Instance, "LoadSound", new List<string>() { "rapidfireshot.wav" });
            QueueLoadCommand(CAudioManager.Instance, "LoadSong", new List<string>() { "test.xm" });
            QueueLoadCommand(CLevel.Instance, "LoadPropData", new List<string>());
            QueueLoadCommand(CRender.Instance, "LoadModel", new List<string>() { "textured_cube" });
            QueueLoadCommand(CRender.Instance, "LoadModel", new List<string>() { "board" });
            QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "cube_tex" });
            QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "grassside" });
            QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "grasstop" });
            QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "swat" });
            //QueueLoadCommand(CRender.Instance, "LoadSkinnedModel", new List<string>() { "soldier" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { "idle" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { "run" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedModel", new List<string>() { "roblox_anim_nod" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { "roblox_anim" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { "roblox_anim_nod" });

            PrepareLevelData("Test");

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

        public void PrepareLevelData(String levelName)
        {
            String path = "AssetData/Levels/" + levelName;

            CConsole.Instance.Print("preparing to load level " + path);

            //pack textures
            string[] textureFiles = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);

            List<string> textureList = new List<string>();
            
            //load textures into memory
            foreach (String texture in textureFiles)
            {
                String texNameShort = texture.Substring(0, texture.Length - 4);
                QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { path, texNameShort });
                textureList.Add(texNameShort);
            }

            //load terrain data
            QueueLoadCommand(CLevel.Instance, "LoadTerrainData", new List<string>() { path+"/terrain.bin" });
            CLevel.Instance.SetTextureArray(textureList.ToArray());
        }

        private void Load()
        {
            Thread loaderThread = new Thread(new ThreadStart(LoadData));
            loaderThread.Start();
        }

        public void QueueLoadCommand(CContentManager instance, String command, List<String> commands)
        {
            sCommands.Add(new Command(instance, command, commands));
        }

        public void Render()
        {
            dir += Math.PI/60;
            dir %= 2*Math.PI;

            float maxLength = 160.0f;
            int xpos = CSettings.GAME_VIEW_HEIGHT-100;
            int ypos = CSettings.Instance.GAME_VIEW_WIDTH/2;

            float length = maxLength * ((float)iExecutedFunctions / (float)sCommands.Count());

            CSprite.Instance.Render("sprLoadBar", xpos, ypos, 0, false, 0.0f, 1.0f, Color.White);
            CSprite.Instance.Render("sprLoadBar", xpos+maxLength, ypos, 2, false, 0.0f, 1.0f, Color.White);

            for (int i = 1; i <= (int)maxLength/2; i++)
            {
                int img = 1;

                if (length >= i*2)
                    img = 3;

                CSprite.Instance.Render("sprLoadBar", xpos+i*2, ypos, img, false, 0.0f, 1.0f, Color.White); // + (int)(Math.Sin(dir+(i/5)) * 3)
            }
        }

        private void LoadData()
        {
            foreach (Command i in sCommands)
            {
                //load data;
                
                MethodInfo method = i.Instance.GetType().GetMethod(i.MethodName);
                method.Invoke(i.Instance, i.Params.ToArray());

                Interlocked.Increment(ref iExecutedFunctions);

                String debug = "";

                foreach (String s in i.Params)
                {
                    debug += s + ", ";
                }

                CConsole.Instance.Print(i.MethodName+"("+debug+")");

                //Thread.Sleep(1000);
            }

            //CAudioManager.Instance.PlaySong("test.xm");

            //clearing the list so we don't accidentally load the same stuff twice...
            sCommands.Clear();

            hasFinishedLoading = true;
        }
    }
}
