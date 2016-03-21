using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Game;

namespace Game
{
    class Castle : ObstacleEntity
    {
        //static Random r = new Random();
        public int lvl = 0;

        public Castle(GameWorld gw) : base(770, 80, gw)
        {
            this.Draw();

        }
        //public Castle(int x, int y, GameWorld gw) : base(x, y, gw)
        //{

        //   this.Draw();

        //}

        public override void Draw()
        {
            img_bitmap = new BitmapImage(new Uri("/Game;component/References/castle0.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = img_bitmap;
            // image.Width = img_bitmap.Width;
            // image.Height = img_bitmap.Height;
            img.Width = img.Height = 228*2;
           // float radiusRatio = 2f;
            base.r = (int)228;

           // img.RenderTransform = new RotateTransform(r.Next(30) * 180 / Math.PI, base.r, base.r);


            Ellipse radius = new Ellipse();
            radius.Width = radius.Height = base.r * 2;
           // radius.RenderTransform = new TranslateTransform(-base.r+base.r/radiusRatio, -base.r +base.r /radiusRatio);
            radius.StrokeThickness = 0;
            radius.Stroke = Brushes.Blue;

            //canvas containing image and radius
            image = new Canvas();
            image.Width = image.Height = base.r*2;
            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);

       
         

            image.Children.Add(img);
            image.Children.Add(radius);

            Canvas.SetZIndex(image, 2);

            gw.canv.Children.Add(image);
        }

        public void upgrade()
        {
            Image c = (Image)image.Children[0];
            if (lvl < 3)
            {
                lvl++;
                c.Source = new BitmapImage(new Uri("/Game;component/References/castle"+lvl+".png", UriKind.RelativeOrAbsolute));

            }
        }
    }
}