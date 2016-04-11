using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game
{
    class Treee : ObstacleEntity
    {
        new static Random r = new Random();
        public bool cut = false;

        public Treee(GameWorld gw) : base(40, 40, gw)
        {
            Draw();

        }
        public Treee(int x, int y, GameWorld gw) : base(x, y, gw)
        {

            Draw();

        }

        public override void Draw()
        {
            img_bitmap = new BitmapImage(new Uri("/Game;component/References/tree.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = img_bitmap;
            img.Width = img.Height = r.Next(45, 65);
            base.r = (int)(img.Width / 2);

            img.RenderTransform = new RotateTransform(r.Next(30) * 180 / Math.PI, base.r, base.r);


            Ellipse radius = new Ellipse();
            radius.Width = radius.Height = base.r * 2;
            radius.StrokeThickness = 0;
            radius.Stroke = Brushes.Red;

            //canvas containing image and radius
            image = new Canvas();
            image.Width = image.Height = base.r * 2;
            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);
            image.Children.Add(img);
            image.Children.Add(radius);


            Canvas.SetZIndex(image, 3);

            gw.canv.Children.Add(image);
        }


    }
}