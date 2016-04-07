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

        int[] WaterCapacity = { 10, 20, 50, 1000 };
        int[] StoneCapacity = { 20, 20, 50, 1000};

        public int WaterAmount = 0;
        public int StoneAmount = 0;

        public Castle(GameWorld gw) : base(770, 80, gw)
        {
            this.Draw();
            WaterAmount = StoneAmount = lvl = 0;
        }

        public int getWaterCapacity()
        {
            return WaterCapacity[lvl];
        }
        public int getStoneCapacity()
        {
            return StoneCapacity[lvl];
        }

        public override void Draw()
        {
            img_bitmap = new BitmapImage(new Uri("/Game;component/References/castle0.png", UriKind.RelativeOrAbsolute));
            Image img = new Image();
            img.Source = img_bitmap;
            img.Width = img.Height = 228*2;
            base.r = (int)228;


            Ellipse radius = new Ellipse();
            radius.Width = radius.Height = base.r * 2;
            radius.StrokeThickness = 0;
            radius.Stroke = Brushes.Blue;

            //canvas containing image and radius
            image = new Canvas();
            image.Width = image.Height = base.r*2;
            Canvas.SetLeft(image, location.X - image.Width / 2);
            Canvas.SetTop(image, location.Y - image.Height / 2);


            //capacity label
            Label txt_cap = new Label();
            txt_cap.Content = "";
            txt_cap.FontWeight = FontWeights.Black;
            txt_cap.FontSize = 16.0;
            txt_cap.Foreground = Brushes.Black;
            txt_cap.Background = Brushes.LightGray;
            Canvas.SetLeft(txt_cap, 40);
            Canvas.SetTop(txt_cap, 150);
            Canvas.SetZIndex(txt_cap, 0);




            image.Children.Add(img);
            image.Children.Add(radius);
            image.Children.Add(txt_cap);

            Canvas.SetZIndex(image, 2);

            gw.canv.Children.Add(image);
        }



        public Boolean canBeUpgraded()
        {
            return (WaterAmount >= getWaterCapacity() && StoneAmount >= getStoneCapacity());
        }

        public void upgrade()
        {
            Image c = (Image)image.Children[0];
            if (lvl < 3)
            {

                WaterAmount -= getWaterCapacity();
                StoneAmount -= getStoneCapacity();

                lvl++;
                c.Source = new BitmapImage(new Uri("/Game;component/References/castle"+lvl+".png", UriKind.RelativeOrAbsolute));


            }
        }
    }
}