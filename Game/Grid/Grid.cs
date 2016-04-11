using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Game.Grid
{
    class Grid
    {
        int width, height;
        internal int space;
        int offset_x, offset_y;
        internal int N;
        internal GameWorld gw;


        //structures
        internal int[,] matrix;  //incidence matrix of all vertices and edges of a grid
                                 // 0 - vertex, no edges, 1 - edge, -1 - no vertex



        public PathsOnGrid Paths;



        public Grid(GameWorld gw)
        {
            this.gw = gw;
            space = 30;

            width = 900 / space;
            height = 600 / space;
            offset_x = 20;
            offset_y = 20;

            N = width * height;

            gridInit();

            addObstacles();
            removeDisjointSets();


            Paths = new PathsOnGrid(this);

            Draw();

        }

        void gridInit()
        {
            matrix = new int[N, N];

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                    matrix[i, j] = 0;


            for (int i = 0; i < width - 1; i++)
                for (int j = 0; j < height - 1; j++)
                {
                    // --
                    addEdge(i + j * width, i + 1 + j * width);

                    // |
                    addEdge(i + j * width, i + width + j * width);

                }

            for (int i = 0; i < width - 1; i++)
                // --
                addEdge(width * (height - 1) + i, width * (height - 1) + i + 1);
            for (int j = 0; j < height - 1; j++)
                // --
                addEdge(j * width + width - 1, j * width + 2 * width - 1);
        }

        //methods for initialising grid
        public void addEdge(int x, int y)
        {
            matrix[x, y] = 1;
            matrix[y, x] = 1;
        }
        public void removeEdge(int x, int y)
        {
            matrix[x, y] = 0;
            matrix[y, x] = 0;
        }
        public void removeVertex(int x)
        {
            if (x > N) return;

            for (int i = 0; i < N; i++)
            {
                matrix[i, x] = -1;
                matrix[x, i] = -1;
            }

        }



        ///---methods for handling obstacles

        //removes occupied vertex from grid
        private void addObstacles()
        {
            foreach (ObstacleEntity o in gw.trees)
            {
                int obstacle_p = getVertex((int)o.location.X, (int)o.location.Y);
                int x = (int)getX(obstacle_p);
                int y = (int)getY(obstacle_p);

                int range = (o.r / space + 1) * space;

                int x_start = x - range, x_end = x + range, y_start = y - range, y_end = y + range;
                if (x_start < offset_x) x_start = offset_x;
                if (x_end > offset_x + width * space) x_end = offset_x + width * space;
                if (y_start < offset_y) y_start = offset_y;
                if (y_end > offset_y + height * space) y_end = offset_y + height * space;


                for (int i = x_start; i <= x_end; i += space)
                {
                    for (int j = y_start; j <= y_end; j += space)
                    {
                        if (isObstacle(i, j, o))
                            removeVertex(getVertex(i, j));

                    }
                }

            }
        }

        //returns whether point lays at given obstacle
        bool isObstacle(int x, int y, ObstacleEntity o)
        {
            if (x < offset_x || x > width * space + offset_x || y < offset_y || y > height * space + offset_y)
                return false;

            return (x - o.location.X) * (x - o.location.X) + (y - o.location.Y) * (y - o.location.Y) < o.r * o.r;
        }

        //removes not connected parts of grid
        private void removeDisjointSets()
        {
            int startP = getVertex(700, 320);

            int[] points = new int[N];

            for (int i = 0; i < N; i++)
                points[i] = 0;

            points[startP] = 1;

            bool foundNew;
            do
            {
                foundNew = false;

                for (int i = 0; i < N; i++)
                {
                    if (points[i] == 1) //if point belongs to big set and havent been visited before
                    {
                        points[i] += 1;
                        foreach (int n in getNeighbours(i))
                        {
                            if (points[n] == 0)
                            {
                                foundNew = true;
                                points[n] = 1;

                            }
                        }
                    }
                }

            } while (foundNew);

            //remove points with value 0 (unconnected)
            for (int i = 0; i < N; i++)
                if (points[i] == 0)
                    removeVertex(i);
        }



        //---auxiliary methods

        //returns real coordinates for point index
        internal System.Windows.Point getCoords(int p)
        {
            int x = p % width;
            int y = (p - x) / width;

            x = x * space + offset_x;
            y = y * space + offset_y;

            return new System.Windows.Point(x, y);
        }
        internal double getX(int p)
        {
            int x = p % width;
            x = x * space + offset_x;
            return x;

        }
        internal double getY(int p)
        {
            int x = p % width;
            int y = (p - x) / width;
            y = y * space + offset_y;
            return y;

        }

        //returns point index for real coordinates (use it before FindClosest)
        public int getVertex(int _x, int _y)
        {
            int x, y;
            x = _x - offset_x;
            y = _y - offset_y;
            x /= space;
            y /= space;

            x += offset_x;
            y += offset_y;

            int vert = y * width + x;

            if (vert >= 0 && vert < N && matrix[vert, vert] != -1)
                return vert;

            int ret = FindClosest(_x, _y);
            return ret;

        }

        //returns closest point index for real coordinates
        public int FindClosest(double xe, double ye)
        {


            double distance = 1000;
            int closest = -1;


            for (int i = 0; i < N; i++)
            {

                if (matrix[i, i] == -1) continue;
                double r = Math.Sqrt(((xe - getCoords(i).X) * (xe - getCoords(i).X) + (ye - getCoords(i).Y) * (ye - getCoords(i).Y)));

                if (r < distance)
                {
                    distance = r;
                    closest = i;
                }
            }
            return closest;
        }

        //returns list of indexes of directly connected points to the index point 
        internal List<int> getNeighbours(int vertex)
        {
            List<int> neighbours = new List<int>();

            for (int i = 0; i < N; i++)
                if (matrix[vertex, i] == 1)
                    neighbours.Add(i);


            return neighbours;
        }






        //
        ///methods showing debug for grid

        public void Draw()
        {
            //Grid vertices
            GeometryGroup g_vertices = new GeometryGroup();
            for (int i = 0; i < N; i++)
            {
                if (matrix[i, 0] != -1) //if point exists
                    g_vertices.Children.Add(new EllipseGeometry(getCoords(i), 1.0, 1.0));
            }

            Paths.vertices = new Path();
            Paths.vertices.Data = g_vertices;
            Paths.vertices.Fill = Brushes.Black;

            gw.canv.Children.Add(Paths.vertices);


            //Grid edges
            GeometryGroup g_edges = new GeometryGroup();
            for (int i = 0; i < N; i++)
                for (int j = i + 1; j < N; j++)
                {
                    if (matrix[i, j] == 1) //if edge exists
                    {
                        g_edges.Children.Add(new LineGeometry(getCoords(i), getCoords(j)));
                    }
                }

            Paths.edges = new Path();
            Paths.edges.Data = g_edges;
            Paths.edges.Stroke = Brushes.Black;
            Paths.edges.StrokeThickness = 1;

            gw.canv.Children.Add(Paths.edges);


            //paths

            Paths.path1 = new Path();
            Paths.path2 = new Path();
            Paths.path3 = new Path();


            hide();
        }

        public void hide()
        {
            Paths.vertices.Fill = null;
            Paths.edges.StrokeThickness = 0;
            Paths.path1.StrokeThickness = 0;
            Paths.path2.StrokeThickness = 0;
            Paths.path3.StrokeThickness = 0;
        }

        public void show()
        {
            Paths.vertices.Fill = Brushes.Black;
            Paths.edges.StrokeThickness = 1;

            //trying path
            //   GeometryGroup g_path1 = new GeometryGroup();
            Paths.path1 = new Path();
            Paths.path1.Stroke = Brushes.LightGreen;
            Paths.path1.StrokeThickness = 1;
            gw.canv.Children.Add(Paths.path1);

            //astar path
            GeometryGroup g_path2 = new GeometryGroup();
            Paths.path2 = new Path();
            Paths.path2.Data = g_path2;
            Paths.path2.Stroke = Brushes.BlueViolet;
            Paths.path2.StrokeThickness = 2;
            gw.canv.Children.Add(Paths.path2);

            //smooth path
            GeometryGroup g_path3 = new GeometryGroup();
            Paths.path3 = new Path();
            Paths.path3.Data = g_path3;
            Paths.path3.Stroke = Brushes.Purple;
            Paths.path3.StrokeThickness = 2;
            gw.canv.Children.Add(Paths.path3);


            if (Paths.drawPath)
            {
                Paths.path1.Data = Paths.g_path1;

                for (int i = 1; i < Paths.lastPath.Length / 2; i++)
                    g_path2.Children.Add(new LineGeometry(new System.Windows.Point(Paths.lastPath[0, i - 1], Paths.lastPath[1, i - 1]), new System.Windows.Point(Paths.lastPath[0, i], Paths.lastPath[1, i])));

                for (int i = 1; i < Paths.smoothedPath.Length / 2; i++)
                    g_path3.Children.Add(new LineGeometry(new System.Windows.Point(Paths.smoothedPath[0, i - 1], Paths.smoothedPath[1, i - 1]), new System.Windows.Point(Paths.smoothedPath[0, i], Paths.smoothedPath[1, i])));


            }

        }

    }
}
