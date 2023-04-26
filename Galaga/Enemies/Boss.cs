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
    class Boss : Enemy
    {
        public Boss(GameInfo gameInfo, int entrance)
        {
            this.gameInfo = gameInfo;
            this.entrance = entrance;

            this.time = new TimeSpan(0);

            initialize();
        }

        public Boss(GameInfo gameInfo, int entrance, int delay)
        {
            this.gameInfo = gameInfo;
            this.entrance = entrance;

            this.time = TimeSpan.FromMilliseconds(delay * -1);

            initialize();
            damaged = false;
        }

        public override void draw()
        {
            if (!damaged)
            {
                gameInfo.spriteRenderers["green"].draw(gameInfo.m_spriteBatch, -(float)((Math.PI / 180) * rotation), new Rectangle((int)x + xSize / 2, (int)y + ySize / 2, xSize, ySize));
            }
            else
            {
                gameInfo.spriteRenderers["blue"].draw(gameInfo.m_spriteBatch, -(float)((Math.PI / 180) * rotation), new Rectangle((int)x + xSize / 2, (int)y + ySize / 2, xSize, ySize));
            }
        }

        public override void update(GameTime gameTime)
        {
            this.timeSinceShot += gameTime.ElapsedGameTime;
            Tuple<double, double> dir;
            this.time += gameTime.ElapsedGameTime;

            //if inFormation it will simply gravitate towards its alotted position
            if (inFormation)
            {
                gravitate();
                handleBail(gameTime);
            }
            else
            {
                enter();
            }
            position = new Vector2((float)x, (float)y);
        }


    }
}
