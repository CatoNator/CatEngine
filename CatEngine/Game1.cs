using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CatEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        enum GameState
        {
            Menu,
            Loading,
            Game,
            Paused
        };

        GameState CurrentGameState = GameState.Loading;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteBatch lightBatch;

        SpriteBatch screenBatch;
        RenderTarget2D renderTarget;

        private Random myRandom = new Random();

        public readonly static BlendState
            bsSubtract = new BlendState
            {
                ColorSourceBlend = Blend.SourceColor,
                ColorDestinationBlend = Blend.One,
                ColorBlendFunction = BlendFunction.ReverseSubtract,
                AlphaSourceBlend = Blend.SourceAlpha,
                AlphaDestinationBlend = Blend.One,
                AlphaBlendFunction = BlendFunction.ReverseSubtract
            };

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            CSettings.Instance.SetGameViewSize();
            CSettings.Instance.SetBackbufferSize(960);

            graphics.PreferredBackBufferWidth = CSettings.Instance.iBackBufferWidth;
            graphics.PreferredBackBufferHeight = CSettings.Instance.iBackBufferHeight;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //lightBatch = new SpriteBatch(GraphicsDevice);

            screenBatch = new SpriteBatch(GraphicsDevice);
            renderTarget = new RenderTarget2D(GraphicsDevice, CSettings.Instance.GAME_VIEW_WIDTH, CSettings.GAME_VIEW_HEIGHT);
            CSprite.Instance.content = Content;

            //loading generic data here for now, these could be loaded in runtime later, though
            /*CSprite.Instance.dTextureDict.Add("Player", Content.Load<Texture2D>("Player"));
            CSprite.Instance.dTextureDict.Add("Enemy", Content.Load<Texture2D>("Enemy"));
            CSprite.Instance.dTextureDict.Add("Props", Content.Load<Texture2D>("Props"));
            CSprite.Instance.dTextureDict.Add("Lights", Content.Load<Texture2D>("Lights"));*/

            //also all sound effects

            //CSprite.Instance.txTexture = Content.Load<Texture2D>("PlayerTest");
            CSprite.Instance.sbSpriteBatch = spriteBatch;
            CSprite.Instance.graphics = graphics;

            //DEBUG: creating a player and some platforms
            //CObjectManager.Instance.CreateInstance(typeof(CLevel), 0, 0);
            CObjectManager.Instance.CreateInstance(typeof(CPlayer), 256, 256);

            //CObjectManager.Instance.CreateInstance(typeof(CEnemy), 256, 180);
            CObjectManager.Instance.CreateInstance(typeof(CEnemy), 16, 16);

            //CObjectManager.Instance.CreateLight(78, 78);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (CurrentGameState == GameState.Menu)
            {
                //CMainMenu.Instance.Update();
            }
            else if (CurrentGameState == GameState.Loading)
            {
                CLoadingScreen.Instance.Update();
                if (CLoadingScreen.Instance.hasFinishedLoading)
                    CurrentGameState = GameState.Game;
            }
            else if (CurrentGameState == GameState.Game)
            {
                CObjectManager.Instance.Update();
            }
            //updating the object list

            //updating the HUD
            //CHud.Instance.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// 

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(renderTarget);

            //the actual game engine draw calls            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //CSprite.Instance.sbSpriteBatch = spriteBatch;

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);

            if (CurrentGameState == GameState.Menu)
            {
                //CMainMenu.Instance.Render();
            }
            else if (CurrentGameState == GameState.Loading)
            {
                CLoadingScreen.Instance.Render();
            }
            else if (CurrentGameState == GameState.Game)
            {
                //rendering the objects
                CObjectManager.Instance.Render();
            }
            else if (CurrentGameState == GameState.Paused)
            {
                //rendering the objects
                CObjectManager.Instance.Render();
                //CPauseMenu.Instance.Render();
            }

            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);

            //drawing the screen scaled on the window
            screenBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
            screenBatch.Draw(renderTarget, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            screenBatch.End();

            //screenBatch.Begin(SpriteSortMode.Immediate, bsSubtract, SamplerState.PointClamp, null, null, null, null);
            //screenBatch.Draw(lightMap, new Rectangle(0, 0, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), Color.White);
            //screenBatch.End();

            base.Draw(gameTime);
        }
    }
}
