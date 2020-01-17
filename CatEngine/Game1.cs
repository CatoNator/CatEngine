using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using CatEngine.Content;

namespace CatEngine
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        public enum GameState
        {
            Menu,
            Loading,
            Game,
            Paused
        };

        public GameState CurrentGameState = GameState.Loading;

        GraphicsDeviceManager graphics;
        DepthStencilState depthBuffer = new DepthStencilState() { DepthBufferEnable = true, DepthBufferFunction = CompareFunction.Less };
        SpriteBatch spriteBatch;

        SpriteBatch screenBatch;
        RenderTarget2D renderTarget;

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
            CSettings.Instance.SetBackbufferSize(360);

            graphics.PreparingDeviceSettings += GraphicsDeviceManager_PreparingDeviceSettings;
            graphics.PreferredBackBufferWidth = CSettings.Instance.iBackBufferWidth;
            graphics.PreferredBackBufferHeight = CSettings.Instance.iBackBufferHeight;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        private void GraphicsDeviceManager_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            e.GraphicsDeviceInformation.GraphicsProfile = GraphicsProfile.HiDef;
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

            this.IsMouseVisible = true;

            screenBatch = new SpriteBatch(GraphicsDevice);
            renderTarget = new RenderTarget2D(GraphicsDevice, CSettings.Instance.GAME_VIEW_WIDTH, CSettings.GAME_VIEW_HEIGHT, false, SurfaceFormat.Color, DepthFormat.Depth24, 0, RenderTargetUsage.DiscardContents);

            CLoadingScreen.Instance.game = this;

            BlendState bs = new BlendState();
            bs.AlphaSourceBlend = Blend.One;
            bs.AlphaDestinationBlend = Blend.Zero;
            bs.ColorSourceBlend = Blend.Zero;
            bs.ColorDestinationBlend = Blend.One;
            bs.AlphaBlendFunction = BlendFunction.Add;
            GraphicsDevice.BlendState = bs;

            //setting up CSpritebasics...
            CSprite.Instance.content = Content;
            CSprite.Instance.sbSpriteBatch = spriteBatch;
            CSprite.Instance.graphicsDevice = GraphicsDevice;
            CSprite.Instance.graphics = graphics;
            //loading the debug texture...
            CSprite.Instance.LoadTextureSheet("empty");
            //loading the loading screen so we don't crash while loading assets
            //CSprite.Instance.LoadTextureSheet("LoadingScreen");
            CSprite.Instance.LoadTextureSheetRaw("AssetData/Textures/Frontend", "LoadingScreen");
            //loading a font for testing
            CSprite.Instance.LoadTextureSheetRaw("AssetData/Textures/Frontend", "spritefont");
            CSprite.Instance.LoadTextureSheetRaw("AssetData/Textures/Frontend", "numeric_font");
            CSprite.Instance.LoadTextureSheetRaw("AssetData/Textures/Frontend", "choco_health");
            CSprite.Instance.LoadTextureSheetRaw("AssetData/Textures/Frontend", "hud_natsa");
            //loading screen stuff NEEDS to be loaded here! it can't be loaded in during runtime, it'll just crash.

            //setting up 3D and loading debug cube
            CRender.Instance.content = Content;
            CRender.Instance.graphics = graphics;
            CRender.Instance.graphicsDevice = GraphicsDevice;
            CRender.Instance.Init();
            //debug: loading some essential models

            //DEBUG: creating objects that aren't configured to be loaded in CLevel yet
            CObjectManager.Instance.CreateInstance(typeof(CCamera), 0, 10, -30);
            CObjectManager.Instance.CreateInstance(typeof(CPlayer), 5, 20, 5);
            CObjectManager.Instance.CreateInstance(typeof(CNatsa), 5, 20, 5);
            CObjectManager.Instance.CreateInstance(typeof(CNatsa), 20, 20, 5);
            CObjectManager.Instance.CreateInstance(typeof(CNatsa), 5, 20, 20);
            CObjectManager.Instance.CreateInstance(typeof(CNatsa), 20, 20, 20);
            CObjectManager.Instance.CreateInstance(typeof(CNatsa), 20, 30, 30);
            //CObjectManager.Instance.CreateInstance(typeof(CEnemy), 16, 16);

            //debug
            CParticleManager.Instance.LoadParticleData();

            CLoadingScreen.Instance.Load();
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

            if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.P))
            {
                if (CurrentGameState == GameState.Game)
                    CurrentGameState = GameState.Paused;
                else
                    CurrentGameState = GameState.Game;
            }

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

                if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    CurrentGameState = GameState.Paused;
                }
            }
            else if (CurrentGameState == GameState.Paused)
            {
                /*if (GamePad.GetState(PlayerIndex.One).Buttons.Start == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.P))
                {
                    CurrentGameState = GameState.Game;
                }*/

                if (Keyboard.GetState().IsKeyDown(Keys.L))
                {
                    CGame.Instance.InitiateFadeLevel("Test2");
                }
            }

            if (CurrentGameState != GameState.Loading)
                CGame.Instance.UpdateFade();
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
            //GraphicsDevice.Clear(Color.CornflowerBlue);

            CRender.Instance.UpdateGameTime(gameTime);
            CRender.Instance.UpdateCamera();

            if (CurrentGameState == GameState.Menu)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                //CMainMenu.Instance.Render();
                CGame.Instance.RenderFadeOut();
                spriteBatch.End();
            }
            else if (CurrentGameState == GameState.Loading)
            {
                GraphicsDevice.Clear(Color.Black);
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                CLoadingScreen.Instance.Render();
                spriteBatch.End();
            }
            else if (CurrentGameState == GameState.Game)
            {
                //skybox
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                CSprite.Instance.DrawSkyBox();
                spriteBatch.End();

                //rendering 3D objects
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.RasterizerState = RasterizerState.CullNone; //kinnunen pls
                //CLevel.Instance.Render();
                CLevel.Instance.Render();
                CObjectManager.Instance.Render();
                CParticleManager.Instance.Render();

                //rendering 2D objects
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                //CObjectManager.Instance.Render2D();
                CHud.Instance.Render();
                CGame.Instance.RenderFadeOut();
                spriteBatch.End();
            }
            else if (CurrentGameState == GameState.Paused)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                CSprite.Instance.DrawSkyBox();
                spriteBatch.End();

                //rendering 3D objects
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.RasterizerState = RasterizerState.CullNone; //kinnunen pls
                //CLevel.Instance.Render();
                CLevel.Instance.Render();
                CObjectManager.Instance.Render();

                //rendering 2D objects
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                //CObjectManager.Instance.Render2D();
                CHud.Instance.Render();
                //CPauseMenu.Instance.Render();
                CGame.Instance.RenderFadeOut();
                spriteBatch.End();
            }

            //global 2D render
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);            
            CConsole.Instance.Render();
            spriteBatch.End();

            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.SetVertexBuffer(null);

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
