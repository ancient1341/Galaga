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
    class Bee : Enemy
    {
        Objects.EnemyModel m_beeEnemy;
        public Bee(GameInfo gameInfo, int entrance)
        {
            this.gameInfo = gameInfo;
            this.entrance = entrance;

            this.time = new TimeSpan(0);

            initialize();
        }

        public Bee(GameInfo gameInfo, int entrance, int delay)
        {
            this.gameInfo = gameInfo;
            this.entrance = entrance;

            this.time = TimeSpan.FromMilliseconds(delay * -1);

            initialize();
        }

        public override void draw()
        {
            gameInfo.spriteRenderers["bee"].draw(gameInfo.m_spriteBatch, -(float)((Math.PI / 180) * rotation), new Rectangle((int)x+xSize/2, (int)y+ySize/2, xSize, ySize));
        }

        public override void update(GameTime gameTime)
        {
            Tuple<double, double> dir;
            this.time += gameTime.ElapsedGameTime;

            //if inFormation it will simply gravitate towards its alotted position
            if (inFormation)
            {
                gravitate();
            }
            else
            {
                enter();
            }
            position = new Vector2(x, y);

        }



    }
}
