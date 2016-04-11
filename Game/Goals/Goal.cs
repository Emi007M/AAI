using System;

namespace Game.Goals
{
    abstract class Goal
    {
        public enum Status { inactive, active, completed, failed };

        protected MovingEntity owner;
        protected int status;

        public Goal(MovingEntity p)
        {
            owner = p;
            status = (int)Status.inactive;
        }


        public abstract void Activate();
        public abstract int Process();
        public abstract void Terminate();
        public abstract void AddSubgoal(Goal g);

        public String getName(int lvl)
        {

            string prefix = "";

            for (int i = 0; i < lvl; i++) prefix += " ";

            return prefix + this.GetType().Name;
        }


        public bool isActive()
        {
            return status == (int)Status.active;
        }
        public bool isInactive()
        {
            return status == (int)Status.inactive;
        }
        public bool isCompleted()
        {
            return status == (int)Status.completed;
        }
        public bool hasFailed()
        {
            return status == (int)Status.failed;
        }


    }
}
