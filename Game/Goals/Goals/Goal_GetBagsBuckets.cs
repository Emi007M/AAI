using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Goals
{
    class Goal_GetBagBuckets : Goal
    {


        public Goal_GetBagBuckets(MovingEntity p) : base(p)
        {

        }


        public override void Activate()
        {
            status = (int)Status.active;
            owner.gw.fl.CalculateFuzzy();
            owner.gw.collecting.capacityWater = owner.gw.fl.Buckets;
            owner.gw.collecting.capacityStone = owner.gw.fl.Bags;
            Console.WriteLine("fuzzy calculated and set!");
        }


        public override int Process()
        {
            if (!isActive()) Activate();
            //  Console.WriteLine("taking bags and buckets process");
            status = (int)Status.completed;
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
