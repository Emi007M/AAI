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
    class ObstacleEntity
    {
        protected GameWorld gw;

       
        protected BitmapImage img_bitmap;
        protected Canvas image;

        //--

        public Vector location;
       
       // protected float mass;
        public int r;
        // protected float maxforce;
        // protected float maxspeed;


        public ObstacleEntity(int x, int y, GameWorld gw)
        {
            this.gw = gw;
         //   mass = _mass;
          //  maxspeed = _maxspeed;

            location = new Vector(x, y);
           // velocity = new Vector(0, 0);
           // acceleration = new Vector(0, 0);
            //force = new Vector(0, 0);


        }

        public virtual void Draw()
        {
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


    }
}