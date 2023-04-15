using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
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

            this.time = new TimeSpan(0);
        }

        public override void draw()
        {
            gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(x, y, xSize, ySize), Color.Yellow);
            //gameInfo.m_spriteBatch.Draw(rectangle, new Vector2(), Nullable, Color, Single, Vector2, Single, SpriteEffects, Single)
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
        }


        //Function to be run when enemies first enter in order to path.
        private void enter()
        {
            Tuple<double, double> dir;

            if(entrance == 0)
            {
                if(time.TotalMilliseconds < 200)
                {
                    this.rotation = 250;
                }
            }

            dir = getVector(this.rotation);
            this.x += (int)(dir.Item1*speed);
            this.y -= (int)(dir.Item2*speed);

        }

    }
}
