using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MathNet.Numerics.LinearAlgebra;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Game
{
    class MovingEntity
    {
        public GameWorld gw;
        public Goals.Goal_Think goal = null;

        //  protected int pos_x;
        // protected int pos_y;

        //  protected BitmapImage img_bitmap;
        //protected Image image;
        public Canvas image;

        //--

        public Vector location;
        protected Vector velocity;
        protected Vector acceleration;
        protected int explorePhase;

        protected float mass;
        public float r;
        //protected float maxforce;
        protected float maxspeed;

        public Boolean seekOn = false;
        public Boolean fleeOn = false;
        public Boolean arrivalOn = false;
        public Boolean leaderFollowOn = false;
        public Boolean exploreOn = false;
        public Boolean dontMoveOn = false;

        public Vector zeroVec = new Vector(0, 0);
        public Vector target;
        public Vector targetFlee;
        public MovingEntity leader;
        protected Vector force;
        protected List<Vector> collisionTargets = new List<Vector>();

        protected bool rotation = true;



        public MovingEntity(int x, int y, float _maxspeed, float _mass, GameWorld gw)
        {
            this.gw = gw;
            mass = _mass;
            maxspeed = _maxspeed;

            explorePhase = 0;
            location = new Vector(x, y);
            velocity = new Vector(0, 0);
            acceleration = new Vector(0, 0);
            force = new Vector(0, 0);

            r = 15;


        }

        public double getX()
        {
            return location.X;
        }

        public double getY()
        {
            return location.Y;
        }

        internal void showRadius()
        {
            Ellipse c = (Ellipse)image.Children[1];
            c.StrokeThickness = 1;
        }

        internal void hideRadius()
        {
            Ellipse c = (Ellipse)image.Children[1];
            c.StrokeThickness = 0;
        }

        public void update()
        {
            if (goal != null) goal.Process();

            force = zeroVec;
            if (seekOn) force += seek(target);
            if (fleeOn) force += 20 * flee(targetFlee);
            if (arrivalOn) force += 1.2 * arrival(target);
            if (leaderFollowOn) force += 1 * leaderFollow(leader);
            if (exploreOn) force += explore();


            force += 0.9 * collisionAvoid(gw.trees);
            force += 0.8 * Separation();



            acceleration = force / mass;
            velocity += acceleration;
            if (dontMoveOn) velocity = new Vector();

            if (length(velocity) > maxspeed)
                velocity = normalize(velocity) * maxspeed;

            location.X += velocity.X;
            location.Y += velocity.Y;



            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);
            //     Console.WriteLine("Velocity:" + velocity);


            if (rotation)
            {
                RotateTransform rotate = new RotateTransform(-Math.Atan2(velocity.X, velocity.Y) * 180 / Math.PI, r, r);
                image.RenderTransform = rotate;
            }

        }

        public void applyForce(Vector force)
        {
            acceleration += force;
        }


        public Vector leaderFollow(MovingEntity leader)
        {
            Vector direction = new Vector(leader.location.X - location.X, leader.location.Y - location.Y);
            Vector newDirection = direction - 35 * normalize(direction);

            Vector desiredVelocity = normalize(newDirection) * maxspeed;
            if (newDirection.Length < 10 * maxspeed)
            {
                desiredVelocity = newDirection / (10 * maxspeed + (mass / 2));
            }

            if ((direction.Length) < 50)
            {
                desiredVelocity = zeroVec;
            }
            if ((direction.Length) < 35)
            {
                desiredVelocity = -(normalize(direction) * maxspeed / 2);
            }
            Vector forceRes = new Vector(desiredVelocity.X - velocity.X, desiredVelocity.Y - velocity.Y);

            return forceRes;
        }

        public Vector arrival(Vector target)
        {
            Vector direction = new Vector(target.X - location.X, target.Y - location.Y);
            Vector desiredVelocity = normalize(direction) * maxspeed;
            if (direction.Length < 10 * velocity.Length + maxspeed)
            {
                desiredVelocity = direction / (10 * maxspeed + (mass / 2));
                //desiredVelocity = direction/10;
            }

            Vector forceRes = new Vector(desiredVelocity.X - velocity.X, desiredVelocity.Y - velocity.Y);

            return forceRes;
        }



        public Vector flee(Vector target)
        {


            Vector direction = new Vector(target.X - location.X, target.Y - location.Y);
            Vector desiredVelocity = normalize(direction) * maxspeed;

            if (direction.Length > 150) return new Vector();

            Vector forceRes = -new Vector(desiredVelocity.X - velocity.X, desiredVelocity.Y - velocity.Y);

            return forceRes;
        }

        public static double distance(Vector a, Vector b)
        {
            return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
        }

        public Vector collisionAvoid(List<ObstacleEntity> obs)
        {
            Vector v = new Vector();
            v = normalize(this.velocity);

            Vector this_center = new Vector(location.X, location.Y);

            Double dynamic_length = length(velocity) * 20 / maxspeed;


            Vector ahead = new Vector(this_center.X + v.X * dynamic_length, this_center.Y + v.Y * dynamic_length);
            Vector ahead2 = new Vector(this_center.X + v.X * dynamic_length / 2, this_center.Y + v.Y * dynamic_length / 2);



            Vector obstacle = default(Vector);

            foreach (ObstacleEntity o in gw.trees)
            {
                Vector o_center = new Vector(o.location.X, o.location.Y);

                // force += collisionAvoid(o);
                if (distance(o_center, ahead) <= o.r + r || distance(o_center, ahead2) <= o.r + r) //if exists collision
                {
                    if ((obstacle == null || distance(this_center, o_center) < distance(this_center, obstacle)))
                    {
                        obstacle = o_center;
                    }
                }
            }




            if (!obstacle.Equals(zeroVec))
            {
                Vector direction = new Vector(ahead.X - obstacle.X, ahead.Y - obstacle.Y);
                Vector desiredVelocity = normalize(direction) * maxspeed;
                return desiredVelocity;
            }
            return new Vector(0, 0);

            // Vector forceRes = new Vector(desiredVelocity.X - velocity.X, desiredVelocity.Y - velocity.Y);


        }

        public Vector Separation()
        {

            Vector this_center = new Vector(location.X, location.Y);

            Vector finalDir = new Vector();
            foreach (MovingEntity o in gw.soldiers)
            {
                Vector o_center = new Vector(o.location.X, o.location.Y);


                if (distance(o_center, this_center) <= 2 * r)
                {
                    Vector sepDir = new Vector(location.X - o.location.X, location.Y - o.location.Y);
                    sepDir = normalize(sepDir) * (2 * r) - sepDir;
                    finalDir += sepDir;

                }
            }
            Vector desiredVelocity = normalize(finalDir) * maxspeed / 2;

            return desiredVelocity;

        }


        //public void PathFindSeek(Vector target)
        //{
        //    int start = gw.grid.FindClosest(location.X, location.Y);
        //    int end = gw.grid.FindClosest(target.X, target.Y);
        //    //int[] path = gw.grid.Dijkstra(start, end);

        //  //  Vector nextV =  

        //}

        public Vector seek(Vector target)
        {


            Vector direction = new Vector(target.X - location.X, target.Y - location.Y);
            Vector desiredVelocity = normalize(direction) * maxspeed;

            Vector forceRes = new Vector(desiredVelocity.X - velocity.X, desiredVelocity.Y - velocity.Y);

            return forceRes;
        }


        public Vector explore()
        {
            Vector direction = new Vector();
            switch (explorePhase)
            {

                case 0:
                    direction = new Vector(825 - location.X, 375 - location.Y);
                    if (direction.Length < 50) explorePhase = 1;
                    break;
                case 1:
                    direction = new Vector(860 - location.X, 590 - location.Y);
                    if (direction.Length < 90) explorePhase = 2;
                    break;
                case 2:
                    direction = new Vector(530 - location.X, 560 - location.Y);
                    if (direction.Length < 90) explorePhase = 3;
                    break;
                case 3:
                    direction = new Vector(440 - location.X, 380 - location.Y);
                    if (direction.Length < 90) explorePhase = 4;
                    break;
                case 4:
                    direction = new Vector(60 - location.X, 580 - location.Y);
                    if (direction.Length < 90) explorePhase = 5;
                    break;
                case 5:
                    direction = new Vector(60 - location.X, 260 - location.Y);
                    if (direction.Length < 90) explorePhase = 6;
                    break;
                case 6:
                    direction = new Vector(230 - location.X, 170 - location.Y);
                    if (direction.Length < 90) explorePhase = 7;
                    break;
                case 7:
                    direction = new Vector(40 - location.X, 30 - location.Y);
                    if (direction.Length < 100) explorePhase = 8;
                    break;
                case 8:
                    direction = new Vector(440 - location.X, 50 - location.Y);
                    if (direction.Length < 50) explorePhase = 9;
                    break;
                case 9:
                    direction = new Vector(500 - location.X, 260 - location.Y);
                    if (direction.Length < 90) explorePhase = 10;
                    break;
                case 10:
                    direction = new Vector(560 - location.X, 400 - location.Y);
                    if (direction.Length < 90) explorePhase = 0;
                    break;

            }

            Vector desiredVelocity = normalize(direction) * maxspeed;
            if (direction.Length < 10 * velocity.Length + maxspeed)
            {
                desiredVelocity = direction / (10 * maxspeed + (mass / 2));
                //desiredVelocity = direction/10;
            }

            Vector forceRes = new Vector(desiredVelocity.X - velocity.X, desiredVelocity.Y - velocity.Y);

            return forceRes;
        }


        public virtual void Draw()
        {
        }

        Vector normalize(Vector v)
        {
            Double r = Math.Sqrt(v.X * v.X + v.Y * v.Y);
            if (r != 0)
                return (new Vector(v.X / r, v.Y / r));

            return (new Vector(0, 0));
        }
        Vector truncate(Vector v)
        {
            if (v.Length < maxspeed) return v;
            else
            {

                Double r = Math.Sqrt(v.X * v.X + v.Y * v.Y);
                return (new Vector(v.X / r, v.Y / r) * maxspeed);
            }

        }
        double length(Vector v)
        {
            return Math.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public void useSeek(Vector target)
        {
            this.target = target;
            this.seekOn = true;

        }
        public void useFlee(Vector targetFlee)
        {
            this.targetFlee = targetFlee;
            this.fleeOn = true;

        }
        public void useExplore()
        {
            this.exploreOn = true;

        }
        public void useDontMoveOn()
        {
            this.dontMoveOn = true;

        }
        public void useArrival(Vector target)
        {
            this.target = target;
            this.arrivalOn = true;

        }
        public void useLeaderFollow(MovingEntity leader)
        {
            this.leader = leader;
            this.leaderFollowOn = true;

        }

        public void moveTo(double x, double y)
        {
            //pos_x = x;
            //pos_y = y;
            location.X = x;
            location.Y = y;
            update();
        }
    }
}
