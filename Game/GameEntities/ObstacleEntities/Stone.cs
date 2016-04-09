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
            capacity = 0;
            this.Draw();

        }
        public Stone(int x, int y, GameWorld gw) : base(x, y, gw)
        {

            capacity = 0;
           this.Draw();

        }

        public override void Draw()
        {
            img_bitmap = new BitmapImage(new Uri("/Game;component/References/rock2.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = img_bitmap;
         
            img.Width = img.Height = r.Next(35,45);
            base.r = (int)(img.Width/2);

            img.RenderTransform = new RotateTransform(r.Next(30) * 180 / Math.PI, base.r, base.r);


            Ellipse radius = new Ellipse();
            radius.Width = radius.Height = base.r * 2;
           // radius.RenderTransform = new TranslateTransform(-base.r+base.r/radiusRatio, -base.r +base.r /radiusRatio);
            radius.StrokeThickness = 0;
            radius.Stroke = Brushes.Green;

            //--canvas containing image and radius
            image = new Canvas();
            image.Width = image.Height = base.r*2;
            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);

            //capacity label
            Label txt_cap = new Label();
            txt_cap.Content = "";
            txt_cap.FontWeight = FontWeights.Black;
            txt_cap.Foreground = Brushes.Black;
            Canvas.SetLeft(txt_cap, image.Width / 3);
            Canvas.SetTop(txt_cap, image.Height / 3);
            Canvas.SetZIndex(txt_cap, 0);


            image.Children.Add(img);
            image.Children.Add(radius);
            image.Children.Add(txt_cap);

            gw.canv.Children.Add(image);
        }
    }
}