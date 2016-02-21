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
            img_bitmap = new BitmapImage(new Uri("/Game;component/References/p1.jpg", UriKind.RelativeOrAbsolute));

           // img_bitmap = new BitmapImage(new Uri("C:\\Users\\admin\\Source\\Repos\\AAI\\Game\\References\\p1.jpg", UriKind.RelativeOrAbsolute));
            image = new Image();
            image.Source = img_bitmap;
           // image.Width = img_bitmap.Width;
          // image.Height = img_bitmap.Height;
           image.Width = 30;
          image.Height = 30;


            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);

            gw.canv.Children.Add(image);
        }
    }
}
