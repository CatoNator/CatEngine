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

namespace CatEngine.Content
{
    public class CRender : CContentManager
    {
        public GraphicsDeviceManager graphics;

        public GraphicsDevice graphicsDevice;

        public ContentManager content;

        private Dictionary<string, ModelData> dModelDataDict = new Dictionary<string, ModelData>();
        private Dictionary<string, Model> dModelDict = new Dictionary<string, Model>();
        private Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();

        private Dictionary<string, SimpleModel> dSimpleModelDict = new Dictionary<string, SimpleModel>();
        private Dictionary<string, SkinnedModel> dSkinnedModelDict = new Dictionary<string, SkinnedModel>();
        private Dictionary<string, SkinnedModelAnimation> dSkinnedAnimationDict = new Dictionary<string, SkinnedModelAnimation>();

        private Vector3 cameraPosition = new Vector3(30.0f, 30.0f, 30.0f);
        private Vector3 cameraTarget = new Vector3(0.0f, 0.0f, 0.0f); // Look back at the origin

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private Vector3 SunOrientation = new Vector3(4.5f, 4.5f, 2.0f);

        private GameTime gameTime;

        //effects
        private BasicEffect basicEffect;
        private VertexBuffer rectangleBuffer;

        private SkinnedModelInstance playerInstance = new SkinnedModelInstance();

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
            rectangleBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), 6, BufferUsage.WriteOnly);

            worldMatrix = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fovAngle, aspectRatio, near, far);

            SimpleModelEffect = content.Load<Effect>("Effects/SimpleModelEffect");
            SkinnedModelEffect = content.Load<Effect>("Effects/SkinnedModelEffect");
        }

        public void InitPlayer()
        {
            playerInstance.Mesh = dSkinnedModelDict["player"];
            playerInstance.Mesh.Meshes[0].Texture = dTextureDict["pankka_body"];
            playerInstance.Mesh.Meshes[1].Texture = dTextureDict["pankka_head"];
            playerInstance.SpeedTransitionSecond = 0.05f;
            playerInstance.Initialize();
            //playerInstance.SetAnimation(dSkinnedAnimationDict["player_tposebones"], gameTime);
        }

        public void InitEditor()
        {
            float fovAngle = MathHelper.ToRadians(CSettings.Instance.iFovAngle);  // convert 45 degrees to radians
            float aspectRatio = CSettings.Instance.GetAspectRatio();
            float near = 0.01f; // the near clipping plane distance
            float far = 600f; // the far clipping plane distance

            basicEffect = new BasicEffect(graphicsDevice);
            rectangleBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), 6, BufferUsage.WriteOnly);

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

        private struct ModelData
        {
            public String Model;
            public String[] Textures;
            public ModelData(String mdl, String[] tx)
            {
                Model = mdl;
                Textures = tx;
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
                FileStream fileStream = new FileStream(path + "/" + textureName + ".png", FileMode.Open);
                CRender.Instance.dTextureDict.Add(textureName, Texture2D.FromStream(graphicsDevice, fileStream));
                fileStream.Dispose();
            }
            catch (Exception e)
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

        public void LoadModel(String modelName)
        {
            try
            {
                CRender.Instance.dModelDict.Add(modelName, content.Load<Model>(modelName));
            }
            catch (Exception e)
            {
                CRender.Instance.dModelDict.Add(modelName, null);
                CConsole.Instance.Print("Tried to load model " + modelName + " but failed, error " + e.ToString());
            }
        }

        public void LoadSimpleModel(String path, String modelName, String fileType)
        {
            try
            {
                SimpleModel mdl = new SimpleModel();
                mdl.GraphicsDevice = graphicsDevice;
                mdl.FilePath = path + "/" + modelName + fileType;
                mdl.Initialize();
                CRender.Instance.dSimpleModelDict.Add(modelName, mdl);
            }
            catch (Exception e)
            {
                try
                {
                    CRender.Instance.dSimpleModelDict.Add(modelName, null);
                }
                catch (Exception a)
                {
                    CConsole.Instance.Print("Tried to load skinned model " + modelName + " but failed, error " + e.ToString());
                }
            }
        }

        public void LoadSkinnedModel(String path, String modelName)
        {
            try
            {
                SkinnedModel mdl = new SkinnedModel();
                mdl.GraphicsDevice = graphicsDevice;
                mdl.FilePath = path+"/"+modelName+".fbx";
                mdl.Initialize();
                CRender.Instance.dSkinnedModelDict.Add(modelName, mdl);
            }
            catch (ContentLoadException e)
            {
                CRender.Instance.dSkinnedModelDict.Add(modelName, null);
                CConsole.Instance.Print("Tried to load skinned model " + modelName + " but failed, error " + e.ToString());
            }
        }

        public void LoadSkinnedAnimation(String path, String animationName)
        {
            try
            {
                SkinnedModelAnimation anim = new SkinnedModelAnimation();
                anim.FilePath = path+"/" + animationName + ".fbx";
                anim.Load();

                CRender.Instance.dSkinnedAnimationDict.Add(animationName, anim);
            }
            catch (ContentLoadException e)
            {
                CRender.Instance.dSkinnedAnimationDict.Add(animationName, null);
                CConsole.Instance.Print("Tried to load skinned animation " + animationName + " but failed, error " + e.ToString());
            }
        }

        public void AddModelData(string modelName, string[] textures)
        {
            try
            {
                CRender.Instance.dModelDataDict.Add(modelName, new ModelData(modelName, textures));
                CConsole.Instance.Print("added model " + modelName);
            }
            catch (Exception e)
            {
                CConsole.Instance.Print("Tried to load model " + modelName + " but failed, error " + e.ToString());
            }
        }

        //disposing of non-contentmanager stuff here
        public void UnloadSimpleModel(String modelName)
        {
            try
            {
                //dSimpleModelDict[modelName].Dispose();
                dSimpleModelDict.Remove(modelName);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("Tried to unload simple model " + modelName + ", but it wasn't present");
            }
        }

        public void UnloadSkinnedModel(String modelName)
        {
            try
            {
                //dSkinnedModelDict[modelName].Dispose();
                dSkinnedModelDict.Remove(modelName);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("Tried to unload skinned model " + modelName + ", but it wasn't present");
            }
        }

        public void UnloadModelData(String modelName)
        {
            try
            {
                ModelData model = dModelDataDict[modelName];
                UnloadSimpleModel(model.Model);
                
                foreach (string s in model.Textures)
                {
                    UnloadTexture(s);
                }

                dModelDataDict.Remove(modelName);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("Tried to unload model data " + modelName + ", but data it contained was missing");
            }
        }

        public void UnloadSkinnedAnimation(String animationName)
        {
            try
            {
                //dSkinnedModelDict[modelName].Dispose();
                dSkinnedAnimationDict.Remove(animationName);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("Tried to unload skinned model " + animationName + ", but it wasn't present");
            }
        }

        public void UnloadTexture(String textureName)
        {
            try
            {
                dTextureDict[textureName].Dispose();
                dTextureDict.Remove(textureName);
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("Tried to unload texture " + textureName + ", but it wasn't present");
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

        public Vector3 GetCameraTarget()
        {
            return cameraTarget;
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

        public void DrawModel(string modelName, Vector3 position, float rotation, float scale)
        {
            Matrix positionMatrix = Matrix.CreateTranslation(position);
            Matrix rotationMatrix =  Matrix.CreateRotationY(rotation);

            Matrix transformMatrix = Matrix.CreateScale(scale)*rotationMatrix * positionMatrix;

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

        public void DrawSimpleModel(string modelName, Vector3 position, Vector3 rotation, float scale)
        {
            ModelData mdl;

            try
            {
                mdl = dModelDataDict[modelName];
            }
            catch (KeyNotFoundException e)
            {
                CConsole.Instance.Print("model " + modelName + " wasn't found in dModelDataDict");
                mdl = new ModelData("fuck", new string[] { "off" });
            }

            string mdlName = mdl.Model;
            string[] textures = mdl.Textures;
            
            //SimpleModel simpleModel = dSimpleModelDict[modelName];
            try
            {
                dSimpleModelDict[mdlName].GraphicsDevice = graphicsDevice;
            }
            catch (Exception e)
            {
                Console.WriteLine("could not load model "+ mdlName + " to render");
            }

            Matrix rotationMatrix = Matrix.CreateRotationX(rotation.X) * Matrix.CreateRotationY(rotation.Y) * Matrix.CreateRotationZ(rotation.Z);
            Matrix transformMatrix = Matrix.CreateTranslation(position);

            Matrix scaleMatrix = Matrix.CreateScale(scale);

            
            SimpleModelEffect.Parameters["World"].SetValue(scaleMatrix * rotationMatrix * transformMatrix * worldMatrix);
            SimpleModelEffect.Parameters["WorldViewProjection"].SetValue(scaleMatrix * rotationMatrix * transformMatrix * worldMatrix * viewMatrix * projectionMatrix);
            try
            {
                SimpleModelEffect.Parameters["Texture1"].SetValue(dTextureDict[textures[0]]);
            }
            catch (Exception e)
            {
                try
                {
                    Console.WriteLine("could not load texture " + textures[0] + " to render");
                }
                catch (Exception a)
                {
                    Console.WriteLine("could not load texture of model " + modelName + " to render");
                }
            }

            graphicsDevice.SetVertexBuffer(dSimpleModelDict[mdlName].VertexBuffer);
            graphicsDevice.Indices = dSimpleModelDict[modelName].IndexBuffer;
            //graphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (EffectPass pass in SimpleModelEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType.TriangleList, 0, 0, dSimpleModelDict[modelName].FaceCount);
            }

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;
        }

        public void DrawSkinnedModel(String modelName, String animName, Vector3 position, float rotation)
        {
            SkinnedModelInstance skinnedModelInstance = new SkinnedModelInstance();
            skinnedModelInstance.Mesh = dSkinnedModelDict[modelName];
            skinnedModelInstance.Mesh.Meshes[0].Texture = dTextureDict["pankka_body"];
            skinnedModelInstance.SpeedTransitionSecond = 0.4f;
            skinnedModelInstance.Initialize();
            skinnedModelInstance.SetAnimation(dSkinnedAnimationDict[animName], gameTime);

            Matrix positionMatrix = Matrix.CreateTranslation(position);
            Matrix rotationMatrix = Matrix.CreateRotationY(rotation);

            float scale = 0.025f;

            skinnedModelInstance.Transformation = Matrix.CreateScale(scale) * rotationMatrix * positionMatrix;

            skinnedModelInstance.Update(gameTime);

            SkinnedModelEffect.Parameters["SunOrientation"].SetValue(Vector3.Normalize(SunOrientation));
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

            graphicsDevice.SetVertexBuffer(null);
            graphicsDevice.Indices = null;

            //skinnedModelInstance.Dispose();
        }

        public void DrawPlayer(String animName, String secondaryAnimName, Vector3 position, float rotation, float animFrame, float animFade)
        {
            if (playerInstance.Animation != dSkinnedAnimationDict[animName])
            {
                try
                {
                    playerInstance.SetAnimation(dSkinnedAnimationDict[animName], gameTime);
                }
                catch (KeyNotFoundException e)
                {
                    playerInstance.SetAnimation(dSkinnedAnimationDict["player_tpose"], gameTime);
                }
            }

            if (secondaryAnimName != null)
                playerInstance.SetSecondaryAnimation(dSkinnedAnimationDict["player_tpose"], gameTime);
            else
                playerInstance.SetSecondaryAnimation(null, gameTime);

            playerInstance.SecondaryAnimationFading = animFade;

            playerInstance.FrameIndex = animFrame;

            Matrix positionMatrix = Matrix.CreateTranslation(position);
            Matrix rotationMatrix = Matrix.CreateRotationY(rotation);

            float scale = 0.025f;

            playerInstance.Transformation = Matrix.CreateScale(scale) * rotationMatrix * positionMatrix;

            playerInstance.Update(gameTime);

            SkinnedModelEffect.Parameters["SunOrientation"].SetValue(Vector3.Normalize(SunOrientation));
            SkinnedModelEffect.Parameters["World"].SetValue(playerInstance.Transformation);
            SkinnedModelEffect.Parameters["WorldViewProjection"].SetValue(playerInstance.Transformation * viewMatrix * projectionMatrix);

            foreach (var meshInstance in playerInstance.MeshInstances)
            {
                SkinnedModelEffect.Parameters["gBonesOffsets"].SetValue(meshInstance.BonesOffsets);
                SkinnedModelEffect.Parameters["Texture1"].SetValue(meshInstance.Mesh.Texture);

                graphicsDevice.SetVertexBuffer(meshInstance.Mesh.VertexBuffer);
                graphicsDevice.Indices = meshInstance.Mesh.IndexBuffer;
                foreach (EffectPass pass in SkinnedModelEffect.CurrentTechnique.Passes)
                {
                    pass.Apply();
                    graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, meshInstance.Mesh.FaceCount);
                }
            }

            //skinnedModelInstance.Dispose();
        }

        private VertexPositionColorTexture[] RectanglePrimitive(Vector3 C1, Vector3 C2, Vector3 C3, Vector3 C4, bool flipTexV)
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

            Color color = Color.Lerp(Color.Black, Color.White, 0.1f + (0.9f * lightValue));

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

            return vertices;
        }

        public void DrawRectangle(Vector3 C1, Vector3 C2, Vector3 C3, Vector3 C4, String textureName, bool flipTexV, float alpha)
        {
            VertexPositionColorTexture[] vertices = RectanglePrimitive(C1, C2, C3, C4, flipTexV);
            //rectangleBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), 6, BufferUsage.WriteOnly);
            rectangleBuffer.SetData<VertexPositionColorTexture>(vertices);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            basicEffect.TextureEnabled = true;
            basicEffect.Alpha = alpha;
            basicEffect.Texture = dTextureDict[textureName];

            graphicsDevice.SetVertexBuffer(rectangleBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            graphicsDevice.SetVertexBuffer(null);

            //rectangleBuffer.Dispose();
            //rectangleBuffer = null;
        }

        public void DrawRectangleWireframe(Vector3 C1, Vector3 C2, Vector3 C3, Vector3 C4)
        {
            VertexPositionColorTexture[] vertices = RectanglePrimitive(C1, C2, C3, C4, false);
            rectangleBuffer.SetData<VertexPositionColorTexture>(vertices);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            basicEffect.TextureEnabled = false;

            RasterizerState ogState = graphicsDevice.RasterizerState;
            RasterizerState newState = new RasterizerState();
            newState.FillMode = FillMode.WireFrame;
            newState.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = newState;

            graphicsDevice.SetVertexBuffer(rectangleBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            graphicsDevice.RasterizerState = ogState;
            graphicsDevice.SetVertexBuffer(null);
        }

        public void DrawHitBox(Vector3 pos, float height, float rad)
        {
            //top
            DrawRectangleWireframe(new Vector3(pos.X - rad, pos.Y + height, pos.Z + rad),
                new Vector3(pos.X + rad, pos.Y + height, pos.Z + rad),
                new Vector3(pos.X - rad, pos.Y + height, pos.Z - rad),
                new Vector3(pos.X + rad, pos.Y + height, pos.Z - rad));
            //bottom
            DrawRectangleWireframe(new Vector3(pos.X - rad, pos.Y, pos.Z + rad),
                new Vector3(pos.X + rad, pos.Y, pos.Z + rad),
                new Vector3(pos.X - rad, pos.Y, pos.Z - rad),
                new Vector3(pos.X + rad, pos.Y, pos.Z - rad));
            //left side
            DrawRectangleWireframe(new Vector3(pos.X - rad, pos.Y + height, pos.Z - rad),
                new Vector3(pos.X - rad, pos.Y + height, pos.Z + rad),
                new Vector3(pos.X - rad, pos.Y, pos.Z - rad),
                new Vector3(pos.X - rad, pos.Y, pos.Z + rad));
            //right side
            DrawRectangleWireframe(new Vector3(pos.X + rad, pos.Y, pos.Z - rad),
                new Vector3(pos.X + rad, pos.Y, pos.Z + rad),
                new Vector3(pos.X + rad, pos.Y + height, pos.Z - rad),
                new Vector3(pos.X + rad, pos.Y + height, pos.Z + rad));
            //bottom side
            DrawRectangleWireframe(new Vector3(pos.X + rad, pos.Y, pos.Z - rad),
                new Vector3(pos.X - rad, pos.Y, pos.Z - rad),
                new Vector3(pos.X + rad, pos.Y + height, pos.Z - rad),
                new Vector3(pos.X - rad, pos.Y + height, pos.Z - rad));
            //top side
            DrawRectangleWireframe(new Vector3(pos.X + rad, pos.Y + height, pos.Z + rad),
                new Vector3(pos.X - rad, pos.Y + height, pos.Z + rad),
                new Vector3(pos.X + rad, pos.Y, pos.Z + rad),
                new Vector3(pos.X - rad, pos.Y, pos.Z + rad));
        }

        public void DrawTriangleWireframe(Vector3 C1, Vector3 C2, Vector3 C3, Color color)
        {
            VertexPositionColor[] vertices = new VertexPositionColor[3]
            {
                //polygon 1
                new VertexPositionColor(new Vector3(C1.X, C1.Y, C1.Z), color),
                new VertexPositionColor(new Vector3(C3.X, C3.Y, C3.Z), color),
                new VertexPositionColor(new Vector3(C2.X, C2.Y, C2.Z), color)
            };

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColor>(vertices);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            basicEffect.TextureEnabled = false;

            RasterizerState ogState = graphicsDevice.RasterizerState;
            RasterizerState newState = new RasterizerState();
            newState.FillMode = FillMode.WireFrame;
            newState.CullMode = CullMode.None;
            graphicsDevice.RasterizerState = newState;

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            graphicsDevice.RasterizerState = ogState;
            graphicsDevice.SetVertexBuffer(null);
            vertexBuffer.Dispose();
        }

        public void DrawTriangleTextured(Vector3 C1, Vector3 C2, Vector3 C3, Color color)
        {
            VertexPositionColorTexture[] vertices = new VertexPositionColorTexture[3]
            {
                //polygon 1
                new VertexPositionColorTexture(new Vector3(C1.X, C1.Y, C1.Z), color, new Vector2(0, 0)),
                new VertexPositionColorTexture(new Vector3(C3.X, C3.Y, C3.Z), color, new Vector2(0, 1)),
                new VertexPositionColorTexture(new Vector3(C2.X, C2.Y, C2.Z), color, new Vector2(1, 1))
            };

            VertexBuffer vertexBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), 3, BufferUsage.WriteOnly);
            vertexBuffer.SetData<VertexPositionColorTexture>(vertices);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = worldMatrix;
            basicEffect.VertexColorEnabled = true;
            basicEffect.LightingEnabled = false;
            basicEffect.TextureEnabled = true;
            basicEffect.Alpha = 1.0f;
            basicEffect.Texture = dTextureDict["tex_empty"];

            graphicsDevice.SetVertexBuffer(vertexBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            graphicsDevice.SetVertexBuffer(null);
            vertexBuffer.Dispose();
        }

        public void DrawBillBoard(Vector3 position, Vector2 scale, Vector2 origin, float angle, float alpha, String textureName)
        {
            float cameraDirection = (float)(Math.PI/2)-(float)Math.Atan2((double)(cameraPosition.Z - cameraTarget.Z), (double)(cameraPosition.X - cameraTarget.X));
            float cameraVDirection = (float)(Math.Atan2((double)(cameraPosition.Y - cameraTarget.Y), 30.0));//(float)Math.Atan2((double)(cameraPosition.Y - cameraTarget.Y), (double)(cameraPosition.X - cameraTarget.X));

            Vector3 C1 = new Vector3(scale.X - origin.X, 0, -origin.Y);
            Vector3 C2 = new Vector3(scale.X - origin.X, 0, scale.Y - origin.Y);
            Vector3 C3 = new Vector3(-origin.X, 0, -origin.Y);
            Vector3 C4 = new Vector3(-origin.X, 0, scale.Y - origin.Y);

            VertexPositionColorTexture[] vertices = RectanglePrimitive(C1, C2, C3, C4, false);
            //rectangleBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), 6, BufferUsage.WriteOnly);
            rectangleBuffer.SetData<VertexPositionColorTexture>(vertices);

            basicEffect.Projection = projectionMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.World = Matrix.CreateRotationX(-(float)Math.PI / 2 - cameraVDirection) * Matrix.CreateRotationY(cameraDirection) * Matrix.CreateRotationZ(angle) * Matrix.CreateTranslation(position) * worldMatrix;
            basicEffect.VertexColorEnabled = false;
            basicEffect.LightingEnabled = false;
            basicEffect.Alpha = alpha;
            basicEffect.TextureEnabled = true;

            basicEffect.Texture = dTextureDict[textureName];

            graphicsDevice.SetVertexBuffer(rectangleBuffer);

            foreach (EffectPass pass in basicEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            //rectangleBuffer.Dispose();
        }

        private float PointDistance(float x1, float y1, float x2, float y2)
        {
            return (float)Math.Sqrt(Math.Pow((double)(x2 - x1), 2) + Math.Pow((double)(y2 - y1), 2));
        }

        public void DrawShadow(Vector3 position, float shadowSize)
        {
            if (PointDistance(position.X, position.Z, cameraPosition.X, cameraPosition.Z) < CSettings.Instance.iShadowDrawDist)
            {
                float cornerHeight1 = CLevel.Instance.GetHeightAt(position.X - shadowSize, position.Z + shadowSize, position.Y);
                float cornerHeight2 = CLevel.Instance.GetHeightAt(position.X + shadowSize, position.Z + shadowSize, position.Y);
                float cornerHeight3 = CLevel.Instance.GetHeightAt(position.X - shadowSize, position.Z - shadowSize, position.Y);
                float cornerHeight4 = CLevel.Instance.GetHeightAt(position.X + shadowSize, position.Z - shadowSize, position.Y);

                CRender.Instance.DrawRectangle(new Vector3(position.X - shadowSize, cornerHeight1 + 0.1f, position.Z + shadowSize),
                        new Vector3(position.X + shadowSize, cornerHeight2 + 0.1f, position.Z + shadowSize),
                        new Vector3(position.X - shadowSize, cornerHeight3 + 0.1f, position.Z - shadowSize),
                        new Vector3(position.X + shadowSize, cornerHeight4 + 0.1f, position.Z - shadowSize), "shadow", false, 0.5f);
            }
        }

        public void DrawShadowSimple(Vector3 position, float shadowSize)
        {
            if (PointDistance(position.X, position.Z, cameraPosition.X, cameraPosition.Z) < CSettings.Instance.iShadowDrawDist)
            {
                float cornerHeight = CLevel.Instance.GetHeightAt(position.X, position.Z, position.Y);

                CRender.Instance.DrawRectangle(new Vector3(position.X - shadowSize, cornerHeight + 0.1f, position.Z + shadowSize),
                        new Vector3(position.X + shadowSize, cornerHeight + 0.1f, position.Z + shadowSize),
                        new Vector3(position.X - shadowSize, cornerHeight + 0.1f, position.Z - shadowSize),
                        new Vector3(position.X + shadowSize, cornerHeight + 0.1f, position.Z - shadowSize), "shadow", false, 0.5f);
            }
        }


        public void UpdateCamera()
        {
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
        }

        public Vector3 SunDebug()
        {
            //SunOrientation = new Vector3(SunOrientation.X + 0.01f, SunOrientation.Y + 0.01f, SunOrientation.Z + 0.01f);
            SunOrientation = new Vector3(SunOrientation.X%(float)(Math.PI*2), SunOrientation.Y % (float)(Math.PI * 2), SunOrientation.Z % (float)(Math.PI * 2));
            return SunOrientation;
        }
    }
}
