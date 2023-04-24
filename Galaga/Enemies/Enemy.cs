using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Galaga.Galaga.Enemies
{
    abstract class Enemy
    {
        public Vector2 position;
        public Vector2 origin;

        public float x, y;
        public float formationX, formationY;
        public int xSize, ySize;
        public int Scale;
        public int speed;
        public float rotation;
        public bool inFormation;

        public GameInfo gameInfo;

        protected int entrance;

        protected Texture2D rectangle;
        protected TimeSpan time;

        public bool damaged = true; //For boss

        public void formationPosition(float formationX, float formationY)
        {
            this.formationX= formationX;
            this.formationY = formationY;
        }

        protected void gravitate()
        {
            Tuple<double, double> dir;

            if (Math.Abs(this.formationX - this.x) < speed || Math.Abs(this.formationY - this.y) < speed)
            {
                this.x = this.formationX;
                this.y = this.formationY;
            }

            //y = formationY;
            //x = formationX;

            if (y == formationY && x == formationX)
            {
                this.rotation = 90;
                return;
            }

            dir = GetSlope(x, y, formationX, formationY);
            rotation = GetAngle(-dir.Item1, dir.Item2);
            //Debug.WriteLine()

            if (this.x != formationX)
            {
                this.x += (int)(dir.Item1 * speed);
            }
            if (this.y != formationY)
            {
                this.y += (int)(dir.Item2 * speed);
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
            if (x2 == x1)
            {
                // handle vertical slope (undefined)
                return Tuple.Create(1.0, 0.0);
            }
            else
            {
                // calculate slope using the formula (y2 - y1) / (x2 - x1)
                double slope = (y2 - y1) / (x2 - x1);

                // calculate the angle of the slope in radians
                double angle = Math.Atan2(y2 - y1, x2 - x1);

                // calculate the x and y components of the slope
                double x = Math.Cos(angle);
                double y = Math.Sin(angle);

                // return tuple with x and y components
                return Tuple.Create(x, y);
            }
        }


        public static float GetAngle(double x, double y)
        {
            double angleRadians = Math.Atan2(y, x);
            float angleDegrees = (float)(angleRadians * (180 / Math.PI));
            return angleDegrees-180;
        }

        public void Shoot()
        {

        }
    }
}
