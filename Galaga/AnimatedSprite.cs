using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Galaga.Galaga
{
    public class AnimatedSprite
    {
        private Texture2D m_spriteSheet;
        private int[] m_spriteTime;

        private TimeSpan m_animationTime;
        private int m_subImageIndex;
        private int m_subImageWidth;

        public AnimatedSprite(Texture2D spriteSheet, int[] spriteTime)
        {
            this.m_spriteSheet = spriteSheet;
            this.m_spriteTime = spriteTime;

            m_subImageWidth = spriteSheet.Width / spriteTime.Length;
        }

        public void update(GameTime gameTime)
        {
            m_animationTime += gameTime.ElapsedGameTime;
            if (m_animationTime.TotalMilliseconds >= m_spriteTime[m_subImageIndex])
            {
                m_animationTime -= TimeSpan.FromMilliseconds(m_spriteTime[m_subImageIndex]);
                m_subImageIndex++;
                m_subImageIndex = m_subImageIndex % m_spriteTime.Length;
            }
        }

        public void draw(SpriteBatch spriteBatch, Objects.AnimatedSprite model, float rotation, Rectangle rect, Vector2 origin)
        {
            spriteBatch.Draw(
                m_spriteSheet,
                rect, // Destination rectangle
                new Rectangle(m_subImageIndex * m_subImageWidth, 0, m_subImageWidth, m_spriteSheet.Height), // Source sub-texture
                Color.White,
                rotation, // Angular rotation
                origin, // Center point of rotation
                SpriteEffects.None, 0);
        }
    }
}
