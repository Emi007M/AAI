using System;

namespace Game.Goals
{
    class Goal_DontMove : Goal
    {

        public Goal_DontMove(MovingEntity p) : base(p)
        {
        }


        public override void Activate()
        {
            status = (int)Status.active;
            owner.dontMoveOn = true;
        }


        public override int Process()
        {
            if (!isActive()) Activate();
            return status;
        }

        public override void Terminate()
        {
            owner.dontMoveOn = false;
        }


        public override void AddSubgoal(Goal g)
        {
            throw new NotImplementedException();
        }
    }
}
