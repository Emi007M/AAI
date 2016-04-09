using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Goals
{
    class Goal_HarvestClosestWater : CompositeGoal
    {



        public Goal_HarvestClosestWater(MovingEntity p) : base(p)
        {

        }


        public override void Activate()
        {
            status = (int)Status.active;
          //  RemoveAllSubgoals();

            int start = owner.gw.grid.getVertex((int)owner.getX(), (int)owner.getY());
            IEnumerable<ObstacleEntity> targets = owner.gw.collecting.ponds;

            int target = owner.gw.grid.Paths.DijkstraClosest(start, targets);
            System.Windows.Vector t = new System.Windows.Vector(owner.gw.grid.getX(target), owner.gw.grid.getY(target));
            
            foreach (MovingEntity m in owner.gw.soldiers)
            {
                m.useLeaderFollow(owner);
            }

            //AddSubgoal(new Goal_Wait(owner,500));
            AddSubgoal(new Goal_DontMove(owner));
            AddSubgoal(new Goal_FollowPath(owner, t));
            AddSubgoal(new Goal_FollowPath(owner, t));

            //   AddSubgoal(new Goal_Wait(owner,20));

        }


        public override int Process()
        {
            if (!isActive()) Activate();

            if (owner.gw.collecting.waterAmount >= owner.gw.collecting.capacityWater ||( owner.gw.collecting.isNearWater(owner) != null && owner.gw.collecting.isNearWater(owner).capacity == 0))
            {
                RemoveAllSubgoals();
                

            }
            //status = Subgoals.First().Process();
            status = (int)ProcessSubgoals();


            return status;
        }

        public override void Terminate()
        {
            Console.WriteLine("harvest water complete");
            RemoveAllSubgoals();

        }


        public override void AddSubgoal(Goal g)
        {
            Subgoals.AddFirst(g);
        }
    }
}
