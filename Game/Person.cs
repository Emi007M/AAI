using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    class Person : GameEntity
    {
        public Person(GameWorld gw) : base(40,40, gw)
        {

            this.Draw();

        }

        public override void Draw(){
            img_bitmap = new BitmapImage(new Uri("C:\\Users\\Emilia\\Desktop\\Dropbox\\materials\\6 semester - windesheim\\AI games\\Game\\Game\\References\\p1.jpg", UriKind.RelativeOrAbsolute));
            image = new Image();
            image.Source = img_bitmap;
            image.Width = img_bitmap.Width;
            image.Height = img_bitmap.Height;

            Canvas.SetLeft(image, pos_x - image.Width / 2);
            Canvas.SetTop(image, pos_y - image.Height / 2);

            gw.canv.Children.Add(image);
        }
    }
}
