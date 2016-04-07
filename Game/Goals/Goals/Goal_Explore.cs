using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Goals
{
    class Goal_Explore : Goal
    {
       

        public Goal_Explore(MovingEntity p) : base(p)
        {
          
        }


        public override void Activate()
        {
            status = (int)Status.active;


           
             owner.exploreOn = true;
        }


        public override int Process()
        {
            if (!isActive()) Activate();

         if(true) ///TTOOODOO
                status = (int)Status.completed;

            return status;
        }

        public override void Terminate()
        {
           
              owner.exploreOn = false;
        }


        public override void AddSubgoal(Goal g)
        {
            throw new NotImplementedException();
        }
    }
}
