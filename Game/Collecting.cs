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

        public Collecting(GameWorld gw)
        {
            this.gw = gw;
            capacity = 3;

           

            ponds = gw.trees.OfType<Pond>().ToList();


        }

        public Pond isNearWater(MovingEntity e)
        {
            foreach(Pond p in ponds)
            {
                if(MovingEntity.distance(e.location, p.location)< e.r + p.r)
                {
                    return p;
                }
            }

            return null;

        }



    }
}
