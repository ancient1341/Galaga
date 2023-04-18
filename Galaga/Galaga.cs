using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga
{
    class Galaga
    {
        GameInfo gameInfo;

        Player player;
        Formation formation;

        public Galaga(GameInfo gameInfo) 
        {
            this.gameInfo= gameInfo;

            initialize();
        }

        public void initialize()
        {
            this.player= new Player(gameInfo);
            this.formation = new Formation(gameInfo, 0);
        }

        public void draw()
        {
            formation.draw();
        }

        public void update(GameTime gameTime) 
        {
            formation.update(gameTime);
        }

    }
}
