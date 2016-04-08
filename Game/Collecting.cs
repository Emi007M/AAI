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
        public int capacity; //capacity of a single unit

        public List<Pond> ponds;
        public int maxPondsCapacity = 20;
        public int waterAmount = 0; //how much units have right know

        public List<Stone> stones;
        public int maxStonesCapacity = 10;
        public int stoneAmount = 0; //how much units have right know

        public Resources Desired = Resources.Water;



        public Collecting(GameWorld gw)
        {
            this.gw = gw;
            capacity = 3;
            waterAmount = stoneAmount = 0;


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

        public bool isNearCastle(MovingEntity e)
        {
            int cX = 750, cY=320;
            int delta = 20;

            return (e.location.X > cX - delta && e.location.X < cX + delta
                 && e.location.Y > cY - delta && e.location.Y < cY + delta);
            
        }



    }
}
