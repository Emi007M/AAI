using System;

namespace Game.Goals
{
    class Goal_Gather : CompositeGoal
    {
        bool waterOn;
        bool stoneOn;

        public Goal_Gather(MovingEntity p) : base(p)
        {
        }


        public override void Activate()
        {
            RemoveAllSubgoals();
            status = (int)Status.active;
            Console.WriteLine("let's gather!");

            AddSubgoal(new Goal_Explore(owner));

            waterOn = false;
            stoneOn = false;

        }


        public override int Process()
        {

            if (!isActive()) Activate();

            if (owner.gw.collecting.waterAmount < owner.gw.collecting.capacityWater && owner.gw.collecting.isNearWater(owner) != null
                && owner.gw.collecting.isNearWater(owner).capacity != 0)
            {

                if (!waterOn)
                {
                    waterOn = true;
                    RemoveAllSubgoals();
                    AddSubgoal(new Goal_HarvestClosestWater(owner));
                }


            }

            else if (owner.gw.collecting.stoneAmount < owner.gw.collecting.capacityStone && owner.gw.collecting.isNearStone(owner) != null
                && owner.gw.collecting.isNearStone(owner).capacity != 0)
            {

                if (!stoneOn)
                {
                    RemoveAllSubgoals();
                    stoneOn = true;
                    AddSubgoal(new Goal_HarvestClosestStone(owner));
                }

            }

            status = (int)ProcessSubgoals();


            return status;
        }

        public override void Terminate()
        {
            RemoveAllSubgoals();
            Console.WriteLine("Gather terminated");
        }


        public override void AddSubgoal(Goal g)
        {
            Subgoals.AddFirst(g);
        }
    }
}

