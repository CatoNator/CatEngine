﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CatEngine.Content;
using Microsoft.Xna.Framework;

namespace CatEngine
{
    class CGame
    {
        public int iNatsas = 7;

        public int iPlayerHealth = 5;
        public int iMaxPlayerHealth = 5;

        private float fNatsaOffset = 10;
        private float fHealthOffset = 0;

        private float fNatsaFrame = 0;

        private float fHealthCycle = 0.0f;

        private Vector2 vPlayerPosition;

        private float fNorthDirection = 0f;

        private float fMapScale = 2f;

        public enum FadeTypes
        {
            FadeLevel,
            FadeMenu
        };

        public FadeTypes currentFadeType = FadeTypes.FadeMenu;

        private float fFadeAlpha = 0;
        private string sNextLevel = "";

        private enum FadeStates
        {
            FadeIn,
            FadeOut
        };

        private FadeStates currentFadeState = FadeStates.FadeIn;

        public enum Player
        {
            Pankka, 
            Ingman
        };

        private CGame()
        {
        }

        //singletoning the singleton
        public static CGame Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CGame instance = new CGame();
        }

        public void CollectNatsa(int amount)
        {
            iNatsas += amount;
            CAudioManager.Instance.PlaySound("natsa");
        }

        public void UpdatePlayer(Vector2 pos)
        {
            vPlayerPosition = pos;
        }

        public void UpdateCamera(float dir)
        {
            fNorthDirection = -(((dir + 90f) * (float)Math.PI) / 180f);
        }

        public void InitiateFadeLevel(string nextLevel)
        {
            currentFadeState = FadeStates.FadeOut;
            currentFadeType = FadeTypes.FadeLevel;

            sNextLevel = nextLevel;
        }

        public void UpdateFade()
        {
            if (currentFadeState == FadeStates.FadeOut)
            {
                fFadeAlpha += 0.025f;

                if (fFadeAlpha >= 1)
                {
                    currentFadeState = FadeStates.FadeIn;
                    CLoadingScreen.Instance.UnloadLevelData();
                    CLoadingScreen.Instance.PrepareLevelData(sNextLevel);
                    CLoadingScreen.Instance.Load();
                }
                    
            }
            else if (currentFadeState == FadeStates.FadeIn)
            {
                if (fFadeAlpha > 0)
                    fFadeAlpha -= 0.025f;
            }
        }

        public void RenderFadeOut()
        {
            if (fFadeAlpha > 0)
                CSprite.Instance.DrawRect(new Rectangle(0, 0, CSettings.Instance.GAME_VIEW_WIDTH, CSettings.GAME_VIEW_HEIGHT), Color.Black * fFadeAlpha);
        }

        private float distDirX(float dist, float dir)
        {
            return (float)(Math.Cos(dir) * dist);
        }

        private float distDirY(float dist, float dir)
        {
            return (float)(-Math.Sin(dir) * dist);
        }

        public float PointDirection(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Atan2((double)(y2 - y1), (double)(x2 - x1));
        }

        public float PointDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow((double)(x2 - x1), 2) + Math.Pow((double)(y2 - y1), 2));
        }

        private float clamp (float val, float min, float max)
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public void RenderHUD()
        {
            fNatsaFrame += 0.125f;
            fNatsaFrame %= 8;

            fHealthCycle += 0.0625f;
            fHealthCycle %= (float)Math.PI * 2;

            for (int i = 0; i < iPlayerHealth; i++)
            {
                CSprite.Instance.Render("sprHudChoco", 5 - fHealthOffset + 3 * (float)Math.Sin(fHealthCycle + (0.5 * i)), 5 + i * 20, 0, false, 0, 1.0f, Color.White);
            }

            //natsa
            CSprite.Instance.Render("sprHudNatsa", CSettings.Instance.GAME_VIEW_WIDTH / 2-32, 5 + fNatsaOffset + 3 * (float)Math.Sin(fHealthCycle + (0.5 * 1)), (int)fNatsaFrame, false, 0, 1, Color.White);

            List<Vector3> objectives = CScenarioManager.Instance.GetObjectivePositions();

            foreach (Vector3 vec in objectives)
            {
                float dir = PointDirection(vPlayerPosition.X, vPlayerPosition.Y, vec.X, vec.Z) - (fNorthDirection - (float)Math.PI);
                float dist = PointDistance(vPlayerPosition.X, vPlayerPosition.Y, vec.X, vec.Z) / fMapScale;

                dist = clamp(dist, 0, 32);
                
                CSprite.Instance.Render("sprRadarBlips", 44 - distDirX(dist, dir), CSettings.GAME_VIEW_HEIGHT - 44 + distDirY(dist, dir), 1, false, 0, 0.5f, Color.White);
            }

            CSprite.Instance.Render("sprRadarBlips", 44 + distDirX(32, fNorthDirection), CSettings.GAME_VIEW_HEIGHT - 44 + distDirY(32, fNorthDirection), 2, false, 0, 0.5f, Color.White);
            CSprite.Instance.Render("sprRadarBorder", 44, CSettings.GAME_VIEW_HEIGHT - 44, 0, false, 0, 1f, Color.White);
            //CSprite.Instance.Render("sprRadarBorder", 44, CSettings.GAME_VIEW_HEIGHT - 44, 1, false, -fNorthDirection, 0.8f, Color.White);

            CScenarioManager.Instance.RenderHUD(new Vector2(92, CSettings.GAME_VIEW_HEIGHT - 13));

            //the x symbol for counting the natsas
            /*CSprite.Instance.Render("numeric_font", CSettings.Instance.GAME_VIEW_WIDTH / 2-6, 7 + fNatsaOffset + 3 * (float)Math.Sin(fHealthCycle + (0.5 * 2)), 10, false, 0, 1, Color.White);

            //number itself
            CSprite.Instance.Render("numeric_font", CSettings.Instance.GAME_VIEW_WIDTH / 2 + 8, 7 + fNatsaOffset + 3 * (float)Math.Sin(fHealthCycle + (0.5 * 3)), iNatsas/10, false, 0, 1, Color.White);
            CSprite.Instance.Render("numeric_font", CSettings.Instance.GAME_VIEW_WIDTH / 2 + 21, 7 + fNatsaOffset + 3 * (float)Math.Sin(fHealthCycle + (0.5 * 4)), iNatsas%10, false, 0, 1, Color.White);*/
        }
    }
}
