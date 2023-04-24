using Galaga.Galaga.Enemies;
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
        List<Particle> particles;
        public int x, y;
        int xSize { get; set; }
        int ySize { get; set; }
        int scale;

        public bool isActive;
        private bool isExploding;
        Texture2D playerTexture;

        public Player(GameInfo gameinfo)
        {
            this.particles = new List<Particle>();
            this.gameinfo = gameinfo;
            selector = new Texture2D(gameinfo.graphicsDevice, 1, 1);
            selector.SetData(new[] { Color.White });
            x = gameinfo.WIDTH / 2;
            y = gameinfo.HEIGHT * 9 / 10;
            xSize = 50;
            ySize = 50;
            this.isActive = false;
            
        }


        public void update(GameTime gametime)
        {
            gameinfo.keyboardInput.Update(gametime);
            for (int i = 0; i < particles.Count; i++)
            {
                particles[i].update(gametime);
                if (particles[i].dead)
                {
                    particles.RemoveAt(i);
                    i--;
                }
            }
        }

        public void draw()
        {
            if (isExploding)
            {
                gameinfo.spriteRenderers["playerExplosion"].draw(gameinfo.m_spriteBatch, 0, new Rectangle(x, y, xSize, ySize));
            }
            else
            {
                gameinfo.m_spriteBatch.Draw(gameinfo.spriteDict["player"], new Rectangle(x, y, xSize, ySize), Color.White);
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

        public void Destroy()
        {
            isActive = false;
            isExploding = true;
            Random rand = new Random();
            for (int i = 0; i < 150; i++)
            {
                particles.Add(new Particle(gameinfo, x, y, rand.Next(200, 350)));
            }
        }

    }
}
