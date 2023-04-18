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

        public Formation(GameInfo gameInfo, int wave) 
        {
            this.x = 0;
            this.y = 0;
            this.gameInfo = gameInfo;
            this.wave = wave;

            this.formation = new List<List<Enemy>>();

            for (int i = 0;i < 3; i++) 
            {
                formation.Add(new List<Enemy>());
                for (int j = 0; j < 8; j++)
                {
                    formation[i].Add(new Bee(gameInfo, 0, i*8+j));
                    formation[i][j].formationPosition(j*(gameInfo.playerScale+2), i*(gameInfo.playerScale+2));
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

        public void update(GameTime gameTime)
        {
            foreach (List<Enemy> enemies in formation)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.update(gameTime);
                }
            }
        }

    }
}
