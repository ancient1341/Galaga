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
            this.gameInfo = gameInfo;
            this.wave = wave;
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
