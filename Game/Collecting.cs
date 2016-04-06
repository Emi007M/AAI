using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Collecting
    {
        //enum Resources{Water, Wood};

        GameWorld gw;
        public int capacity;

        public List<Pond> ponds;
        public int maxPondsCapacity = 20;
        public int waterAmount = 0;

        public List<Stone> stones;
        public int maxStonesCapacity = 10;
        public int stoneAmount = 0;

        public Collecting(GameWorld gw)
        {
            this.gw = gw;
            capacity = 3;



            ponds = gw.trees.OfType<Pond>().ToList();
            stones = gw.trees.OfType<Stone>().ToList();


        }

        public Pond isNearWater(MovingEntity e)
        {
            foreach (Pond p in ponds)
            {
                if (MovingEntity.distance(e.location, p.location) < 2 * p.r)
                {
                    return p;
                }
            }

            return null;

        }
        public Stone isNearStone(MovingEntity e)
        {
            foreach (Stone p in stones)
            {
                if (MovingEntity.distance(e.location, p.location) < 2 * p.r)
                {
                    return p;
                }
            }

            return null;

        }



    }
}
