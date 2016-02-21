using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    class GameEntity
    {
        protected GameWorld gw;

      //  protected int pos_x;
       // protected int pos_y;

        protected BitmapImage img_bitmap;
        protected Image image;

        //--
        protected Vector location;
        protected Vector velocity;
        protected Vector acceleration;

        protected float r;
        protected float maxforce;
        protected float maxspeed;

        //public Vector target;
        protected Vector force;



        public GameEntity(int x, int y, GameWorld gw)
        {
            this.gw = gw;

            //pos_x = x;
            //pos_y = y;
            location = new Vector(x, y);
            velocity = new Vector(0, 0);
            acceleration = new Vector(0, 0);
            force = new Vector(0, 0);

            maxspeed = 5;

        }


        void update()
        {
            //velocity += acceleration;
            // if (velocity > maxspeed) velocity = maxspeed;
            // location += velocity;
            // acceleration *= 0;

            //   Vector force = seek(target);
            //  acceleration. = force.divideBy(mass); // mass = 1.
            //  velocity = velocity.add(acceleration);
            acceleration = force /5;
           // velocity.X += acceleration.X; velocity.Y += acceleration.Y;
            velocity += acceleration;
            location.X += velocity.X; location.Y += velocity.Y;


            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);
            Console.WriteLine("Velocity:" + velocity);
           

        }

        void applyForce(Vector force)
        {
            acceleration+=force;
        }

        public void seek(Vector target)
        {
            //Vector desired = Vector.sub(target, location);
            //desired.normalize();
            //desired.mult(maxspeed);
            //PVector steer = PVector.sub(desired, velocity);
            //steer.limit(maxforce);
            //applyForce(steer);

            Vector direction = new Vector(target.X - location.X, target.Y-location.Y);
            Vector desiredVelocity = normalize(direction) * maxspeed;

            force = new Vector(desiredVelocity.X - velocity.X, desiredVelocity.Y - velocity.Y);
            Console.WriteLine(force);
            update();
            //Draw();

            if (velocity.X > 0.08 || velocity.X < -0.08 || velocity.Y > 0.08 || velocity.Y < -0.08) seek(target);
        }

        public virtual void Draw() 
        { 
        }

        Vector normalize(Vector v)
        {
            Double r = Math.Sqrt(v.X * v.X + v.Y * v.Y);
           // if(r!=0)
            return (new Vector(v.X / r, v.Y / r));
           // return (new Vector(0, 0));
        }

        public void moveTo(int x, int y)
        {
            //pos_x = x;
            //pos_y = y;
            location.X = x;
            location.Y = y;
            update();
        }
    }
}
