using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Forms;
using System.IO;
using CatEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace CatEd
{
    public class CLevelView : MonoGame.Forms.Controls.MonoGameControl
    {
        private float fCameraRotation = 0.0f;
        private float fCameraVRotation = 5.0f;
        private float fCameraDistance = 30.0f;

        private bool contentLoaded = false;

        protected override void Initialize()
        {
            base.Initialize();

            CRender.Instance.graphicsDevice = GraphicsDevice;
            CRender.Instance.content = Editor.Content;
            //CRender.Instance.graphics = Editor.graphics;
            CRender.Instance.Init();

            if (!contentLoaded)
            {
                //CLevel.Instance.GenerateLevel();
                PrepareLevelData("Test");
                contentLoaded = true;
            }

        }

        public void PrepareLevelData(String levelName)
        {
            String path = "AssetData/Levels/" + levelName;

            //pack textures
            string[] textureFiles = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);

            List<string> textureList = new List<string>();

            //load textures into memory
            foreach (String texture in textureFiles)
            {
                String texNameShort = texture.Substring(0, texture.Length - 4);
                CRender.Instance.LoadTextureRaw(path, texNameShort);
                textureList.Add(texNameShort);
            }

            //load terrain data
            CLevel.Instance.LoadTerrainData(path + "/terrain.bin");
            CLevel.Instance.SetTextureArray(textureList.ToArray());
        }

        private void CameraBehaviour(int rotDir, int rotDirV)
        {
            int LevelW = CLevel.Instance.iLevelWidth;
            int LevelH = CLevel.Instance.iLevelHeight;
            int TileS = CLevel.Instance.iTileSize;

            float x = 0.0f;
            float y = 0.0f;
            float z = 0.0f;

            Vector3 targetPos = new Vector3((LevelW / 2) * TileS, 10.0f, (LevelH / 2) * TileS);

            CRender.Instance.SetCameraTarget(targetPos);

            float fCameraRotationSpeed = 4.0f;

            fCameraRotation += (float)rotDir * fCameraRotationSpeed;

            fCameraDistance = LevelW * TileS + 30.0f;

            if (fCameraRotation > 180.0f)
                fCameraRotation -= 360.0f;
            else if (fCameraRotation < -180.0f)
                fCameraRotation += 360.0f;

            fCameraVRotation += (float)rotDirV * fCameraRotationSpeed;

            if (fCameraVRotation > 80.0f)
                fCameraVRotation = 80.0f;
            else if (fCameraVRotation < 5.0f)
                fCameraVRotation = 5.0f;

            Vector3 cameraPos = new Vector3(x, z, y);

            float camDist = Mathf.PointDistance(cameraPos.X, cameraPos.Z, targetPos.X, targetPos.Z);

            x = targetPos.X + Mathf.distDirX(fCameraDistance, Mathf.degToRad(fCameraRotation));
            y = targetPos.Z + Mathf.distDirY(fCameraDistance, Mathf.degToRad(fCameraRotation));
            z = targetPos.Y + -Mathf.distDirY(fCameraDistance, Mathf.degToRad(fCameraVRotation));

            CRender.Instance.SetCameraPosition(new Vector3(x, z, y));
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            CameraBehaviour(1, 0);
        }

        protected override void Draw()
        {
            base.Draw();

            this.GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;
            this.GraphicsDevice.Clear(Color.Black);

            CRender.Instance.UpdateCamera();

            //Editor.spriteBatch.Begin();

            //this creates a memory leak for whatever reason
            CLevel.Instance.Render();

            //Editor.spriteBatch.End();
        }
    }
}
