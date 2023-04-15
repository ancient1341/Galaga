using Microsoft.Xna.Framework;
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
        int x, y;
        int xSize, ySize;
        int scale;

        public Player()
        {
            
        }
        public void movement() 
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Left)) 
            {
                x -= 1;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) 
            {
                x += 1;
            }
        }
    }
}
