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
        List<Rectangle> projectiles;
        Texture2D selector;

        int x, y;
        int xSize, ySize;
        int scale;

        public Player(GameInfo gameinfo)
        {
            this.gameinfo = gameinfo;
            this.projectiles = projectiles;
            selector = new Texture2D(gameinfo.graphicsDevice, 1, 1);
            selector.SetData(new[] { Color.White });
            x = gameinfo.WIDTH / 2;
            y = gameinfo.HEIGHT * 9 / 10;
            xSize = 50;
            ySize = 50;

            gameinfo.keyboardInput.registerCommand(Keys.Left, false, new InputDeviceHelper.CommandDelegate(OnLeftKey));
            gameinfo.keyboardInput.registerCommand(Keys.Right, false, new InputDeviceHelper.CommandDelegate(OnRightKey));
            
        }


        public void update(GameTime gametime)
        {
            gameinfo.keyboardInput.Update(gametime);
        }

        public void draw()
        {
            gameinfo.m_spriteBatch.Draw(gameinfo.spriteDict["player"], new Rectangle(x, y, xSize, ySize), Color.White);
        }

        /*public void movement(GameTime gametime) 
        {


            if (Keyboard.GetState().IsKeyDown(Keys.Left)) 
            {
                x -= (int)(0.5 * gametime.ElapsedGameTime.TotalMilliseconds);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right)) 
            {
                x += (int)(0.5 * gametime.ElapsedGameTime.TotalMilliseconds);
            }
        }*/

        public void OnLeftKey(GameTime gameTime, float value)
        {
            x -= (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        public void OnRightKey(GameTime gameTime, float value)
        {
            x += (int)(0.25 * gameTime.ElapsedGameTime.TotalMilliseconds);
        }
        

        public int getX()
        {
            return this.x;
        }

        public int getY()
        {
            return this.y;
        }

        public int getSize()
        {
            return this.xSize;
        }
    }
}
