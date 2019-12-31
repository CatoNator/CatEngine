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
        VertexBuffer vertexBuffer;
        DepthStencilState depthBuffer = new DepthStencilState() { DepthBufferEnable = true, DepthBufferFunction = CompareFunction.Less };
        SpriteBatch spriteBatch;
        SpriteBatch lightBatch;

        SpriteBatch screenBatch;
        RenderTarget2D renderTarget;

        Model cube;

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

            vertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColor), 3, BufferUsage.WriteOnly);
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

            //setting up CSpritebasics...
            CSprite.Instance.content = Content;
            CSprite.Instance.sbSpriteBatch = spriteBatch;
            CSprite.Instance.graphics = graphics;
            //loading the debug texture...
            CSprite.Instance.LoadTextureSheet("empty");
            CSprite.Instance.LoadTextureSheet("LoadingScreen");
            //loading screen stuff NEEDS to be loaded here! it can't be loaded in during runtime, it'll just crash.

            //setting up 3D and loading debug cube
            CRender.Instance.content = Content;
            CRender.Instance.graphics = graphics;
            CRender.Instance.LoadModel("textured_cube");
            CRender.Instance.LoadModel("board");
            CRender.Instance.LoadTexture("cube_tex");
            CRender.Instance.LoadTexture("grassside");
            CRender.Instance.LoadTexture("grasstop");
            //DEBUG: creating objects that aren't configured in CLevel yet
            CObjectManager.Instance.CreateInstance(typeof(CCamera), 0, 10, -30);
            CObjectManager.Instance.CreateInstance(typeof(CPlayer), 0, 20, 0);
            //CObjectManager.Instance.CreateInstance(typeof(CEnemy), 16, 16);

            //lights don't work lol
            //CObjectManager.Instance.CreateLight(78, 78);
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
            GraphicsDevice.SetVertexBuffer(vertexBuffer);

            //the actual game engine draw calls            
            GraphicsDevice.Clear(Color.CornflowerBlue);

            CSprite.Instance.sbSpriteBatch = spriteBatch;

            CRender.Instance.UpdateCamera();

            if (CurrentGameState == GameState.Menu)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                //CMainMenu.Instance.Render();
                spriteBatch.End();
            }
            else if (CurrentGameState == GameState.Loading)
            {
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                CLoadingScreen.Instance.Render();
                spriteBatch.End();
            }
            else if (CurrentGameState == GameState.Game)
            {
                //rendering 3D objects
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;
                GraphicsDevice.RasterizerState = RasterizerState.CullNone;
                CLevel.Instance.Render(GraphicsDevice);
                CObjectManager.Instance.Render();

                //rendering 2D objects
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                CObjectManager.Instance.Render2D();
                CHud.Instance.Render();
                spriteBatch.End();
            }
            else if (CurrentGameState == GameState.Paused)
            {
                //rendering 3D objects
                GraphicsDevice.DepthStencilState = DepthStencilState.Default;
                GraphicsDevice.BlendState = BlendState.Opaque;
                CObjectManager.Instance.Render();

                //rendering 2D objects
                spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, null);
                //CObjectManager.Instance.Render2D();
                //CPauseMenu.Instance.Render();
                spriteBatch.End();
            }

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
