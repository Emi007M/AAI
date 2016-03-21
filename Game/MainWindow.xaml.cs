using System;
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

        int isPath = -1;
        int sCapacity = 0;

        public MainWindow()
        {
            InitializeComponent();
            CanvInit();
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 20);


            gameInit();

            dispatcherTimer.Start();

        }

        void gameInit()
        {

            //gw = null;
            CanvMain.Children.Clear();
            gw = new GameWorld(CanvMain);

            addCursor();

            gw.addRandStones(5);
            gw.addRandPonds(2);
            gw.addRandTrees(55);
            gw.grid = new Grid(gw);

            gw.soldiers.Add(new Person(700, 320, 10, 15, gw));
            gw.soldiers.Add(new Person(720, 330, 10, 15, gw));
            gw.soldiers.Add(new Person(700, 310, 10, 15, gw));
            gw.soldiers.Add(new Person(730, 330, 10, 15, gw));

            foreach (MovingEntity m in gw.soldiers)
                m.useLeaderFollow(gw.soldiers.ElementAt(0));
            
            //  gw.moveMan(1,100,300);


            gw.collecting = new Collecting(gw);

            gw.soldiers.ElementAt(0).goal = new Goals.Goal_Think(gw.soldiers.ElementAt(0));

            isPath = -1;
        }

        private int timerCounter = 0;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            ////going by the path
            //if (isPath != -1)
            //{

            //    if (isPath == gw.grid.lastPath.Length / 2 - 1)
            //    {
            //        gw.soldiers.ElementAt(0).useArrival(new Vector(gw.grid.lastPath[0, isPath], gw.grid.lastPath[1, isPath]));
            //        isPath = -1;

            //    }
            //    else
            //    {
            //        gw.soldiers.ElementAt(0).useSeek(new Vector(gw.grid.lastPath[0, isPath], gw.grid.lastPath[1, isPath]));

            //        if ((int)gw.soldiers.ElementAt(0).getX() > gw.grid.lastPath[0, isPath] - 15 && (int)gw.soldiers.ElementAt(0).getX() < gw.grid.lastPath[0, isPath] + 15
            //            && (int)gw.soldiers.ElementAt(0).getY() > gw.grid.lastPath[1, isPath] - 15 && (int)gw.soldiers.ElementAt(0).getY() < gw.grid.lastPath[1, isPath] + 15)
            //            isPath++;
            //    }

                
            //}


            //-----resources-----
            timerCounter++;
            if(timerCounter==50||timerCounter==100||timerCounter==150||timerCounter==200)
            {
                Console.WriteLine("counter next");

                timerCounter = (timerCounter==200) ? 0 : timerCounter+1;

                if(gw.soldiers.Any())
                {

                    //--actions

                    //water
                    Pond p = gw.collecting.isNearWater(gw.soldiers.ElementAt(0));
                    if(p!=null)
                    {
                        Console.WriteLine("close to water");
                        //gather water
                        if (p.capacity > 0 && sCapacity < gw.collecting.capacity * gw.soldiers.Count())
                        {
                            p.capacity -= 1;
                            sCapacity++;
                            gw.collecting.waterAmount++;
                            if (timerCounter == 0) p.capacity--;
                        }

                    }
                    //todo

                }



                //respawning water
                if (timerCounter == 0)
                {

                    foreach(Pond p in gw.collecting.ponds)
                    { if (p.capacity < gw.collecting.maxPondsCapacity) p.capacity++; }
                }


                //updating labels
                foreach (Pond p in gw.collecting.ponds)
                {
                    Console.WriteLine(p.capacity);
                    Label txt = (Label)p.image.Children[2];
                    txt.Content = p.capacity + "/" + gw.collecting.maxPondsCapacity;
                }

                soldiers_capacity.Text = sCapacity + "/" + gw.collecting.capacity * gw.soldiers.Count();
            }






            gw.soldiers.ForEach(delegate (MovingEntity m) { m.update(); });


        }



        public void CanvInit()
        {
            Canvas.SetZIndex(Canv_bttns, 10);

        }

        public void addCursor()
        {
            //initialize target cursor
            target_img = new BitmapImage(new Uri("/Game;component/References/target.png", UriKind.RelativeOrAbsolute));

            target = new Image();
            target.Source = target_img;

            target.Width = 40;
            target.Height = 40;

            Canvas.SetLeft(target, Mouse.GetPosition(MainGrid).X - target.Width / 2);
            Canvas.SetTop(target, Mouse.GetPosition(MainGrid).Y - target.Height / 2);

            Canvas.SetZIndex(target, 5);
            CanvMain.Children.Add(target);

        }

       

        private void MainGrid_MouseMove(object sender, MouseEventArgs e)
        {


            Canvas.SetLeft(target, Mouse.GetPosition(MainGrid).X - target.Width / 2);
            Canvas.SetTop(target, Mouse.GetPosition(MainGrid).Y - target.Height / 2);

            consoleTxt.Text = Mouse.GetPosition(MainGrid).X + " " + Mouse.GetPosition(MainGrid).Y;


        }

   


        private void CanvMain_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            Console.WriteLine("click!");
            if (!gw.soldiers.Any()) return; //if no moving soldiers

            foreach (MovingEntity m in gw.soldiers) 
                m.useLeaderFollow(gw.soldiers.ElementAt(0));
            

            //// gw.soldiers.ElementAt(0).useArrival(new Vector(Mouse.GetPosition(MainGrid).X, (int)Mouse.GetPosition(MainGrid).Y));
            //gw.findPath(gw.soldiers.ElementAt(0).getX(), gw.soldiers.ElementAt(0).getY(), Mouse.GetPosition(MainGrid).X, Mouse.GetPosition(MainGrid).Y);

            //isPath = 1;

            gw.soldiers.ElementAt(0).goal.AddGoal_FollowPath(new Vector(Mouse.GetPosition(MainGrid).X, Mouse.GetPosition(MainGrid).Y));

            Console.WriteLine("clicked!");
        }


        //
        //--BUTTONS--

        private void btn_grid_Click(object sender, RoutedEventArgs e)
        {
            gw.changeThingsDisplay();
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            gameInit();
        }


        private void btn_upgrade_Click(object sender, RoutedEventArgs e)
        {
            gw.castle.upgrade();
        }

        private void btn_add_man_Click(object sender, RoutedEventArgs e)
        {
            Person p = new Person(700, 300, 5, 15, gw);
            gw.soldiers.Add(p);
            p.useLeaderFollow(gw.soldiers.ElementAt(0));
        }

        private void btn_remove_man_Click(object sender, RoutedEventArgs e)
        {
            if (!gw.soldiers.Any()) return; //if no moving soldiers

            CanvMain.Children.Remove(gw.soldiers.ElementAt(0).image);
            gw.soldiers.RemoveAt(0);
        }



        private void btn_water_Click(object sender, RoutedEventArgs e)
        {
            //look for the closest pond using dijkstra

            if (!gw.soldiers.Any()) return;

            double dist = 1000;

            Pond closestPond = null;
            foreach(Pond p in gw.collecting.ponds)
            {
                double d = MovingEntity.distance(gw.soldiers.ElementAt(0).location, p.location);
                if (d<dist && p.capacity>1)
                {
                    dist = d;
                    closestPond = p;
                }
             
            }

            if (closestPond==null) return;
            //gw.findPath(gw.soldiers.ElementAt(0).getX(), gw.soldiers.ElementAt(0).getY(), closestPond.location.X, closestPond.location.Y);
            //isPath = 1;
            gw.soldiers.ElementAt(0).goal.AddGoal_FollowPath(closestPond.location);

            foreach (MovingEntity m in gw.soldiers)
            {
                m.useLeaderFollow(gw.soldiers.ElementAt(0));
            }
        }
    }
}

