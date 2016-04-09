using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class Collecting
    {
        public enum Resources{Water, Stone};

        GameWorld gw;
        public int capacityWater; 
        public int capacityStone;
        public int sCapacity = 3;

        public List<Pond> ponds;
        public int maxPondsCapacity = 5;
        public int waterAmount = 0; //how much units have right know

        public List<Stone> stones;
        public int maxStonesCapacity = 3;
        public int stoneAmount = 0; //how much units have right know

        public Resources Desired = Resources.Water;



        public Collecting(GameWorld gw)
        {
            this.gw = gw;

            capacityWater = 0;
            capacityStone = 0;
            waterAmount = stoneAmount = 0;


            ponds = gw.trees.OfType<Pond>().ToList();
            stones = gw.trees.OfType<Stone>().ToList();


        }

        public Pond isNearWater(MovingEntity e)
        {
            foreach (Pond p in ponds)
            {
                if (MovingEntity.distance(e.location, p.location) < p.r+e.r+30)
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
                if (MovingEntity.distance(e.location, p.location) < p.r + e.r + 30)
                {
                    return p;
                }
            }

            return null;

        }

        public bool isNearCastle(MovingEntity e)
        {
            int cX = 750, cY=320;
            int delta = 30;

            return (e.location.X > cX - delta && e.location.X < cX + delta
                 && e.location.Y > cY - delta && e.location.Y < cY + delta);
            
        }



    }
}
