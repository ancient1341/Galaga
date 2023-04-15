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
        public int x, y;
        public int formationX, formationY;
        public int xSize, ySize;
        public int Scale;
        public int speed;
        public double rotation;
        public bool inFormation;

        public GameInfo gameInfo;

        protected int entrance;

        protected Texture2D rectangle;
        protected TimeSpan time;

        public void formationPosition(int formationX, int formationY)
        {
            this.formationX= formationX;
            this.formationY = formationY;
        }
        public abstract void draw();
        public abstract void update(GameTime gameTime);


        //Chat-GPT created
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
