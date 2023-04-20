using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga
{
    class Player
    {
        GameInfo gameinfo;
        Texture2D selector;

        int x, y;
        int xSize, ySize;
        int scale;

        public Player(GameInfo gameinfo)
        {
            this.gameinfo = gameinfo;
            selector = new Texture2D(gameinfo.graphicsDevice, 1, 1);
            selector.SetData(new[] { Color.White });
            x = gameinfo.WIDTH/2;
            y = gameinfo.HEIGHT*4/5;
            xSize = 50;
            ySize = 50;
        }
        

        public void update(GameTime gametime)
        {
            movement(gametime);
        }

        public void draw()
        {
            gameinfo.m_spriteBatch.Draw(gameinfo.player, new Rectangle(x, y, xSize, ySize), Color.White);
        }

        public void movement(GameTime gametime) 
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) 
            {
                x -= (int)(0.5 * gametime.ElapsedGameTime.TotalMilliseconds);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) 
            {
                x += (int)(0.5 * gametime.ElapsedGameTime.TotalMilliseconds);
            }
        }
    }
}
