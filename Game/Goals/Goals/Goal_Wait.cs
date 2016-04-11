using System;

namespace Game.Goals
{
    class Goal_Wait : Goal
    {
        private int counter;

        public Goal_Wait(MovingEntity p, int i) : base(p)
        {
            counter = i;
        }


        public override void Activate()
        {
            status = (int)Status.active;

            owner.gw.leaveResources = true;

        }


        public override int Process()
        {
            if (!isActive()) Activate();

            counter--;
            if (counter <= 0)
            {
                owner.gw.leaveResources = false;
                status = (int)Status.completed;
            }

            return status;
        }

        public override void Terminate()
        {

            owner.gw.leaveResources = false;
        }


        public override void AddSubgoal(Goal g)
        {
            throw new NotImplementedException();
        }
    }
}
