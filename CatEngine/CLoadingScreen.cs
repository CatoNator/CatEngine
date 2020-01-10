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
using CatEngine.Content;

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

        public Game1 game;

        private CLoadingScreen()
        {
            //DEBUG: preparing level data for first load
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
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures/Particles", "dustcloud" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "pankka_body" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "pankka_head" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedModel", new List<string>() { "player" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { "player_tposebones" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { "player_walkcyclebones" });
            QueueLoadCommand(CRender.Instance, "LoadSimpleModel", new List<string>() { "natsa" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "natsa" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "shadow" });
            QueueLoadCommand(CRender.Instance, "InitPlayer", new List<string>());

            PrepareLevelData("Test");
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

            DirectoryInfo D = new DirectoryInfo(path);

            //pack textures
            FileInfo[] textureFiles = D.GetFiles("*.png");

            List<string> textureList = new List<string>();
            
            //load ground textures into memory
            foreach (FileInfo texture in textureFiles)
            {
                String texNameShort = texture.Name.Substring(0, texture.Name.Length - 4);
                Debug.Print(path+texNameShort);
                QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { path, texNameShort });
                textureList.Add(texNameShort);
            }

            //load terrain data
            QueueLoadCommand(CLevel.Instance, "LoadTerrainData", new List<string>() { path+"/terrain.bin" });
            CLevel.Instance.SetTextureArray(textureList.ToArray());

            //skybox
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "background" });

            //load prop models into memory

            //load prop textures into memory

            //load music into memory
            //QueueLoadCommand(CAudioManager.Instance, "LoadSong", new List<string>() { "test.xm" });
        }

        public void UnloadLevelData()
        {
            //unload textures

            //unload prop models

            //unload prop textures

            //unload music
        }

        public void Load()
        {
            hasFinishedLoading = false;
            iExecutedFunctions = 0;
            game.CurrentGameState = Game1.GameState.Loading;
            Thread loaderThread = new Thread(new ThreadStart(LoadData));
            loaderThread.Start();
        }

        public void QueueLoadCommand(CContentManager instance, String command, List<String> commands)
        {
            sCommands.Add(new Command(instance, command, commands));
        }

        public void Render()
        {
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
