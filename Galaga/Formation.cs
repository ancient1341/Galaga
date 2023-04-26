using Galaga.Galaga.Enemies;
using Galaga.Objects;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga
{
    class Formation
    {
        const int START_TIME = 3000;

        double x, y;

        public List<List<Enemy>> formation;
        GameInfo gameInfo;

        int wave;
        float spacing;
        int formationWidth;
        bool growing;
        float speed;
        int enemyWidthCount = 10;

        public bool defeated;

        Random rand;
        List<Particle> particles;

        TimeSpan timer;

        public Formation(GameInfo gameInfo, int wave)
        {
            this.gameInfo = gameInfo;
            this.wave = wave;


            this.spacing = gameInfo.enemyScale;
            this.formationWidth = (gameInfo.enemyScale + (int)spacing - gameInfo.enemyScale) * enemyWidthCount;
            this.x = gameInfo.WIDTH / 2 - formationWidth / 2;
            this.y = gameInfo.HEIGHT / 8;
            this.speed = (float)gameInfo.WIDTH / 8000;

            this.formation = new List<List<Enemy>>();
            timer = new TimeSpan(0);

            if (wave == 0)
            {
                generateFirstWave();
            }
            else if (wave == 1)
            {
                generateSecondWave();
            }
            else if (wave == 2)
            {
                this.y = -5000;
                generateThirdWave();
            }

            rand = new Random();
            particles = new List<Particle>();
        }


        public void update(GameTime gameTime, List<Bullet> projectiles)
        {
            timer += gameTime.ElapsedGameTime;


            if (timer.TotalMilliseconds > START_TIME)
            {
                updateFormation();
                updateEnemies(gameTime, projectiles);
            }
        }


        public void draw()
        {
            if (timer.TotalMilliseconds < START_TIME / 2)
            {
                gameInfo.m_spriteBatch.DrawString(gameInfo.ELNATH, "Wave " + (wave + 1) + " Start", new Vector2(gameInfo.WIDTH / 2 - gameInfo.WIDTH / 6, gameInfo.HEIGHT / 2), Color.OrangeRed);
            }
            else if (timer.TotalMilliseconds <= START_TIME)
            {
                gameInfo.m_spriteBatch.DrawString(gameInfo.ELNATH, "Go", new Vector2(gameInfo.WIDTH / 2 - gameInfo.WIDTH / 30, gameInfo.HEIGHT / 2), Color.OrangeRed);
            }
            foreach (List<Enemy> enemies in formation)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.draw();
                }
            }

            foreach (Particle particle in particles)
            {
                particle.draw();
            }
        }



        private void explode(int x, int y)
        {

            for (int i = 0; i < 150; i++)
            {
                particles.Add(new Particle(gameInfo, x, y, rand.Next(200, 350)));
            }
        }

        public Tuple<float, float> GetRandomEnemyLocation()
        {
            List<Enemy> tempList = new List<Enemy>();
            foreach (List<Enemy> row in formation)
            {
                foreach (Enemy enemy in row)
                {
                    tempList.Add(enemy);
                }
            }

            Random random = new Random();
            int index = random.Next(tempList.Count);
            return Tuple.Create((float)tempList[index].x, (float)tempList[index].y);
        }


        //Placed in own function far away so my eyes dont have a stroke
        void updateEnemies(GameTime gameTime, List<Bullet> projectiles)
        {
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].update(gameTime);
                if (particles[i].dead)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }

            for (int rowIndex = formation.Count - 1; rowIndex >= 0; rowIndex--)
            {
                for (int enemyIndex = formation[rowIndex].Count - 1; enemyIndex >= 0; enemyIndex--)
                {
                    Enemy enemy = formation[rowIndex][enemyIndex];
                    enemy.formationPosition(enemyIndex * (spacing) + (int)x, rowIndex * (spacing) + (int)y);
                    for (int bulletIndex = projectiles.Count - 1; bulletIndex >= 0; bulletIndex--)
                    {
                        Bullet bullet = projectiles[bulletIndex];
                        if (enemy.x < bullet.x + 9 &&
                            enemy.x + enemy.xSize > bullet.x &&
                            enemy.y < bullet.y + 24 &&
                            enemy.y + enemy.ySize > bullet.y)
                        {
                            if (enemy is Bee)
                            {
                                if (enemy.inFormation)
                                {
                                    gameInfo.score += 50;
                                }
                                else
                                {
                                    gameInfo.score += 100;
                                }
                            }
                            if (enemy is Butterfly)
                            {
                                if (enemy.inFormation)
                                {
                                    gameInfo.score += 80;
                                }
                                else
                                {
                                    gameInfo.score += 160;
                                }
                            }
                            if (enemy is Boss)
                            {
                                if (enemy.inFormation)
                                {
                                    gameInfo.score += 150;
                                }
                                else
                                {
                                    gameInfo.score += 400;
                                }
                            }
                            if (formation[rowIndex][enemyIndex].damaged)
                            {
                                explode((int)bullet.x + 9 / 2, (int)bullet.y);
                                formation[rowIndex][enemyIndex] = new EmptyEnemy();
                                gameInfo.explosion.Play();
                            }
                            else
                            {
                                formation[rowIndex][enemyIndex].damaged = true;
                            }
                            projectiles.RemoveAt(bulletIndex);
                        }
                    }
                    enemy.update(gameTime);
                }
            }

            //check if wave Cleared Write no code after this.
            foreach (List<Enemy> enemyList in formation)
            {
                foreach (Enemy enemy in enemyList)
                {
                    if (!enemy.dead)
                    {
                        return;
                    }
                }
            }
            defeated = true;
        }

        void updateFormation()
        {
            if (timer.TotalSeconds < 10000)
            {
                if (growing)
                {
                    spacing += speed;
                    x -= speed * enemyWidthCount / 2;
                    if (spacing > gameInfo.WIDTH / 30 + gameInfo.enemyScale)
                    {
                        growing = false;
                    }
                }
                else
                {
                    spacing -= speed;
                    x += speed * enemyWidthCount / 2;
                    if (spacing <= 1 + gameInfo.enemyScale)
                    {
                        growing = true;
                    }
                }
            }
        }

        public List<List<Enemy>> GetFormationLocations()
        {
            return this.formation;
        }

        private void generateFirstWave()
        {
            List<Enemy> list;
            formation.Add(new List<Enemy>());
            list = formation[0];
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new Boss(gameInfo, 2, 3000));
            list.Add(new Boss(gameInfo, 2, 3400));
            list.Add(new Boss(gameInfo, 2, 3600));
            list.Add(new Boss(gameInfo, 2, 3200));
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());

            for (int i = 0; i < 2; i++)
            {
                formation.Add(new List<Enemy>());
                list = formation[i + 1];
                list.Add(new EmptyEnemy());
                list.Add(new Butterfly(gameInfo, 3, 5000 + i * 400));
                list.Add(new Butterfly(gameInfo, 3, 5200 + i * 400));
                list.Add(new Butterfly(gameInfo, 2, 3100 + i * 400));
                list.Add(new Butterfly(gameInfo, 1, 0 + i * 200));
                list.Add(new Butterfly(gameInfo, 1, 100 + i * 200));
                list.Add(new Butterfly(gameInfo, 2, 3300 + i * 400));
                list.Add(new Butterfly(gameInfo, 3, 5300 + i * 400));
                list.Add(new Butterfly(gameInfo, 3, 5100 + i * 400));
                list.Add(new EmptyEnemy());
            }

            for (int i = 0; i < 2; i++)
            {
                formation.Add(new List<Enemy>());
                list = formation[i + 3];
                list.Add(new Bee(gameInfo, 1, 9000 + i * 400));
                list.Add(new Bee(gameInfo, 1, 9200 + i * 400));
                list.Add(new Bee(gameInfo, 0, 7000 + i * 400));
                list.Add(new Bee(gameInfo, 0, 7200 + i * 400));
                list.Add(new Bee(gameInfo, 0, 0 + i * 200));
                list.Add(new Bee(gameInfo, 0, 100 + i * 200));
                list.Add(new Bee(gameInfo, 0, 7300 + i * 400));
                list.Add(new Bee(gameInfo, 0, 7100 + i * 400));
                list.Add(new Bee(gameInfo, 1, 9300 + i * 400));
                list.Add(new Bee(gameInfo, 1, 9100 + i * 400));
            }
        }

        private void generateSecondWave()
        {
            List<Enemy> list;
            formation.Add(new List<Enemy>());
            list = formation[0];
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new Boss(gameInfo, 6, 3000));
            list.Add(new Boss(gameInfo, 6, 3100));
            list.Add(new Boss(gameInfo, 6, 3300));
            list.Add(new Boss(gameInfo, 6, 3200));
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());

            for (int i = 0; i < 2; i++)
            {
                formation.Add(new List<Enemy>());
                list = formation[i + 1];
                list.Add(new EmptyEnemy());
                list.Add(new Butterfly(gameInfo, 3, 5000 + i * 200));
                list.Add(new Butterfly(gameInfo, 3, 5100 + i * 200));
                list.Add(new Butterfly(gameInfo, 2, 3000 + i * 200));
                list.Add(new Butterfly(gameInfo, 1, 0 + i * 200));
                list.Add(new Butterfly(gameInfo, 5, 100 + i * 200));
                list.Add(new Butterfly(gameInfo, 2, 3100 + i * 200));
                list.Add(new Butterfly(gameInfo, 7, 5000 + i * 200));
                list.Add(new Butterfly(gameInfo, 7, 5100 + i * 200));
                list.Add(new EmptyEnemy());
            }

            for (int i = 0; i < 2; i++)
            {
                formation.Add(new List<Enemy>());
                list = formation[i + 3];
                list.Add(new Bee(gameInfo, 1, 9000 + i * 200));
                list.Add(new Bee(gameInfo, 1, 9100 + i * 200));
                list.Add(new Bee(gameInfo, 0, 7000 + i * 200));
                list.Add(new Bee(gameInfo, 0, 7100 + i * 200));
                list.Add(new Bee(gameInfo, 0, 0 + i * 200));
                list.Add(new Bee(gameInfo, 4, 100 + i * 200));
                list.Add(new Bee(gameInfo, 8, 7000 + i * 200));
                list.Add(new Bee(gameInfo, 8, 7100 + i * 200));
                list.Add(new Bee(gameInfo, 9, 9100 + i * 200));
                list.Add(new Bee(gameInfo, 9, 9000 + i * 200));
            }
        }

        private void generateThirdWave()
        {
            List<Enemy> list;
            formation.Add(new List<Enemy>());
            list = formation[0];
<<<<<<< HEAD
            for (int i = 0; i < 4; i++)
            {
                list.Add(new Bee(gameInfo, 10, i * 100));
                list.Add(new Bee(gameInfo, 11, i * 100));
            }

            for (int i = 0; i < 4; i++)
            {
                list.Add(new Boss(gameInfo, 12, i * 200 + 3000));
                list.Add(new Bee(gameInfo, 12, i * 200 + 3100));
            }

            for (int i = 0; i < 8; i++)
            {
                list.Add(new Bee(gameInfo, 13, i * 100 + 6000));
            }
            for (int i = 0; i < 8; i++)
            {
                list.Add(new Bee(gameInfo, 10, i * 100 + 10000));
            }

            for (int i = 0; i < 8; i++)
            {
                list.Add(new Bee(gameInfo, 12, i * 100 + 14000));
=======
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new Boss(gameInfo, 2, 3000));
            list.Add(new Boss(gameInfo, 2, 3400));
            list.Add(new Boss(gameInfo, 2, 3600));
            list.Add(new Boss(gameInfo, 2, 3200));
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());
            list.Add(new EmptyEnemy());

            for (int i = 0; i < 2; i++)
            {
                formation.Add(new List<Enemy>());
                list = formation[i + 1];
                list.Add(new EmptyEnemy());
                list.Add(new Butterfly(gameInfo, 3, 5000 + i * 400));
                list.Add(new Butterfly(gameInfo, 3, 5200 + i * 400));
                list.Add(new Butterfly(gameInfo, 2, 3100 + i * 400));
                list.Add(new Butterfly(gameInfo, 1, 0 + i * 200));
                list.Add(new Butterfly(gameInfo, 1, 100 + i * 200));
                list.Add(new Butterfly(gameInfo, 2, 3300 + i * 400));
                list.Add(new Butterfly(gameInfo, 3, 5300 + i * 400));
                list.Add(new Butterfly(gameInfo, 3, 5100 + i * 400));
                list.Add(new EmptyEnemy());
            }

            for (int i = 0; i < 3; i++)
            {
                formation.Add(new List<Enemy>());
                list = formation[i + 3];
                list.Add(new Bee(gameInfo, 1, 9000 + i * 400));
                list.Add(new Bee(gameInfo, 1, 9200 + i * 400));
                list.Add(new Bee(gameInfo, 0, 7000 + i * 400));
                list.Add(new Bee(gameInfo, 0, 7200 + i * 400));
                list.Add(new Bee(gameInfo, 0, 0 + i * 200));
                list.Add(new Bee(gameInfo, 0, 100 + i * 200));
                list.Add(new Bee(gameInfo, 0, 7300 + i * 400));
                list.Add(new Bee(gameInfo, 0, 7100 + i * 400));
                list.Add(new Bee(gameInfo, 1, 9300 + i * 400));
                list.Add(new Bee(gameInfo, 1, 9100 + i * 400));
>>>>>>> 8e0b2ba3fab39289e34b766b16fec4039d6096b1
            }
        }
    }
}
