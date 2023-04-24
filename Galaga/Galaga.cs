using Galaga.Galaga.Enemies;
using Galaga.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        TimeSpan timeSinceFire;
        TimeSpan destroyTimer;

        public Galaga(GameInfo gameInfo) 
        {
            this.gameInfo= gameInfo;

            rectangle = new Texture2D(gameInfo.graphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });

            timeSinceFire = new TimeSpan(0);

            initialize();

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
            destructTriggered = false;
            projectiles = new List<Bullet>();
            enemyProjectiles = new List<Bullet>();
            this.player= new Player(gameInfo);
            this.player.isActive = true;
            this.formation = new Formation(gameInfo, 0);
            gameInfo.keyboardInput.registerCommand(Keys.Space, true, new InputDeviceHelper.CommandDelegate(Shoot));
            gameInfo.keyboardInput.registerCommand(Keys.Left, false, new InputDeviceHelper.CommandDelegate(OnLeftKey));
            gameInfo.keyboardInput.registerCommand(Keys.Right, false, new InputDeviceHelper.CommandDelegate(OnRightKey));
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
                    destructTriggered = false;
                }
            }
            else
            {
                timeSinceFire += gameTime.ElapsedGameTime;
                if (TimeSpan.Compare(timeSinceFire, new TimeSpan(0, 0, 2)) == 1)
                {
                    Tuple<float, float> location = formation.GetRandomEnemyLocation();
                    enemyProjectiles.Add(new EnemyBullet((int)location.Item1 + gameInfo.enemyScale / 2, (int)location.Item2, gameInfo.m_spriteBatch, gameInfo.spriteDict["bullet"]));
                    timeSinceFire = new TimeSpan(0);
                }
                foreach (Bullet bullet in projectiles)
                {
                    bullet.update(gameTime);
                }
                foreach (Bullet bullet in enemyProjectiles)
                {
                    bullet.update(gameTime);
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
                formation.update(gameTime, projectiles);
                player.update(gameTime);
            }
        }

        public void Shoot(GameTime gameTime, float value)
        {
            if (this.player.isActive)
            {
                projectiles.Add(new PlayerBullet(player.getX() + (player.getSize() / 2) - ((3 * 3) / 2), player.getY(), gameInfo.m_spriteBatch, gameInfo.spriteDict["bullet"]));
            }
        }

        public void OnLeftKey(GameTime gameTime, float value)
        {
            if (this.player.isActive)
            {
                this.player.x -= (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
            }
        }
        public void OnRightKey(GameTime gameTime, float value)
        {
            if (this.player.isActive)
            {
                this.player.x += (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
            }    
        }
    }
}
