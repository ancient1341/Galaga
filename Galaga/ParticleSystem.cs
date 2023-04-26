using Galaga.Galaga.Enemies;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga
{
    internal class ParticleSystem
    {
        List<Particle> particleList;
        public ParticleSystem()
        {
            particleList = new List<Particle>();
        }

        public void draw()
        {
            foreach (Particle particle in particleList)
            {
                particle.draw();
            }
        }

        public void update(GameTime gameTime)
        {
            for (int i = 0; i < particleList.Count; i++)
            {
                particleList[i].update(gameTime);
                if (particleList[i].dead)
                {
                    particleList.RemoveAt(i);
                    i--;
                }
            }
        }

        public void EnemyDeath(GameInfo gameInfo, int x, int y)
        {
            for (int i = 0; i < 150; i++)
            {
                Random rand = new Random();
                particleList.Add(new Particle(gameInfo, x, y, rand.Next(200, 350)));
            }
        }

        public void PlayerDeath(GameInfo gameInfo, int x, int y)
        {
            for (int i = 0; i < 150; i++)
            {
                Random rand = new Random();
                particleList.Add(new Particle(gameInfo, x, y, rand.Next(200, 350)));
            }
        }
    }
}
