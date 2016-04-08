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

         if(owner.gw.collecting.Desired==Collecting.Resources.Stone && owner.gw.collecting.isNearStone(owner) !=null
                || owner.gw.collecting.Desired == Collecting.Resources.Water && owner.gw.collecting.isNearWater(owner) != null
                )
            {
                Console.WriteLine("CLOSEEEE");
                status = (int)Status.completed;
                Terminate();
            }

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
