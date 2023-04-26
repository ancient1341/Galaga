using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

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
        int controlSelected;
        bool mouseDown;
        bool escape;
        TimeSpan controlTimer;

        Color selectorColor;
        bool isChangingControl;

        MouseState cursor;

        SpriteFont ELNATH;
        SpriteFont smallEl;
        Texture2D selector;

        GameInfo gameInfo; // Variable to be passed to all sub classes so they can draw, and access other important info


        Galaga game;

        int menuOptions;
        // 0-MainMenu 1-Game 2-HighScores 3-Credits 4-Pause 5-GameOver 6-Controls


        public Menu(GameInfo gameinfo)
        {
            this.m_spriteBatch = gameinfo.m_spriteBatch;
            this.graphicsDevice = gameinfo.graphicsDevice;

            this.smallEl = gameinfo.smallEl;
            this.ELNATH = gameinfo.ELNATH;

            this.WIDTH = gameinfo.WIDTH;
            this.HEIGHT = gameinfo.HEIGHT;
            controlSelected = 0;
            isChangingControl = false;
            selectorColor = Color.Blue;
            menuOptions = -1;


            mouseDown = false;
            escape= false;

            selector = new Texture2D(graphicsDevice, 1, 1);
            selector.SetData(new[] { Color.White });

            this.gameInfo = gameinfo;
            controlTimer = new TimeSpan(0);

            this.game = new Galaga(gameInfo);
            gameInfo.keyboardInput.registerCommand(Keys.Escape, true, new InputDeviceHelper.CommandDelegate(OnEscape));
            gameInfo.keyboardInput.registerCommand(Keys.Down, true, new InputDeviceHelper.CommandDelegate(OnDownKey));
            gameInfo.keyboardInput.registerCommand(Keys.Up, true, new InputDeviceHelper.CommandDelegate(OnUpKey));
            gameInfo.keyboardInput.registerCommand(gameInfo.keys["shoot"], true, new InputDeviceHelper.CommandDelegate(OnSelectControl));

        }

        public void update(GameTime gameTime)
        {
            cursor = Mouse.GetState();
            menuOptions = checkMouseMenu();
            HandleMouseInput();

            if (gameInfo.mode == 1) // If game is running
            {
                game.update(gameTime);
            }

            controlTimer += gameTime.ElapsedGameTime;
        }

        public void draw()
        {
            if(gameInfo.mode == 1 || gameInfo.mode == 4) //Game is running or Pause Menu is up
            {
                game.draw();
            } 
            if(gameInfo.mode == 2)
            {
                showHighScores();
            } 
            if(gameInfo.mode == 3)
            {
                showAboutMenu();
            }
            if (gameInfo.mode == 4)
            {
                showPauseMenu();
            } 
            if (gameInfo.mode == 0)
            {
                showMainMenu();
            }
            if (gameInfo.mode == 5)
            {
                showGameOver();
            }
            if (gameInfo.mode == 6)
            {
                showControlsMenu();
            }
        }
        private int checkMouseMenu()
        {
            if (gameInfo.mode == 0)
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
                        return 6;
                    }
                    if (cursor.Y > (HEIGHT * 4) / 8 - HEIGHT / 30 && cursor.Y < (HEIGHT * 7) / 8 - HEIGHT / 30 + SELECTOR_HEIGHT)
                    {
                        return 3;
                    }
                }
            }
            if (gameInfo.mode == 4)
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
            if (menuOptions == 6)
            {
                m_spriteBatch.Draw(selector, new Rectangle(WIDTH / 4 - FONT_SIZE, (HEIGHT * 6) / 8 - HEIGHT / 30, SELECTOR_LENGTH, SELECTOR_HEIGHT), Color.Blue);
            }
            if (menuOptions == 3)
            {
                m_spriteBatch.Draw(selector, new Rectangle(WIDTH / 4 - FONT_SIZE, (HEIGHT * 7) / 8 - HEIGHT / 30, SELECTOR_LENGTH, SELECTOR_HEIGHT), Color.Blue);
            }

            m_spriteBatch.DrawString(ELNATH, "GALAGA", new Vector2(WIDTH / 5, HEIGHT / 4), Color.White);
            m_spriteBatch.DrawString(ELNATH, "New Game", new Vector2(WIDTH / 4, HEIGHT * 4 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "High Scores", new Vector2(WIDTH / 4, HEIGHT * 5 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Controls", new Vector2(WIDTH / 4, HEIGHT * 6 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "About", new Vector2(WIDTH / 4, HEIGHT * 7 / 8), Color.White);


            m_spriteBatch.Draw(selector, new Rectangle(cursor.X, cursor.Y, 10, 10), Color.Blue);
        }

        void showAboutMenu()
        {
            m_spriteBatch.DrawString(ELNATH, "Created By:", new Vector2(WIDTH / 10, HEIGHT / 10), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Matt Scribner", new Vector2(WIDTH / 8, HEIGHT*2 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Matthew Bingham", new Vector2(WIDTH / 8, HEIGHT*3 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Kelton Chenworth", new Vector2(WIDTH / 8, HEIGHT*4 / 8), Color.White);
            m_spriteBatch.DrawString(ELNATH, "Click to return", new Vector2(WIDTH / 20, HEIGHT * 7 / 8), Color.White);
        }

        void showHighScores()
        {
            List<int> highScores = new List<int>();
            try
            {
                using (StreamReader sr = new StreamReader("./HighScores.txt"))
                {
                    string line;
                    Debug.WriteLine("Reading from file");
                    while ((line = sr.ReadLine()) != null)
                    {
                        highScores.Add(Int32.Parse(line));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
            m_spriteBatch.DrawString(ELNATH, "-HIGH SCORES-", new Vector2(WIDTH / 10, HEIGHT / 10), Color.White);
            List<int> scores = highScores.OrderByDescending(x => x).ToList();
            for (int i = 0; i < scores.Count; i++)
            {
                m_spriteBatch.DrawString(ELNATH, scores[i].ToString(), new Vector2(WIDTH / 8, HEIGHT * (2 + i) / 8), Color.White);
            }
            
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
                if (gameInfo.mode == 2 || gameInfo.mode == 3)
                {
                    gameInfo.mode = 0;
                }
                if (menuOptions == 1)
                {
                    gameInfo.mode = 1;
                    game.initialize();
                }
                if (menuOptions == 2)
                {
                    gameInfo.mode = 2;
                }
                if (menuOptions == 3)
                {
                    gameInfo.mode = 3;
                }
                if (menuOptions == 4)
                {
                    gameInfo.mode = 1;
                }
                if (menuOptions == 5)
                {
                    gameInfo.mode = 0;
                }
                if (menuOptions == 6)
                {
                    gameInfo.mode = 6;
                }
                mouseDown = true;
            }
            if (cursor.LeftButton == ButtonState.Released)
            {
                mouseDown= false;
            }

        }

        void showGameOver()
        {
            KeyboardState state = Keyboard.GetState();

            m_spriteBatch.DrawString(ELNATH, "Game Over", new Vector2(WIDTH / 4, HEIGHT / 4), Color.White);
            m_spriteBatch.DrawString(smallEl, "Shots Fired: " + gameInfo.shotsFired, new Vector2(WIDTH / 4, HEIGHT * 4 / 8), Color.White);
            m_spriteBatch.DrawString(smallEl, "Shots Hit: " + gameInfo.hits, new Vector2(WIDTH / 4, HEIGHT * 9 / 16), Color.White);
            if (gameInfo.hits != 0)
            {
                m_spriteBatch.DrawString(smallEl, "Accuracy: " + (float)gameInfo.shotsFired / (float)gameInfo.hits, new Vector2(WIDTH / 4, HEIGHT * 10 / 16), Color.White);
            }else
            {
                m_spriteBatch.DrawString(smallEl, "N/A: ", new Vector2(WIDTH / 4, HEIGHT * 10 / 16), Color.White);
            }
            m_spriteBatch.DrawString(smallEl, "Press Space to return to menu", new Vector2(WIDTH / 4, HEIGHT * 3 / 4), Color.White);

            if (state.IsKeyDown(Keys.Space))
            {
                gameInfo.mode = 0;
            }
        }

        private void showControlsMenu()
        {

            m_spriteBatch.Draw(selector, new Rectangle((WIDTH * 3 / 5) - (5), HEIGHT * (2 + (controlSelected)) / 12, WIDTH * 1/5, 30), Color.Blue);

            m_spriteBatch.DrawString(smallEl, gameInfo.keys["left"].ToString(), new Vector2(WIDTH * 3 / 5, HEIGHT * 2 / 12), Color.White);
            m_spriteBatch.DrawString(smallEl, gameInfo.keys["right"].ToString(), new Vector2(WIDTH * 3 / 5, HEIGHT * 3 / 12), Color.White);
            m_spriteBatch.DrawString(smallEl, gameInfo.keys["shoot"].ToString(), new Vector2(WIDTH * 3 / 5, HEIGHT * 4 / 12), Color.White);

            m_spriteBatch.DrawString(ELNATH, "-Controls-", new Vector2(WIDTH / 10, HEIGHT / 10), Color.White);
            m_spriteBatch.DrawString(smallEl, "Move Left: ", new Vector2(WIDTH / 8, HEIGHT * 2 / 12), Color.White);
            m_spriteBatch.DrawString(smallEl, "Move Right: ", new Vector2(WIDTH / 8, HEIGHT * 3 / 12), Color.White);
            m_spriteBatch.DrawString(smallEl, "Shoot", new Vector2(WIDTH / 8, HEIGHT * 4 / 12), Color.White);

            m_spriteBatch.DrawString(smallEl, "Use up and down keys to navigate and Space to select", new Vector2(WIDTH / 20, HEIGHT * 6 / 12), Color.White);
            m_spriteBatch.DrawString(smallEl, "Press escape to return", new Vector2(WIDTH / 20, HEIGHT * 8 / 12), Color.White);

            if (isChangingControl)
            {
                m_spriteBatch.Draw(selector, new Rectangle(WIDTH * 1 / 4, HEIGHT * 1 / 2, WIDTH * 1 / 2, HEIGHT * 1 / 10), Color.Gray);
                m_spriteBatch.DrawString(smallEl, "Press the new key for this control", new Vector2(WIDTH * 1 / 4, HEIGHT * 1 / 2), Color.White);
                if (Keyboard.GetState().GetPressedKeys().Length > 0 && controlTimer > new TimeSpan(0, 0, 1))
                {
                    Keys newKey = Keyboard.GetState().GetPressedKeys()[0];
                    Dictionary<string, Keys> keys = new Dictionary<string, Keys>();
                    TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
                    using (StreamReader sr = new StreamReader("Keys.txt"))
                    {
                        keys.Add("left", (Keys)converter.ConvertFromString(sr.ReadLine()));
                        keys.Add("right", (Keys)converter.ConvertFromString(sr.ReadLine()));
                        keys.Add("shoot", (Keys)converter.ConvertFromString(sr.ReadLine()));
                    }
                    if (controlSelected == 0)
                    {
                        keys["left"] = newKey;
                    }
                    else if (controlSelected == 1)
                    {
                        keys["right"] = newKey;
                    }
                    else
                    {
                        keys["shoot"] = newKey;
                    }
                    using (StreamWriter sw = new StreamWriter("Keys.txt"))
                    {
                        sw.WriteLine((string)converter.ConvertToString(keys["left"]));
                        sw.WriteLine((string)converter.ConvertToString(keys["right"]));
                        sw.WriteLine((string)converter.ConvertToString(keys["shoot"]));
                    }
                    gameInfo.keys = gameInfo.ReadKeys();
                    isChangingControl = !isChangingControl;
                }
            }
        }

        private void OnEscape(GameTime gameTime, float value)
        {
            if (gameInfo.mode == 6)
            {
                gameInfo.mode = 0;
            }
            else if (gameInfo.mode == 1)
            {
                gameInfo.mode = 4;
            }
            else if (gameInfo.mode == 4)
            {
                gameInfo.mode = 1;
            }
            else if (gameInfo.mode == 2 || gameInfo.mode == 3)
            {
                gameInfo.mode = 0;
            }
            else if (gameInfo.mode == 0)
            {
                gameInfo.mode = -1;
            }
        }

        private void OnUpKey(GameTime gameTime, float value)
        {
            if (gameInfo.mode == 6 && controlSelected != 0)
            {
                controlSelected--;
            }
        }

        private void OnDownKey(GameTime gameTime, float value)
        {
            if (gameInfo.mode == 6 && controlSelected < 2)
            {
                controlSelected++;
            }
        }

        private void OnSelectControl(GameTime gameTime, float value)
        {
            KeyboardState state = Keyboard.GetState();
            if (gameInfo.mode == 6 && !isChangingControl)
            {
                selectorColor = Color.White;
                isChangingControl = true;
                controlTimer = new TimeSpan(0);
            }
        }

    }
}
