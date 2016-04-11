using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game
{
    class ObstacleEntity
    {
        protected GameWorld gw;

        public int capacity = -1;

        protected BitmapImage img_bitmap;
        public Canvas image;

      
        public Vector location;
        public int r;


        public ObstacleEntity(int x, int y, GameWorld gw)
        {
            this.gw = gw;

            location = new Vector(x, y);


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