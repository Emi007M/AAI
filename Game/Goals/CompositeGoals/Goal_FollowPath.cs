﻿namespace Game.Goals
{
    class Goal_FollowPath : CompositeGoal
    {
        private int[,] path = null;
        private System.Windows.Vector target;
        private int i;


        public Goal_FollowPath(MovingEntity p, System.Windows.Vector t) : base(p)
        {
            target = t;
            i = 0;
        }


        public override void Activate()
        {
            status = (int)Status.active;

            if (path == null)
            {
                owner.gw.findPath(owner.getX(), owner.getY(), target.X, target.Y);
                path = owner.gw.grid.Paths.smoothedPath;
            }

            System.Windows.Vector edge = new System.Windows.Vector(path[0, i], path[1, i]);

            bool isLastEdge = (i == path.Length / 2 - 1);

            AddSubgoal(new Goal_TraverseEdge(owner, edge, isLastEdge));


            i++;
        }


        public override int Process()
        {
            if (!isActive()) Activate();

            status = (int)ProcessSubgoals();

            if (status == (int)Status.completed && i != path.Length / 2)
                Activate();

            return status;
        }

        public override void Terminate()
        {
            path = null;
        }


        public override void AddSubgoal(Goal g)
        {
            Subgoals.AddFirst(g);
        }
    }
}
