using System.Linq;
using System.Windows;

namespace Game.Goals
{
    class Goal_Explore : CompositeGoal
    {

        public Goal_Explore(MovingEntity p) : base(p)
        {
        }


        public override void Activate()
        {
            status = (int)Status.active;


            if (!owner.gw.grid.Paths.explorePath.Any()) owner.gw.grid.Paths.InitExplorePath();

            double dist = 5000;
            Vector closestTarget = new Vector();
            foreach (Vector v in owner.gw.grid.Paths.explorePath)
            {
                double d = MovingEntity.distance(owner.location, v);
                if (d < dist)
                {
                    dist = d;
                    closestTarget = v;
                }
            }

            AddSubgoal(new Goal_FollowPath(owner, closestTarget));
            owner.gw.grid.Paths.explorePath.Remove(closestTarget);

        }

        public override int Process()
        {
            if (!isActive()) Activate();

            status = (int)ProcessSubgoals();

            return status;
        }

        public override void Terminate()
        {

        }


        public override void AddSubgoal(Goal g)
        {
            Subgoals.AddFirst(g);
        }
    }
}

