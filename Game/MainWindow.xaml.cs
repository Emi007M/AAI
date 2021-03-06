﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;



namespace Game
{

    public partial class MainWindow : Window
    {
        BitmapImage target_img;
        Image target;


        GameWorld gw;

        private BackgroundWorker worker;

        int isPath;




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

            CanvMain.Children.Clear();
            gw = new GameWorld(CanvMain, this);

            addCursor();

            gw.addRandStones(5);
            gw.addRandPonds(3);
            gw.addRandTrees(45);
            gw.grid = new Grid.Grid(gw);

            gw.soldiers.Add(new Person(700, 320, 10, 15, gw));
            gw.soldiers.Add(new Person(720, 330, 10, 15, gw));
            gw.soldiers.Add(new Person(700, 310, 10, 15, gw));
            gw.soldiers.Add(new Person(730, 330, 10, 15, gw));

             foreach (MovingEntity m in gw.soldiers)
                m.useLeaderFollow(gw.soldiers.ElementAt(0));


            gw.collecting = new Collecting(gw);

            gw.soldiers.ElementAt(0).goal = new Goals.Goal_Think(gw.soldiers.ElementAt(0));

            isPath = -1;

            checkBox_help.IsChecked = true;
        }

        private int timerCounter = 0;

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {



            //-----resources-----
            timerCounter++;
            if (timerCounter == 50 || timerCounter == 100 || timerCounter == 150 || timerCounter == 200)
            {
     
                timerCounter = (timerCounter == 200) ? 0 : timerCounter + 1;

                if (gw.soldiers.Any())
                {

                    //--actions

                    //water
                    Pond p = gw.collecting.isNearWater(gw.soldiers.ElementAt(0));
                    if (p != null)
                    {
                        Console.WriteLine("close to water");
                        //gather water
                        if (p.capacity > 0 && gw.collecting.waterAmount < gw.collecting.capacityWater)
                        {
                            p.capacity -= 1;
                           
                            gw.collecting.waterAmount++;
                            if (timerCounter == 0) p.capacity--;
                        }

                    }


                    //stone
                    Stone s = gw.collecting.isNearStone(gw.soldiers.ElementAt(0));
                    if (s != null)
                    {
                        Console.WriteLine("close to stone");
                        //gather stone
                        if (s.capacity > 0 && gw.collecting.stoneAmount < gw.collecting.capacityStone)
                        {
                            s.capacity -= 1;
                           
                            gw.collecting.stoneAmount++;
                            if (timerCounter == 0) s.capacity--;
                        }

                    }

                    if (gw.collecting.isNearCastle(gw.soldiers.ElementAt(0)) && gw.leaveResources)
                    {
                        gw.castle.StoneAmount += gw.collecting.stoneAmount;
                        gw.castle.WaterAmount += gw.collecting.waterAmount;

                        gw.collecting.stoneAmount = gw.collecting.waterAmount = 0;


                        if (gw.castle.canBeUpgraded())
                        {
                            //set upgrade btn enabled
                            System.Windows.Media.ImageBrush brush = new System.Windows.Media.ImageBrush();
                            brush.ImageSource = new BitmapImage(new Uri("../../References/buttons/btn5.png", UriKind.RelativeOrAbsolute));
                            btn_upgrade.Background = brush;
                            btn_upgrade.Cursor = Cursors.Hand;
                        }


                    }





                }


                //--Respawning resources

                //respawning water
                if (timerCounter == 0)
                {
                    foreach (Pond p in gw.collecting.ponds)
                    { if (p.capacity < gw.collecting.maxPondsCapacity) p.capacity++; }
                }
                //respawning stone
                if (timerCounter == 0)
                {
                    foreach (Stone s in gw.collecting.stones)
                    { if (s.capacity < gw.collecting.maxStonesCapacity) s.capacity++; }
                }


                //--Updating labels
                foreach (Pond p in gw.collecting.ponds)
                {
                    //         Console.WriteLine(p.capacity);
                    Label txt = (Label)p.image.Children[2];
                    txt.Content = p.capacity + "/" + gw.collecting.maxPondsCapacity;
                }
                foreach (Stone p in gw.collecting.stones)
                {
                    //       Console.WriteLine(p.capacity);
                    Label txt = (Label)p.image.Children[2];
                    txt.Content = p.capacity + "/" + gw.collecting.maxStonesCapacity;
                }
                {//castle
                    Label txt = (Label)gw.castle.image.Children[2];
                    txt.Content = "   Lvl " + gw.castle.lvl;
                    txt.Content += "\nwater:  " + gw.castle.WaterAmount + "/" + gw.castle.getWaterCapacity();
                    txt.Content += "\nstones: " + gw.castle.StoneAmount + "/" + gw.castle.getStoneCapacity();
                }

                water_capacity.Text = gw.collecting.waterAmount + "/" + gw.fl.Buckets;
                stone_capacity.Text = gw.collecting.stoneAmount + "/" + gw.fl.Bags;


            }


            //Goals debugging
            if (gw.soldiers.Any())
                goals_txt.Text = gw.soldiers.ElementAt(0).goal.getSubNames(0);



            gw.soldiers.ForEach(delegate (MovingEntity m) { m.update(); });

            if (gw.pigeon != null)
            {

                gw.pigeon.update();
                foreach (MovingEntity p in gw.soldiers)
                {
                    p.useFlee(gw.pigeon.location);
                }
                gw.pigeon.life--;
                if (gw.pigeon.life == 0)
                {
                    gw.pigeon.kill();
                }
            }

        }



