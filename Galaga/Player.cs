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
        GameInfo gameInfo;
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
            this.gameInfo = gameinfo;
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
            gameInfo.keyboardInput.Update(gametime);
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
                gameInfo.spriteRenderers["playerExplosion"].draw(gameInfo.m_spriteBatch, 0, new Rectangle(x + (int)(xSize * 0.5), y + (int)(xSize * 0.5), (int)(xSize * 2), (int)(ySize * 2)));
            }
            else
            {
                gameInfo.m_spriteBatch.Draw(gameInfo.spriteDict["player"], new Rectangle(x, y, xSize, ySize), Color.White);
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


        public void attractMode(List<List<Enemy>> formation, GameTime gameTime, List<Bullet> projectiles, List<Bullet> enemyProjectiles)
        {
            double closestX = gameInfo.WIDTH;
            double closestY = 0;
            foreach (List<Enemy> row in formation)
            {
                foreach (Enemy enemy in row)
                {
                    if (enemy is not EmptyEnemy)
                    {
                        closestX = enemy.x;
                        closestY = enemy.y;
                        break;
                    }
                }
            }
            List<Bullet> dangerBullets = new List<Bullet>();
            foreach (Bullet bullet in enemyProjectiles)
            {
                if (bullet.y < y)
                {
                    dangerBullets.Add(bullet);
                }
            }
            if (dangerBullets.Count > 0)
            {
                dangerBullets = dangerBullets.OrderBy(b => b.y).ToList();
                dangerBullets.Reverse();
                if (dangerBullets[0].x > x && x > 0 && Math.Abs(dangerBullets[0].x - x) < 100)
                {
                    this.x -= (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
                }
                else if (dangerBullets[0].x < x && x < gameInfo.WIDTH && Math.Abs(dangerBullets[0].x - x) < 100)
                {
                    this.x += (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
                }
            }
            else
            {
                if ((gameInfo.HEIGHT - closestY) < 200)
                {
                    if (closestX > this.x + (xSize / 2) && this.x > 0 && Math.Abs(closestX - x) < 100)
                    {
                        this.x -= (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                    else if (closestX < this.x + (xSize / 2) && this.x < gameInfo.WIDTH && Math.Abs(closestX - x) < 100)
                    {
                        this.x += (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                }
                else
                {
                    if (closestX > this.x + (xSize / 2) && this.x < gameInfo.WIDTH)
                    {
                        this.x += (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                    else if (closestX < this.x + (xSize / 2) && this.x > 0)
                    {
                        this.x -= (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
                    }
                }


            }
            if (timeSinceFire > new TimeSpan(0, 0, 0, 0, 500))
            {
                projectiles.Add(new PlayerBullet(x + (xSize / 2) - ((3 * 3) / 2), y, gameInfo.m_spriteBatch, gameInfo.spriteDict["bullet"]));
                gameInfo.shot.Play();
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
            gameInfo.explosion.Play();
            Random rand = new Random();
            for (int i = 0; i < 150; i++)
            {
                particles.Add(new Particle(gameInfo, x, y, rand.Next(200, 350)));
            }
        }

    }
}
