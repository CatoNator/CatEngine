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

        private Dictionary<string, Sprite> dSpriteNameDict = new Dictionary<string, Sprite>();

        public Dictionary<string, Texture2D> dTextureDict = new Dictionary<string, Texture2D>();

        private SpriteFont testFont;

        private class Sprite
        {
            public String Name;
            public String TextureSheet;
            public int Left;
            public int Top;
            public int Width;
            public int Height;
            public int Images;
            public int XOrig;
            public int YOrig;

            public Sprite(String name, String tex, int left, int top, int w, int h, int img, int xorig, int yorig)
            {
                Name = name;
                TextureSheet = tex;
                Left = left;
                Top = top;
                Width = w;
                Height = h;
                Images = img;
                XOrig = xorig;
                YOrig = yorig;
            }
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
            CConsole.Instance.Print("Opening SpriteData");
            string xmlText = System.IO.File.ReadAllText("AssetData/SpriteData.xml");
            CConsole.Instance.Print("Text file loaded");
            XDocument file = XDocument.Parse(xmlText);
            CConsole.Instance.Print("XML parsed");

            //hardcoded error sprite
            dSpriteNameDict.Add("error", new Sprite("error", "empty", 0, 0, 16, 16, 0, 8, 8));

            //loading the data using a foreach loop
            CConsole.Instance.Print("Entering Spritedata loop");

            foreach (XElement element in file.Descendants("sprite"))
            {
                String name = element.Element("name").Value;
                String tex = element.Element("texture").Value;
                int l = Int32.Parse(element.Element("left").Value);
                int t = Int32.Parse(element.Element("top").Value);
                int w = Int32.Parse(element.Element("width").Value);
                int h = Int32.Parse(element.Element("height").Value);
                int img = Int32.Parse(element.Element("images").Value);
                int xo = Int32.Parse(element.Element("xorig").Value);
                int yo = Int32.Parse(element.Element("yorig").Value);

                dSpriteNameDict.Add(name, new Sprite(name, tex, l, t, w, h, img, xo, yo));
            }

            testFont = content.Load<SpriteFont>("testfont");
        }

        public void LoadTextureSheet(String sheetName)
        {
            try
            {
                CSprite.Instance.dTextureDict.Add(sheetName, content.Load<Texture2D>(sheetName));
            }
            catch(ContentLoadException e)
            {
                CSprite.Instance.dTextureDict.Add(sheetName, content.Load<Texture2D>("empty"));
                CConsole.Instance.Print("Tried to load texture " + sheetName + " but failed, error " + e.ToString());
            }
        }

        //mem leak, texture2ds won't get deleted once they fall out of scope
        public void DrawRect(Rectangle rectangle, Color color)
        {
            /*Texture2D rect = new Texture2D(graphics.GraphicsDevice, rectangle.Width, rectangle.Height);

            Color[] data = new Color[rectangle.Width*rectangle.Height];
            for (int i = 0; i < data.Length; ++i) data[i] = color;
            rect.SetData(data);

            Vector2 coor = new Vector2(rectangle.X, rectangle.Y);
            sbSpriteBatch.Draw(rect, coor, Color.White);

            sbSpriteBatch.End();

            rect.Dispose();

            sbSpriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);*/
        }

        public void DrawText(String text, Vector2 location, Color color)
        {
            sbSpriteBatch.DrawString(testFont, text, location, color);
        }
        
        public void Render(string spriteName, float x, float y, int imageIndex, bool flip, float rotation, float layerDepth, Color color)
        {
            Sprite spr;

            //getting the sprite index
            try
            {
                spr = dSpriteNameDict[spriteName];
            }
            catch(Exception e)
            {
                spr = dSpriteNameDict["error"];
                CConsole.Instance.Print("tried to get data for sprite " + spriteName + ", but it does not exist");
            }

            Texture2D texture;
            bool wrongTex = false;

            try
            {
                texture = dTextureDict[spr.TextureSheet];
            }
            catch (Exception e)
            {
                texture = dTextureDict["empty"];
                CConsole.Instance.Print("tried to render texturesheet " + spr.TextureSheet + ", but it does not exist");
            }

            int imgIndex;

            //making sure the image index doesn't go out of bounds
            if (imageIndex <= spr.Images)
                imgIndex = imageIndex;
            else
                imgIndex = spr.Images;

            Rectangle sourceRectangle;

            //mathsssss
            if (!wrongTex)
                sourceRectangle = new Rectangle(spr.Left + (spr.Width * imgIndex), spr.Top, spr.Width, spr.Height);
            else
                sourceRectangle = new Rectangle(0, 0, 16, 16);

            Rectangle destRectangle = new Rectangle((int)x, (int)y, spr.Width, spr.Height);
            Vector2 Origin = new Vector2(spr.XOrig, spr.YOrig);

            SpriteEffects spriteEffect;

            if (flip)
                spriteEffect = SpriteEffects.FlipHorizontally;
            else
                spriteEffect = SpriteEffects.None;

            //drawing the sprite
            //sbSpriteBatch.Draw(texture, destRectangle, sourceRectangle, color, rotation, Origin, spriteEffect, layerDepth);
        }

        public void RenderTile(int x, int y, int left, int top)
        {
            Rectangle sourceRectangle = new Rectangle(left, top, 16, 16);
            Rectangle destRectangle = new Rectangle(x, y, 16,16);

            Texture2D texture = dTextureDict["Tiles"];

            //drawing the sprite
            //sbSpriteBatch.Draw(texture, destRectangle, sourceRectangle, Color.White, 0, new Vector2(0, 0), SpriteEffects.None, 1.0f);
        }
    }
}
