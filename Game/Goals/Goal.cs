using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Goals
{
    abstract class Goal
    {
        public enum Status { inactive, active, completed, failed};

        protected MovingEntity owner;
        protected int status;
        protected int type;


        public Goal(MovingEntity p)
        {
            owner = p;
            status = (int)Status.inactive;
        }


        public abstract void Activate();
        public abstract int Process();
        public abstract void Terminate();
        public abstract void AddSubgoal(Goal g);

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

        new public int GetType()
        {
            return type;
        }



    }
}
