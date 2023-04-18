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
        public Bee(GameInfo gameInfo, int entrance)
        {
            this.gameInfo = gameInfo;
            this.entrance = entrance;

            rectangle = new Texture2D(gameInfo.graphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });

            this.formationX= 0;
            this.formationY= 0;

            this.xSize = 90;
            this.ySize = 90;

            this.x = gameInfo.WIDTH/2;
            this.y = gameInfo.HEIGHT/2;

            this.speed = 10;

            this.time = new TimeSpan(0);

            position = new Vector2(x, y);
            origin = new Vector2(0.5f, 0.5f);


        }

        public override void draw()
        {
 
            //gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(x, y, xSize, ySize), Color.Yellow);
            gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(x, y, xSize, ySize), null, Color.Yellow, -(float)((Math.PI / 180) * rotation), origin, SpriteEffects.None, 0f);
        }

        public override void update(GameTime gameTime)
        {
            this.time += gameTime.ElapsedGameTime;
            if (inFormation)
            {
                this.x = this.formationX;
                this.y = this.formationY;
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

            if(entrance == 0)
            {
                if(time.TotalMilliseconds < 200)
                {
                    this.rotation = 290;
                }
                else
                {
                    this.rotation += 5;
                }
            }

            dir = getVector(this.rotation);
            //Debug.WriteLine(rotation);
            //Debug.WriteLine("X: " + dir.Item1 + "  Y: " + dir.Item2);
            this.x += (int)(dir.Item1*speed);
            this.y -= (int)(dir.Item2*speed);

        }

    }
}
