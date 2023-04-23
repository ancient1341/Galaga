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

        public Formation(GameInfo gameInfo, int wave) 
        {
            this.x = gameInfo.WIDTH/3;
            this.y = gameInfo.WIDTH / 4; ;
            this.gameInfo = gameInfo;
            this.wave = wave;

            this.formation = new List<List<Enemy>>();

            spacing = gameInfo.playerScale;

            for (int i = 0;i < 3; i++) 
            {
                formation.Add(new List<Enemy>());
                for (int j = 0; j < 10; j++)
                {
                    formation[i].Add(new Bee(gameInfo, 0, i*10+j));
                    formation[i][j].formationPosition(j*(spacing)+x, i*(spacing)+y);
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
        }

        public void update(GameTime gameTime, List<Rectangle> projectiles)
        {
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
                            explode((int)enemy.x, (int)enemy.y);
                            formation[rowIndex].RemoveAt(enemyIndex);

                            projectiles.RemoveAt(bulletIndex);
                        }
                    }
                    enemy.update(gameTime);
                }
            }
        }

        private void explode(int x, int y)
        {

        }

    }
}
