using Galaga.Galaga.Enemies;
using Galaga.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga
{
    class Player
    {
        GameInfo gameinfo;
        Texture2D selector;
        List<Particle> particles;
        public int x, y;
        int xSize { get; set; }
        int ySize { get; set; }
        int scale;

        public bool isActive;
        private bool isExploding;
        Texture2D playerTexture;
        private TimeSpan timeSinceFire;

        public Player(GameInfo gameinfo)
        {
            this.particles = new List<Particle>();
            this.gameinfo = gameinfo;
            selector = new Texture2D(gameinfo.graphicsDevice, 1, 1);
            selector.SetData(new[] { Color.White });
            x = gameinfo.WIDTH / 2;
            y = gameinfo.HEIGHT * 9 / 10;
            xSize = 50;
            ySize = 50;
            this.isActive = false;
            this.timeSinceFire = new TimeSpan(0);
        }


        public void update(GameTime gametime)
        {
            timeSinceFire += gametime.ElapsedGameTime;
            gameinfo.keyboardInput.Update(gametime);
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].update(gametime);
                if (particles[i].dead)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void draw()
        {
            if (isExploding)
            {
                gameinfo.spriteRenderers["playerExplosion"].draw(gameinfo.m_spriteBatch, 0, new Rectangle(x + (int)(xSize * 0.5), y + (int)(xSize * 0.5), (int)(xSize*2), (int)(ySize*2)));
            }
            else
            {
                gameinfo.m_spriteBatch.Draw(gameinfo.spriteDict["player"], new Rectangle(x, y, xSize, ySize), Color.White);
            }
            
        }

        /*public void movement(GameTime gametime) 
        {


            if (Keyboard.GetState().IsKeyDown(Keys.Left)) 
            {
                x -= (int)(0.5 * gametime.ElapsedGameTime.TotalMilliseconds);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) 
            {
                x += (int)(0.5 * gametime.ElapsedGameTime.TotalMilliseconds);
            }
        }*/

        
        public void attractMode(List<List<Enemy>> formation, GameTime gameTime, List<Bullet> projectiles)
        {
            double closestX = gameinfo.WIDTH;
            foreach (List<Enemy> row in formation) 
            {
                List<Enemy> enemies = row.OrderBy(e => Math.Abs(e.x - this.x)).ToList();
                foreach (Enemy enemy in enemies)
                {
                    if (enemy is not EmptyEnemy && (Math.Abs(enemy.x - this.x) < closestX))
                    {
                        closestX = enemy.x;
                    }
                }
                Debug.WriteLine(enemies[0].x);
            }
            if (closestX > this.x + (xSize / 2) && this.x > 0)
            {
                this.x += (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            else if (closestX < this.x + (xSize / 2) && this.x < gameinfo.WIDTH)
            {
                this.x -= (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            if (timeSinceFire > new TimeSpan(0, 0, 0, 0, 500))
            {
                projectiles.Add(new PlayerBullet(x + (xSize / 2) - ((3 * 3) / 2), y, gameinfo.m_spriteBatch, gameinfo.spriteDict["bullet"]));
                gameinfo.shot.Play();
                timeSinceFire = new TimeSpan(0);
            }
        }
        public int getX()
        {
            return this.x;
        }
        public int getY()
        {
            return this.y;
        }

        public int getSize()
        {
            return this.xSize;
        }

        public void Destroy()
        {
            isActive = false;
            isExploding = true;
            gameinfo.explosion.Play();
            Random rand = new Random();
            for (int i = 0; i < 150; i++)
            {
                particles.Add(new Particle(gameinfo, x, y, rand.Next(200, 350)));
            }
        }

    }
}
