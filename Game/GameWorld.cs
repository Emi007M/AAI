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

        public Canvas canv;

        public GameWorld(Canvas canv)
        {
            this.canv = canv;
        }

        public void moveMan(int index, int x, int y)
        {
            entities.ElementAt(index).moveTo(x, y);
        }
    }
}
