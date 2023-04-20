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
        public Texture2D player;
        public AnimatedSprite bossFull, butterfly, bossDamaged, bee;

        public int playerScale;

        public GameInfo(SpriteBatch m_spriteBatch, GraphicsDevice graphicsDevice, int WIDTH, int HEIGHT, SpriteFont ELNATH, Texture2D player, AnimatedSprite bossFull, AnimatedSprite butterfly, AnimatedSprite bossDamaged, AnimatedSprite bee) 
        {
            this.m_spriteBatch = m_spriteBatch;
            this.graphicsDevice = graphicsDevice;

            this.ELNATH = ELNATH;

            this.WIDTH = WIDTH;
            this.HEIGHT = HEIGHT;

            this.player = player;
            this.bossFull = bossFull;
            this.bossDamaged = bossDamaged;
            this.butterfly = butterfly;
            this.bee = bee;

            this.playerScale = HEIGHT / 10;

            this.gameTime = new TimeSpan(0);
        }
        public void update(TimeSpan elapsed)
        {
            gameTime += elapsed;
        }

    }
}
