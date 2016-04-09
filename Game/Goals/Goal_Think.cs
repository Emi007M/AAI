using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using System.Windows;

namespace Game.Goals
{
    class Goal_Think : CompositeGoal
    {
        //desirabilities
       // int takeBuckets = 0;
        int goHome = 0;
      //  int takeBags = 0;
        int explore = 10;
        int stopAtPond = 0;
        int stopAtStone = 0;

        int gather = 0;

        public Goal_Think(MovingEntity p) : base(p)
        {

        }


        public override void Activate()
        {
            Console.WriteLine("im thinking");
            status = (int)Status.active;
            RemoveAllSubgoals();
           // explore = CalculateExplore();
            // takeBuckets = CalculateWater();
            //   takeBags = CalculateStone();
            //stopAtPond = CalculateWater();
            //stopAtStone = CalculateStone();
            gather = CalculateGather();
            goHome = CalculateGoHome();

            switch (SelectMostDiserable())
            {
                case 0:
                    AddGoal_Gather();
                    break;
                case 1:
                    AddGoal_GoBackToBase();
                    break;

            }

        }

        private void AddGoal_Gather()
        {
            AddSubgoal(new Goal_Gather(owner));
        }

        private int CalculateGather()
        {
            //

            return 20;
        }
        private int CalculateGoHome()
        {
            if (owner.gw.collecting.capacity * 2 <= owner.gw.collecting.stoneAmount + owner.gw.collecting.waterAmount) return 50;
            return 0;
        }
        //public int CalculateWater()
        //{
        //    //  Vector ownerC = new Vector(owner.location.X, owner.location.Y);
        //    foreach (Pond p in owner.gw.collecting.ponds)
        //    {
        //        // Vector p_center = new Vector(p.location.X, p.location.Y);
        //        double lenght = Math.Sqrt((Math.Pow(owner.getX() - p.location.X, 2)) + Math.Pow(owner.getY() - p.location.Y, 2));
        //        if (lenght < 2 * p.r)
        //        {
        //            return 100  - 80 * owner.gw.collecting.waterAmount; //- x*owner.gw.castle.waterAmount

        //        }
        //    }
        //    return 0;
        //}

        public int CalculateExplore()
        {

            return 0;
        }
        //public int CalculateStone()
        //{

        //    foreach (Stone s in owner.gw.collecting.stones)
        //    {
        //        // Vector p_center = new Vector(p.location.X, p.location.Y);
        //        double lenght = Math.Sqrt((Math.Pow(owner.getX() - s.location.X, 2)) + Math.Pow(owner.getY() - s.location.Y, 2));
        //        if (lenght < 2 * s.r)
        //        {
        //            return 99 * owner.gw.soldiers.Count - 80 * owner.gw.collecting.stoneAmount; //- x*owner.gw.castle.waterAmount
        //        }
        //    }
        //    return 0;
        //}
        public override int Process()
        {
            if (!isActive()) Activate();
            //Console.WriteLine("procssing think");
            status = (int)ProcessSubgoals();
            //
            return status;
        }

        public int SelectMostDiserable()
        {

            double[] desirability = { gather, goHome };

            return desirability.ToList().IndexOf(desirability.Max()); //returns 0 for gather,        1 for home,      
           



        }
        public override void Terminate()
        {
            RemoveAllSubgoals();
            
            //
        }



        public void AddGoal_Explore()
        {
            AddSubgoal(new Goal_Explore(owner));
        }

        public void AddGoal_FollowPath(System.Windows.Vector target)
        {
            //  owner.gw.findPath(owner.getX(), owner.getY(), target.X, target.Y);
            //int[,] path = owner.gw.grid.lastPath;
            AddSubgoal(new Goal_FollowPath(owner, target));
        }

        public void AddGoal_GoBackToBase()
        {
            AddSubgoal(new Goal_Return(owner));

        }

        internal void AddGoal_FindClosestWater()
        {

            //int start = owner.gw.grid.getVertex((int)owner.getX(), (int)owner.getY());
            //IEnumerable<ObstacleEntity> targets = owner.gw.collecting.ponds;

            //int target = owner.gw.grid.Paths.DijkstraClosest(start, targets);
            //System.Windows.Vector t = new System.Windows.Vector(owner.gw.grid.getX(target), owner.gw.grid.getY(target));

            //AddGoal_FollowPath(t);

            //foreach (MovingEntity m in owner.gw.soldiers)
            //{
            //    m.useLeaderFollow(owner);
            //}
        

            AddSubgoal(new Goal_HarvestClosestWater(owner));
        }


        internal void AddGoal_FindClosestStone()
        {

            //int start = owner.gw.grid.getVertex((int)owner.getX(), (int)owner.getY());
            //IEnumerable<ObstacleEntity> targets = owner.gw.collecting.stones;

            //int target = owner.gw.grid.Paths.DijkstraClosest(start, targets);
            //System.Windows.Vector t = new System.Windows.Vector(owner.gw.grid.getX(target), owner.gw.grid.getY(target));

            //AddGoal_FollowPath(t);

            //foreach (MovingEntity m in owner.gw.soldiers)
            //{
            //    m.useLeaderFollow(owner);
            //}
            AddSubgoal(new Goal_HarvestClosestStone(owner));
        }


    }
}
