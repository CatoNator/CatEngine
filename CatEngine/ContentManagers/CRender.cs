using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using CatEngine.SimpleMdl;
using CatEngine.SkinnedMdl;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CatEngine
{
    public class CRender : CContentManager
    {
        public GraphicsDeviceManager graphics;

        public GraphicsDevice graphicsDevice;

        public ContentManager content;

        private Dictionary<string, Model> dModelDict = new Dictionary<string, Model>();
        private Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();

        private Dictionary<string, SkinnedModel> dSkinnedModelDict = new Dictionary<string, SkinnedModel>();
        private Dictionary<string, SkinnedModelAnimation> dSkinnedAnimationDict = new Dictionary<string, SkinnedModelAnimation>();

        private Vector3 cameraPosition = new Vector3(30.0f, 30.0f, 30.0f);
        private Vector3 cameraTarget = new Vector3(0.0f, 0.0f, 0.0f); // Look back at the origin

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private Vector3 SunOrientation = new Vector3(0, 0, 3);

        private GameTime gameTime;

        //effects
        private BasicEffect basicEffect;

        //we set this up for skinned models
        public Effect SimpleModelEffect { get; set; }
        public Effect SkinnedModelEffect { get; set; }

        //public Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();
        private CRender()
        {
        }

        public void Init()
        {
            float fovAngle = MathHelper.ToRadians(CSettings.Instance.iFovAngle);  // convert 45 degrees to radians
            float aspectRatio = CSettings.Instance.GetAspectRatio();
            float near = 0.01f; // the near clipping plane distance
            float far = 600f; // the far clipping plane distance

            basicEffect = new BasicEffect(graphicsDevice);

            worldMatrix = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fovAngle, aspectRatio, near, far);

            SimpleModelEffect = content.Load<Effect>("SimpleModelEffect");
            SkinnedModelEffect = content.Load<Effect>("SkinnedModelEffect");
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

        public void LoadSkinnedModel(String modelName)
        {
            try
            {
                SkinnedModel mdl = new SkinnedModel();
                mdl.GraphicsDevice = graphicsDevice;
                mdl.FilePath = "AssetData/Models/"+modelName+".dae";
                mdl.Initialize();
                CRender.Instance.dSkinnedModelDict.Add(modelName, mdl);
            }
            catch (ContentLoadException e)
            {
                CRender.Instance.dSkinnedModelDict.Add(modelName, null);
                CConsole.Instance.Print("Tried to load skinned model " + modelName + " but failed, error " + e.ToString());
            }
        }

        public void LoadSkinnedAnimation(String animationName)
        {
            try
            {
                SkinnedModelAnimation anim = new SkinnedModelAnimation();
                anim.FilePath = "AssetData/Models/Animations/" + animationName + ".dae";
                anim.Load();

                CRender.Instance.dSkinnedAnimationDict.Add(animationName, anim);
            }
            catch (ContentLoadException e)
            {
                CRender.Instance.dSkinnedAnimationDict.Add(animationName, null);
                CConsole.Instance.Print("Tried to load skinned animation " + animationName + " but failed, error " + e.ToString());
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
                try
                {
                    CRender.Instance.dTextureDict.Add(textureName, content.Load<Texture2D>("empty"));
                    CConsole.Instance.Print("Tried to load texture " + textureName + " but failed, error " + e.ToString());
                }
                catch (Exception a)
                {
                    CConsole.Instance.Print("Tried to add texture " + textureName + " but it was already there");
                }
            }
        }

        public void LoadTextureRaw(String path, String textureName)
        {
            try
            {
                FileStream fileStream = new FileStream(textureName+".png", FileMode.Open);
                CRender.Instance.dTextureDict.Add(textureName, Texture2D.FromStream(graphicsDevice, fileStream));
                fileStream.Dispose();
            }
            catch (ContentLoadException e)
            {
                try
                {
                    CRender.Instance.dTextureDict.Add(textureName, content.Load<Texture2D>("empty"));
                    CConsole.Instance.Print("Tried to load texture " + textureName + " but failed, error " + e.ToString());
                }
                catch (Exception a)
                {
                    CConsole.Instance.Print("Tried to add texture " + textureName + " but it was already there");
                }
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

        public void UpdateGameTime(GameTime time)
        {
            gameTime = time;
        }

        private float CalculateAngleDifference(float A1, float A2)
        {
            float diff = 0.0f;

            A1 = Math.Abs(A1) % 360;
            A2 = Math.Abs(A2) % 360;

            diff = (float)Math.Abs((Math.Cos((A2 - A1)) - 1) / 2);

            return diff;
        }

        private Vector3 GetNormal(Vector3 C1, Vector3 C2, Vector3 C3)
        {
            //subtract the vectors
            Vector3 ab = C1 - C3;
            Vector3 cb = C2 - C3;

            ab.Normalize();
            cb.Normalize();
            //get a vector perpendicular to those two edges
            return Vector3.Cross(ab, cb);
        }

        private Color ColorBlend(Color C1, Color C2, float amount)
        {
            return new Color(
                    (C1.R * (1 - amount) + C2.R * amount) / 2.0f,
                    (C1.G * (1 - amount) + C2.G * amount) / 2.0f,
                    (C1.B * (1 - amount) + C2.B * amount) / 2.0f);
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

        public void DrawSkinnedModel(String modelName, String animName, Vector3 position, float rotation)
        {
            SkinnedModelInstance skinnedModelInstance = new SkinnedModelInstance();
            dSkinnedModelDict[modelName].Meshes[0].Texture = dTextureDict["cube_tex"];
            skinnedModelInstance.Mesh = dSkinnedModelDict[modelName];
            skinnedModelInstance.SpeedTransitionSecond = 0.4f;
            skinnedModelInstance.Initialize();
            skinnedModelInstance.SetAnimation(dSkinnedAnimationDict[animName]);

            Matrix positionMatrix = Matrix.CreateTranslation(position);
            Matrix rotationMatrix = Matrix.CreateRotationY(rotation);

            skinnedModelInstance.Transformation = Matrix.CreateScale(1.0f) * Matrix.CreateRotationX(-(float)Math.PI / 2) * rotationMatrix * positionMatrix;

            skinnedModelInstance.Update(gameTime);

            SkinnedModelEffect.Parameters["Texture1"].SetValue(dTextureDict["swat"]);
            SkinnedModelEffect.Parameters["SunOrientation"].SetValue(Vector3.Normalize(new Vector3(3, 5, 2)));
            SkinnedModelEffect.Parameters["World"].SetValue(skinnedModelInstance.Transformation);
            SkinnedModelEffect.Parameters["WorldViewProjection"].SetValue(skinnedModelInstance.Transformation * viewMatrix * projectionMatrix);

            foreach (var meshInstance in skinnedModelInstance.MeshInstances)
            {
                SkinnedModelEffect.Parameters["gBonesOffsets"].SetValue(meshInstance.BonesOffsets);
                SkinnedModelEffect.Parameters["Texture1"].SetValue(meshInstance.Mesh.Texture);

                graphicsDevice.SetVertexBuffer(meshInstance.Mesh.VertexBuffer);
                graphicsDevice.Indices = meshInstance.Mesh.IndexBuffer;
                graphicsDevice.DepthStencilState = DepthStencilState.Default;
                foreach (EffectPass pass in SkinnedModelEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, meshInstance.Mesh.FaceCount);
                }
            }

            //skinnedModelInstance.Dispose();
        }

        private VertexBuffer RectanglePrimitive(Vector3 C1, Vector3 C2, Vector3 C3, Vector3 C4, bool flipTexV)
        {
            /*
            1-----2
            |     |
            |     |
            3-----4
            */

            int flipTexVInt = flipTexV ? 1 : 0;
            int notFlipTexVInt = !flipTexV ? 1 : 0;

            Vector3 Normal = GetNormal(C2, C3, C4);

            float lightValue = (CalculateAngleDifference(SunOrientation.X, Normal.X) + CalculateAngleDifference(SunOrientation.Y, Normal.Y) + CalculateAngleDifference(SunOrientation.Z, Normal.Z)) / 3.0f;

            Color color = Color.White * (0.3f + (0.7f * lightValue));

            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[6]
            {
                //polygon 1
                new VertexPositionColorTexture(new Vector3(C1.X, C1.Y, C1.Z), color, new Vector2(0, 1*(flipTexVInt))),
                new VertexPositionColorTexture(new Vector3(C3.X, C3.Y, C3.Z), color, new Vector2(0, 1*(notFlipTexVInt))),
                new VertexPositionColorTexture(new Vector3(C2.X, C2.Y, C2.Z), color, new Vector2(1, 1*(flipTexVInt))),

                //polygon 2
                new VertexPositionColorTexture(new Vector3(C2.X, C2.Y, C2.Z), color, new Vector2(1, 1*(flipTexVInt))),
                new VertexPositionColorTexture(new Vector3(C3.X, C3.Y, C3.Z), color, new Vector2(0, 1*(notFlipTexVInt))),
                new VertexPositionColorTexture(new Vector3(C4.X, C4.Y, C4.Z), color, new Vector2(1, 1*(notFlipTexVInt)))
            };

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), 6, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

            return vertexBuffer;
        }

        public void DrawRectangle(Vector3 C1, Vector3 C2, Vector3 C3, Vector3 C4, String textureName, bool flipTexV)
        {
            VertexBuffer vertexBuffer = RectanglePrimitive(C1, C2, C3, C4, flipTexV);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = dTextureDict[textureName];

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            basicEffect.Texture = dTextureDict[textureName];

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            vertexBuffer.Dispose();
        }

        public void DrawBillBoard(Vector3 position, Vector2 size, Vector2 origin, String textureName)
        {
            /*
            C1 =
            C2 =
            C3 =
            C4 =
            
            VertexBuffer vertexBuffer = RectanglePrimitive(graphicsDevice, C1, C2, C3, C4, false);

            BasicEffect basicEffect = new BasicEffect(graphicsDevice);
            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = false;
            basicEffect.LightingEnabled = false;
            basicEffect.TextureEnabled = true;
            basicEffect.Texture = dTextureDict["grasstop"];

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            basicEffect.Texture = dTextureDict[textureName];

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            basicEffect.Dispose();
            vertexBuffer.Dispose();
            */
        }

        public void UpdateCamera()
        {
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            //SunOrientation = new Vector3(SunOrientation.X, SunOrientation.Y, SunOrientation.Z + 0.01f);
        }
    }
}
