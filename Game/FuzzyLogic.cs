using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    class FuzzyLogic
    {
        GameWorld gw;

        private int lvl;
        private int desiredWater, desiredStone;
        
        //fuzzy DOMs
        private double water_needed, water_not_needed;
        private double stone_needed, stone_not_needed;
        private double collect_water, collect_stone, collect_both;


        //crisp calculated values
        public int Buckets { get; private set; } //for water
        public int Barrows { get; private set; } //for stones

        private string info;
        


        public FuzzyLogic(GameWorld gw)
        {
            this.gw = gw;
            Buckets = Barrows = 0;

            lvl = gw.castle.lvl;
            UpdateData();
        }

        public void CalculateFuzzy()
        {
            info = System.String.Empty;

            if (lvl != gw.castle.lvl)
                UpdateData();

            //Fuzzification
            UpdateWater(gw.castle.WaterAmount);
            UpdateStone(gw.castle.StoneAmount);

           info +="Fuzzy data:\n";
           info += " Water:  needed=" + water_needed + ", not_needed=" + water_not_needed + "\n";
           info += " Stones: needed=" + stone_needed + ", not_needed=" + stone_not_needed +"\n";


            //Fuzzy Rules
            //IF stone_needed       AND water_needed        THEN collect_both
            //IF stone_not_needed   AND water_needed        THEN collect_water
            //IF stone_not_needed   AND water_not_needed    THEN collect_both
            //IF stone_needed       AND water_not_needed    THEN collect_stone
           
            collect_water = AND(stone_not_needed, water_needed);
            collect_stone = AND(water_not_needed, stone_needed);

            double tmp1 = AND(stone_needed, water_needed);
            double tmp2 = AND(stone_not_needed, water_not_needed);
            collect_both = OR(tmp1, tmp2);

            info += "Collect: water=" + collect_water + ", stones=" + collect_stone + ", both=" + collect_both + "\n";


            //Defuzzification
            CountMaxAv();

            info+="So take " + Buckets + " buckets and " + Barrows + " barrows.";

            Console.WriteLine(this);
        }

        private void UpdateData()
        {
            lvl = gw.castle.lvl;

            desiredWater = gw.castle.getWaterCapacity();
            desiredStone = gw.castle.getStoneCapacity();

        }


        private void UpdateWater(int x)
        {
            if (x > desiredWater)
            {
                water_needed = 0;
                water_not_needed = 1;
            }
            else
            {
                double a = 1 / desiredWater;

                water_needed = -a * x + 1;
                water_not_needed = a * x;
            }
        }
        private void UpdateStone(int x)
        {
            if (x > desiredStone)
            {
                stone_needed = 0;
                stone_not_needed = 1;
            }
            else
            {
                double a = 1 / desiredStone;

                stone_needed = -a * x + 1;
                stone_not_needed = a * x;
            }
        }

        private void CountMaxAv()
        {
            int maxTake = gw.sCapacity * gw.soldiers.Count();

            //Max

            //count water: y = -1/(maxTake/2) * x + 1 
            double max_w = (-1 / (maxTake / 2) * collect_water + 1) / 2;

            //count stones: y = 1/(maxTake/2) * x - 1
            double max_s = (1 / (maxTake / 2) * collect_stone - 1 + maxTake) / 2;

            //count both
            double max_b = maxTake / 2;

            info += "Max: water=" + max_w + ", stones=" + max_s + ", both=" + max_b + "\n";


            //MaxAv
            double MaxAv = (max_w * collect_water + max_s * collect_stone + max_b * collect_both) / (collect_water + collect_stone + collect_both);

            info += "MaxAv=" + MaxAv + "\n";

            //Defuzzification
            if (MaxAv < maxTake / 2) //first half
            {
                Buckets = (int)(-1 / (maxTake / 2) * MaxAv + 1) * maxTake;
                Barrows = 0;

                int both = (int)(1 / (maxTake / 2) * MaxAv) * maxTake;
                Buckets += both / 2;
                Barrows += both / 2;
            }
            else if(MaxAv > maxTake/2) //second half
            {
                Buckets = 0;
                Barrows = (int)(1 / (maxTake / 2) * MaxAv - 1) * maxTake;

                int both = (int)(-1 / (maxTake / 2) * MaxAv - 2) * maxTake;
                Buckets += both / 2;
                Barrows += both / 2;
            }
            else //exactly at half
            {
                Buckets = maxTake / 2;
                Barrows = maxTake / 2;
            }

        }

        private double AND(double x, double y) //return min
        {
            return x < y ? x : y;
        }
        private double OR(double x, double y) //return max
        {
            return x > y ? x : y;
        }


        override public string ToString()
        {
            return info;
        }
    }
}
