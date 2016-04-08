using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

          
            //todo
          //  owner.seekOn = true;
        }


        public override int Process()
        {
            if (!isActive()) Activate();

            counter--;
            if (counter <= 0)
                status = (int)Status.completed;

            return status;
        }

        public override void Terminate()
        {
            //todo
          //  owner.seekOn = false;
        }


        public override void AddSubgoal(Goal g)
        {
            throw new NotImplementedException();
        }
    }
}
