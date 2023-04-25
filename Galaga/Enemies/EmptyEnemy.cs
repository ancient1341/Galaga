using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        public EmptyEnemy()
        {
            dead = true;
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
