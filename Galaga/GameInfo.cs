using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public AnimatedSprite bossFull, butterfly, bossDamaged, bee;

        public KeyboardInput keyboardInput;

        public int playerScale;

        public GameInfo(SpriteBatch m_spriteBatch, GraphicsDevice graphicsDevice, int WIDTH, int HEIGHT, SpriteFont ELNATH, AnimatedSprite bossFull, AnimatedSprite butterfly, AnimatedSprite bossDamaged, AnimatedSprite bee, KeyboardInput keyboardInput) 
        {
            this.m_spriteBatch = m_spriteBatch;
            this.graphicsDevice = graphicsDevice;

            this.ELNATH = ELNATH;

            this.WIDTH = WIDTH;
            this.HEIGHT = HEIGHT;

            this.bossFull = bossFull;
            this.bossDamaged = bossDamaged;
            this.butterfly = butterfly;
            this.bee = bee;

            this.keyboardInput = keyboardInput;

            this.playerScale = HEIGHT / 10;

            this.gameTime = new TimeSpan(0);
        }
        public void update(TimeSpan elapsed)
        {
            gameTime += elapsed;
        }

    }
}
