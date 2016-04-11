using System;

namespace Game.Goals
{
    class Goal_TraverseEdge : Goal
    {

        private System.Windows.Vector target;
        private bool lastEdgeInPath;

        public Goal_TraverseEdge(MovingEntity p, System.Windows.Vector t, bool lastEdge) : base(p)
        {
            target = t;
            lastEdgeInPath = lastEdge;
        }


        public override void Activate()
        {
            status = (int)Status.active;

            if (!lastEdgeInPath)
            {
                owner.seekOn = true;
                owner.useSeek(target);
            }
            else
            {
                owner.seekOn = false;
                owner.arrivalOn = true;
                owner.useArrival(target);
            }
        }


        public override int Process()
        {
            if (!isActive()) Activate();

            if (owner.getX() > target.X - 15 && owner.getX() < target.X + 15
                       && owner.getY() > target.Y - 15 && owner.getY() < target.Y + 15)
                status = (int)Status.completed;

            return status;
        }

        public override void Terminate()
        {
            owner.seekOn = false;
            owner.arrivalOn = false;

        }


        public override void AddSubgoal(Goal g)
        {
            throw new NotImplementedException();
        }
    }
}
