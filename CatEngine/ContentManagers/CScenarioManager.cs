using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace CatEngine.Content
{
    public class CScenarioManager : CContentManager
    {
        private List<Objective> pObjectiveList = new List<Objective>();

        private CScenarioManager()
        {
        }

        //singletoning the singleton
        public static CScenarioManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CScenarioManager instance = new CScenarioManager();
        }

        public void LoadScenario(String level, String scenarioFile)
        {
            //string path = "AssetData/Models/";

            string sceData = "AssetData/Levels/" + level + "/Scenario/" + scenarioFile + ".sce";

            List<string> tex = new List<string>();
            string mdlName;

            if (File.Exists(sceData))
            {
                XDocument file;
                string xmlText = File.ReadAllText(sceData);
                file = XDocument.Parse(xmlText);

                //Console.WriteLine(xmlText);

                string scenarioName = file.Root.Attribute("name").Value;

                foreach (XElement e in file.Descendants("objective"))
                {
                    Objective objective = new Objective();

                    Objective.ObjectiveType objectiveType = (Objective.ObjectiveType)Enum.Parse(typeof(Objective.ObjectiveType), e.Attribute("type").Value);

                    string str = e.Attribute("string").Value;

                    Console.WriteLine(objectiveType.ToString());

                    if (objectiveType == Objective.ObjectiveType.Elimination)
                    {
                        foreach (XElement t in e.Descendants("target"))
                        {
                            float x = Int32.Parse(t.Attribute("x").Value);
                            float y = Int32.Parse(t.Attribute("y").Value);
                            float z = Int32.Parse(t.Attribute("z").Value);

                            Type targetType = typeof(CNatsa);//Type.GetType(t.Attribute("type").Value);

                            if (targetType == null)
                                Console.WriteLine("nigga the type is null " + t.Attribute("type").Value);
                            else
                                Console.WriteLine("nigga the type is "+targetType.ToString());

                            objective.SetObjective(str, targetType, x, y, z);
                        }

                        foreach (XElement t in e.Descendants("enemyspawn"))
                        {
                            float x = float.Parse(t.Attribute("x").Value);
                            float y = float.Parse(t.Attribute("y").Value);

                            int amount = Int32.Parse(t.Attribute("amount").Value);

                            //Type targetType = Type.GetType(t.Attribute("type").Value);

                            objective.CreateEnemySpawner(typeof(CPlayerBullet),amount, x, y);
                        }
                    }
                    else if (objectiveType == Objective.ObjectiveType.Reach)
                    {
                        foreach (XElement t in e.Descendants("checkpoint"))
                        {
                            float x = Int32.Parse(t.Attribute("x").Value);
                            float y = Int32.Parse(t.Attribute("y").Value);

                            objective.SetObjective(str, x, y);
                        }

                        foreach (XElement t in e.Descendants("enemyspawn"))
                        {
                            float x = float.Parse(t.Attribute("x").Value);
                            float y = float.Parse(t.Attribute("y").Value);

                            int amount = Int32.Parse(t.Attribute("amount").Value);

                            //Type targetType = Type.GetType(t.Attribute("type").Value);

                            objective.CreateEnemySpawner(typeof(CPlayerBullet), amount, x, y);
                        }
                    }
                    else if (objectiveType == Objective.ObjectiveType.Survival)
                    {
                        foreach (XElement t in e.Descendants("timer"))
                        {
                            int time = Int32.Parse(t.Attribute("time").Value);

                            objective.SetObjective(str, time);
                        }

                        foreach (XElement t in e.Descendants("enemyspawn"))
                        {
                            float x = float.Parse(t.Attribute("x").Value);
                            float y = float.Parse(t.Attribute("y").Value);

                            int amount = Int32.Parse(t.Attribute("amount").Value);

                            //Type targetType = Type.GetType(t.Attribute("type").Value);

                            objective.CreateEnemySpawner(typeof(CPlayerBullet), amount, x, y);
                        }
                    }
                    else if (objectiveType == Objective.ObjectiveType.Event)
                    {
                        foreach (XElement t in e.Descendants("bubble"))
                        {
                            List<string> dialog = new List<string>();
                            List<string> subtitles = new List<string>();

                            foreach (XElement l in t.Descendants("line"))
                            {
                                dialog.Add(l.Attribute("soundbyte").Value);
                                subtitles.Add(l.Value);
                            }

                            objective.SetObjective(str, dialog, subtitles);
                        }
                    }

                    pObjectiveList.Add(objective);
                }

                foreach (XElement e in file.Descendants("model"))
                {
                    mdlName = e.Attribute("name").Value;
                    string type = e.Attribute("filetype").Value;
                }

                foreach (XElement e in file.Descendants("animation"))
                {
                    string name = e.Attribute("name").Value;

                    tex.Add(name);

                    Console.WriteLine("texture " + name);
                }
            }
            else
                CConsole.Instance.Print("Scenario data for " + sceData + " wasn't found!");
        }

        private void RemoveObjective(Objective objective)
        {
            pObjectiveList.Remove(objective);
        }

        public void Update()
        {
            if (pObjectiveList.Count > 0)
            {
                if (pObjectiveList[0].isActive)
                    pObjectiveList[0].UpdateObjective();
                else
                    RemoveObjective(pObjectiveList[0]);
            }
        }

        public void Render()
        {
            if (pObjectiveList.Count > 0)
            {
                if (pObjectiveList[0].isActive)
                    pObjectiveList[0].DrawMarker();
            }
        }

        public void RenderHUD()
        {
            if (pObjectiveList.Count > 0)
            {
                if (pObjectiveList[0].isActive)
                    pObjectiveList[0].DrawObjective();
            }
        }
    }

    class Objective
    {
        public enum ObjectiveType
        {
            Survival,
            Reach,
            Elimination,
            Event
        };

        public ObjectiveType currentObjective = ObjectiveType.Reach;

        private CGameObject Target;

        private int Timer;

        private int TextInd = 0;

        private List<string> DialogSoundBytes;
        private List<string> DialogSubtitles;

        private List<EnemySpawner> spawnerList = new List<EnemySpawner>();

        public bool isActive = true;

        private string sObjectiveString = "";

        public Objective()
        {

        }

        public void SetObjective(string objStr, float x, float y)
        {
            currentObjective = ObjectiveType.Reach;
            float h = CLevel.Instance.GetHeightAt(x, y, 999);
            Target = CObjectManager.Instance.CreateInstance(typeof(CCheckpoint), x, h, y);
            sObjectiveString = objStr;
        }

        public void SetObjective(string objStr, Type targetType, float x, float y, float z)
        {
            currentObjective = ObjectiveType.Elimination;
            Target = CObjectManager.Instance.CreateInstance(targetType, x, z, y);
            sObjectiveString = objStr;
        }

        public void SetObjective(string objStr, int time)
        {
            currentObjective = ObjectiveType.Survival;
            sObjectiveString = objStr;
            Timer = time;
        }

        public void SetObjective(string objStr, List<string> dialog, List<string> subtitles)
        {
            currentObjective = ObjectiveType.Event;
            sObjectiveString = objStr;

            DialogSoundBytes = dialog;

            DialogSubtitles = subtitles;
        }

        public void CreateEnemySpawner(Type enemyType, int amount, float x, float y)
        {
            spawnerList.Add(new EnemySpawner(this, enemyType, amount, x, y));
        }

        public void RemoveEnemySpawner(EnemySpawner spawner)
        {
            spawnerList.Remove(spawner);
        }

        public void UpdateObjective()
        {
            if (currentObjective == ObjectiveType.Elimination)
            {
                if (Target == null || !CObjectManager.Instance.IndexExists(Target.iIndex))
                {
                    Console.WriteLine("objective complete!");
                    Target = null;
                    isActive = false;
                }
            }
            else if (currentObjective == ObjectiveType.Reach)
            {
                if (Target == null || !CObjectManager.Instance.IndexExists(Target.iIndex))
                {
                    Console.WriteLine("objective complete!");
                    Target = null;
                    isActive = false;
                }
            }
            else if (currentObjective == ObjectiveType.Survival)
            {
                if (Timer <= 0)
                {
                    Console.WriteLine("objective complete!");
                    isActive = false;
                }

                Timer--;
            }
            else if (currentObjective == ObjectiveType.Event)
            {
                bool dialogueOn = false;

                for (int i = 0; i < DialogSoundBytes.Count; i++)
                {
                    if (CAudioManager.Instance.IsSoundPlaying(DialogSoundBytes[i]))
                        dialogueOn = true;
                }

                if (TextInd < DialogSoundBytes.Count && !dialogueOn)
                {
                    CAudioManager.Instance.PlaySound(DialogSoundBytes[TextInd]);
                    TextInd++;
                }
                else if (TextInd >= DialogSoundBytes.Count && !dialogueOn)
                {
                    Console.WriteLine("objective complete!");
                    Target = null;
                    isActive = false;
                }
            }

            foreach (EnemySpawner e in spawnerList)
            {
                e.Update();
            }
        }

        public void DrawMarker()
        {
            if (Target != null)
                CRender.Instance.DrawBillBoard(new Vector3(Target.x, Target.z + 10, Target.y), new Vector2(5, 5), new Vector2(2.5f, 2.5f), 0, 1, "arrow");
        }

        public void DrawObjective()
        {
            if (currentObjective == ObjectiveType.Survival)
                CSprite.Instance.DrawText(string.Format(sObjectiveString, Timer), new Vector2(64, 10), Color.Red);
            else
                CSprite.Instance.DrawText(sObjectiveString, new Vector2(64, 10), Color.Red);

            if (currentObjective == ObjectiveType.Event)
            {
                if (TextInd > 0)
                CSprite.Instance.DrawText(DialogSubtitles[TextInd-1], new Vector2(CSettings.Instance.GAME_VIEW_WIDTH / 2, CSettings.GAME_VIEW_HEIGHT - 20), Color.White); ;
            }
        }
    }

    class EnemySpawner
    {
        private Objective ParentObjective;

        private Type EnemyType;
        private int Amount;
        private Vector2 Position;

        private CGameObject Enemy = null;

        public EnemySpawner(Objective par, Type enemyType, int amount, float x, float y)
        {
            ParentObjective = par;

            EnemyType = enemyType;
            Amount = amount;
            Position = new Vector2(x, y);
        }

        public void Update()
        {
            if (Amount > 0 && Enemy == null)
            {
                Enemy = CObjectManager.Instance.CreateInstance(EnemyType, Position.X, Position.Y, 20);
            }
            else if (Amount <= 0)
            {
                RemoveSpawner();
            }
        }

        private void RemoveSpawner()
        {
            ParentObjective.RemoveEnemySpawner(this);
        }
    }
}
