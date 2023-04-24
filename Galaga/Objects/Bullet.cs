using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Objects
{
    abstract class Bullet
    {
        public int x { get; set; }
        public int y { get; set; }

        public SpriteBatch m_spriteBatch;
        public Texture2D bulletTexture;

        public abstract void update(GameTime gameTime);
        public abstract void draw();
    }

    internal class EnemyBullet : Bullet
    {
        
        public EnemyBullet(int x, int y, SpriteBatch spriteBatch, Texture2D bulletTexture) 
        {
            this.x = x;
            this.y = y;
            this.m_spriteBatch = spriteBatch;
            this.bulletTexture = bulletTexture;
        }

        public override void update(GameTime gameTime)
        {
            this.y += (int)(1 * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public override void draw()
        {
            m_spriteBatch.Draw(bulletTexture, new Vector2(x, y), null, Color.White, MathHelper.ToRadians(180), new Vector2(0, 0), new Vector2(3, 3), SpriteEffects.None, 0);
        }
    }

    internal class PlayerBullet : Bullet
    {

        public PlayerBullet(int x, int y, SpriteBatch spriteBatch, Texture2D bulletTexture)
        {
            this.x = x;
            this.y = y;
            this.m_spriteBatch = spriteBatch;
            this.bulletTexture = bulletTexture;
        }

        public override void update(GameTime gameTime)
        {
            this.y -= (int)(1 * gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        public override void draw()
        {
            m_spriteBatch.Draw(bulletTexture, new Vector2(x, y), null, Color.White, MathHelper.ToRadians(0), new Vector2(0, 0), new Vector2(3, 3), SpriteEffects.None, 0);
        }
    }
}
