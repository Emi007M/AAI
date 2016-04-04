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
        public Path vertices, edges, path;
        public int[,] lastPath;
        bool drawPath = false;

        public Grid(GameWorld gw)
        {
            this.gw = gw;
            space = 30;

            width = 900/space;
            height = 600/space;
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

        public int[] Dijkstra(int start, int end) //takes starting and ending points
        {

            int[] dist = new int[N];
            bool[] sptSet = new bool[N];
            // int[] path = new int[N];
            LinkedList<int> path = new LinkedList<int>();

            for (int i = 0; i < N; i++)
            {
                dist[i] = Int32.MaxValue;
                sptSet[i] = false;

            }
            dist[start] = 0;
            //path[start] = 0;

            for (int i = 0; i < N; i++)
            {
                int u = minDistance(dist, sptSet);
                if (u == end)


                    break;
                   // return dist;
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
            int vert = end;
            path.AddFirst(end);
            for(int i = dist[end]-1; i>=0; i--)
            {
                foreach(int neighbour in getNeighbours(vert))
                {
                    if (dist[neighbour] == i)
                    {
                        path.AddFirst(neighbour);
                        vert = neighbour;
                        break;
                    }
                }
            }
            int[] ret = path.ToArray<int>();
           //drawPaths(ret);

            return ret ;
        }

        int getHeuristic(int j, int end)
        {
            int endX = end % width;
            int endY = (endX - end) / width;

            int jX = j % width;
            int jY = (jX - j) / width;

            return Math.Abs(endX - jX) + Math.Abs(endY - jY);
            
        }

        int AminDistance(int[] dist, bool[] sptSet, int[] score)
        {
            // Initialize min value
            double min = Int32.MaxValue;
            int min_index = -1;


            for (int v = 0; v < N; v++)

                if (sptSet[v] == false && dist[v]+score[v] <= min)
                {
                    min = dist[v]+score[v];
                    min_index = v;
                }

            return min_index;
        }
        public int[] Astar(int start, int end) //takes starting and ending points
        {
  
            int[] dist = new int[N];
            int[] score = new int[N];
            bool[] sptSet = new bool[N];
            // int[] path = new int[N];
            LinkedList<int> path = new LinkedList<int>();

            for (int i = 0; i < N; i++)
            {
                dist[i] = Int32.MaxValue/2;
                score[i] = Int32.MaxValue/2;
                sptSet[i] = false;

            }
            dist[start] = 0;
            score[start] = getHeuristic(start, end);
            //path[start] = 0;

            for (int i = 0; i < N; i++)
            {
                int u = AminDistance(dist, sptSet,score);
                if (u == end)


                    break;
                // return dist;
                sptSet[u] = true;

                for (int j = 0; j < N; j++)
                {
                    if (matrix[u, j] > 0 && !sptSet[j] && dist[u] != Int32.MaxValue && dist[u] + matrix[u, j] < dist[j])
                    {
                        dist[j] = dist[u] + matrix[u, j]; // +1
                        score[j] = getHeuristic(j, end);
                        //path[j] = u;
                    }
                }
            }
            int vert = end;
            path.AddFirst(end);
            for (int i = dist[end] - 1; i >= 0; i--)
            {
                foreach (int neighbour in getNeighbours(vert))
                {
                    if (dist[neighbour] == i)
                    {
                        path.AddFirst(neighbour);
                        vert = neighbour;
                        break;
                    }
                }
            }
            int[] ret = path.ToArray<int>();
            //drawPaths(ret);

            return ret;
        }
        internal void drawPaths(int[] path, double v1, double v2, double x1, double x2)
        {
            drawPath = true;
            lastPath = new int[2, path.Length+2];

            lastPath[0, 0] = (int)v1;
            lastPath[1, 0] = (int)v2;
            

            for (int i = 1; i <= path.Length; i++)
            {
                lastPath[0, i] = (int)getCoords(path[i-1]).X;
                lastPath[1, i] = (int)getCoords(path[i-1]).Y;
            }

            lastPath[0, lastPath.Length/2 - 1] = (int)x1;
            lastPath[1, lastPath.Length/2 - 1] = (int)x2;


            pathSmoothing();

            if (this.path.StrokeThickness == 2)
            {
                hide(); show();
            }

            Console.WriteLine(lastPath);
        }


        //public void drawPaths(int[] path)
        //{
        //    drawPath = true;
        //    lastPath = new int[2, path.Length];
        //    for (int i = 0; i < path.Length; i++)
        //    {
        //        lastPath[0, i] = (int)getCoords(path[i]).X;
        //        lastPath[1, i] = (int)getCoords(path[i]).Y;


        //    }

        //    pathSmoothing();

        //    if (this.path.StrokeThickness == 2)
        //    {
        //        hide(); show();
        //    }
        //}

        private void pathSmoothing()
        {
            
            int e1 = 0, e2=0;

            while(e1!=lastPath.Length/2-1)
            {
                e2 = e1 + 1;
                while(e2!=lastPath.Length/2)
                {
                    if (canWalkBetween(e1, e2))
                    {
                        removeFromPath(e1,e2);
                        e2 = e1 + 2;
                    }
                    else
                    {
                        e2++;
                    }

                }
                e1++;
            }
    }

        private bool canWalkBetween(int e1, int e2)
        {

            double x1 = lastPath[0, e1], y1 = lastPath[1, e1],
                x2 = lastPath[0, e2], y2 = lastPath[1, e2];

            if (x1 - x2 != 0)//if line is not vertical
            {

            //find y=ax+b
            double a =(double) ((y1 - y2) / (x1 - x2));
            double b = y1 - a * x1;

            // y=-1/a*x+b consisting center of obstacle
            double a2 = -1 / a, b2;
            System.Windows.Vector P;
            foreach(ObstacleEntity o in gw.trees)
            {
                b2 = o.location.Y - a2 * o.location.X;
                P = new System.Windows.Vector((b - b2) / (a2 - a), a * (b - b2) / (a2 - a) + b);


                   // Console.WriteLine(x1 + "," + y1 + " " + x2 + "," + y2 + " obs=" + o.location + ",r=" + o.r+", inter="+P.X+","+P.Y+ " a="+a+" b="+b);

                    if (MovingEntity.distance(P,o.location)<=(double)o.r*2)
                {
                    //check if point of crossection of 2 lines belongs to the edge
                    if((o.location.X>=x1||o.location.X>=x2) && (o.location.X <= x1 || o.location.X <= x2) &&
                       (o.location.Y >= y1 || o.location.Y >= y2) && (o.location.Y <= y1 || o.location.Y <= y2))
                    {

                        return false;
                    }
                }
            }

            }
            else //vertical straight
            {
                
                double b =  x1;
 
                System.Windows.Vector P;
                foreach (ObstacleEntity o in gw.trees)
                {
                   
                    P = new System.Windows.Vector(b,o.location.Y);

                    if (MovingEntity.distance(P, o.location) <= (double)o.r*2)
                    {
                        //check if point of crossection of 2 lines belongs to the edge
                        if ((o.location.Y >= y1 || o.location.Y >= y2) && (o.location.Y <= y1 || o.location.Y <= y2))
                        {
                            return false;
                        }
                    }
                }
            }


            return true;
        }  

        void removeFromPath(int from, int to)
        {
            int[,] ret = new int[2, lastPath.Length/2 - (to-from-1)];
            int j = 0;

            for(int i=0;i<ret.Length/2;i++)
            {
                if (j == from+1) j = to;

                ret[0, i] = lastPath[0, j];
                ret[1, i] = lastPath[1, j];
                j++;
            }
            lastPath = ret;
        }



        //
        public int FindClosest(double xe, double ye)
        {
             
       
            double distance = 1000;
            int closest = -1;


            for (int i = 0; i < N; i++)
            {

                // double xp = getX(matrix[i, j]);
                // double yp = getY(matrix[i, j]);
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

        List<int> getNeighbours(int vertex)
        {
            List<int> neighbours = new List<int>();

            for (int i = 0; i < N; i++)
                if (matrix[vertex, i] == 1)
                    neighbours.Add(i);


            return neighbours;
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
                 //   addEdge(i + j * width, i + 1 + width + j * width);

                    // /
                //    addEdge(i + j * width+1, i  + width + j * width);

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
        public int getVertex(int _x, int _y)
        {
           // x -= offset_x;
           // y -= offset_y;
            int x = _x / space;
            int y = _y / space;

            int vert = y * width + x;
     
            if(vert>=0 && vert<N && matrix[vert,vert]!= -1)
                return vert;

            int ret = FindClosest(_x, _y);
            return ret;

        }

        public void Draw()
        {

            GeometryGroup g_vertices = new GeometryGroup();

      
            for (int i = 0; i < N; i++)
                {
                    if (matrix[i, 0] != -1) //if point exists
                        g_vertices.Children.Add(new EllipseGeometry(getCoords(i), 1.0, 1.0));
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


            path = new Path();

            gw.canv.Children.Add(vertices);
            gw.canv.Children.Add(edges);


     
            this.hide();
        }

        public void hide()
        {
            vertices.Fill = null;
            edges.StrokeThickness = 0;
            path.StrokeThickness = 0;
        }

        public void show()
        {
            vertices.Fill = Brushes.Black;
            edges.StrokeThickness = 1;
            
            GeometryGroup g_path = new GeometryGroup();
            path = new Path();
            path.Data = g_path;
            path.Stroke = Brushes.Green;
            path.StrokeThickness = 2;
            gw.canv.Children.Add(path);
            if (drawPath)
            {
                
                for (int i = 1; i <lastPath.Length/2; i++)
                    g_path.Children.Add(new LineGeometry(new System.Windows.Point(lastPath[0, i - 1], lastPath[1, i - 1]), new System.Windows.Point(lastPath[0, i], lastPath[1, i])));


                
               
            }

        }

    }
}
