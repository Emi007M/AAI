using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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


            //switch (owner.gw.explorePhase) {
            //    case 0:
            //        AddSubgoal(new Goal_FollowPath(owner, new Vector(560, 400))); owner.gw.explorePhase = 1;
            //        break;
            //}
            if (owner.gw.explorePhase == 0) { AddSubgoal(new Goal_FollowPath(owner, new Vector(560, 400))); owner.gw.explorePhase = 1; }
            if (owner.gw.explorePhase == 1) { AddSubgoal(new Goal_FollowPath(owner, new Vector(500, 260))); owner.gw.explorePhase = 2; }
            if (owner.gw.explorePhase == 2) { AddSubgoal(new Goal_FollowPath(owner, new Vector(440, 50))); owner.gw.explorePhase = 3; }
            if (owner.gw.explorePhase == 3) { AddSubgoal(new Goal_FollowPath(owner, new Vector(40, 30))); owner.gw.explorePhase = 4; }
            if (owner.gw.explorePhase == 4) { AddSubgoal(new Goal_FollowPath(owner, new Vector(230, 170))); owner.gw.explorePhase = 5; }
            if (owner.gw.explorePhase == 5) { AddSubgoal(new Goal_FollowPath(owner, new Vector(60, 260))); owner.gw.explorePhase = 6; }
            if (owner.gw.explorePhase == 6) { AddSubgoal(new Goal_FollowPath(owner, new Vector(440, 580))); owner.gw.explorePhase = 7; }
            if (owner.gw.explorePhase == 7) { AddSubgoal(new Goal_FollowPath(owner, new Vector(530, 560))); owner.gw.explorePhase = 8; }
            if (owner.gw.explorePhase == 8) { AddSubgoal(new Goal_FollowPath(owner, new Vector(860, 590))); owner.gw.explorePhase = 9; }
            if (owner.gw.explorePhase == 9) { AddSubgoal(new Goal_FollowPath(owner, new Vector(825, 375))); owner.gw.explorePhase = 0; }
            
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

            