        public void CanvInit()
        {
            Canvas.SetZIndex(Canv_bttns, 10);

            //set upgrade btn disabled
            System.Windows.Media.ImageBrush brush = new System.Windows.Media.ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("../../References/buttons/btn5_disabled.png", UriKind.RelativeOrAbsolute));
            btn_upgrade.Background = brush;
            btn_upgrade.Cursor = Cursors.No;
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

           if (!gw.soldiers.Any()) return; //if no moving soldiers

            foreach (MovingEntity m in gw.soldiers)
                m.useLeaderFollow(gw.soldiers.ElementAt(0));

     
            Console.WriteLine(gw.soldiers.ElementAt(0).goal.getSubNames(0));
            gw.soldiers.ElementAt(0).goal.RemoveAllSubgoals();
            gw.soldiers.ElementAt(0).goal.AddGoal_FollowPath(new Vector(Mouse.GetPosition(MainGrid).X, Mouse.GetPosition(MainGrid).Y));

        }


        //
        //--BUTTONS--

        private void btn_grid_Click(object sender, RoutedEventArgs e)
        {

            gw.changeThingsDisplay();
        }

        private void btn_reset_Click(object sender, RoutedEventArgs e)
        {
            gw.soldiers.ElementAt(0).goal.RemoveAllSubgoals();
            gameInit();
        }


        private void btn_upgrade_Click(object sender, RoutedEventArgs e)
        {
            if (!gw.castle.canBeUpgraded()) return;

            gw.castle.upgrade();

            System.Windows.Media.ImageBrush brush = new System.Windows.Media.ImageBrush();
            brush.ImageSource = new BitmapImage(new Uri("../../References/buttons/btn5_disabled.png", UriKind.RelativeOrAbsolute));
            btn_upgrade.Background = brush;
            btn_upgrade.Cursor = Cursors.No;
        }

        private void btn_add_man_Click(object sender, RoutedEventArgs e)
        {
            Person p = new Person(700, 300, 10, 15, gw);
            gw.soldiers.Add(p);
            if (gw.soldiers.Count == 1) p.goal = new Goals.Goal_Think(p);
            else p.useLeaderFollow(gw.soldiers.ElementAt(0));

            foreach (MovingEntity m in gw.soldiers)
            {
                m.useLeaderFollow(gw.soldiers.ElementAt(0));
            }
        }

        private void btn_remove_man_Click(object sender, RoutedEventArgs e)
        {
            if (!gw.soldiers.Any()) return; //if no moving soldiers

            CanvMain.Children.Remove(gw.soldiers.ElementAt(0).image);
            gw.soldiers.RemoveAt(0);
            if (!gw.soldiers.Any()) return;
            gw.soldiers.ElementAt(0).goal = new Goals.Goal_Think(gw.soldiers.ElementAt(0));


            foreach (MovingEntity m in gw.soldiers)
            {
                m.useLeaderFollow(gw.soldiers.ElementAt(0));
            }

        }



        private void btn_water_Click(object sender, RoutedEventArgs e)
        {
            //look for the closest pond using dijkstra

            if (!gw.soldiers.Any()) return;

            gw.soldiers.ElementAt(0).goal.RemoveAllSubgoals();
            gw.soldiers.ElementAt(0).goal.AddGoal_FindClosestWater();


            foreach (MovingEntity m in gw.soldiers)
            {
                m.useLeaderFollow(gw.soldiers.ElementAt(0));
            }
        }

        private void btn_stone_Click(object sender, RoutedEventArgs e)
        {
            //look for the closest pond using dijkstra

            if (!gw.soldiers.Any()) return;

            gw.soldiers.ElementAt(0).goal.RemoveAllSubgoals();

            gw.soldiers.ElementAt(0).goal.AddGoal_FindClosestStone();

            foreach (MovingEntity m in gw.soldiers)
            {
                m.useLeaderFollow(gw.soldiers.ElementAt(0));
            }
        }

        private void btn_return_Click(object sender, RoutedEventArgs e)
        {
            if (!gw.soldiers.Any()) return;
            gw.soldiers.ElementAt(0).goal.RemoveAllSubgoals();
            gw.soldiers.ElementAt(0).goal.AddGoal_GoBackToBase();
        }

        private void checkBox_fuzzy_Checked(object sender, RoutedEventArgs e)
        {
            fuzzy_txt.Visibility = Visibility.Visible;
        }

        private void checkBox_fuzzy_Unchecked(object sender, RoutedEventArgs e)
        {
            fuzzy_txt.Visibility = Visibility.Hidden;
        }

        private void checkBox_goals_Checked(object sender, RoutedEventArgs e)
        {
            goals_txt.Visibility = Visibility.Visible;
        }

        private void checkBox_goals_Unchecked(object sender, RoutedEventArgs e)
        {
            goals_txt.Visibility = Visibility.Hidden;
        }

        private void pigeon_label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            gw.AddPigeon();
        }

        private void checkBox_help_Checked(object sender, RoutedEventArgs e)
        {
            Help.Visibility = Visibility.Visible;
        }

        private void checkBox_help_Unchecked(object sender, RoutedEventArgs e)
        {
            Help.Visibility = Visibility.Hidden;
        }

    }
}

