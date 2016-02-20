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

        protected int pos_x;
        protected int pos_y;

        protected BitmapImage img_bitmap;
        protected Image image;

        //--
        protected Vector location;
        protected Vector velocity;
        protected Vector acceleration;

        protected float r;
        protected float maxforce;
        protected float maxspeed;



        public GameEntity(int x, int y, GameWorld gw)
        {
            this.gw = gw;

            pos_x = x;
            pos_y = y;
        }


        void update()
        {
            //velocity += acceleration;
           // if (velocity > maxspeed) velocity = maxspeed;
           // location += velocity;
           // acceleration *= 0;

            Canvas.SetLeft(image, pos_x - image.Width / 2);
            Canvas.SetTop(image, pos_y - image.Height / 2);

        }

        void applyForce(Vector force)
        {
            acceleration+=force;
        }

        void seek(Vector target)
        {
            //Vector desired = Vector.sub(target, location);
            //desired.normalize();
            //desired.mult(maxspeed);
            //PVector steer = PVector.sub(desired, velocity);
            //steer.limit(maxforce);
            //applyForce(steer);
        }

        public virtual void Draw() 
        { 
        }

        public void moveTo(int x, int y)
        {
            pos_x = x;
            pos_y = y;
            update();
        }
    }
}
