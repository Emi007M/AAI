using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using Game.Grid;

namespace Game
{
    class GameWorld
    {
        public List<MovingEntity> soldiers = new List<MovingEntity>();
        public int sCapacity = 0;
        public List<ObstacleEntity> trees = new List<ObstacleEntity>();
        public FuzzyLogic fl;
        public Canvas canv;
        public MainWindow mainWindow;

        public int explorePhase;
        public Castle castle;

        public Game.Grid.Grid grid;

        public Collecting collecting;
        public bool leaveResources = false;

        public bool showThings = false;
      //  public bool showThingsFlag = false;

        public GameWorld(Canvas canv, MainWindow mainWindow)
        {
            this.canv = canv;
            this.mainWindow = mainWindow;

            castle = new Castle(this);
            trees.Add(castle);
            fl = new FuzzyLogic(this);


        }

        public void moveMan(int index, int x, int y)
        {
            soldiers.ElementAt(index).moveTo(x, y);
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
                x = r.Next(550);
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
                foreach (MovingEntity e in soldiers)
                    e.hideRadius();
                foreach (ObstacleEntity e in trees)
                    e.hideRadius();
                grid.hide();
            }
            else
            {
                showThings = true;
                foreach (MovingEntity e in soldiers)
                    e.showRadius();
                foreach (ObstacleEntity e in trees)
                    e.showRadius();
                grid.show();
            }
        }

        internal void findPath(double v1, double v2, double x1, double x2)
        {
            int a = grid.getVertex((int)v1, (int)v2);
            int b = grid.getVertex((int)x1, (int)x2);
            int[] p = grid.Paths.Astar(a, b);
            grid.Paths.drawPaths(p, v1, v2, x1, x2);

            
        }

       
    }
}
