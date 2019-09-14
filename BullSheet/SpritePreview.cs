using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BullSheet
{
    class SpritePreview : MonoGame.Forms.Controls.MonoGameControl
    {
        public int iImg = 0;

        private bool Loaded = false;

        private Random random = new Random();

        public Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();

        public SpriteFont test;

        private String sTex = "Player";
        private int iLeft = 0;
        private int iTop = 0;
        private int iWidth = 16;
        private int iHeight = 16;
        private int iImages = 0;
        private int iXorig = 8;
        private int iYorig = 8;

        protected override void Initialize()
        {
            base.Initialize();

            Editor.Content.RootDirectory = "Content";
        }

        public void Load()
        {
            dTextureDict.Add("Player", Editor.Content.Load<Texture2D>("Player"));
            dTextureDict.Add("Enemy", Editor.Content.Load<Texture2D>("Enemy"));
            dTextureDict.Add("Props", Editor.Content.Load<Texture2D>("Props"));
            dTextureDict.Add("Weapons", Editor.Content.Load<Texture2D>("Weapons"));

            test = Editor.Content.Load<SpriteFont>("testfont");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw()
        {
            base.Draw();

            GraphicsDevice.Clear(Color.Black);

            Editor.spriteBatch.Begin();

            Render(sTex, iLeft, iTop, iWidth, iHeight, iImages, iXorig, iYorig);

            Editor.spriteBatch.End();
        }

        public void SetSprite(String textureSheet, int Left, int Top, int Width, int Height, int Imgs, int Xorig, int Yorig, int Img)
        {
            sTex = textureSheet;
            iLeft = Left;
            iTop = Top;
            iWidth = Width;
            iHeight = Height;
            iImages = Imgs;
            iXorig = Xorig;
            iYorig = Yorig;
            iImg = Img;
        }

        public void Render(String textureSheet, int Left, int Top, int Width, int Height, int Img, int Xorig, int Yorig)
        {
            //mathsssss
            Rectangle sourceRectangle = new Rectangle(Left + (Width * iImg), Top, Width, Height);
            Rectangle destRectangle = new Rectangle(Editor.graphics.Viewport.Width / 2, Editor.graphics.Viewport.Height/2, Width*2, Height*2);

            //Rectangle destRectangle = new Rectangle(0, 0, Editor.graphics.Viewport.Width, Editor.graphics.Viewport.Height);
            Vector2 Origin = new Vector2(Width/2, Height/2);

            if (!Loaded)
            {
                Load();
                Loaded = true;
            }

            Texture2D texture = dTextureDict[textureSheet];

            //drawing the sprite
            Editor.spriteBatch.Draw(texture, destRectangle, sourceRectangle, Color.White, 0.0f, Origin, SpriteEffects.None, 1.0f);
        }
    }
}
