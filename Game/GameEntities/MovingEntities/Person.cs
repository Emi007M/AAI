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

namespace Game
{
    class Person : MovingEntity
    {
        public Person(GameWorld gw) : base(40,40,15,5, gw)
        {

            this.Draw();

        }
        public Person(int x,int y, float _maxspeed,float _mass, GameWorld gw) : base(x, y,_maxspeed,_mass, gw)
        {
          
            this.Draw();

        }

        public override void Draw()
        {

            BitmapImage bi = new BitmapImage(new Uri("/Game;component/References/p1.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = bi;
          img.Width = 30;
          img.Height = 30;
    

          
            Ellipse radius = new Ellipse();
            radius.Width = r * 2;
            radius.Height = r * 2;
            radius.StrokeThickness = 0;
            radius.Stroke = Brushes.Black;

            image = new Canvas();
            image.Width = img.Width;
            image.Height = img.Height;
            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);

            image.Children.Add(img);
            image.Children.Add(radius);

            gw.canv.Children.Add(image);

            if (gw.showThings)
                this.showRadius();

        }
    }
}
