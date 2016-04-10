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
        public int Bags { get; private set; } //for stones

        private string info;
        


        public FuzzyLogic(GameWorld gw)
        {
            this.gw = gw;
            Buckets = Bags = 0;

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

            info += "Fuzzy logic for data: \nwater: " + gw.castle.WaterAmount + "/"+ desiredWater+", stones: " + gw.castle.StoneAmount + "/" + desiredStone + "\n";
          
           info += "\nWater:  needed=" + Math.Round(water_needed,3) + ", not_needed=" + Math.Round(water_not_needed,3) + "\n";
           info += "Stones: needed=" + Math.Round(stone_needed,3) + ", not_needed=" + Math.Round(stone_not_needed,3) +"\n";


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

            info += "Collect: water=" + Math.Round(collect_water,3) + ", stones=" + Math.Round(collect_stone,3) + ", both=" + Math.Round(collect_both,3) + "\n\n";


            //Defuzzification
            CountMaxAv();

            info+="So take " + Buckets + " buckets and " + Bags + " bags.";

            Console.WriteLine(this);
            gw.mainWindow.fuzzy_txt.Text = this.ToString();
                
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
                double a =(double) 1 / desiredWater;

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
                double a = (double) 1 / desiredStone;

                stone_needed = -a * x + 1;
                stone_not_needed = a * x;
            }
        }

        private void CountMaxAv()
        {
            int maxTake = gw.collecting.sCapacity * gw.soldiers.Count();

            //Max
            //x=(y-b)/a
            //count water: x = (y-1) / (-1/(maxTake/2)) 
            double max_w = (double)(collect_water - 1) / (-1.0 / ((double)maxTake / 2.0));
            //count stones: x = (y-(-1)) / (1/(maxTake/2))
            double max_s = (double)(collect_stone-(-1)) / (1.0 / ((double)maxTake / 2.0));
            //count both
            double max_b = (double)maxTake / 2;

            info += "Max: water=" + Math.Round(max_w,3) + ", stones=" + Math.Round(max_s,3) + ", both=" + Math.Round(max_b,3) + "\n";


            //MaxAv
            double MaxAv = (max_w * collect_water + max_s * collect_stone + max_b * collect_both) / (collect_water + collect_stone + collect_both);

            info += "MaxAv=" + Math.Round(MaxAv,3) + "\n\n";

            //Defuzzification
            if (MaxAv < maxTake / 2) //first half
            {
                Buckets = (int)((double)((double)-1 / (double)(maxTake / 2) * MaxAv + 1) * maxTake);
                Bags = 0;

                int both = (int)(double)(((double)1 / ((double)maxTake / 2) * MaxAv) * maxTake);
                info += "Crisp values: buckets=" + Buckets + ", bags=" + Bags + ", random=" + both+"\n";
                int x = new Random().Next(both);
                Buckets += x;
                Bags += both - x;
            }
            else if(MaxAv > maxTake/2) //second half
            {
                Buckets = 0;
                Bags = (int)(double)(((double)1 / ((double)maxTake / 2) * MaxAv - 1) * maxTake);

                int both = (int)(double)(((double)-1 / ((double)maxTake / 2) * MaxAv + 2) * maxTake);
                info += "Crisp values: buckets=" + Buckets + ", bags=" + Bags + ", random=" + both + "\n";
                int x = new Random().Next(both);
                Buckets += x;
                Bags += both - x;
            }
            else //exactly at half
            {
                info += "Crisp values: buckets=" + 0 + ", bags=" + 0 + ", random=" + maxTake + "\n";

                int x = new Random().Next(maxTake);
                Buckets = x;
                Bags = maxTake - x;
              
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
