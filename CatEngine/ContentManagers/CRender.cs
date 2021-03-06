﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;
using CatEngine.SimpleMdl;
using CatEngine.SkinnedMdl;
using CatEngine.SkeletalAnimation;
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

        private Dictionary<string, SkeletalSprite> dSkeletalSpriteDict = new Dictionary<string, SkeletalSprite>();
        private Dictionary<string, BoneAnimation> dBoneAnimationDict = new Dictionary<string, BoneAnimation>();

        private Vector3 cameraPosition = new Vector3(30.0f, 30.0f, 30.0f);
        private Vector3 cameraTarget = new Vector3(0.0f, 0.0f, 0.0f); // Look back at the origin

        private Matrix worldMatrix;
        private Matrix viewMatrix;
        private Matrix projectionMatrix;

        private Vector3 SunOrientation = new Vector3(4.5f, -4.5f, 2.0f);

        private GameTime gameTime;

        //effects
        private BasicEffect basicEffect;
        private VertexBuffer rectangleBuffer;
        private VertexBuffer spriteBuffer;

        private SkinnedModelInstance playerInstance = new SkinnedModelInstance();

        //we set this up for skinned models
        private Effect SimpleModelEffect;
        private Effect SkinnedModelEffect;
        private Effect SpriteEffect;

        //shadow map
        private RenderTarget2D shadowMap;
        Vector3 lightPos = new Vector3(0, 20, 0);
        Vector3 lightLookat = new Vector3(3, 0, 3);
        Matrix lightViewProjectionMatrix;

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
            spriteBuffer = new VertexBuffer(graphicsDevice, typeof(VertexPositionNormalTexture), 6, BufferUsage.WriteOnly);

            worldMatrix = Matrix.CreateTranslation(0.0f, 0.0f, 0.0f);
            viewMatrix = Matrix.CreateLookAt(cameraPosition, cameraTarget, Vector3.Up);
            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(fovAngle, aspectRatio, near, far);

            SimpleModelEffect = content.Load<Effect>("Effects/SimpleModelEffect");
            SkinnedModelEffect = content.Load<Effect>("Effects/SkinnedModelEffect");
            SpriteEffect = content.Load<Effect>("Effects/SpriteEffect");

            //shadow map initialization
            shadowMap = new RenderTarget2D(graphicsDevice, CSettings.Instance.iShadowMapSize, CSettings.Instance.iShadowMapSize, true, graphicsDevice.DisplayMode.Format, DepthFormat.Depth24);
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
                    CConsole.Instance.Print("Tried to load simple model " + modelName + " but failed, error " + e.ToString());
                }
                catch (Exception a)
                {
                    CConsole.Instance.Print("Tried to load simple model " + modelName + " but failed, error " + e.ToString());
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

        public void LoadSkeletalSprite(String path, String skelName)
        {
            try
            {
                SkeletalSprite spr = new SkeletalSprite();
                spr.LoadSkeleton(path, skelName);
                dSkeletalSpriteDict.Add(skelName, spr);
            }
            catch (ContentLoadException e)
            {
                dSkeletalSpriteDict.Add(skelName, null);
                CConsole.Instance.Print("Tried to load skeletal sprite " + skelName + " but failed, error " + e.ToString());
            }
        }

        public void LoadBoneAnimation(String path, String animName)
        {
            try
            {
                BoneAnimation anim = new BoneAnimation();
                anim.LoadAnimation(path, animName);
                dBoneAnimationDict.Add(animName, anim);
            }
            catch (ContentLoadException e)
            {
                dSkeletalSpriteDict.Add(animName, null);
                CConsole.Instance.Print("Tried to load skeletal sprite " + animName + " but failed, error " + e.ToString());
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

        public void DrawSimpleModel(string technique, string modelName, Vector3 position, Vector3 rotation, float scale)
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

            SimpleModelEffect.CurrentTechnique = SimpleModelEffect.Techniques[technique];
            SimpleModelEffect.Parameters["LightPos"].SetValue(lightPos);
            SimpleModelEffect.Parameters["LightPower"].SetValue(2.0f);
            SimpleModelEffect.Parameters["Ambient"].SetValue(0.2f);
            SimpleModelEffect.Parameters["AmbientColor"].SetValue(new Vector4(Color.White.R, Color.White.G, Color.White.B, 1));
            SimpleModelEffect.Parameters["World"].SetValue(scaleMatrix * rotationMatrix * transformMatrix * Matrix.Identity);
            SimpleModelEffect.Parameters["WorldViewProjection"].SetValue(scaleMatrix * rotationMatrix * transformMatrix * Matrix.Identity * viewMatrix * projectionMatrix);
            SimpleModelEffect.Parameters["LightWorldViewProjection"].SetValue(scaleMatrix * rotationMatrix * transformMatrix * Matrix.Identity * lightViewProjectionMatrix);
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
            SimpleModelEffect.Parameters["ShadowMap"].SetValue(shadowMap);
            SimpleModelEffect.Parameters["LightCookie"].SetValue(dTextureDict["lightcookie"]);
            

            graphicsDevice.SetVertexBuffer(dSimpleModelDict[mdlName].VertexBuffer);
            graphicsDevice.Indices = dSimpleModelDict[modelName].IndexBuffer;
            //graphicsDevice.DepthStencilState = DepthStencilState.Default;
            foreach (EffectPass pass in SimpleModelEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, dSimpleModelDict[modelName].FaceCount);
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

        public void PlayerSetAdditionalRotation(string BoneName, Vector3 Rotation)
        {
            SkinnedModelInstance.BoneAnimationInstance boneInstance = playerInstance.GetBoneAnimationInstance(BoneName);

            if (boneInstance != null)
                boneInstance.AdditionalTransform = Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z);
            else
                Console.WriteLine("bone was null");
        }

        public void DrawPlayer(string technique, String animName, String secondaryAnimName, Vector3 position, float rotation, float animFrame, float animFade)
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

            //SkinnedModelEffect.CurrentTechnique = SimpleModelEffect.Techniques[technique];
            //SkinnedModelEffect.Parameters["LightPos"].SetValue(lightPos);
            //SkinnedModelEffect.Parameters["LightPower"].SetValue(2.0f);
            //SkinnedModelEffect.Parameters["Ambient"].SetValue(0.2f);
            SkinnedModelEffect.Parameters["World"].SetValue(playerInstance.Transformation * worldMatrix);
            SkinnedModelEffect.Parameters["WorldViewProjection"].SetValue(playerInstance.Transformation * worldMatrix * viewMatrix * projectionMatrix);
            Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(playerInstance.Transformation * worldMatrix));
            SkinnedModelEffect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
            //SkinnedModelEffect.Parameters["LightWorldViewProjection"].SetValue(playerInstance.Transformation * worldMatrix * lightViewProjectionMatrix);

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

            Color color = Color.White;

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

            try
            {
                basicEffect.Texture = dTextureDict[textureName];
            }
            catch (Exception e)
            {
                basicEffect.Texture = dTextureDict["empty"];
            }

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

        public void Draw3DSprite(string technique, String spriteName, int imageIndex, Vector3 Position, float Scale, Vector3 Rotation, float alpha)
        {
            Vector3 Normal = GetNormal(new Vector3(-1, -0, -1), new Vector3(1, 0, 1), new Vector3(1, 0, -1));

            float spriteSize = 32f;
            float spriteLeft = (spriteSize * (float)(imageIndex)) / dTextureDict[spriteName].Width;
            float spriteRight = (spriteSize * (float)(imageIndex + 1)) / dTextureDict[spriteName].Width;

            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[6]
            {
                //polygon 1
                new VertexPositionNormalTexture(new Vector3(-1, 0, 1), Normal, new Vector2(spriteLeft, 0)),
                new VertexPositionNormalTexture(new Vector3(1, 0, 1), Normal, new Vector2(spriteRight, 0)),
                new VertexPositionNormalTexture(new Vector3(-1, 0, -1), Normal, new Vector2(spriteLeft, 1)),

                //polygon 2
                new VertexPositionNormalTexture(new Vector3(-1, 0, -1), Normal, new Vector2(spriteLeft, 1)),
                new VertexPositionNormalTexture(new Vector3(1, 0, 1), Normal, new Vector2(spriteRight, 0)),
                new VertexPositionNormalTexture(new Vector3(1, 0, -1), Normal, new Vector2(spriteRight, 1))
            };

            spriteBuffer.SetData<VertexPositionNormalTexture>(vertices);

            Matrix rotationMatrix = Matrix.CreateRotationX(Rotation.X) * Matrix.CreateRotationY(Rotation.Y) * Matrix.CreateRotationZ(Rotation.Z);
            Matrix positionMatrix = Matrix.CreateTranslation(Position);
            Matrix scaleMatrix = Matrix.CreateScale(Scale);



            Matrix transformMatrix = scaleMatrix * rotationMatrix * positionMatrix;

            Matrix transformMatrix2 = Matrix.CreateTranslation(new Vector3(3, 3, 3));

            SpriteEffect.Parameters["Transform"].SetValue(transformMatrix*transformMatrix2);
            SpriteEffect.Parameters["World"].SetValue(worldMatrix);
            SpriteEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * viewMatrix * projectionMatrix);
            try
            {
                SpriteEffect.Parameters["Texture1"].SetValue(dTextureDict[spriteName]);
            }
            catch (Exception e)
            {
                SpriteEffect.Parameters["Texture1"].SetValue(dTextureDict["empty"]);
            }

            graphicsDevice.SetVertexBuffer(spriteBuffer);

            foreach (EffectPass pass in SpriteEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            graphicsDevice.SetVertexBuffer(null);
        }

        public void RenderSkeletalSprite(string sprite, string animation, Vector3 position, float frame, float rotation, float scale)
        {
            SkeletalSprite spr = dSkeletalSpriteDict[sprite];
            spr.SetAnimation(dBoneAnimationDict[animation]);

            spr.GetBone("root").AddRotation(rotation);
            spr.GetBone("root").AddPosition(position);

            spr.UpdateSkeleton(frame);
            spr.RenderSkeleton(scale);
        }

        public void RenderBone(string technique, String spriteName, int imageIndex, Matrix transformMatrix, Vector2 Size, Vector2 Origin)
        {
            /*Vector3 C1 = new Vector3(-(Size.X - Origin.X), 0, (Size.Y - Origin.Y));
            Vector3 C2 = new Vector3(-(Size.X - Origin.X), 0, -(Size.Y - Origin.Y));
            Vector3 C3 = new Vector3((Size.X - Origin.X), 0, (Size.Y - Origin.Y));
            Vector3 C4 = new Vector3((Size.X - Origin.X), 0, -(Size.Y - Origin.Y));*/

            Vector3 C1 = new Vector3(-Origin.X, 0, (Size.Y - Origin.Y));
            Vector3 C2 = new Vector3(-Origin.X, 0, -Origin.Y);
            Vector3 C3 = new Vector3((Size.X - Origin.X), 0, (Size.Y - Origin.Y));
            Vector3 C4 = new Vector3((Size.X - Origin.X), 0,  -Origin.Y);

            Vector3 Normal = GetNormal(C2, C3, C4);

            float spriteLeft = (Size.X * (float)(imageIndex)) / dTextureDict[spriteName].Width;
            float spriteRight = (Size.X * (float)(imageIndex + 1)) / dTextureDict[spriteName].Width;

            VertexPositionNormalTexture[] vertices = new VertexPositionNormalTexture[6]
            {
                //polygon 1
                new VertexPositionNormalTexture(C1, Normal, new Vector2(spriteLeft, 1)),
                new VertexPositionNormalTexture(C3, Normal, new Vector2(spriteRight, 1)),
                new VertexPositionNormalTexture(C2, Normal, new Vector2(spriteLeft, 0)),

                //polygon 2
                new VertexPositionNormalTexture(C2, Normal, new Vector2(spriteLeft, 0)),
                new VertexPositionNormalTexture(C3, Normal, new Vector2(spriteRight, 1)),
                new VertexPositionNormalTexture(C4, Normal, new Vector2(spriteRight, 0))
            };

            spriteBuffer.SetData<VertexPositionNormalTexture>(vertices);

            SpriteEffect.Parameters["Transform"].SetValue(transformMatrix);
            SpriteEffect.Parameters["World"].SetValue(worldMatrix);
            SpriteEffect.Parameters["WorldViewProjection"].SetValue(worldMatrix * viewMatrix * projectionMatrix);
            try
            {
                SpriteEffect.Parameters["Texture1"].SetValue(dTextureDict[spriteName]);
            }
            catch (Exception e)
            {
                SpriteEffect.Parameters["Texture1"].SetValue(dTextureDict["empty"]);
            }

            graphicsDevice.SetVertexBuffer(spriteBuffer);

            foreach (EffectPass pass in SpriteEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                graphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 3);
            }

            graphicsDevice.SetVertexBuffer(null);
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

            //temp
            UpdateLight();
        }

        public RenderTarget2D GetShadowMap()
        {
            return shadowMap;
        }

        public void SetLightPosition(Vector3 position, Vector3 target)
        {
            lightPos = position;
            lightLookat = target;
        }

        public void UpdateLight()
        {
            Matrix lightsView = Matrix.CreateLookAt(lightPos, lightLookat, new Vector3(0, 1, 0));
            Matrix lightsProjection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 1f, 5f, 200f);

            lightViewProjectionMatrix = lightsView * lightsProjection;
        }
    }
}
