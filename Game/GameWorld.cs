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

        public Castle castle;

        public Grid grid;

        public bool showThings = false;
      //  public bool showThingsFlag = false;

        public GameWorld(Canvas canv)
        {
            this.canv = canv;

            castle = new Castle(this);
            trees.Add(castle);
            

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
                x = r.Next(880);
                y = r.Next(560);

                //    Console.WriteLine("Tree: " + x + " " + y);

                if (!(x>550 && y<350))//not on castle area
                trees.Add(new Treee(x, y, this));
            }

        }

        public void addRandStones(int i)
        {
            Random r = new Random();
            int x;
            int y;
            while (i-- > 0)
            {
                x = r.Next(880);
                y = r.Next(560);

                //  Console.WriteLine("Tree: " + x + " " + y);
                trees.Add(new Stone(x, y, this));
            }
        }

        public void addRandPonds(int i)
        {
            Random r = new Random();
            r.Next();

            int x;
            int y;
            while (i-- > 0)
            {
                x = r.Next(550);
                y = r.Next(560);

                trees.Add(new Pond(x, y, this));
            }
        }


        public void changeThingsDisplay() //show/hide grids, radiuses
        {

            if (showThings)
            {
                showThings = false;
                foreach (MovingEntity e in entities)
                    e.hideRadius();
                foreach (ObstacleEntity e in trees)
                    e.hideRadius();
                grid.hide();
            }
            else
            {
                showThings = true;
                foreach (MovingEntity e in entities)
                    e.showRadius();
                foreach (ObstacleEntity e in trees)
                    e.showRadius();
                grid.show();
            }
        }
    }
}
