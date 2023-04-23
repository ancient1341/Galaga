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

            this.time = TimeSpan.FromMilliseconds(delay * -100);

            initialize();
        }

        private void initialize()
        {
            damaged= false;

            this.formationX = 0;
            this.formationY = 0;

            this.xSize = gameInfo.enemyScale;
            this.ySize = gameInfo.enemyScale;

            this.x = gameInfo.WIDTH / 2;
            this.y = -gameInfo.enemyScale;

            this.speed = 15;


            position = new Vector2(x, y);
            origin = new Vector2(0.5f, 0.5f);

        }

        public override void draw()
        {

            //gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(x, y, xSize, ySize), Color.Yellow);
            //gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(x, y, xSize, ySize), null, Color.Yellow, -(float)((Math.PI / 180) * rotation), origin, SpriteEffects.None, 0f);
            if (!damaged)
            {
                gameInfo.bossFull.draw(gameInfo.m_spriteBatch, -(float)((Math.PI / 180) * rotation), new Rectangle((int)x + xSize / 2, (int)y + ySize / 2, xSize, ySize));
            }
            else
            {
                gameInfo.bossDamaged.draw(gameInfo.m_spriteBatch, -(float)((Math.PI / 180) * rotation), new Rectangle((int)x + xSize / 2, (int)y + ySize / 2, xSize, ySize));
            }
            //gameInfo.bee.draw(gameInfo.m_spriteBatch, m_beeEnemy, );
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
            //origin = new Vector2(xSize/2, ySize/2);
        }


        //Function to be run when enemies first enter in order to path.
        private void enter()
        {
            Tuple<double, double> dir;

            if (entrance == 0)
            {
                if (time.TotalMilliseconds < 500)
                {
                    this.rotation = 270;
                }
                else if (time.TotalMilliseconds < 700)
                {
                    this.rotation += 7;
                }
                else if (time.TotalMilliseconds < 1000)
                {
                    //this.rotation += 5;
                }
                else
                {
                    inFormation = true;
                    rotation = 0;
                }
            }

            dir = getVector(this.rotation);
            //Debug.WriteLine(rotation);
            //Debug.WriteLine("X: " + dir.Item1 + "  Y: " + dir.Item2);
            if (time.TotalMilliseconds > 0)
            {
                this.x += (int)(dir.Item1 * speed);
                this.y -= (int)(dir.Item2 * speed);
            }
        }
    }
}
