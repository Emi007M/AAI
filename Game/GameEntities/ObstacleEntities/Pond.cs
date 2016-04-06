using System;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Game
{
    class Pond : ObstacleEntity
    {
        

        static Random r = new Random();

        public Pond(GameWorld gw) : base(20,20, gw)
        {
            capacity = 20;
            this.Draw();

        }
        public Pond(int x, int y, GameWorld gw) : base(x, y, gw)
        {
            capacity = 20;
           this.Draw();

        }

        public override void Draw()
        {
            img_bitmap = new BitmapImage(new Uri("/Game;component/References/pond.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = img_bitmap;
        
            img.Width = img.Height = r.Next(55,95);    
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
            txt_cap.Foreground = Brushes.DarkBlue;
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