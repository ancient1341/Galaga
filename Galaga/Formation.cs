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
        int x, y;

        List<List<Enemy>> formation;
        GameInfo gameInfo;

        int wave;

        float spacing;

        int formationWidth;

        int[] waveOne = { 4, 8, 10 };
        int enemyWidthCount = 10;

        Random rand;
        List<Particle> particles;

        public Formation(GameInfo gameInfo, int wave)
        {
            this.gameInfo = gameInfo;
            this.wave = wave;


            this.spacing = gameInfo.enemyScale;
            this.formationWidth = (gameInfo.enemyScale + (int)spacing - gameInfo.enemyScale) * enemyWidthCount;
            this.x = gameInfo.WIDTH / 2 - formationWidth / 2;
            this.y = gameInfo.WIDTH / 4; ;

            this.formation = new List<List<Enemy>>();
            /*
            for (int i = 0; i < 3; i++)
            {
                formation.Add(new List<Enemy>());
                for (int j = 0; j < enemyWidthCount; j++)
                {
                    formation[i].Add(new Bee(gameInfo, 0, i * 10 + j));
                    formation[i][j].formationPosition(j * (spacing) + x, i * (spacing) + y);
                }
            }
            */
            generateFirstWave();

            rand = new Random();
            particles = new List<Particle>();
        }


        public void generateFirstWave()
        {
            for (int row = 0; row < 5; row++)
            {
                formation.Add(new List<Enemy>());
                if (row == 0)
                {
                    for (int col = 0; col < waveOne[2]; col++)
                    {
                        if (col < waveOne[2] / 2 - waveOne[0] / 2 || col >= waveOne[2] / 2 + waveOne[0] / 2)
                        {
                            formation[row].Add(new EmptyEnemy(gameInfo)); //4000 4 seconds for entry + 200*Entry Order
                        } else
                        {
                            formation[row].Add(new Boss(gameInfo, 0, 4000 + 200 * col));
                        }
                        formation[row][col].formationPosition(col * (spacing) + x, row * (spacing) + y);
                    }
                }
                else if (row == 1 || row == 2)
                {
                    for (int col = 0; col < waveOne[2]; col++)
                    {
                        if (col < waveOne[2] / 2 - waveOne[0] / 2 || col >= waveOne[2] / 2 + waveOne[0] / 2)
                        {
                            formation[row].Add(new EmptyEnemy(gameInfo)); //4000 4 seconds for entry + 200*Entry Order
                        }
                        else
                        {
                            formation[row].Add(new Butterfly(gameInfo, 0, 2000 + 200 * col));
                        }
                        formation[row][col].formationPosition(col * (spacing) + x, row * (spacing) + y);
                    }
                }
                else
                {
                    for (int col = 0; col < waveOne[2]; col++)
                    {
                        formation[row].Add(new Bee(gameInfo, 0, col * 200)); //4000 4 seconds for entry + 200*Entry Order
                        formation[row][col].formationPosition(col * (spacing) + x, row * (spacing) + y);
                    }
                }
            }
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

        public void update(GameTime gameTime, List<Bullet> projectiles)
        {
            for(int i = 0; i < particles.Count; i++)
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
                                explode((int)bullet.x + 9/2, (int)bullet.y);
                                formation[rowIndex][enemyIndex] = new EmptyEnemy(gameInfo);
                            }
                            else
                            {
                                formation[rowIndex][enemyIndex].damaged= true;
                            }
                            projectiles.RemoveAt(bulletIndex);
                        }
                    }
                    enemy.update(gameTime);
                }
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
    }
}
