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
        double x, y;

        List<List<Enemy>> formation;
        GameInfo gameInfo;

        int wave;
        float spacing;
        int formationWidth;
        bool growing;
        float speed;

        int[] waveOne = { 4, 8, 10 };
        int enemyWidthCount = 10;

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
            this.y = gameInfo.WIDTH / 4;
            this.speed = 0.2f;

            this.formation = new List<List<Enemy>>();
            timer = new TimeSpan(0);

            generateFirstWave();

            rand = new Random();
            particles = new List<Particle>();
        }


        public void update(GameTime gameTime, List<Bullet> projectiles)
        {
            updateFormation();
            updateEnemies(gameTime, projectiles);
        }


        public void draw()
        {
            foreach (List<Enemy> enemies in formation)
            {
                foreach(Enemy enemy in enemies)
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

            for(int i = 0; i < 150; i++)
            {
                particles.Add(new Particle(gameInfo, x, y, rand.Next(200, 350)));
            }
        }

        public Tuple<float, float> GetRandomEnemyLocation()
        {
            List<Enemy> tempList = new List<Enemy>();
            foreach(List<Enemy> row in formation)
            {
                foreach(Enemy enemy in row)
                {
                    tempList.Add(enemy);
                }
            }

            Random random = new Random();
            int index = random.Next(tempList.Count);
            return Tuple.Create(tempList[index].x, tempList[index].y);
        }







        public void generateFirstWave()
        {
            for (int row = 0; row < 5; row++)
            {
                formation.Add(new List<Enemy>());
                if (row == 0)
                {
                    for (int col = 0; col < enemyWidthCount; col++)
                    {
                        if (col < enemyWidthCount / 2 - waveOne[0] / 2 || col >= enemyWidthCount / 2 + waveOne[0] / 2)
                        {
                            formation[row].Add(new EmptyEnemy(gameInfo)); //4000 4 seconds for entry + 200*Entry Order
                        }
                        else
                        {
                            formation[row].Add(new Boss(gameInfo, 0, 4000 + 200 * col));
                        }
                    }
                }
                else if (row == 1 || row == 2)
                {
                    for (int col = 0; col < enemyWidthCount; col++)
                    {
                        if (col < enemyWidthCount / 2 - waveOne[1] / 2 || col >= enemyWidthCount / 2 + waveOne[1] / 2)
                        {
                            formation[row].Add(new EmptyEnemy(gameInfo)); //4000 4 seconds for entry + 200*Entry Order
                        }
                        else
                        {
                            formation[row].Add(new Butterfly(gameInfo, 1, 2000 + 200 * col));
                        }
                    }
                }
                else
                {
                    for (int col = 0; col < enemyWidthCount; col++)
                    {
                        if (col < enemyWidthCount / 2 - waveOne[2] / 2 || col >= enemyWidthCount / 2 + waveOne[2] / 2)
                        {
                            formation[row].Add(new EmptyEnemy(gameInfo)); //4000 4 seconds for entry + 200*Entry Order
                        }
                        else
                        {
                            formation[row].Add(new Bee(gameInfo, 0, col * 200 + (row - 3) * enemyWidthCount * 200)); //4000 4 seconds for entry + 200*Entry Order
                        }
                    }
                }
            }
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
                            if (formation[rowIndex][enemyIndex].damaged)
                            {
                                explode((int)bullet.x + 9 / 2, (int)bullet.y);
                                formation[rowIndex][enemyIndex] = new EmptyEnemy(gameInfo);
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
        }

        void updateFormation()
        {
            if (timer.TotalSeconds < 10)
            {
                if (growing)
                {
                    spacing += speed;
                    x -= speed * enemyWidthCount/ 2;
                    if (spacing > 40+gameInfo.enemyScale)
                    {
                        growing = false;
                    }
                }
                else
                {
                    spacing -= speed;
                    x += speed * enemyWidthCount / 2;
                    if (spacing < 1 + gameInfo.enemyScale)
                    {
                        growing = true;
                    }
                }
            }
        }
    }
}
