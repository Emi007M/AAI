using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Goals
{
    class Goal_Return : CompositeGoal
    {
       


        public Goal_Return(MovingEntity p) : base(p)
        {
           
        }


        public override void Activate()
        {
            status = (int)Status.active;
            AddSubgoal(new Goal_GetBagBuckets(owner));
            AddSubgoal(new Goal_Wait(owner, 60));
            AddSubgoal(new Goal_FollowPath(owner, new System.Windows.Vector(750.0, 320.0)));

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
