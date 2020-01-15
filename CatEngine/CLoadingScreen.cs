using System;
using System.Threading;
using System.Diagnostics;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
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

        float fRotation = 0.0f;

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
            QueueLoadCommand(CAudioManager.Instance, "LoadSound", new List<string>() { "footstep_solid1.wav" });
            QueueLoadCommand(CAudioManager.Instance, "LoadSound", new List<string>() { "footstep_solid1.wav" });
            QueueLoadCommand(CAudioManager.Instance, "LoadSong", new List<string>() { "test.xm" });
            //QueueLoadCommand(CLevel.Instance, "LoadPropData", new List<string>());
            //QueueLoadCommand(CRender.Instance, "LoadModel", new List<string>() { "textured_cube" });
            //QueueLoadCommand(CRender.Instance, "LoadModel", new List<string>() { "board" });
            QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "cube_tex" });
            QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "grassside" });
            QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "grasstop" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures/Particles", "dustcloud" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "pankka_body" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "pankka_head" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedModel", new List<string>() { "AssetData/Models/Player", "player" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { "AssetData/Models/Player/Animations", "player_tposebones" });
            QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { "AssetData/Models/Player/Animations", "player_walkcyclebones" });
            //QueueLoadCommand(CRender.Instance, "LoadSimpleModel", new List<string>() { "AssetData/Models", "natsa", ".fbx" });
            //QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "natsa" });
            PrepareModel("Natsa");
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "shadow" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "tex_empty" });
            QueueLoadCommand(CRender.Instance, "InitPlayer", new List<string>());
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "background" });

            PrepareLevelData("Test3");
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
            QueueLoadCommand(CLevel.Instance, "LoadTerrainData", new List<string>() { path });
            //CLevel.Instance.SetTextureArray(textureList.ToArray());

            //QueueLoadCommand(CRender.Instance, "LoadModel", new List<string>() { "terrain" });
            QueueLoadCommand(CRender.Instance, "LoadSimpleModel", new List<string>() { path, "terrain", ".dae" });
            CLevel.Instance.SetLevelInfo(textureList.ToArray(), "terrain");

            //skybox
            //QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "background" });

            //load prop models into memory

            //load prop textures into memory

            //load music into memory
            //QueueLoadCommand(CAudioManager.Instance, "LoadSong", new List<string>() { "test.xm" });
        }

        public void UnloadLevelData()
        {
            //unload textures
            string[] textures = CLevel.Instance.GetTextureArray();

            for(int i = 0; i < textures.Length; i++)
            {
                QueueLoadCommand(CRender.Instance, "UnloadTexture", new List<string>() { textures[i] });
            }

            //unload prop models

            //unload prop textures

            //unload music
        }

        public void PrepareModel(String modelName)
        {
            string path = "AssetData/Models/";

            string metaData = path + modelName + "/" + modelName + ".meta";

            List<string> tex = new List<string>();
            string mdlName;

            if (File.Exists(metaData))
            {
                XDocument file;
                string xmlText = File.ReadAllText(metaData);
                file = XDocument.Parse(xmlText);

                Console.WriteLine(xmlText);

                foreach (XElement e in file.Descendants("texture"))
                {
                    string name = e.Attribute("name").Value;

                    tex.Add(name);

                    Console.WriteLine("texture " + name);
                    QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { path + modelName + "/Textures", name });
                }

                foreach (XElement e in file.Descendants("model"))
                {
                    mdlName = e.Attribute("name").Value;
                    string type = e.Attribute("filetype").Value;

                    Console.WriteLine("model " + mdlName);
                    QueueLoadCommand(CRender.Instance, "LoadSimpleModel", new List<string>() { path + modelName, mdlName, "."+type });
                }
            }
            else
                CConsole.Instance.Print("Metadata for " + modelName + " wasn't found!");

            //QueueLoadCommand(CRender.Instance, "LoadSimpleModel", new List<string>() { "AssetData/Models", modelName, ".fbx" });
            //QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", modelName });
            CRender.Instance.AddModelData(modelName, tex.ToArray());
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
            CSprite.Instance.DrawRect(new Rectangle(0, 0, CSettings.Instance.GAME_VIEW_WIDTH, CSettings.GAME_VIEW_HEIGHT), Color.Black);

            float maxLength = 160.0f;
            int xpos = CSettings.GAME_VIEW_HEIGHT-100;
            int ypos = CSettings.Instance.GAME_VIEW_WIDTH/2;

            int xpos2 = CSettings.Instance.GAME_VIEW_WIDTH - 64;
            int ypos2 = CSettings.GAME_VIEW_HEIGHT - 64;


            float length = maxLength * ((float)iExecutedFunctions / (float)sCommands.Count());

            /*CSprite.Instance.Render("sprLoadBar", xpos, ypos, 0, false, 0.0f, 1.0f, Color.White);
            CSprite.Instance.Render("sprLoadBar", xpos+maxLength, ypos, 2, false, 0.0f, 1.0f, Color.White);

            for (int i = 1; i <= (int)maxLength/2; i++)
            {
                int img = 1;

                if (length >= i*2)
                    img = 3;

                CSprite.Instance.Render("sprLoadBar", xpos+i*2, ypos, img, false, 0.0f, 1.0f, Color.White); // + (int)(Math.Sin(dir+(i/5)) * 3)
            }*/

            fRotation += 0.1f;
            CSprite.Instance.Render("sprLoadCircle", xpos2, ypos2, 0, false, fRotation, 1.0f, Color.White);
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
