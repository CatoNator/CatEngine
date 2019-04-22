using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatEngine
{
    public sealed class CGameManager
    {
        //this shouldn't be here but it is now because I can't be bothered to make a new gamestate manager
        public int iScore = 0;

        public int iLives = 3;

        private CGameManager()
        {
        }

        //singletoning the singleton
        public static CGameManager Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CGameManager instance = new CGameManager();
        }

        public void UpdateGame()
        {
            //if no bricks exist then make some
            if (!BallExists())
            {
                CObjectManager.Instance.CreateInstance(typeof(CBall), 208, 180);
            }

            if (!WallExists())
            {
                for (int j = 0; j < 4; j++)
                {
                    for (int i = 0; i < 26; i++)
                    {
                        CObjectManager.Instance.CreateInstance(typeof(CWall), 16 * i, 2 + 10 * j);
                    }
                }
            }
        }

        private bool WallExists()
        {
            bool doesItReallyExistIWonder = false;

            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                if (CObjectManager.Instance.pGameObjectList[i] != null
                    && Object.ReferenceEquals(typeof(CWall), CObjectManager.Instance.pGameObjectList[i].GetType()))
                    doesItReallyExistIWonder = true;
            }
            
            return doesItReallyExistIWonder;
        }

        private bool BallExists()
        {
            bool doesItReallyExistIWonder = false;

            for (int i = 0; i < CObjectManager.MAX_INSTANCES; i++)
            {
                if (CObjectManager.Instance.pGameObjectList[i] != null
                    && Object.ReferenceEquals(typeof(CBall), CObjectManager.Instance.pGameObjectList[i].GetType()))
                    doesItReallyExistIWonder = true;
            }

            return doesItReallyExistIWonder;
        }
    }
}
