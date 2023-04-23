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
    class Galaga
    {
        GameInfo gameInfo;

        Player player;
        Formation formation;

        Texture2D rectangle;

        List<Rectangle> projectiles;

        public Galaga(GameInfo gameInfo) 
        {
            this.gameInfo= gameInfo;

            rectangle = new Texture2D(gameInfo.graphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });

            initialize();
        }

        public void initialize()
        {
            projectiles = new List<Rectangle>();
            this.player= new Player(gameInfo);
            this.formation = new Formation(gameInfo, 0);
            gameInfo.keyboardInput.registerCommand(Keys.Space, true, new InputDeviceHelper.CommandDelegate(Shoot));
        }

        public void draw()
        {
            formation.draw();
            foreach (Rectangle bullet in projectiles)
            {
                gameInfo.m_spriteBatch.Draw(gameInfo.spriteDict["bullet"], bullet, Color.White);
            }
            player.draw();

            gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(0, 0, gameInfo.HEIGHT*4/9, gameInfo.HEIGHT), Color.White);
            gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle(gameInfo.WIDTH-gameInfo.HEIGHT*4/9, 0, gameInfo.HEIGHT * 4 /9, gameInfo.HEIGHT), Color.White);
        }

        public void update(GameTime gameTime) 
        {
            List<Rectangle> tempList = new List<Rectangle>();
            foreach (Rectangle bullet in projectiles)
            {
                var newBullet = bullet;
                newBullet.Y -= (int)(0.5 * gameTime.ElapsedGameTime.TotalMilliseconds);
                tempList.Add(newBullet);
            }
            projectiles = tempList;
            formation.update(gameTime, projectiles);
            player.update(gameTime);
        }

        public void Shoot(GameTime gameTime, float value)
        {
            projectiles.Add(new Rectangle(player.getX() + (player.getSize() / 2) - ((3 * 3) / 2), player.getY(), 3 * 3, 8 * 3));
        }
    }
}
