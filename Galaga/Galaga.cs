using Galaga.Galaga.Enemies;
using Galaga.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga
{
    class Galaga
    {
        GameInfo gameInfo;

        bool destructTriggered;

        Player player;
        Formation formation;

        Texture2D rectangle;

        List<Bullet> projectiles;
        List<Bullet> enemyProjectiles;
        List<Player> extraLives;

        TimeSpan destroyTimer;
        TimeSpan attractTimer;

        int wave = 0;

        public Galaga(GameInfo gameInfo) 
        {
            this.gameInfo= gameInfo;

            rectangle = new Texture2D(gameInfo.graphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });
            attractTimer = new TimeSpan(0);

        }

        public void initialize()
        {
            extraLives = new List<Player>();
            for (int i = 0; i < 2; i++)
            {
                Player extraLife = new Player(gameInfo);
                extraLife.y = gameInfo.HEIGHT - extraLife.getSize();
                extraLife.x = 0 + (i * extraLife.getSize());
                extraLives.Add(extraLife);
            }
            gameInfo.score = 0;
            destructTriggered = false;
            projectiles = new List<Bullet>();
            gameInfo.enemyProjectiles = new List<Bullet>();
            enemyProjectiles = gameInfo.enemyProjectiles;

            this.player= new Player(gameInfo);
            this.player.isActive = true;
            this.formation = new Formation(gameInfo, 0);
            gameInfo.keyboardInput.registerCommand(gameInfo.keys["shoot"], true, new InputDeviceHelper.CommandDelegate(Shoot));
            gameInfo.keyboardInput.registerCommand(gameInfo.keys["left"], false, new InputDeviceHelper.CommandDelegate(OnLeftKey));
            gameInfo.keyboardInput.registerCommand(gameInfo.keys["right"], false, new InputDeviceHelper.CommandDelegate(OnRightKey));
        }

        public void draw()
        {
            formation.draw();
            foreach (Bullet bullet in projectiles)
            {
                bullet.draw();
            }
            foreach (Bullet bullet in enemyProjectiles)
            {
                bullet.draw();
            }
            foreach (Player life in extraLives)
            {
                life.draw();
            }
            player.draw();
            gameInfo.m_spriteBatch.DrawString(gameInfo.ELNATH, gameInfo.score.ToString(), new Vector2(5, 5), Color.White);
            //gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(0, 0, gameInfo.HEIGHT*4/9, gameInfo.HEIGHT), Color.White);
            //gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(gameInfo.WIDTH-gameInfo.HEIGHT*4/9, 0, gameInfo.HEIGHT * 4 /9, gameInfo.HEIGHT), Color.White);
        }

        public void update(GameTime gameTime) 
        {
            attractTimer += gameTime.ElapsedGameTime;
            if (attractTimer > new TimeSpan(0, 0, 10))
            {
                player.attractMode(formation.formation, gameTime, projectiles, gameInfo.enemyProjectiles);
            }
            if (destructTriggered)
            {
                destroyTimer += gameTime.ElapsedGameTime;
                if (destroyTimer > new TimeSpan(0, 0, 2))
                {
                    if (extraLives.Count > 0)
                    {
                        this.player = extraLives[0];
                        this.player.x = (gameInfo.WIDTH / 2);
                        this.player.y = gameInfo.HEIGHT * 9 / 10;
                        extraLives.RemoveAt(0);
                        this.player.isActive = true;
                    }
                    else
                    {
                        this.gameInfo.mode = 5;
                        this.gameInfo.writeScoreToFile(this.gameInfo.score);
                        this.gameInfo.score = 0;
                    }
                    destructTriggered = false;
                }
            }
            else
            {
                attractTimer += gameTime.ElapsedGameTime;
                if (attractTimer > new TimeSpan(0, 0, 10))
                {
                    player.attractMode(formation.formation, gameTime, projectiles, enemyProjectiles);
                }
                foreach (Bullet bullet in projectiles)
                {
                    bullet.update(gameTime);
                }
                foreach (Bullet bullet in gameInfo.enemyProjectiles)
                {
                    bullet.update(gameTime);
                }
                for (int row = formation.formation.Count - 1; row >= 0; row--)
                {
                    for (int col = formation.formation[row].Count - 1; col >= 0; col--)
                    {
                        Enemy enemy = formation.formation[row][col];
                        if (enemy.x < player.x + 9 &&
                            enemy.x + enemy.xSize > player.x &&
                            enemy.y < player.y + 24 &&
                            enemy.y + enemy.ySize > player.y)
                        {
                            formation.formation[row].RemoveAt(col);
                            player.Destroy();
                            destructTriggered = true;
                            destroyTimer = new TimeSpan(0);
                        }
                    }
                }
                for (int i = enemyProjectiles.Count - 1; i >= 0; i--)
                {
                    Bullet bullet = enemyProjectiles[i];
                    if (player.getX() < bullet.x + 9 &&
                        player.getX() + player.getSize() > bullet.x &&
                        player.getY() < bullet.y + 24 &&
                        player.getY() + player.getSize() > bullet.y &&
                        player.isActive)
                    {
                        enemyProjectiles.RemoveAt(i);
                        player.Destroy();
                        destructTriggered = true;
                        destroyTimer = new TimeSpan(0);
                    }
                }
                if (formation.defeated)
                {
                    nextWave();
                }
                    formation.update(gameTime, projectiles);
                player.update(gameTime);
            }
        }

        public void Shoot(GameTime gameTime, float value)
        {
            if (this.player.isActive)
            {
                projectiles.Add(new PlayerBullet(player.getX() + (player.getSize() / 2) - ((3 * 3) / 2), player.getY(), gameInfo.m_spriteBatch, gameInfo.spriteDict["bullet"]));
                gameInfo.shot.Play();
            }
            attractTimer = new TimeSpan(0);
        }

        public void OnLeftKey(GameTime gameTime, float value)
        {
            if (this.player.isActive && this.player.x > 0)
            {
                this.player.x -= (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            attractTimer = new TimeSpan(0);
        }
        public void OnRightKey(GameTime gameTime, float value)
        {
            if (this.player.isActive && this.player.x + this.player.getSize() < this.gameInfo.WIDTH)
            {
                this.player.x += (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
            attractTimer = new TimeSpan(0);
        }

        private void nextWave()
        {
            if (wave < 2)
            {
                wave++;
            }
            else 
            { 
                wave = 0; 
            }
            formation = new Formation(gameInfo, wave);
        }
    }
}
