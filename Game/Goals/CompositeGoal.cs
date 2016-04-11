using System.Collections.Generic;
using System.Linq;

namespace Game.Goals
{
    abstract class CompositeGoal : Goal
    {
        protected LinkedList<Goal> Subgoals;

        public CompositeGoal(MovingEntity p) :  base(p)
        {
            Subgoals = new LinkedList<Goal>();
        }

        public override abstract void Activate();
        public override abstract int Process();
        public override abstract void Terminate();

        public override void AddSubgoal(Goal g)
        {
            Subgoals.AddFirst(g);
        }

        public string getSubNames(int lvl)
        {
            string prefix = "";

            for (int i = 0; i < lvl; i++) prefix += " ";
            string res = prefix+getName(0);

            foreach (Goal subg in Subgoals)
            {
                CompositeGoal Csubg = subg as CompositeGoal;
                if(Csubg!= null)
                {
                    res += "\n"+prefix + Csubg.getSubNames(lvl+1);
                }
                else
                {
                    res += "\n" + prefix + subg.getName(lvl + 1) + "*";
                }

            }
            return res;

            
        }

      
        public Status ProcessSubgoals()
        {

            //remove all completed and failed goals from the front of the subgoal list 
            while (Subgoals.Any() && (Subgoals.First().isCompleted() || Subgoals.First().hasFailed()))
            {
                Subgoals.First().Terminate();
                Subgoals.RemoveFirst();
            }
            //if any subgoals remain, process the one at the front of the list 
            if (Subgoals.Any())
            {
                //grab the status of the frontmost subgoal
                int StatusOfSubGoals = Subgoals.First().Process();
                //we have to test for the special case where the frontmost subgoal 
                //reports "completed" and the subgoal list contains additional goals. 
                //When this is the case, to ensure the parent keeps processing its 
                //subgoal list,the "active" status is returned 
                if (StatusOfSubGoals == (int)Status.completed && Subgoals.Count() > 1)
                    return Status.active; 

                return (Status)StatusOfSubGoals;

            }
            //no more subgoals to process - return "completed" 
            else 
                return Status.completed; 
            
        }

        public void RemoveAllSubgoals()
        {
            foreach(Goal g in Subgoals)
            {
                g.Terminate();
            }
            Subgoals.Clear();
        }
    }
}
