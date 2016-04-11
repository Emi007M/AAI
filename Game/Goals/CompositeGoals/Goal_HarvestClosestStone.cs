using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Goals
{
    class Goal_HarvestClosestStone : CompositeGoal
    {



        public Goal_HarvestClosestStone(MovingEntity p) : base(p)
        {
        }


        public override void Activate()
        {
            status = (int)Status.active;

            int start = owner.gw.grid.getVertex((int)owner.getX(), (int)owner.getY());
            IEnumerable<ObstacleEntity> targets = from s in owner.gw.collecting.stones where s.capacity > 0 select s;


            int target = owner.gw.grid.Paths.DijkstraClosest(start, targets);
            System.Windows.Vector t = new System.Windows.Vector(owner.gw.grid.getX(target), owner.gw.grid.getY(target));

            foreach (MovingEntity m in owner.gw.soldiers)
            {
                m.useLeaderFollow(owner);
            }

            AddSubgoal(new Goal_DontMove(owner));
            AddSubgoal(new Goal_FollowPath(owner, t));

        }


        public override int Process()
        {
            if (!isActive()) Activate();

            if (owner.gw.collecting.stoneAmount >= owner.gw.collecting.capacityStone || (owner.gw.collecting.isNearStone(owner) != null && owner.gw.collecting.isNearStone(owner).capacity == 0))
            {
                RemoveAllSubgoals();
            }

            status = (int)ProcessSubgoals();


            return status;
        }

        public override void Terminate()
        {
            Console.WriteLine("harvest stone complete");

            RemoveAllSubgoals();
        }


        public override void AddSubgoal(Goal g)
        {
            Subgoals.AddFirst(g);
        }
    }
}
