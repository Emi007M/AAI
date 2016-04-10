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
            if (owner.gw.collecting.capacityWater <= owner.gw.collecting.waterAmount)
            {
                if (owner.gw.collecting.capacityStone <= owner.gw.collecting.stoneAmount) return 50;
            }
            return 0;
        }
     

        //public int CalculateExplore()
        //{

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
    
        }



        public void AddGoal_Explore()
        {
            AddSubgoal(new Goal_Explore(owner));
        }

        public void AddGoal_FollowPath(System.Windows.Vector target)
        { 
            AddSubgoal(new Goal_FollowPath(owner, target));
        }

        public void AddGoal_GoBackToBase()
        {
            AddSubgoal(new Goal_Return(owner));
        }

        internal void AddGoal_FindClosestWater()
        {
            AddSubgoal(new Goal_HarvestClosestWater(owner));
        }

        internal void AddGoal_FindClosestStone()
        {
            AddSubgoal(new Goal_HarvestClosestStone(owner));
        }


    }
}
