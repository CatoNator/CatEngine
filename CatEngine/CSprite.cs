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
    public class CSprite : CContentManager
    {
        public Texture2D txTexture;

        public ContentManager content;

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

        private class SpriteDataHolder
        {
            public int index { get; set; }
            public string sSpriteTexture { get; set; }
            public int iSpriteLeft { get; set; }
            public int iSpriteTop { get; set; }
            public int iSpriteWidth { get; set; }
            public int iSpriteHeight { get; set; }
            public int iSpriteImages { get; set; }
            public int iSpriteXOrigin { get; set; }
            public int iSpriteYOrigin { get; set; }
        }

        private CSprite()
        {
            this.txTexture = CObjectManager.Instance.txTexture;
            this.sbSpriteBatch = CObjectManager.Instance.sbSpriteBatch;

            this.graphics = CObjectManager.Instance.graphics;
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

        public void AllocateSprites()
        {
            //opening the xml file
            Debug.WriteLine("Opening SpriteData");
            string xmlText = System.IO.File.ReadAllText("AssetData/SpriteData.xml");
            Debug.WriteLine("Text file loaded");
            XDocument file = XDocument.Parse(xmlText);
            Debug.WriteLine("XML parsed");
            
            //this is going to be loaded from an ini later on
            iSprites = Int32.Parse(file.Root.Element("sprites").Value);

            //allocating the arrays
            sSpriteTexture = new string[iSprites];
            iSpriteLeft = new int[iSprites];
            iSpriteTop = new int[iSprites];
            iSpriteWidth = new int[iSprites];
            iSpriteHeight = new int[iSprites];
            iSpriteImages = new int[iSprites];
            iSpriteXOrigin = new int[iSprites];
            iSpriteYOrigin = new int[iSprites];

            //loading the data using a foreach loop
            Debug.WriteLine("Entering Spritedata loop");

            foreach (XElement element in file.Descendants("sprite"))
            {
                int index = Int32.Parse(element.Element("index").Value);

                Debug.WriteLine("Reading element index " + index);

                dSpriteNameDict.Add(element.Element("name").Value, index);

                sSpriteTexture[index] = element.Element("texture").Value;
                iSpriteLeft[index] = Int32.Parse(element.Element("left").Value);
                iSpriteTop[index] = Int32.Parse(element.Element("top").Value);
                iSpriteWidth[index] = Int32.Parse(element.Element("width").Value);
                iSpriteHeight[index] = Int32.Parse(element.Element("height").Value);
                iSpriteImages[index] = Int32.Parse(element.Element("images").Value);
                iSpriteXOrigin[index] = Int32.Parse(element.Element("xorig").Value);
                iSpriteYOrigin[index] = Int32.Parse(element.Element("yorig").Value);

                Debug.Print(index + " w " + iSpriteWidth[index] + " h " + iSpriteHeight[index]);
            }
        }

        public void LoadTextureSheet(String sheetName)
        {
            CSprite.Instance.dTextureDict.Add(sheetName, content.Load<Texture2D>(sheetName));
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
            Rectangle destRectangle = new Rectangle((int)x, (int)y, iSpriteWidth[index], iSpriteHeight[index]);
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

        public void RenderTile(int x, int y, int left, int top)
        {
            Rectangle sourceRectangle = new Rectangle(left, top, 16, 16);
            Rectangle destRectangle = new Rectangle(x, y, 16,16);

            Texture2D texture = dTextureDict["Tiles"];

            //drawing the sprite
            sbSpriteBatch.Draw(texture, destRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1.0f);
        }
    }
}
