using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using System.ComponentModel;

namespace Galaga.Galaga
{
    class GameInfo
    {
        public SpriteBatch m_spriteBatch;
        public GraphicsDevice graphicsDevice;

        public int WIDTH;
        public int HEIGHT;

        public SpriteFont ELNATH;
        public SpriteFont smallEl;

        public TimeSpan gameTime;
        public Dictionary<string, Texture2D> spriteDict;
        public Dictionary<string, AnimatedSprite> spriteRenderers;
        public AnimatedSprite bossFull, butterfly, bossDamaged, bee;

        public KeyboardInput keyboardInput;

        public Dictionary<string, Keys> keys;

        public int playerScale;
        public int enemyScale;

        public int score;
        public int mode;

        public GameInfo(SpriteBatch m_spriteBatch, GraphicsDevice graphicsDevice, int WIDTH, int HEIGHT, SpriteFont ELNATH, SpriteFont smallEl, Dictionary<string, AnimatedSprite> spriteRenderers, Dictionary<string, Texture2D> spriteDict, KeyboardInput keyboardInput) 
        {
            this.m_spriteBatch = m_spriteBatch;
            this.graphicsDevice = graphicsDevice;

            this.ELNATH = ELNATH;
            this.smallEl = smallEl;

            this.WIDTH = WIDTH;
            this.HEIGHT = HEIGHT;

            this.spriteDict = spriteDict;
            this.spriteRenderers = spriteRenderers;

            this.keyboardInput = keyboardInput;

            this.keys = ReadKeys();

            this.playerScale = HEIGHT / 20;
            this.enemyScale = playerScale * 13 / 17; 

            this.gameTime = new TimeSpan(0);
            
            this.score = 0;
            this.mode = 0;

        }
        public void update(TimeSpan elapsed)
        {
            gameTime += elapsed;
        }

        public void writeScoreToFile(int score)
        {
            List<int> scores = new List<int>();
            try
            {
                using (StreamReader sr = new StreamReader("./HighScores.txt"))
                {
                    string line;
                    Debug.WriteLine("Reading from file");
                    while ((line = sr.ReadLine()) != null)
                    {
                        scores.Add(Int32.Parse(line));
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                using (StreamWriter sw = new StreamWriter("./HighScores.txt"))
                {
                    sw.WriteLine(score.ToString());
                }
            }
            scores.Add(score);
            if (scores.Count > 5)
            {
                scores.Sort();
                scores.RemoveAt(0);
            }
            using (StreamWriter sw = new StreamWriter("./HighScores.txt"))
            {
                foreach (int i in scores)
                {
                    sw.WriteLine(i.ToString());
                }
            }
            
        }

        public Dictionary<string, Keys> ReadKeys()
        {
            Dictionary<string, Keys> keys = new Dictionary<string, Keys>();
            TypeConverter converter = TypeDescriptor.GetConverter(typeof(Keys));
            try
            {
                using (StreamReader sr = new StreamReader("Keys.txt"))
                {
                    keys.Add("left", (Keys)converter.ConvertFromString(sr.ReadLine()));
                    keys.Add("right", (Keys)converter.ConvertFromString(sr.ReadLine()));
                    keys.Add("shoot", (Keys)converter.ConvertFromString(sr.ReadLine()));
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Creating File");
                using (StreamWriter sw = new StreamWriter("Keys.txt"))
                {
                    sw.WriteLine((string)converter.ConvertToString(Keys.Left));
                    sw.WriteLine((string)converter.ConvertToString(Keys.Right));
                    sw.WriteLine((string)converter.ConvertToString(Keys.Space));
                }
            }

            return keys;
        }


    }
}
