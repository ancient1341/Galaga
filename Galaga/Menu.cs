using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga
{
    class Menu
    {
        const int FONT_SIZE = 32;
        const int SELECTOR_LENGTH = 11 * FONT_SIZE;
        const int SELECTOR_HEIGHT = FONT_SIZE*2;

        SpriteBatch m_spriteBatch;
        GraphicsDevice graphicsDevice;

        int WIDTH;
        int HEIGHT;
        bool mouseDown;
        bool escape;

        MouseState cursor;

        SpriteFont ELNATH;
        Texture2D selector;

        GameInfo gameInfo; // Variable to be passed to all sub classes so they can draw, and access other important info


        Galaga game;

        int menuOptions;
        public int mode; // 0-MainMenu 1-Game 2-HighScores 3-About 4-Pause


        public Menu(SpriteBatch m_spriteBatch, GraphicsDevice graphicsDevice, int WIDTH, int HEIGHT, SpriteFont ELNATH)
        {
            this.m_spriteBatch = m_spriteBatch;
            this.graphicsDevice = graphicsDevice;

            this.ELNATH = ELNATH;

            this.WIDTH = WIDTH;
            this.HEIGHT = HEIGHT;

            menuOptions = -1;
            mode = 0;

            mouseDown = false;
            escape= false;

            selector = new Texture2D(graphicsDevice, 1, 1);
            selector.SetData(new[] { Color.White });

            gameInfo = new GameInfo(m_spriteBatch, graphicsDevice, WIDTH, HEIGHT, ELNATH);

            this.game = new Galaga(gameInfo);
        }

        public void update(GameTime gameTime)
        {
            HandleKeyBoardInput();
            cursor = Mouse.GetState();
            menuOptions = checkMouseMenu();
            HandleMouseInput();

            if (mode == 1) // If game is running
            {
                game.update(gameTime);
            }
        }

        public void draw()
        {
            if(mode == 1 || mode == 4) //Game is running or Pause Menu is up
            {
                game.draw();
            } 
            if(mode == 2)
            {
                showHighScores();
            } 
            if(mode == 3)
            {
                showAboutMenu();
            }
            if (mode == 4)
            {
                showPauseMenu();
            } 
            if (mode == 0)
            {
                showMainMenu();
            }
        }
        private int checkMouseMenu()
        {
            if (mode == 0)
            {
                //Within X bounds for Main Menu
                if (cursor.X > WIDTH / 4 - FONT_SIZE && cursor.X < WIDTH / 4 - FONT_SIZE + SELECTOR_LENGTH)
                {
                    if (cursor.Y > (HEIGHT * 4) / 8 - HEIGHT / 30 && cursor.Y < (HEIGHT * 4) / 8 - HEIGHT / 30 + SELECTOR_HEIGHT)
                    {
                        return 1;
                    }
                    if (cursor.Y > (HEIGHT * 4) / 8 - HEIGHT / 30 && cursor.Y < (HEIGHT * 5) / 8 - HEIGHT / 30 + SELECTOR_HEIGHT)
                    {
                        return 2;
                    }
                    if (cursor.Y > (HEIGHT * 4) / 8 - HEIGHT / 30 && cursor.Y < (HEIGHT * 6) / 8 - HEIGHT / 30 + SELECTOR_HEIGHT)
                    {
                        return 3;
                    }
                }
            }
            if (mode == 4)
            {
                if (cursor.Y > (HEIGHT * 4) / 8 - HEIGHT / 30 && cursor.Y < (HEIGHT * 4) / 8 - HEIGHT / 30 + SELECTOR_HEIGHT)
                {
                    return 4;
                }
                if (cursor.Y > (HEIGHT * 4) / 8 - HEIGHT / 30 && cursor.Y < (HEIGHT * 5) / 8 - HEIGHT / 30 + SELECTOR_HEIGHT)
                {
                    return 5;
                }
            }
                return -1;
        }

        void showMainMenu()
        {
            if (menuOptions == 1)
            {
                m_spriteBatch.Draw(selector, new Rectangle(WIDTH / 4 - FONT_SIZE, (HEIGHT * 4) / 8 - HEIGHT / 30, SELECTOR_LENGTH, SELECTOR_HEIGHT), Color.Blue);
            }
            if (menuOptions == 2)
            {
                m_spriteBatch.Draw(selector, new Rectangle(WIDTH / 4 - FONT_SIZE, (HEIGHT * 5) / 8 - HEIGHT / 30, SELECTOR_LENGTH, SELECTOR_HEIGHT), Color.Blue);
            }
            if (menuOptions == 3)
            {
                m_spriteBatch.Draw(selector, new Rectangle(WIDTH / 4 - FONT_SIZE, (HEIGHT * 6) / 8 - HEIGHT / 30, SELECTOR_LENGTH, SELECTOR_HEIGHT), Color.Blue);
            }

            m_spriteBatch.DrawString(ELNATH, "GALAGA", new Vector2(WIDTH / 5, HEIGHT / 4), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Play", new Vector2(WIDTH / 4, HEIGHT * 4 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "High Scores", new Vector2(WIDTH / 4, HEIGHT * 5 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "About", new Vector2(WIDTH / 4, HEIGHT * 6 / 8), Color.White);

            m_spriteBatch.Draw(selector, new Rectangle(cursor.X, cursor.Y, 10, 10), Color.Blue);
        }

        void showAboutMenu()
        {
            m_spriteBatch.DrawString(ELNATH, "-ABOUT-", new Vector2(WIDTH / 10, HEIGHT / 10), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Menu By Kelton Chenworth", new Vector2(WIDTH / 8, HEIGHT*2 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Thing", new Vector2(WIDTH / 8, HEIGHT*3 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Bugs By Kelton Chenworth", new Vector2(WIDTH / 8, HEIGHT*4 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Click to return", new Vector2(WIDTH / 20, HEIGHT * 7 / 8), Color.White);
        }

        void showHighScores()
        {
            m_spriteBatch.DrawString(ELNATH, "-HIGH SCORES-", new Vector2(WIDTH / 10, HEIGHT / 10), Color.White);
            m_spriteBatch.DrawString(ELNATH, "This isnt implemented", new Vector2(WIDTH / 8, HEIGHT * 2 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "pointz", new Vector2(WIDTH / 8, HEIGHT * 3 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "sCoreee", new Vector2(WIDTH / 8, HEIGHT * 4 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Click to return", new Vector2(WIDTH / 20, HEIGHT * 7 / 8), Color.White);
        }

        void showPauseMenu()
        {
            m_spriteBatch.Draw(selector, new Rectangle(WIDTH / 4 - FONT_SIZE, HEIGHT/5, SELECTOR_LENGTH, HEIGHT*13 / 24), Color.Gray);
            if (menuOptions == 4)
            {
                m_spriteBatch.Draw(selector, new Rectangle(WIDTH / 4 - FONT_SIZE, (HEIGHT * 4) / 8 - HEIGHT / 30, SELECTOR_LENGTH, SELECTOR_HEIGHT), Color.Blue);
            }
            if (menuOptions == 5)
            {
                m_spriteBatch.Draw(selector, new Rectangle(WIDTH / 4 - FONT_SIZE, (HEIGHT * 5) / 8 - HEIGHT / 30, SELECTOR_LENGTH, SELECTOR_HEIGHT), Color.Blue);
            }

            m_spriteBatch.DrawString(ELNATH, "Pause", new Vector2(WIDTH / 4, HEIGHT / 4), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Resume", new Vector2(WIDTH / 4, HEIGHT * 4 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Quit", new Vector2(WIDTH / 4, HEIGHT * 5 / 8), Color.White);
        }

        void HandleMouseInput()
        {
            if (cursor.LeftButton == ButtonState.Pressed && !mouseDown)
            {
                if (mode == 2 || mode == 3)
                {
                    mode = 0;
                }
                if (menuOptions == 1)
                {
                    mode = 1;
                    game.initialize();
                }
                if (menuOptions == 2)
                {
                    mode = 2;
                }
                if (menuOptions == 3)
                {
                    mode = 3;
                }
                if (menuOptions == 4)
                {
                    mode = 1;
                }
                if (menuOptions == 5)
                {
                    mode = 0;
                }
                mouseDown = true;
            }
            if (cursor.LeftButton == ButtonState.Released)
            {
                mouseDown= false;
            }

        }
        void HandleKeyBoardInput()
        {
            KeyboardState state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Left))
            {
                //breakout.paddle.updateDirection(-1);
            }
            if (state.IsKeyDown(Keys.Right))
            {
                //breakout.paddle.updateDirection(1);
            }

            if (state.IsKeyDown(Keys.Escape) && !escape)
            {
                if (mode == 1)
                {
                    mode = 4;
                }
                else if (mode == 4)
                {
                    mode = 1;
                }
                if (mode == 2 || mode == 3)
                {
                    mode = 0;
                }
                escape = true;
            }


            if (state.IsKeyUp(Keys.Left) && state.IsKeyUp(Keys.Right))
            {
                //breakout.paddle.updateDirection(0);
            }

            if (state.IsKeyUp(Keys.Escape))
            {
                escape= false;
            }
        }

    }
}
