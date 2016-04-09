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
    class Treee : ObstacleEntity
    {
        new static Random r = new Random();
        public Boolean cut = false;

        public Treee(GameWorld gw) : base(40,40, gw)
        {
            this.Draw();

        }
        public Treee(int x, int y, GameWorld gw) : base(x, y, gw)
        {

           this.Draw();

        }

        public override void Draw()
        {
            img_bitmap = new BitmapImage(new Uri("/Game;component/References/tree.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = img_bitmap;
            // image.Width = img_bitmap.Width;
            // image.Height = img_bitmap.Height;
            img.Width = img.Height = r.Next(45,65);
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

         //   Button b = new Button();
            //b.Width = b.Height = image.Width;
         //   b.Cursor = Cursors.Hand;
         //   b.PreviewMouseUp += cutTree;
        //    b.Click += cutTree;
         //   b.PreviewMouseLeftButtonDown += cutTree;
            image.Children.Add(img);
            image.Children.Add(radius);

          //  image.Children.Add(b);

          //  b.MouseUp += cutTree;

        //    b.MouseEnter+= cutTree;

            Canvas.SetZIndex(image, 3);

            gw.canv.Children.Add(image);
        }


        //public void cutTree(object sender, RoutedEventArgs e)
        //{
        //    cut = true;

        //    Image img = (Image)image.Children[0];
        //    img.Source = new BitmapImage(new Uri("/Game;component/References/woody.png", UriKind.RelativeOrAbsolute));
        //    img.Width = img.Height = r.Next(10,20);
            

        //    base.r = 0;
        //    image.Children.RemoveAt(1);

        //    Console.WriteLine("drzewko");
        //}
    }
}