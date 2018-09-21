using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CatEngine
{
    public class CSprite
    {
        public Texture2D txTexture;

        public SpriteBatch sbSpriteBatch;

        public GraphicsDeviceManager graphics;

        private Dictionary<string, int> dSpriteNameDict = new Dictionary<string,int>();

        public Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();

        private int iSprites;

        private string[] sSpriteTexture;
        private int[] iSpriteLeft;
        private int[] iSpriteTop;
        private int[] iSpriteWidth;
        private int[] iSpriteHeight;
        private int[] iSpriteImages;
        private int[] iSpriteXOrigin;
        private int[] iSpriteYOrigin;

        private CSprite()
        {
            this.txTexture = CObjectManager.Instance.txTexture;
            this.sbSpriteBatch = CObjectManager.Instance.sbSpriteBatch;

            this.graphics = CObjectManager.Instance.graphics;

            AllocateSprites();
        }

        //singletoning the singleton
        public static CSprite Instance { get { return Nested.instance; } }

        private class Nested
        {
            static Nested()
            {
            }

            internal static readonly CSprite instance = new CSprite();
        }

        private void AllocateSprites()
        {
            //this is going to be loaded from an ini later on
            iSprites = 2;

            sSpriteTexture = new string[iSprites];
            iSpriteLeft = new int[iSprites];
            iSpriteTop = new int[iSprites];
            iSpriteWidth = new int[iSprites];
            iSpriteHeight = new int[iSprites];
            iSpriteImages = new int[iSprites];
            iSpriteXOrigin = new int[iSprites];
            iSpriteYOrigin = new int[iSprites];

            //DELETE ME
            //this is going to be loaded from an ini later on
            dSpriteNameDict.Add("sprTest", 0);

            sSpriteTexture[0] = "Player";
            iSpriteLeft[0] = 0;
            iSpriteTop[0] = 0;
            iSpriteWidth[0] = 64;
            iSpriteHeight[0] = 64;
            iSpriteImages[0] = 0;
            iSpriteXOrigin[0] = 18;
            iSpriteYOrigin[0] = 30;

            dSpriteNameDict.Add("sprLight", 1);

            sSpriteTexture[1] = "Lights";
            iSpriteLeft[1] = 0;
            iSpriteTop[1] = 0;
            iSpriteWidth[1] = 256;
            iSpriteHeight[1] = 256;
            iSpriteImages[1] = 0;
            iSpriteXOrigin[1] = 128;
            iSpriteYOrigin[1] = 128;
        }

        //mem leak, texture2ds won't get deleted once they fall out of scope
        public void DrawRect(Rectangle rectangle, Color color)
        {
            Texture2D rect = new Texture2D(graphics.GraphicsDevice, rectangle.Width, rectangle.Height);

            Color[] data = new Color[rectangle.Width*rectangle.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rect.SetData(data);

            Vector2 coor = new Vector2(rectangle.X, rectangle.Y);
            sbSpriteBatch.Draw(rect, coor, Color.White);

            sbSpriteBatch.End();

            rect.Dispose();

            sbSpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
        }
        
        public void Render(string spriteName, float x, float y, int imageIndex, bool flip, float rotation, float layerDepth, Color color)
        {
            //getting the sprite index
            int index = dSpriteNameDict[spriteName];

            int imgIndex;

            //making sure the image index doesn't go out of bounds
            if (imageIndex <= iSpriteImages[index])
                imgIndex = imageIndex;
            else
                imgIndex = iSpriteImages[index];

            //mathsssss
            Rectangle sourceRectangle = new Rectangle(iSpriteLeft[index]+(iSpriteWidth[index]*imgIndex), iSpriteTop[index], iSpriteWidth[index], iSpriteHeight[index]);
            Rectangle destRectangle = new Rectangle((int)x, (int)y, iSpriteWidth[index], iSpriteWidth[index]);
            Vector2 Origin = new Vector2(iSpriteXOrigin[index], iSpriteYOrigin[index]);

            Texture2D texture = dTextureDict[sSpriteTexture[index]];

            SpriteEffects spriteEffect;

            if (flip)
                spriteEffect = SpriteEffects.FlipHorizontally;
            else
                spriteEffect = SpriteEffects.None;

            //drawing the sprite
            sbSpriteBatch.Draw(texture, destRectangle, sourceRectangle, color, rotation, Origin, spriteEffect, layerDepth);
        }
    }
}
