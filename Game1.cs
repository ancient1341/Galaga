using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Galaga
{
    public class Game1 : Game
    {
        const int WIDTH = 1280;
        const int HEIGHT = 720;

        private GraphicsDeviceManager m_graphics;
        private SpriteBatch m_spriteBatch;

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
            gameMenu = new Galaga.Menu(m_spriteBatch, GraphicsDevice, WIDTH, HEIGHT, ELNATH);
        }

        protected override void LoadContent()
        {
            m_spriteBatch = new SpriteBatch(GraphicsDevice);
            ELNATH = Content.Load<SpriteFont>("Font/ELNATH");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape) && gameMenu.mode == 0)
                Exit();

            gameMenu.update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            m_spriteBatch.Begin();
            //m_spriteBatch.Draw(zappy, new Rectangle(0, 0, WIDTH, HEIGHT), Color.White); BACKGROUND IF DESIRED
            gameMenu.draw();
            m_spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}