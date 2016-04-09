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
          //  Console.WriteLine("explore process");

           // if (owner.gw.collecting.waterAmount < owner.gw.collecting.capacity && owner.gw.collecting.isNearWater(owner) != null)
            //    if (owner.gw.collecting.isNearWater(owner) != null)
            //    {
            //    status = (int)Status.completed;

            //}

            //else if (owner.gw.collecting.isNearStone(owner) != null)
            //{
            //    status = (int)Status.completed;

            //}

            return status;
        }

        public override void Terminate()
        {
            
            Console.WriteLine("explore terminated");
            owner.exploreOn = false;
        }


        public override void AddSubgoal(Goal g)
        {
            throw new NotImplementedException();
        }
    }
}
