using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CatEngine
{
    public class CRender : CContentManager
    {
        public GraphicsDeviceManager graphics;

        public ContentManager content;

        private Dictionary<string, Model> dModelDict = new Dictionary<string, Model>();
        private Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();

        private Vector3 cameraPosition = new Vector3(30.0f, 30.0f, 30.0f);
        private Vector3 cameraTarget = new Vector3(0.0f, 0.0f, 0.0f); // Look back at the origin

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        //public Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();
        private CRender()
        {
            float fovAngle = MathHelper.ToRadians(CSettings.Instance.iFovAngle);  // convert 45 degrees to radians
            float aspectRatio = CSettings.Instance.GetAspectRatio();
            float near = 0.01f; // the near clipping plane distance
            float far = 600f; // the far clipping plane distance

            worldMatrix = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fovAngle, aspectRatio, near, far);
        }

        //singletoning the singleton
        public static CRender Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CRender instance = new CRender();
        }

        public void LoadModel(String modelName)
        {
            try
            {
                CRender.Instance.dModelDict.Add(modelName, content.Load<Model>(modelName));
            }
            catch (ContentLoadException e)
            {
                CRender.Instance.dModelDict.Add(modelName, null);
                CConsole.Instance.Print("Tried to load model " + modelName + " but failed, error " + e.ToString());
            }
        }

        public void LoadTexture(String textureName)
        {
            try
            {
                CRender.Instance.dTextureDict.Add(textureName, content.Load<Texture2D>(textureName));
            }
            catch (ContentLoadException e)
            {
                CRender.Instance.dTextureDict.Add(textureName, null);
                CConsole.Instance.Print("Tried to load texture " + textureName + " but failed, error " + e.ToString());
            }
        }

        public void SetCameraPosition(Vector3 position)
        {
            cameraPosition = position;
        }

        public Vector3 GetCameraPosition()
        {
            return cameraPosition;
        }

        public void SetCameraTarget(Vector3 targetPosition)
        {
            cameraTarget = targetPosition;
        }

        public void DrawModel(string modelName, Vector3 position, float rotation)
        {
            Matrix positionMatrix = Matrix.CreateTranslation(position);
            Matrix rotationMatrix =  Matrix.CreateRotationY(rotation);

            Matrix transformMatrix = rotationMatrix * positionMatrix;

            Model model = dModelDict[modelName];

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.Texture = dTextureDict["cube_tex"];
                    effect.AmbientLightColor = new Vector3(1f, 0, 0);
                    effect.View = viewMatrix;
                    effect.World = worldMatrix*transformMatrix;
                    effect.Projection = projectionMatrix;
                }
                mesh.Draw();
            }
        }

        private VertexBuffer RectanglePrimitive(GraphicsDevice graphicsDevice, Vector3 C1, Vector3 C2, Vector3 C3, Vector3 C4, Color color)
        {
            /*
            1-----2
            |     |
            |     |
            3-----4
            */

            VertexPositionColor[] vertices = new VertexPositionColor[6]
            {
                //polygon 1
                new VertexPositionColor(new Vector3(C1.X, C1.Y, C1.Z), color),
                new VertexPositionColor(new Vector3(C3.X, C3.Y, C3.Z), color),
                new VertexPositionColor(new Vector3(C2.X, C2.Y, C2.Z), color),

                //polygon 2
                new VertexPositionColor(new Vector3(C2.X, C2.Y, C2.Z), color),
                new VertexPositionColor(new Vector3(C3.X, C3.Y, C3.Z), color),
                new VertexPositionColor(new Vector3(C4.X, C4.Y, C4.Z), color)
            };

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 6, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            return vertexBuffer;
        }

        public void DrawRectangle(GraphicsDevice graphicsDevice, Vector3 C1, Vector3 C2, Vector3 C3, Vector3 C4, Color color)
        {
            VertexBuffer vertexBuffer = RectanglePrimitive(graphicsDevice, C1, C2, C3, C4, color);

            BasicEffect basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            vertexBuffer.Dispose();
        }

        public void DrawTile(GraphicsDevice graphicsDevice, int[] iPosition, float[]fCornerHeights, int iTileSize)
        {
            VertexBuffer tileBuffer = RectanglePrimitive(graphicsDevice,
                new Vector3(iPosition[0] * iTileSize, fCornerHeights[0], iPosition[1] * iTileSize),
                new Vector3(iPosition[0] * iTileSize + iTileSize, fCornerHeights[1], iPosition[1] * iTileSize),
                new Vector3(iPosition[0] * iTileSize, fCornerHeights[2], iPosition[1] * iTileSize + iTileSize),
                new Vector3(iPosition[0] * iTileSize + iTileSize, fCornerHeights[3], iPosition[1] * iTileSize + iTileSize), Color.Green);


            BasicEffect basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;

            graphicsDevice.SetVertexBuffer(tileBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            tileBuffer.Dispose();
        }

        public void UpdateCamera()
        {
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);

            /*RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = rasterizerState;*/
        }
    }
}
