using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boner
{
    public class CAnimationPreview : MonoGame.Forms.Controls.MonoGameControl
    {
        private class Transformation
        {
            /// <summary>
            /// Transformation position.
            /// </summary>
            public Vector2 Position = Vector2.Zero;

            /// <summary>
            /// Transformation scale.
            /// </summary>
            public Vector2 Scale = Vector2.One;

            /// <summary>
            /// Transformation rotation.
            /// </summary>
            public float Rotation = 0;

            // identity transformation object.
            private static readonly Transformation _identity = new Transformation();

            /// <summary>
            /// Get the identity transformations.
            /// </summary>
            public static Transformation Identity { get { return _identity; } }

            /// <summary>
            /// Merge two transformations into one.
            /// </summary>
            /// <param name="a">First transformation.</param>
            /// <param name="b">Second transformation.</param>
            /// <returns>Merged transformation.</returns>
            public static Transformation Compose(Transformation a, Transformation b)
            {
                Transformation result = new Transformation();
                Vector2 transformedPosition = a.TransformVector(b.Position);
                result.Position = transformedPosition;
                result.Rotation = a.Rotation + b.Rotation;
                result.Scale = a.Scale;//a.Scale * b.Scale;
                return result;
            }

            /// <summary>
            /// Lerp between two transformation states.
            /// </summary>
            /// <param name="key1">Transformations from.</param>
            /// <param name="key2">Transformations to.</param>
            /// <param name="amount">How much to lerp.</param>
            /// <param name="result">Out transformations.</param>
            public static void Lerp(ref Transformation key1, ref Transformation key2, float amount, ref Transformation result)
            {
                result.Position = Vector2.Lerp(key1.Position, key2.Position, amount);
                result.Scale = Vector2.Lerp(key1.Scale, key2.Scale, amount);
                result.Rotation = MathHelper.Lerp(key1.Rotation, key2.Rotation, amount);
            }

            /// <summary>
            /// Transform a vector.
            /// </summary>
            /// <param name="point">Vector to transform.</param>
            /// <returns>Transformed vector.</returns>
            public Vector2 TransformVector(Vector2 point)
            {
                Vector2 result = Vector2.Transform(point, Matrix.CreateRotationZ(-Rotation));
                result *= Scale;
                result += Position;
                return result;
            }
        }

        public int iImg = 0;

        public Form1 form;

        private bool Loaded = false;

        private Random random = new Random();

        public Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();

        public SpriteFont test;

        //GraphicsDevice gD;

        float scale = 2f;

        protected override void Initialize()
        {
            base.Initialize();

            Editor.Content.RootDirectory = "Content";

            //LoadTextureRaw("AssetData/Textures", "empty");
        }

        public void LoadTextureRaw(String path, String textureName)
        {
            Console.WriteLine("loading " + path + "/" + textureName);

            //dTextureDict.Add(textureName, Editor.Content.Load<Texture2D>(textureName));

            if (!dTextureDict.ContainsKey(textureName))
            {
                try
                {
                    FileStream fileStream = new FileStream(path + "/" + textureName + ".png", FileMode.Open);
                    dTextureDict.Add(textureName, Texture2D.FromStream(GraphicsDevice, fileStream));
                    fileStream.Dispose();
                    Console.WriteLine("loaded " + textureName + " successfully");
                }
                catch (Exception e)
                {
                    dTextureDict.Add(textureName, null);
                    Console.WriteLine("Tried to load texture " + textureName + " but failed, error " + e.ToString());
                }
            }
            else
                Console.WriteLine("Tried to add texture " + textureName + " but it was already there");
        }

        public void Load()
        {
            LoadTextureRaw("AssetData/Textures", "empty");
            LoadTextureRaw("AssetData/Textures/Player", "p_body");
            LoadTextureRaw("AssetData/Textures/Player", "p_head");
            LoadTextureRaw("AssetData/Textures/Player", "p_l_forearm");
            LoadTextureRaw("AssetData/Textures/Player", "p_l_upperarm");
            LoadTextureRaw("AssetData/Textures/Player", "p_r_forearm");
            LoadTextureRaw("AssetData/Textures/Player", "p_r_upperarm");
            LoadTextureRaw("AssetData/Textures/Player", "p_revolver");
            LoadTextureRaw("AssetData/Textures/Player", "p_torso");
            LoadTextureRaw("AssetData/Textures/Player", "p_ak");
            LoadTextureRaw("AssetData/Textures/Player/Walk", "p_legs_walk");

            //test = Editor.Content.Load<SpriteFont>("testfont");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            form.currentFrame += form.animSpeed;

            if (form.currentFrame >= form.animLength)
                form.currentFrame = 0;
        }

        protected override void Draw()
        {
            base.Draw();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            if (Loaded == false)
            {
                Load();
                Loaded = true;
                Console.WriteLine("loaded textures");
            }

            Transformation rootTrans = Transformation.Identity;
            rootTrans.Scale = new Vector2(scale, scale);

            float frame = (form.currentFrame);
            int maxF = (form.animLength);

            if (form.BoneTreeView.Nodes.Count > 0)
            {
                BoneNode rootNode = ((BoneNode)form.BoneTreeView.Nodes[0]);

                BoneDrawingHack(rootNode, rootTrans, frame, maxF);
            }
        }

        private float Lerp(float a, float b, float amount)
        {
            return a * (1 - amount) + b * amount;
        }

        private void BoneDrawingHack(BoneNode b, Transformation parentTransformation, float frame, int maxFrames)
        {
            Transformation localTransformation = Transformation.Identity;

            localTransformation.Position = new Vector2(b.Position.X, b.Position.Z);

            if (b.Rotations != null)
            {
                if (frame >= b.Rotations.Count)
                    localTransformation.Rotation = 0;
                else
                {
                    int currentFrame = (int)frame;
                    int nextFrame = (int)(frame+1)%maxFrames;
                    float lerpAmount = frame % 1;

                    float r = Lerp(b.Rotations[currentFrame], b.Rotations[nextFrame], lerpAmount);

                    localTransformation.Rotation = r;
                }
            }
            else
                localTransformation.Rotation = 0f;

            Transformation worldTrans = Transformation.Compose(parentTransformation, localTransformation);

            int imageInd = 0;

            if (b.Images != null && frame <= b.Images.Count)
            {
                imageInd = b.Images[(int)frame];
            }

            Color highlight = Color.White;

            if (form.BoneTreeView.SelectedNode == b)
                highlight = Color.Red;

            DrawBone(b.Texture, b.Size, imageInd, b.Origin, worldTrans, highlight);

            if (b.Nodes.Count > 0)
            {
                foreach (BoneNode e in b.Nodes)
                {
                    BoneDrawingHack(e, worldTrans, frame, maxFrames);
                }
            }
        }

        private void DrawBone(String Texture, Vector2 Size, int imageIndex, Vector2 Origin, Transformation transformation, Color color)
        {

            Vector2 ScreenPos = new Vector2((Editor.graphics.Viewport.Width / 2) + transformation.Position.X,
                (Editor.graphics.Viewport.Height / 2) + transformation.Position.Y);

            //mathsssss
            Rectangle sourceRectangle = new Rectangle(((int)Size.X * imageIndex), 0, (int)Size.X, (int)Size.Y);
            Rectangle destRectangle = new Rectangle((int)(ScreenPos.X), (int)(ScreenPos.Y), (int)(Size.X * scale), (int)(Size.Y * scale));

            Texture2D texture;

            if (dTextureDict.ContainsKey(Texture))
            {
                texture = dTextureDict[Texture];

                if (texture == null)
                {
                    Console.WriteLine("texture "+Texture+" is null!");
                }
            }
            else
            {
                Console.WriteLine("key " + Texture + " did not exist");
                texture = dTextureDict["empty"];
            }

            //drawing the sprite
            Editor.spriteBatch.Begin(SpriteSortMode.FrontToBack,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                DepthStencilState.Default,
                RasterizerState.CullNone,
                null,
                Matrix.Identity);
            Editor.spriteBatch.Draw(texture, destRectangle, sourceRectangle, color, -transformation.Rotation, Origin, SpriteEffects.None, 1.0f);
            Editor.spriteBatch.End();
        }
    }
}
