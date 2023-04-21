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

        List<Rectangle> projectiles;

        int x, y;
        int xSize, ySize;
        int scale;

        public Player(GameInfo gameinfo)
        {
            this.gameinfo = gameinfo;
            selector = new Texture2D(gameinfo.graphicsDevice, 1, 1);
            selector.SetData(new[] { Color.White });
            x = gameinfo.WIDTH / 2;
            y = gameinfo.HEIGHT * 4 / 5;
            xSize = 50;
            ySize = 50;

            projectiles = new List<Rectangle>();

            gameinfo.keyboardInput.registerCommand(Keys.Left, false, new InputDeviceHelper.CommandDelegate(OnLeftKey));
            gameinfo.keyboardInput.registerCommand(Keys.Right, false, new InputDeviceHelper.CommandDelegate(OnRightKey));
            gameinfo.keyboardInput.registerCommand(Keys.Space, true, new InputDeviceHelper.CommandDelegate(Shoot));
        }


        public void update(GameTime gametime)
        {
            gameinfo.keyboardInput.Update(gametime);
            List<Rectangle> tempList = new List<Rectangle>();
            foreach (Rectangle bullet in projectiles)
            {
                var newBullet = bullet;
                newBullet.Y -= (int)(0.5 * gametime.ElapsedGameTime.TotalMilliseconds);
                tempList.Add(newBullet);
            }
            projectiles = tempList;
        }

        public void draw()
        {
            gameinfo.m_spriteBatch.Draw(gameinfo.spriteDict["player"], new Rectangle(x, y, xSize, ySize), Color.White);
            foreach (Rectangle bullet in projectiles)
            {
                gameinfo.m_spriteBatch.Draw(gameinfo.spriteDict["bullet"], bullet, Color.White);
            }
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
        public void Shoot(GameTime gameTime, float value)
        {
            projectiles.Add(new Rectangle(x + (xSize / 2) - ((3 * 3) / 2), y, 3 * 3, 8 * 3));
        }
    }
}
