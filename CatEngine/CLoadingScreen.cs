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
            QueueLoadCommand(CLevel.Instance, "LoadLevelData", new List<string>() { "LevelTest.xml" });

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

        public void QueueLoadCommand(CContentManager instance, String command, List<String> commands)
        {
            sCommands.Add(new Command(instance, command, commands));
        }

        public void Render()
        {
            dir += Math.PI/60;
            dir %= 2*Math.PI;

            float maxLength = 160.0f;
            int xpos = (CSettings.Instance.GAME_VIEW_WIDTH / 2) - (int)(maxLength / 2);
            int ypos = (CSettings.GAME_VIEW_HEIGHT / 4) * 3;

            float length = maxLength * ((float)iExecutedFunctions / (float)sCommands.Count());

            /*CSprite.Instance.DrawRect(new Rectangle(xpos, ypos, (int)maxLength+69, 12), Color.Gray);
            CSprite.Instance.DrawRect(new Rectangle(xpos, ypos, (int)length+69, 12), Color.White);*/

            CSprite.Instance.Render("sprLoadBar", xpos, ypos+(int)(Math.Sin(dir)*6), 0, false, 0.0f, 1.0f, Color.White);
            CSprite.Instance.Render("sprLoadBar", xpos+maxLength, ypos + (int)(Math.Sin(dir+(maxLength/10)) * 3), 2, false, 0.0f, 1.0f, Color.White);

            for (int i = 1; i <= (int)maxLength/2; i++)
            {
                int img = 1;

                if (length >= i*2)
                    img = 3;

                CSprite.Instance.Render("sprLoadBar", xpos+i*2, ypos + (int)(Math.Sin(dir+(i/5)) * 3), img, false, 0.0f, 1.0f, Color.White);
            }

            //CSprite.Instance.Render("sprTest", 160+64, 30, 0, false, (float)dir, 1.0f, Color.White);
        }

        private void LoadData()
        {
            foreach (Command i in sCommands)
            {
                //load data;
                
                MethodInfo method = i.Instance.GetType().GetMethod(i.MethodName);
                method.Invoke(i.Instance, i.Params.ToArray());

                Interlocked.Increment(ref iExecutedFunctions);
                CConsole.Instance.Print("performed command " + iExecutedFunctions + ", " + i.MethodName);

                Thread.Sleep(1000);
            }

            //CAudioManager.Instance.PlaySong("test.xm");

            //clearing the list so we don't accidentally load the same stuff twice...
            sCommands.Clear();

            //hasFinishedLoading = true;
        }
    }
}
