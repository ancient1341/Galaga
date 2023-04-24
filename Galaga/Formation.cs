using Galaga.Galaga.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
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

        int[] waveOne = { 4, 8, 2 };
        int enemyWidthCount = 10;

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

            for (int i = 0; i < 3; i++)
            {
                formation.Add(new List<Enemy>());
                for (int j = 0; j < enemyWidthCount; j++)
                {
                    formation[i].Add(new Bee(gameInfo, 0, i * 10 + j));
                    formation[i][j].formationPosition(j * (spacing) + x, i * (spacing) + y);
                }
            }

            particles = new List<Particle>();
        }


        public void generateFirstWave()
        {
            for(int row = 0; row < 5; row++)
            {
                formation.Add(new List<Enemy>());
                if (row == 0)
                {
                    for(int i = 0; i < waveOne[0]; i++)
                    {
                        formation[row].Add(new Boss(gameInfo, 0, 4000+200*i)); //4000 4 seconds for entry + 200*Entry Order
                        formation[row][i].formationPosition(x + formationWidth/2 - waveOne[0]/2*gameInfo.enemyScale + gameInfo.enemyScale*i, i * (spacing) + y);
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

        public void update(GameTime gameTime, List<Rectangle> projectiles)
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
                        Rectangle bullet = projectiles[bulletIndex];
                        if (enemy.x < bullet.Left + bullet.Width &&
                            enemy.x + enemy.xSize > bullet.Left &&
                            enemy.y < bullet.Top + bullet.Height &&
                            enemy.y + enemy.ySize > bullet.Top)
                        {
                            if (formation[rowIndex][enemyIndex].damaged)
                            {
                                explode((int)bullet.Left + bullet.Width/2, (int)bullet.Top);
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
                particles.Add(new Particle(gameInfo, x, y, 300));
            }
        }

    }
}
