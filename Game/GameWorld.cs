using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Game
{
    class GameWorld
    {
        public List<MovingEntity> entities = new List<MovingEntity>();
        public List<ObstacleEntity> trees = new List<ObstacleEntity>();

        public Canvas canv;

        public bool showThings = false;
      //  public bool showThingsFlag = false;

        public GameWorld(Canvas canv)
        {
            this.canv = canv;
        }

        public void moveMan(int index, int x, int y)
        {
            entities.ElementAt(index).moveTo(x, y);
        }

        public void addRandTrees(int i)
        {
            Random r = new Random();
            int x;
            int y;
            while (i-- > 0)
            {
                x = r.Next(580);
                y = r.Next(460);

                Console.WriteLine("Tree: " + x + " " + y);
                trees.Add(new Treee(x, y, this));
            }
           // trees.Add(t);
            //trees.Add(new Treee(150, 150, this));
            //entities.Add(new Person(160, 160, 10, 10, this));
        }

        public void changeThingsDisplay()
        {

           // showThings = showThings ? false : true;
            if (showThings)
            {
                showThings = false;
                foreach (MovingEntity e in entities)
                    e.hideRadius();
                foreach (ObstacleEntity e in trees)
                    e.hideRadius();
            }
            else
            {
                showThings = true;
                foreach (MovingEntity e in entities)
                    e.showRadius();
                foreach (ObstacleEntity e in trees)
                    e.showRadius();
            }
        }
    }
}
