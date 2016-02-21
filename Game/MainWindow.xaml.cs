﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Threading;


namespace Game
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BitmapImage target_img;
        Image target;

        BitmapImage btn_img;
        private Vector targetC = new Vector();
        GameWorld gw;

        private BackgroundWorker worker = null;
        public MainWindow()
        {
            InitializeComponent();
            CanvInit();
            addCursor();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);
            
           

            gw = new GameWorld(CanvMain);
            gw.entities.Add(new Person(20,20,15,10,gw));
            gw.entities.Add(new Person (100,300,5,15,gw));
          //  gw.moveMan(1,100,300);
            dispatcherTimer.Start();


        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            gw.entities.ForEach(delegate(MovingEntity m) { m.update(); });


        }



        public void CanvInit()
        {
            Canvas.SetZIndex(Canv_bttns, 10);

        }

        public void addCursor()
        {


            //initialize target cursor
            target_img = new BitmapImage(new Uri("/Game;component/References/target.png", UriKind.RelativeOrAbsolute));

            //target_img = new BitmapImage(new Uri("C:\\Users\\admin\\Source\\Repos\\AAI\\Game\\References\\target.png", UriKind.RelativeOrAbsolute));
            target = new Image();
            target.Source = target_img;
            //  target.Width = target_img.Width;
            // target.Height = target_img.Height;
            target.Width = 40;
            target.Height = 40;

            Canvas.SetLeft(target, Mouse.GetPosition(MainGrid).X - target.Width/2);
            Canvas.SetTop(target, Mouse.GetPosition(MainGrid).Y - target.Height/2);

            CanvMain.Children.Add(target);

        }

        private void Canvas_Initialized(object sender, EventArgs e)
        {

        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {

            Line l = new Line();
            l.Stroke = Brushes.Black;
            l.X1 = 100;
            l.Y1 = 100;
            l.X2 = 110;
            l.Y2 = 100;
            // this.AddChild(r);
            MainGrid.Children.Add(l);

            Ellipse s = new Ellipse();
            s.Stroke = Brushes.Blue;

            // Create a red Ellipse.
            Ellipse myEllipse = new Ellipse();

            // Create a SolidColorBrush with a red color to fill the 
            // Ellipse with.
            SolidColorBrush mySolidColorBrush = new SolidColorBrush();

            // Describes the brush's color using RGB values. 
            // Each value has a range of 0-255.
            mySolidColorBrush.Color = Color.FromArgb(255, 255, 255, 0);
            myEllipse.Fill = mySolidColorBrush;
            myEllipse.StrokeThickness = 2;
            myEllipse.Stroke = Brushes.Black;

            // Set the width and height of the Ellipse.
            myEllipse.Width = 200;
            myEllipse.Height = 100;

            MainGrid.Children.Add(myEllipse);
        }


        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {


            Canvas.SetLeft(target, Mouse.GetPosition(MainGrid).X - target.Width/2);
            Canvas.SetTop(target, Mouse.GetPosition(MainGrid).Y - target.Height/2);

            consoleTxt.Text = Mouse.GetPosition(MainGrid).X + " " + Mouse.GetPosition(MainGrid).Y;


        }

        private void btn_add_MouseEnter(object sender, MouseEventArgs e)
        {

        }

       private int count = 0;


        private void CanvMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            gw.entities.ElementAt(0).useArrival(new Vector(Mouse.GetPosition(MainGrid).X, (int)Mouse.GetPosition(MainGrid).Y));
            //gw.entities.ElementAt(1).useArrival(new Vector(Mouse.GetPosition(MainGrid).X, (int)Mouse.GetPosition(MainGrid).Y));
            gw.entities.ElementAt(1).useLeaderFollow(gw.entities.ElementAt(0));


        }
    }
}

