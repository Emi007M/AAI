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
    class Stone : ObstacleEntity
    {
        static Random r = new Random();

        public Stone(GameWorld gw) : base(20,20, gw)
        {
            this.Draw();

        }
        public Stone(int x, int y, GameWorld gw) : base(x, y, gw)
        {

           this.Draw();

        }

        public override void Draw()
        {
            img_bitmap = new BitmapImage(new Uri("/Game;component/References/rock2.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = img_bitmap;
            // image.Width = img_bitmap.Width;
            // image.Height = img_bitmap.Height;
            img.Width = img.Height = r.Next(15,35);
           // float radiusRatio = 2f;
            base.r = (int)(img.Width/2);

            img.RenderTransform = new RotateTransform(r.Next(30) * 180 / Math.PI, base.r, base.r);


            Ellipse radius = new Ellipse();
            radius.Width = radius.Height = base.r * 2;
           // radius.RenderTransform = new TranslateTransform(-base.r+base.r/radiusRatio, -base.r +base.r /radiusRatio);
            radius.StrokeThickness = 0;
            radius.Stroke = Brushes.Red;

            //canvas containing image and radius
            image = new Canvas();
            image.Width = image.Height = base.r*2;
            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);

       
         

            image.Children.Add(img);
            image.Children.Add(radius);
           

            gw.canv.Children.Add(image);
        }
    }
}