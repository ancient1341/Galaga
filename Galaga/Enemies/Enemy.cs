using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga.Enemies
{
    abstract class Enemy
    {
        int x, y;
        int formationX, formationY;
        int xSize, ySize;
        int Scale;
        double direction;

        GameInfo gameInfo;

        int entrance;

        Texture2D rectangle;


        public Enemy(GameInfo gameInfo, int entrance)
        {
            this.gameInfo = gameInfo;
            this.entrance = entrance;

            rectangle = new Texture2D(gameInfo.graphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });
        }

        public void draw()
        {
            gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(x, y, xSize, ySize), Color.Blue);
        }

        public void update()
        {

        }

    }
}
