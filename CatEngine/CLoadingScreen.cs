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
            //QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Player" });
            //QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Enemy" });
            //QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Props" });
            //QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "Weapons" });

            QueueLoadCommand(CAudioManager.Instance, "LoadBank", new List<string>() { "AssetData/Sounds", "Collectible" });
            QueueLoadCommand(CAudioManager.Instance, "LoadBank", new List<string>() { "AssetData/Sounds", "Menu" });
            QueueLoadCommand(CAudioManager.Instance, "LoadBank", new List<string>() { "AssetData/Sounds", "Player" });
            QueueLoadCommand(CAudioManager.Instance, "LoadSong", new List<string>() { "AssetData/Music", "test", "xm" });
            //QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "cube_tex" });
            //QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "grassside" });
            //QueueLoadCommand(CRender.Instance, "LoadTexture", new List<string>() { "grasstop" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures/Particles", "dustcloud" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures/Particles", "muzzleflash" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures/Particles", "smoke" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures/Particles", "bullet_casing" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "bullet" });

            PrepareModel("AssetData/Models/Player/", "player");
            PrepareModel("AssetData/Models/Natsa/", "natsa");

            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "shadow" });
            QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { "AssetData/Textures", "tex_empty" });
            QueueLoadCommand(CRender.Instance, "InitPlayer", new List<string>());
            //QueueLoadCommand(CSprite.Instance, "LoadTextureSheet", new List<string>() { "background" });

            //PrepareLevelData("Test3");
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
            /*FileInfo[] textureFiles = D.GetFiles("*.png");

            List<string> textureList = new List<string>();
            
            //load ground textures into memory
            foreach (FileInfo texture in textureFiles)
            {
                String texNameShort = texture.Name.Substring(0, texture.Name.Length - 4);
                Debug.Print(path+texNameShort);
                QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { path, texNameShort });
                textureList.Add(texNameShort);
            }*/

            //load terrain data
            QueueLoadCommand(CLevel.Instance, "LoadTerrainData", new List<string>() { path });
            //CLevel.Instance.SetTextureArray(textureList.ToArray());

            //QueueLoadCommand(CRender.Instance, "LoadModel", new List<string>() { "terrain" });
            //QueueLoadCommand(CRender.Instance, "LoadSimpleModel", new List<string>() { path, "terrain", ".dae" });
            CLevel.Instance.SetLevelModelName(levelName);
            PrepareModel(path+"/", levelName);
            //CLevel.Instance.SetLevelInfo(textureList.ToArray(), "terrain");

            //skybox
            QueueLoadCommand(CSprite.Instance, "LoadTextureSheetRaw", new List<string>() { "AssetData/Textures", "background" });

            //load props into memory
            PrepareProp("AssetData/Props/hexatri/", "hexatri");

            //load music into memory
            //QueueLoadCommand(CAudioManager.Instance, "LoadSong", new List<string>() { "test.xm" });
        }

        public void UnloadLevelData()
        {
            //unload level mesh and textures
            /*string[] textures = CLevel.Instance.GetTextureArray();

            for(int i = 0; i < textures.Length; i++)
            {
                QueueLoadCommand(CRender.Instance, "UnloadTexture", new List<string>() { textures[i] });
            }*/

            QueueLoadCommand(CRender.Instance, "UnloadModelData", new List<string>() { CLevel.Instance.GetLevelModelName() });

            //unload prop models

            //unload prop textures

            //unload music
        }

        public void PrepareProp(String path, String propName)
        {
            //string path = "AssetData/Models/";

            PrepareModel(path, propName);
        }

        public void PrepareModel(String path, String modelName)
        {
            //string path = "AssetData/Models/";

            string metaData = path + modelName + ".meta"; //+ modelName + "/" 

            List<string> tex = new List<string>();
            string mdlName;

            if (File.Exists(metaData))
            {
                XDocument file;
                string xmlText = File.ReadAllText(metaData);
                file = XDocument.Parse(xmlText);

                //Console.WriteLine(xmlText);

                string modelType = file.Root.Attribute("type").Value;

                foreach (XElement e in file.Descendants("texture"))
                {
                    string name = e.Attribute("name").Value;

                    tex.Add(name);

                    Console.WriteLine("texture " + name);
                    QueueLoadCommand(CRender.Instance, "LoadTextureRaw", new List<string>() { path + "Textures", name });
                }

                foreach (XElement e in file.Descendants("model"))
                {
                    mdlName = e.Attribute("name").Value;
                    string type = e.Attribute("filetype").Value;

                    if (modelType.Equals("skinnedmodel"))
                        QueueLoadCommand(CRender.Instance, "LoadSkinnedModel", new List<string>() { path, mdlName });
                    else
                        QueueLoadCommand(CRender.Instance, "LoadSimpleModel", new List<string>() { path, mdlName, "."+type });

                    Console.WriteLine("model " + path + mdlName);
                }

                foreach (XElement e in file.Descendants("animation"))
                {
                    string name = e.Attribute("name").Value;

                    tex.Add(name);

                    Console.WriteLine("texture " + name);
                    QueueLoadCommand(CRender.Instance, "LoadSkinnedAnimation", new List<string>() { path + "Animations", name });
                }
            }
            else
                CConsole.Instance.Print("Metadata for " + metaData + " wasn't found!");

            //we are fucked lol
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
            Console.WriteLine("added command " + command + " to loadcommands");
        }

        public void Render()
        {
            int xpos2 = CSettings.Instance.GAME_VIEW_WIDTH - 64;
            int ypos2 = CSettings.GAME_VIEW_HEIGHT - 64;

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

            if (CGame.Instance.currentFadeType == CGame.FadeTypes.FadeLevel)
            {
                game.CurrentGameState = Game1.GameState.Game;
                //CAudioManager.Instance.PlaySong("test");
            }
            else if (CGame.Instance.currentFadeType == CGame.FadeTypes.FadeMenu)
                game.CurrentGameState = Game1.GameState.Menu;
        }
    }
}
