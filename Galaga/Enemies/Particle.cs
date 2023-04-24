using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Galaga.Galaga.Enemies
{
    internal class Particle
    {
        GameInfo gameInfo;

        double x, y;
        double speed;
        int size;
        
        int dir; //degrees
        Tuple<double, double> velocity;

        Texture2D rectangle;
        Color color;

        List<List<double>> particles;

        Random rand;

        TimeSpan time;
        int life;
        public bool dead;

        public Particle(GameInfo gameInfo, int x, int y, int life)
        {
            this.gameInfo = gameInfo;
            this.x = x;
            this.y = y;
            this.life = life;

             
            rand = new Random();
            speed = rand.NextDouble() * 3 + 2;
            dir = rand.Next(0, 360);
            velocity = getVector(dir);
            size = gameInfo.WIDTH / 400;

            dir = rand.Next(4);

            if(dir == 0)
            {
                color = Color.Red;
            }
            else if(dir == 1) 
            {
                color = Color.Orange;
            }
            else if (dir == 2)
            {
                color = Color.Blue;
            }
            else if (dir == 3)
            {
                color = Color.White;
            }


            time = new TimeSpan(0);
            rectangle = new Texture2D(gameInfo.graphicsDevice, 1, 1);
            rectangle.SetData(new[] { Color.White });
        }

        public void update(GameTime gameTime)
        {
            x += velocity.Item1*speed;
            y += velocity.Item2*speed;

            time += gameTime.ElapsedGameTime;
            if(time.TotalMilliseconds > life)
            {
                dead = true;
            }
        }

        public void draw() 
        {
            gameInfo.m_spriteBatch.Draw(rectangle, new Rectangle((int)x, (int)y, size, size), color);
        }

        public static Tuple<double, double> getVector(double angle)
        {
            // Convert angle from degrees to radians
            double radians = angle * Math.PI / 180.0;

            // Calculate x and y components of vector
            double x = Math.Cos(radians);
            double y = Math.Sin(radians);

            // Create vector with length of 1
            double length = Math.Sqrt(x * x + y * y);
            x /= length;
            y /= length;

            return Tuple.Create(x, y);
        }
    }
}
