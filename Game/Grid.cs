using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MathNet.Numerics;

namespace Game
{
    class Grid
    {
        int width, height;
        int space;
        int offset_x, offset_y;
        int N;
        GameWorld gw;

        int[,] matrix;  //incidence matrix of all vertices and edges of a grid
        public Path vertices, edges;

        public Grid(GameWorld gw)
        {
            this.gw = gw;
            space = 100;

            width = 500/space;
            height = 500/space;
            offset_x = 20;
            offset_y = 20;

            N = width * height;

            gridInit();

            

           addObstacles();



          this.Draw();

        }

        int minDistance(int[] dist, bool[] sptSet)
        {
            // Initialize min value
            int min = Int32.MaxValue;
            int min_index = -1;
            

            for (int v = 0; v < N; v++)
                if (sptSet[v] == false && dist[v] <= min)
                {
                    min = dist[v];
                    min_index = v;
                }

            return min_index;
        }

        public int[] Dijkstra(int start, int end)
        {

            int[] dist = new int[N];
            bool[] sptSet = new bool[N];
            int[] path = new int[N];
 

            for (int i = 0; i < N; i++)
            {
                dist[i] = Int32.MaxValue;
                sptSet[i] = false;

            }
            dist[start] = 0;
            path[start] = 0;

            for (int i = 0; i < N; i++)
            {
                int u = minDistance(dist, sptSet);
                if (u == end)



                    return dist;
                sptSet[u] = true;

                for (int j = 0; j < N; j++)
                {
                    if (matrix[u, j] > 0 && !sptSet[j] && dist[u] != Int32.MaxValue && dist[u] + matrix[u, j] < dist[j])
                    {
                        dist[j] = dist[u] + matrix[u, j]; // +1
                        //path[j] = u;
                    }
                }
            }

            return dist;
        }

        public int FindClosest(double xe, double ye)
        {
             
       
            double distance = 1000;
            int closest = -1;
            

            for (int i = 0; i < N; i++)
                for (int j = 0; j < N; j++)
                {
                    double xp = getX(matrix[i, j]);
                    double yp = getY(matrix[i, j]);

                    double r = Math.Sqrt(((xe-xp)*(xe-xp) + (ye-yp)*(ye-yp)));

                    if (r < distance)
                    {
                        closest = matrix[i, j];
                    }
                }
            return closest;
        }
        private void addObstacles()
        {
          foreach(ObstacleEntity o in gw.trees)
            {
                int x = (int)(o.location.X + offset_x - o.location.X % space);
                int y = (int)(o.location.Y + offset_y - o.location.Y % space);
               // Console.WriteLine(o.location + " " +x + " " + y);
                int range = (o.r / space + 1)*space;


                   // Console.WriteLine("\n");
                int x_start = x-range, x_end = x+range, y_start = y-range, y_end = y+range;
                if (x_start < offset_x)                 x_start = offset_x;
                if (x_end > offset_x + width*space)     x_end = offset_x + width*space;
                if (y_start < offset_y)                 y_start = offset_y;
                if (y_end > offset_y + height * space)  y_end = offset_y + height * space;


                for (int i = x_start; i <= x_end; i+=space)
                {
                    for(int j = y_start; j<=y_end;j+=space)
                    {
                        //Console.Write("point:"+i + " " + j + "; ");
                        if (isObstacle(i, j, o))
                            removeVertex(getVertex(i, j));

                    }
                }

            }
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

                    // \
                    addEdge(i + j * width, i + 1 + width + j * width);

                    // /
                    addEdge(i + j * width+1, i  + width + j * width);

                }

            for (int i = 0; i < width - 1; i++)
                // --
                addEdge(width * (height - 1) + i, width * (height - 1) + i + 1);
            for (int j = 0; j < height - 1; j++)
                // --
                addEdge(j * width + width - 1, j * width + 2 * width - 1);
        }

        bool isObstacle(int x, int y, ObstacleEntity o)
        {
            return (x - o.location.X) * (x - o.location.X) + (y - o.location.Y) * (y - o.location.Y) <= o.r*o.r;

            //return false;
        }

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

            for(int i = 0; i< N;i++)
            {
                matrix[i, x] = -1;
                matrix[x, i] = -1;
            }
     
        }

        System.Windows.Point getCoords(int p)
        {
            int x = p % width;
            int y = (p - x) / width;

            x = x * space + offset_x;
            y = y * space + offset_y;

            return new System.Windows.Point(x,y);
        }

        double getX(int p)
        {
            int x = p%width;
            x = x * space + offset_x;
            return x;

        }
        double getY(int p)
        {
            int x = p % width;
            int y = (p - x) / width;
            y = y * space + offset_y;
            return y;

        }
        int getVertex(int x, int y)
        {
           // x -= offset_x;
           // y -= offset_y;
            x /= space;
            y /= space;


            return y * width + x;

        }

        public void Draw()
        {

            GeometryGroup g_vertices = new GeometryGroup();

      
            for (int i = 0; i < N; i++)
                {
                    if (matrix[i, 0] != -1) //if point exists
                        g_vertices.Children.Add(new EllipseGeometry(getCoords(i), 2.0, 2.0));
                }


            GeometryGroup g_edges = new GeometryGroup();

            for (int i = 0; i < N; i++)
                for (int j = i+1; j < N; j++)
                {
                    if (matrix[i,j] == 1) //if edge exists
                    {
                        g_edges.Children.Add(new LineGeometry(getCoords(i), getCoords(j)));
                    }
                }



            //  Canvas.SetZIndex(image, 3);


            vertices = new Path();
            vertices.Data = g_vertices;
            vertices.Fill = Brushes.Black;

            edges = new Path();
            edges.Data = g_edges;
            edges.Stroke = Brushes.Black;
            edges.StrokeThickness = 1;

            gw.canv.Children.Add(vertices);
            gw.canv.Children.Add(edges);

            this.hide();
        }

        public void hide()
        {
            vertices.Fill = null;
            edges.StrokeThickness = 0;
        }

        public void show()
        {
            vertices.Fill = Brushes.Black;
            edges.StrokeThickness = 1;
        }

    }
}
