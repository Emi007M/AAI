using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Goals
{
    class Goal_Think : CompositeGoal
    {
        

        public Goal_Think(MovingEntity p) : base(p)
        {
            
        }


        public override void Activate()
        {
            status = (int)Status.active;

          //  AddGoal_GoBackToBase();
           // AddGoal_FollowPath(new System.Windows.Vector(300, 500));
          //  AddGoal_FindClosestWater();
           //
        }


        public override int Process()
        {
            if (!isActive()) Activate();

            status = (int)ProcessSubgoals();
//
            return status;
        }

        public override void Terminate()
        {
            //
        }



        public void AddGoal_Explore()
        {
            //
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

        internal void AddGoal_FindClosestWater()
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


        internal void AddGoal_FindClosestStone()
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
