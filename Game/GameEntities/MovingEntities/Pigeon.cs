using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Game
{
    class Pigeon : MovingEntity
    {
        public int life;

        public Pigeon(GameWorld gw) : base(0, 0, 5, 15, gw)
        {
            life = 300;
            seekOn = true;
            useSeek(new Vector(1000, 800));
            rotation = false;

            Draw();

        }


        public override void Draw()
        {
            BitmapImage bi = new BitmapImage(new Uri("/Game;component/References/pigeon.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = bi;

            img.Width = 60;
            img.Height = 60;


            image = new Canvas();
            image.Width = img.Width;
            image.Height = img.Height;
            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);

            image.Children.Add(img);

            gw.canv.Children.Add(image);

        }

        public void kill()
        {
            image.Visibility = Visibility.Hidden;
            gw.pigeon = null;
        }
    }
}
