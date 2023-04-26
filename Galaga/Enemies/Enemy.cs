using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galaga.Objects;

namespace Galaga.Galaga.Enemies
{
    abstract class Enemy
    {
        public Vector2 position;

        Random rand;

        public double x, y;
        public float formationX, formationY;
        public int xSize, ySize;
        public int Scale;
        public double speed;
        public float rotation;
        public bool inFormation;
        public TimeSpan timeSinceShot;

        public GameInfo gameInfo;

        protected int entrance;

        protected Texture2D rectangle;
        protected TimeSpan time;
        protected int bailTime; // when the ship leaves formation and attacks

        public bool damaged = true; //only matters for boss
        public bool dead = false;

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
                this.x += (dir.Item1 * speed);
            }
            if (this.y != formationY)
            {
                this.y += (dir.Item2 * speed);
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

        protected void initialize()
        {
            this.formationX = 0;
            this.formationY = 0;

            this.xSize = gameInfo.enemyScale;
            this.ySize = gameInfo.enemyScale;

            this.x = gameInfo.WIDTH / 2;
            this.y = -gameInfo.enemyScale;
            this.timeSinceShot = new TimeSpan(0);

            rand = new Random();
            bailTime = rand.Next(40000);


            if (entrance == 0)
            {
                this.x = gameInfo.WIDTH * 2 / 3;
            }
            else if (entrance == 1)
            {
                this.x = gameInfo.WIDTH / 3;
            }
            else if (entrance == 2)
            {
                this.x = -gameInfo.enemyScale;
                this.y = gameInfo.HEIGHT*3 / 4;
            }
            else if (entrance == 3)
            {
                this.x = gameInfo.WIDTH + gameInfo.enemyScale;
                this.y = gameInfo.HEIGHT * 3 / 4;
            }

            this.speed = gameInfo.HEIGHT/80;


            position = new Vector2((float)x, (float)y);
        }

        //Function to be run when enemies first enter in order to path.
        protected void enter()
        {
            Tuple<double, double> dir;

            if (entrance == 0) // fly in from top right
            {
                if (time.TotalMilliseconds < 1000)
                {
                    this.rotation = 240;
                }
                else if (time.TotalMilliseconds < 1500)
                {
                    this.rotation += 7;
                }
                else if (time.TotalMilliseconds < 1700)
                {
                    //
                }
                else
                {
                    inFormation = true;
                    rotation = 0;
                }
            }
            else if (entrance == 1) // fly in from top left
            {
                if (time.TotalMilliseconds < 1000)
                {
                    this.rotation = 300;
                }
                else if (time.TotalMilliseconds < 1500)
                {
                    this.rotation -= 7;
                }
                else if (time.TotalMilliseconds < 1700)
                {
                    //
                }
                else
                {
                    inFormation = true;
                    rotation = 0;
                }
            }
            else if (entrance == 2) // fly in from bottom left
            {
                if (time.TotalMilliseconds < 500)
                {
                    this.rotation = 55;
                }
                else if (time.TotalMilliseconds < 1350)
                {
                    this.rotation -= 7;
                }
                else if (time.TotalMilliseconds < 1500)
                {
                    //
                }
                else
                {
                    inFormation = true;
                    rotation = 0;
                }
            }
            else if (entrance == 3) // fly in from bottom left
            {
                if (time.TotalMilliseconds < 500)
                {
                    this.rotation = 125;
                }
                else if (time.TotalMilliseconds < 1350)
                {
                    this.rotation += 7;
                }
                else if (time.TotalMilliseconds < 1500)
                {
                    //
                }
                else
                {
                    inFormation = true;
                    rotation = 0;
                }
            }


            //Breakouts
            if (entrance == -1)
            {
                if (time.TotalMilliseconds < 400)
                {
                    this.rotation += 10;
                }
                else if (time.TotalMilliseconds < 450)
                {
                    Shoot();
                }
                else if (time.TotalMilliseconds < 500)
                {
                    Shoot();
                }
                else if (time.TotalMilliseconds < 1400)
                {
                    //
                }
                else if (time.TotalMilliseconds < 1600)
                {
                    this.rotation -= 10;
                }
                else if (time.TotalMilliseconds < 2600)
                {
                    //
                }
                else if (time.TotalMilliseconds < 2700)
                {
                    this.rotation += 10;
                }
                else if (time.TotalMilliseconds < 4000)
                {
                    //
                }
                else
                {
                    speed *= 2;
                    this.x = gameInfo.WIDTH / 2;
                    this.y = gameInfo.enemyScale * -1;
                    inFormation = true;
                    rotation = 0;
                }
            }
            else if (entrance == -2)
            {
                if (time.TotalMilliseconds < 400)
                {
                    this.rotation -= 10;
                }
                else if (time.TotalMilliseconds < 450)
                {
                    Shoot();
                }
                else if (time.TotalMilliseconds < 500)
                {
                    Shoot();
                }
                else if (time.TotalMilliseconds < 1400)
                {
                    //
                }
                else if (time.TotalMilliseconds < 1600)
                {
                    this.rotation += 10;
                }
                else if (time.TotalMilliseconds < 2600)
                {
                    //
                }
                else if (time.TotalMilliseconds < 2700)
                {
                    this.rotation -= 10;
                }
                else if (time.TotalMilliseconds < 4000)
                {
                    //
                }
                else
                {
                    speed *= 2;
                    this.x = gameInfo.WIDTH / 2;
                    this.y = gameInfo.enemyScale * -1;
                    inFormation = true;
                    rotation = 0;
                }
            }


            dir = getVector(this.rotation);
            if (time.TotalMilliseconds > 0)
            {
                this.x += (int)(dir.Item1 * speed);
                this.y -= (int)(dir.Item2 * speed);
            }
        }

        protected void handleBail(GameTime gameTime)
        {
            if (!inFormation)
            {
                return;
            }
            bailTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if(bailTime <= 0)
            {
                speed /= 2;
                entrance = rand.Next(2)*-1 -1;
                inFormation= false;
                time = new TimeSpan(0);
                bailTime = rand.Next(10000, 30000);
            }
        }

        public void Shoot()
        {
            if (this.timeSinceShot > new TimeSpan(0, 0, 0, 0, 22))
            {
                gameInfo.enemyProjectiles.Add(new EnemyBullet((int)this.x, (int)this.y, gameInfo.m_spriteBatch, gameInfo.spriteDict["bullet"]));
                this.timeSinceShot = new TimeSpan(0);
            }
            
        }
    }
}
