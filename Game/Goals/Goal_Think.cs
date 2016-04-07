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
        double stopAtPond = 0;
        double goHome = 0;
        double stopAtStone = 0;
        double explore = 10;

        public Goal_Think(MovingEntity p) : base(p)
        {
            
        }


        public override void Activate()
        {
            status = (int)Status.active;

            stopAtPond = CalculateWater();
            stopAtStone = CalculateStone();
            //goHome = CalculateGoHome();

            switch (SelectMostDiserable())
            {
                case 0:
                    AddGoal_StopAtClosestWater();
                    break;
                case 1:
                    AddGoal_StopAtClosestStone();
                    break;
                case 2:
                    AddGoal_Explore();
                    break;
                case 3:
                    AddGoal_GoBackToBase();
                    break;

            }

        }

        public int CalculateWater()
        {
            //  Vector ownerC = new Vector(owner.location.X, owner.location.Y);
            foreach (Pond p in owner.gw.collecting.ponds)
            {
                // Vector p_center = new Vector(p.location.X, p.location.Y);
                double lenght = Math.Sqrt((Math.Pow(owner.getX() - p.location.X, 2)) + Math.Pow(owner.getY() - p.location.Y, 2));
                if (lenght < 2 * p.r)
                {
                    return 100 * owner.gw.soldiers.Count - 80 * owner.gw.collecting.waterAmount; //- x*owner.gw.castle.waterAmount
                }
            }
            return 0;
        }

        public int CalculateStone()
        {

            foreach (Stone s in owner.gw.collecting.stones)
            {
                // Vector p_center = new Vector(p.location.X, p.location.Y);
                double lenght = Math.Sqrt((Math.Pow(owner.getX() - s.location.X, 2)) + Math.Pow(owner.getY() - s.location.Y, 2));
                if (lenght < 2 * s.r)
                {
                    return 99 * owner.gw.soldiers.Count - 80 * owner.gw.collecting.stoneAmount; //- x*owner.gw.castle.waterAmount
                }
            }
            return 0;
        }
        public override int Process()
        {
            if (!isActive()) Activate();

            status = (int)ProcessSubgoals();
//
            return status;
        }

        public int SelectMostDiserable()
        {

            double[] desirability = { stopAtPond, stopAtStone, explore, goHome };

            return desirability.ToList().IndexOf(desirability.Max()); //returns 0 for pond,        1 for stone,      2 for explore,          3 for goHome


            
        }
        public override void Terminate()
        {
            //
        }



        public void AddGoal_Explore()
        {
            owner.gw.soldiers.ElementAt(0).useExplore();
        }

        public void AddGoal_FollowPath(System.Windows.Vector target)
        {
          //  owner.gw.findPath(owner.getX(), owner.getY(), target.X, target.Y);
            //int[,] path = owner.gw.grid.lastPath;
            AddSubgoal(new Goal_FollowPath(owner, target));
        }

        public void AddGoal_GoBackToBase()
        {
            //owner.gw.findPath(owner.getX(), owner.getY(), 750.0, 320.0);
            //int[,] path = owner.gw.grid.lastPath;
            AddSubgoal(new Goal_FollowPath(owner, new System.Windows.Vector(750.0,320.0)));
        }

        internal void AddGoal_StopAtClosestWater()
        {

            int start = owner.gw.grid.getVertex((int)owner.getX(), (int)owner.getY());
            IEnumerable<ObstacleEntity> targets = owner.gw.collecting.ponds;

            int target = owner.gw.grid.Paths.DijkstraClosest(start, targets);
            System.Windows.Vector t = new System.Windows.Vector(owner.gw.grid.getX(target), owner.gw.grid.getY(target));

            AddGoal_FollowPath(t);

            foreach (MovingEntity m in owner.gw.soldiers)
            {
                m.useLeaderFollow(owner);
            }
        }
    


        internal void AddGoal_StopAtClosestStone()
        {

            int start = owner.gw.grid.getVertex((int)owner.getX(), (int)owner.getY());
            IEnumerable<ObstacleEntity> targets = owner.gw.collecting.stones;

            int target = owner.gw.grid.Paths.DijkstraClosest(start, targets);
            System.Windows.Vector t = new System.Windows.Vector(owner.gw.grid.getX(target), owner.gw.grid.getY(target));

            AddGoal_FollowPath(t);

            foreach (MovingEntity m in owner.gw.soldiers)
            {
                m.useLeaderFollow(owner);
            }
        }


    }
}
