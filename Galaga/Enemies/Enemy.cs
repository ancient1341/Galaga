using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Galaga.Objects;
using System.Runtime.Intrinsics.X86;

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
            this.formationX = formationX;
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
            return angleDegrees - 180;
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
            bailTime = rand.Next(8000, 30000);

            setupEntrances();

            this.speed = gameInfo.HEIGHT / 80;


            position = new Vector2((float)x, (float)y);
        }

        void setupEntrances()
        {
            if (entrance == 0 || entrance == 4 || entrance == 10)
            {
                this.x = gameInfo.WIDTH * 2 / 3;
            }
            else if (entrance == 1 || entrance == 5 || entrance == 11)
            {
                this.x = gameInfo.WIDTH / 3;
            }
            else if (entrance == 2)
            {
                this.x = -gameInfo.enemyScale * 1.7;
                this.y = gameInfo.HEIGHT * 3 / 4;
            }
            else if (entrance == 3)
            {
                this.x = gameInfo.WIDTH + gameInfo.enemyScale * 1.7;
                this.y = gameInfo.HEIGHT * 3 / 4;
            }
            else if (entrance == 6)
            {
                this.x = -gameInfo.enemyScale;
                this.y = gameInfo.HEIGHT * 3 / 4 + gameInfo.playerScale;
            }
            else if (entrance == 7)
            {
                this.x = gameInfo.WIDTH + gameInfo.enemyScale;
                this.y = gameInfo.HEIGHT * 3 / 4 + gameInfo.playerScale;
            }
            else if (entrance == 8)
            {
                this.x = gameInfo.WIDTH * 2 / 3 - gameInfo.playerScale;
            }
            else if (entrance == 9)
            {
                this.x = gameInfo.WIDTH / 3 + gameInfo.playerScale;
            }
            else if (entrance == 12)
            {
                this.x = -gameInfo.enemyScale;
                this.y = gameInfo.HEIGHT - 3 * gameInfo.playerScale;
            }
            else if (entrance == 13)
            {
                this.x = gameInfo.WIDTH + gameInfo.enemyScale;
                this.y = gameInfo.HEIGHT - 3 * gameInfo.playerScale;
            }
        }


        //Function to be run when enemies first enter in order to path.
        protected void enter()
        {
            Tuple<double, double> dir;
            double varSpeed = speed;

            if (entrance == 0 || entrance == 4 || entrance == 8) // fly in from top right
            {
                if (time.TotalMilliseconds < 300)
                {
                    this.rotation = 240;
                }
                else if (time.TotalMilliseconds < 325)
                {
                    if (entrance == 4)
                    {
                        Shoot();
                    }
                }
                else if (time.TotalMilliseconds < 1000)
                {
                    //
                }
                else if (time.TotalMilliseconds < 1500)
                {
                    this.rotation += 7;
                    if (entrance == 8)
                    {
                        varSpeed = speed * 1.5;
                    }
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
            else if (entrance == 1 || entrance == 5 || entrance == 9) // fly in from top left
            {
                if (time.TotalMilliseconds < 400)
                {
                    this.rotation = 300;
                }
                else if (time.TotalMilliseconds < 425)
                {
                    if (entrance == 5)
                    {
                        Shoot();
                    }
                }
                else if (time.TotalMilliseconds < 1000)
                {
                    //
                }
                else if (time.TotalMilliseconds < 1500)
                {
                    this.rotation -= 7;
                    if (entrance == 9)
                    {
                        varSpeed = speed * 1.5;
                    }
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
            else if (entrance == 2 || entrance == 6) // fly in from bottom left
            {
                if (time.TotalMilliseconds < 500)
                {
                    this.rotation = 35;
                }
                else if (time.TotalMilliseconds < 1400)
                {
                    this.rotation += 7;
                    if (entrance == 6)
                    {
                        varSpeed = speed * 1.5;
                    }
                }
                else if (time.TotalMilliseconds < 1550)
                {
                    //
                }
                else
                {
                    inFormation = true;
                    rotation = 0;
                }
            }
            else if (entrance == 3 || entrance == 7) // fly in from bottom right
            {
                if (time.TotalMilliseconds < 500)
                {
                    this.rotation = 145;
                }
                else if (time.TotalMilliseconds < 1400)
                {
                    this.rotation -= 7;
                    if (entrance == 7)
                    {
                        varSpeed = speed * 1.5;
                    }
                }
                else if (time.TotalMilliseconds < 1550)
                {
                    //
                }
                else
                {
                    inFormation = true;
                    rotation = 0;
                }
            }
            //CHALLENGING WAVE
            else if (entrance == 10) // Fly in from top right
            {
                if (time.TotalMilliseconds < 400)
                {
                    this.rotation = 270;
                }
                else if (time.TotalMilliseconds < 1000)
                {
                    this.rotation -= 1;
                }
                else if (time.TotalMilliseconds < 1900)
                {
                    this.rotation -= 4;
                }
                else if (time.TotalMilliseconds < 3200)
                {
                    //
                }
                else
                {
                    dead = true;
                    inFormation = true;
                    rotation = 0;
                }
            }
            else if (entrance == 11) // Fly in from top right
            {
                if (time.TotalMilliseconds < 400)
                {
                    this.rotation = 270;
                }
                else if (time.TotalMilliseconds < 1000)
                {
                    this.rotation += 1;
                }
                else if (time.TotalMilliseconds < 1900)
                {
                    this.rotation += 4;
                }
                else if (time.TotalMilliseconds < 3200)
                {
                    //
                }
                else
                {
                    dead = true;
                    inFormation = true;
                    rotation = 0;
                }
            }
            else if (entrance == 12) // Fly in from bottom left and exit right
            {
                if (time.TotalMilliseconds < 400)
                {
                    this.rotation = 15;
                }
                else if (time.TotalMilliseconds < 1000)
                {
                    this.rotation += 1;
                }
                else if (time.TotalMilliseconds < 1300)
                {
                    this.rotation += 5;
                    if(rotation > 90)
                    {
                        rotation = 90;
                    }
                }
                else if (time.TotalMilliseconds < 1800)
                {
                    this.rotation += 13;
                    if (rotation > 270)
                    {
                        rotation = 270;
                    }
                }
                else if (time.TotalMilliseconds < 1950)
                {
                    this.rotation += 13;
                }
                else if (time.TotalMilliseconds < 3200)
                {
                    //
                }
                else
                {
                    dead = true;
                    inFormation = true;
                    rotation = 0;
                }
            }
            else if (entrance == 13) // Fly in from bottom right and exit left
            {
                if (time.TotalMilliseconds < 400)
                {
                    this.rotation = 165;
                }
                else if (time.TotalMilliseconds < 1000)
                {
                    this.rotation -= 1;
                }
                else if (time.TotalMilliseconds < 1300)
                {
                    this.rotation -= 5;
                    if (rotation < 90)
                    {
                        rotation = 90;
                    }
                }
                else if (time.TotalMilliseconds < 1800)
                {
                    this.rotation -= 13;
                    if (rotation < -90)
                    {
                        rotation = -90;
                    }
                }
                else if (time.TotalMilliseconds < 1950)
                {
                    this.rotation -= 13;
                }
                else if (time.TotalMilliseconds < 3200)
                {
                    //
                }
                else
                {
                    dead = true;
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
                else if (time.TotalMilliseconds < 475)
                {
                    //
                }
                else if (time.TotalMilliseconds < 500)
                {
                    Shoot();
                }
                else if (time.TotalMilliseconds < 875)
                {
                    //
                }
                else if (time.TotalMilliseconds < 900)
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
                else if (time.TotalMilliseconds < 475)
                {
                    //
                }
                else if (time.TotalMilliseconds < 500)
                {
                    Shoot();
                }
                else if (time.TotalMilliseconds < 1075)
                {
                    //
                }
                else if (time.TotalMilliseconds < 1100)
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
                this.x += (int)(dir.Item1 * varSpeed);
                this.y -= (int)(dir.Item2 * varSpeed);
            }
        }

        protected void handleBail(GameTime gameTime)
        {
            if (!inFormation)
            {
                return;
            }
            bailTime -= (int)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (bailTime <= 0)
            {
                speed /= 2;
                entrance = rand.Next(2) * -1 - 1;
                inFormation = false;
                time = new TimeSpan(0);
                bailTime = rand.Next(5000, 30000);
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
