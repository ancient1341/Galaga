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
        public Vector2 position;
        public Vector2 origin;

        public int x, y;
        public int formationX, formationY;
        public int xSize, ySize;
        public int Scale;
        public int speed;
        public float rotation;
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

        protected void gravitate()
        {
            Tuple<double, double> dir;

            if (Math.Abs(this.formationX - this.x) < speed)
            {
                this.x = this.formationX;
            }

            if (Math.Abs(this.formationY - this.y) < speed)
            {
                this.y = this.formationY;
            }

            dir = GetSlope(x, y, formationX, formationY);

            if (this.x != formationX)
            {
                this.x -= (int)(dir.Item1 * speed);
            }
            if (this.y != formationY)
            {
                this.y -= (int)(dir.Item2 * speed);
            }
        }


        public abstract void draw();
        public abstract void update(GameTime gameTime);






        //Chat-GPT created Functions

        //Given an angle Return A slope in that direction where |x|+|y| = 1
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

        //Given 2 points generate A slope from one point to the other where |x|+|y| = 1
        public static Tuple<double, double> GetSlope(double x1, double y1, double x2, double y2)
        {
            if (x2 - x1 == 0)
            {
                // handle vertical slope (undefined)
                return Tuple.Create(1.0, 0.0);
            }
            else
            {
                // calculate slope using the formula (y2 - y1) / (x2 - x1)
                double slope = (y2 - y1) / (x2 - x1);

                // calculate x and y components of the slope
                double x = 1.0 / Math.Sqrt(1 + slope * slope);
                double y = slope * x;

                // return tuple with x and y components
                return Tuple.Create(x, y);
            }
        }
    }
}
