using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;
using System.Windows.Controls;

namespace Game
{
    class GameWorld
    {
        public List<GameEntity> entities = new List<GameEntity>();

        public Canvas canv;

        public GameWorld(Canvas canv)
        {
            this.canv = canv;
        }

        public void moveMan(int x, int y)
        {
            entities.First().moveTo(x, y);
        }
    }
}
