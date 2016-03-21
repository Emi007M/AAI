using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Goals
{
    class Seek : Goal
    {

        public Seek(MovingEntity p): base(p) { }


        public override void Activate()
        {
            status = (int)Status.active;


            owner.seekOn = true;
        }


        public override int Process()
        {
            if (!isActive()) Activate();

            return status;
        }

        public override void Terminate()
        {
            owner.seekOn = false;
        }


        public override void AddSubgoal(Goal g)
        {
            throw new NotImplementedException();
        }
    }
}
