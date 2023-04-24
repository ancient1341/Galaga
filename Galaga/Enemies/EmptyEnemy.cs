using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Galaga.Galaga.Enemies
{
    class EmptyEnemy : Enemy
    {
        Objects.EnemyModel m_beeEnemy;
        public EmptyEnemy(GameInfo gameInfo)
        {
            this.gameInfo = gameInfo;
        }

        public EmptyEnemy(GameInfo gameInfo, int entrance, int delay)
        {
            this.gameInfo = gameInfo;
        }



        public override void draw()
        {
            //no
        }

        public override void update(GameTime gameTime)
        {
            //no
        }





    }
}
