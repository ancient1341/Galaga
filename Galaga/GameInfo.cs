using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.IO;
using System.Diagnostics;

namespace Galaga.Galaga
{
    class GameInfo
    {
        public SpriteBatch m_spriteBatch;
        public GraphicsDevice graphicsDevice;

        public int WIDTH;
        public int HEIGHT;

        public SpriteFont ELNATH;

        public TimeSpan gameTime;
        public Dictionary<string, Texture2D> spriteDict;
        public Dictionary<string, AnimatedSprite> spriteRenderers;
        public AnimatedSprite bossFull, butterfly, bossDamaged, bee;

        public KeyboardInput keyboardInput;

        public int playerScale;
        public int enemyScale;

        public int score;
        public int mode;

        public GameInfo(SpriteBatch m_spriteBatch, GraphicsDevice graphicsDevice, int WIDTH, int HEIGHT, SpriteFont ELNATH, Dictionary<string, AnimatedSprite> spriteRenderers, Dictionary<string, Texture2D> spriteDict, KeyboardInput keyboardInput) 
        {
            this.m_spriteBatch = m_spriteBatch;
            this.graphicsDevice = graphicsDevice;

            this.ELNATH = ELNATH;

            this.WIDTH = WIDTH;
            this.HEIGHT = HEIGHT;

            this.spriteDict = spriteDict;
            this.spriteRenderers = spriteRenderers;

            this.keyboardInput = keyboardInput;

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


    }
}
