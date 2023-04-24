using Galaga.Galaga;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Galaga
{
    public class Game1 : Game
    {
        const int WIDTH = 1280;
        const int HEIGHT = 720;

        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

        private Texture2D m_playerSprite;
        private Texture2D m_bulletSprite;
        private Galaga.AnimatedSprite m_greenAlienRenderer;
        private Galaga.AnimatedSprite m_redAlienRenderer;
        private Galaga.AnimatedSprite m_blueAlienRenderer;
        private Galaga.AnimatedSprite m_beeAlienRenderer;
        private Galaga.AnimatedSprite m_playerExplosionRenderer;

        private Objects.EnemyModel m_greenEnemy;
        private Objects.EnemyModel m_blueEnemy;
        private Objects.EnemyModel m_redEnemy;
        private Objects.EnemyModel m_beeEnemy;

        private Dictionary<string, AnimatedSprite> spriteRenderers;

        GameInfo gameInfo;

        private KeyboardInput m_keyboardInput;
        private Dictionary<string, Texture2D> spriteDict;

        Galaga.Menu gameMenu;

        SpriteFont ELNATH;

        public Game1()
        {
            m_graphics = new GraphicsDeviceManager(this);
            m_graphics.PreferredBackBufferWidth = WIDTH;
            m_graphics.PreferredBackBufferHeight = HEIGHT;
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
            m_keyboardInput = new KeyboardInput();
            gameInfo = new GameInfo(m_spriteBatch, GraphicsDevice, WIDTH, HEIGHT, ELNATH, spriteRenderers, spriteDict, m_keyboardInput);
            gameMenu = new Galaga.Menu(gameInfo);
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp;


        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            ELNATH = Content.Load<SpriteFont>("Font/ELNATH");
            // TODO: use this.Content to load your game content here

            m_greenEnemy = new Objects.EnemyModel(
                new Vector2(75, 75),
                new Vector2(200, 200),
                50 / 1000,
                0);
            m_blueEnemy = new Objects.EnemyModel(
                new Vector2(75, 75),
                new Vector2(100, 200),
                50 / 1000,
                0);
            m_redEnemy = new Objects.EnemyModel(
                new Vector2(75, 75),
                new Vector2(200, 100),
                50 / 1000,
                0);
            m_beeEnemy = new Objects.EnemyModel(
                new Vector2(75, 75),
                new Vector2(100, 100),
                50 / 1000,
                0);

            m_playerSprite = this.Content.Load<Texture2D>("player");
            m_bulletSprite = this.Content.Load<Texture2D>("bullet");

            spriteDict = new Dictionary<string, Texture2D>
            {
                { "player", m_playerSprite },
                { "bullet", m_bulletSprite }
            };

            spriteRenderers = new Dictionary<string, AnimatedSprite>();

            m_greenAlienRenderer = new Galaga.AnimatedSprite(
                this.Content.Load<Texture2D>("green-alien"),
                new int[] { 500, 500 }
                );
            m_redAlienRenderer = new Galaga.AnimatedSprite(
                this.Content.Load<Texture2D>("red-alien"),
                new int[] { 500, 500 }
                );
            m_blueAlienRenderer = new Galaga.AnimatedSprite(
                this.Content.Load<Texture2D>("blue-alien"),
                new int[] { 500, 500 }
                );
            m_beeAlienRenderer = new Galaga.AnimatedSprite(
                this.Content.Load<Texture2D>("bee-alien"),
                new int[] { 500, 500 }
                );

            m_playerExplosionRenderer = new Galaga.AnimatedSprite(
                this.Content.Load<Texture2D>("ship-explosion"),
                new int[] { 500, 500, 500, 500 }
                );

            spriteRenderers.Add("green",  m_greenAlienRenderer);
            spriteRenderers.Add("butterfly", m_redAlienRenderer);
            spriteRenderers.Add("blue", m_blueAlienRenderer);
            spriteRenderers.Add("bee", m_beeAlienRenderer);
            spriteRenderers.Add("playerExplosion", m_playerExplosionRenderer);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && gameMenu.mode == 0)
                Exit();

            m_greenAlienRenderer.update(gameTime);
            m_blueAlienRenderer.update(gameTime);
            m_redAlienRenderer.update(gameTime);
            m_beeAlienRenderer.update(gameTime);
            m_playerExplosionRenderer.update(gameTime);

            m_keyboardInput.Update(gameTime);
            gameMenu.update(gameTime);
            base.Update(gameTime);

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_spriteBatch.Begin();
            gameMenu.draw();

            m_spriteBatch.End();

            base.Draw(gameTime);
        }


    }
}