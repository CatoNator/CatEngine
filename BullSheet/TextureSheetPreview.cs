using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoGame.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BullSheet
{
    public class TextureSheetPreview : MonoGame.Forms.Controls.MonoGameControl
    {
        public Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();

        private bool Loaded = false;

        private String sTex = "Weapons";
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
        }

        public void Load()
        {
            dTextureDict.Add("Player", Editor.Content.Load<Texture2D>("Player"));
            dTextureDict.Add("Enemy", Editor.Content.Load<Texture2D>("Enemy"));
            dTextureDict.Add("Props", Editor.Content.Load<Texture2D>("Props"));
            dTextureDict.Add("Weapons", Editor.Content.Load<Texture2D>("Weapons"));
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
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw()
        {
            base.Draw();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            Editor.spriteBatch.Begin();

            if (!Loaded)
            {
                Load();
                Loaded = true;
            }

            Texture2D texture = dTextureDict[sTex];

            //mathsssss
            Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            Rectangle destRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
            //drawing the sprite
            Editor.spriteBatch.Draw(texture, destRectangle, sourceRectangle, Color.White*0.5f, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1.0f);

            sourceRectangle = new Rectangle(iLeft, iTop, iWidth*(iImages+1), iHeight);
            destRectangle = new Rectangle(iLeft, iTop, iWidth*(iImages+1), iHeight);
            //drawing the sprite
            Editor.spriteBatch.Draw(texture, destRectangle, sourceRectangle, Color.White, 0.0f, new Vector2(0, 0), SpriteEffects.None, 1.0f);

            Editor.spriteBatch.End();
        }
    }
}
