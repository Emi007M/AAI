using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows;

namespace Game.Grid
{
    class PathsOnGrid
    {
        Grid grid;
        int N;

        internal GeometryGroup g_path1;
        public int[,] lastPath;
        public int[,] smoothedPath;

        //to paint
        internal bool drawPath = false;
        public Path vertices, edges, path1, path2, path3;


        //exploring
        public List<Vector> explorePath;



        public PathsOnGrid(Grid g)
        {
            grid = g;
            N = grid.N;

            explorePath = new List<Vector>();
        }



        ///---Path algorithms

        //Dijkstra algorithm
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
                    if (grid.matrix[u, j] > 0 && !sptSet[j] && dist[u] != Int32.MaxValue && dist[u] + grid.matrix[u, j] < dist[j])
                    {
                        dist[j] = dist[u] + grid.matrix[u, j]; // +1
                        //path[j] = u;
                    }
                }
            }
            int vert = end;
            path.AddFirst(end);
            for (int i = dist[end] - 1; i >= 0; i--)
            {
                foreach (int neighbour in grid.getNeighbours(vert))
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

        public int DijkstraClosest(int start, IEnumerable<ObstacleEntity> targets) //takes starting and targets finds index point
        {

            int[] dist = new int[N];
            bool[] sptSet = new bool[N];

            int end = -1;

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
                foreach (ObstacleEntity target in targets)
                {
                    if (MovingEntity.distance(new System.Windows.Vector(grid.getX(u), grid.getY(u)), target.location) < 2 * target.r)
                    {
                        return end = u;

                    }


                    sptSet[u] = true;

                    for (int j = 0; j < N; j++)
                    {
                        if (grid.matrix[u, j] > 0 && !sptSet[j] && dist[u] != Int32.MaxValue && dist[u] + grid.matrix[u, j] < dist[j])
                        {
                            dist[j] = dist[u] + grid.space; // +1

                        }
                    }
                }
            }

            return end;
        }


        //A* algorithm
        int getHeuristic(int j, int end)//returns heuristic for index points
        {
            return (int)(Math.Abs(grid.getX(end) - grid.getX(j)) + Math.Abs(grid.getY(end) - grid.getY(j)));
        }
        int AminDistance(int[] dist, bool[] sptSet, int[] score)
        {
            // Initialize min value
            double min = Int32.MaxValue;
            int min_index = -1;


            for (int v = 0; v < N; v++)

                if (sptSet[v] == false && dist[v] + score[v] <= min)
                {
                    min = dist[v] + score[v];
                    min_index = v;
                }

            return min_index;
        }
        public int[] Astar(int start, int end) //takes starting and ending points
        {
            g_path1 = new GeometryGroup();
            //initialzation
            int[] dist = new int[N];
            int[] score = new int[N];
            bool[] sptSet = new bool[N];

            for (int i = 0; i < N; i++)
            {
                dist[i] = Int32.MaxValue / 2;
                score[i] = Int32.MaxValue / 2;
                sptSet[i] = false;
            }
            dist[start] = 0;
            score[start] = getHeuristic(start, end);

            //start Astar
            int u = start;

            for (int i = 0; i < N; i++)
            {
                if (u == end) break;

                sptSet[u] = true;

                foreach (int n in grid.getNeighbours(u))
                {
                    if (!sptSet[n])
                    {
                        dist[n] = dist[u] + grid.space; // +1 space
                        score[n] = getHeuristic(n, end);
                        g_path1.Children.Add(new LineGeometry(new System.Windows.Point(grid.getX(u), grid.getY(u)), new System.Windows.Point(grid.getX(n), grid.getY(n))));

                    }
                }

                u = AminDistance(dist, sptSet, score);

            }


            //forming path
            LinkedList<int> path = new LinkedList<int>();

            int vert = end;
            path.AddFirst(end);
            for (int i = dist[end] - 1; i >= 0; i--)
            {
                foreach (int neighbour in grid.getNeighbours(vert))
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

            return ret;
        }

        //Path smoothing algorithm
        private void pathSmoothing()
        {
            smoothedPath = new int[2, lastPath.Length / 2];
            for (int i = 0; i < lastPath.Length / 2; i++)
            {
                smoothedPath[0, i] = lastPath[0, i];
                smoothedPath[1, i] = lastPath[1, i];
            }

            int e1 = 0, e2 = 0;
            int to_remove = -1;

            while (e1 != smoothedPath.Length / 2 - 1)
            {
                e2 = e1 + 1;
                to_remove = e2;
                while (e2 != smoothedPath.Length / 2)
                {
                    if (canWalkBetween(e1, e2, smoothedPath))
                    {
                        //   Console.WriteLine(e1 + " " + e2);
                        to_remove = e2;
                        // smoothedPath = removeFromPath(e1,e2, smoothedPath);
                        //e2=e1+2;
                        e2++;
                    }
                    else
                    {
                        e2++;
                    }

                }
                smoothedPath = removeFromPath(e1, to_remove, smoothedPath);

                e1++;
            }
        }
        private bool canWalkBetween(int e1, int e2, int[,] path)
        {

            double x1 = path[0, e1], y1 = path[1, e1],
                x2 = path[0, e2], y2 = path[1, e2];

            if (x1 - x2 != 0)//if line is not vertical
            {

                //find y=ax+b
                double a = (double)((y1 - y2) / (x1 - x2));
                double b = y1 - a * x1;

                // y=-1/a*x+b consisting center of obstacle
                double a2 = -1 / a, b2;
                System.Windows.Vector P;
                foreach (ObstacleEntity o in grid.gw.trees)
                {
                    b2 = o.location.Y - a2 * o.location.X;
                    P = new System.Windows.Vector((b - b2) / (a2 - a), a * (b - b2) / (a2 - a) + b);


                    // Console.WriteLine(x1 + "," + y1 + " " + x2 + "," + y2 + " obs=" + o.location + ",r=" + o.r+", inter="+P.X+","+P.Y+ " a="+a+" b="+b);

                    if (MovingEntity.distance(P, o.location) <= (double)o.r)
                    {
                        if ((P.X >= x1 || P.X >= x2) && (P.X <= x1 || P.X <= x2) &&
                           (P.Y >= y1 || P.Y >= y2) && (P.Y <= y1 || P.Y <= y2))
                        {

                            return false;
                        }
                    }
                }

            }
            else //vertical straight
            {

                double b = x1;

                System.Windows.Vector P;
                foreach (ObstacleEntity o in grid.gw.trees)
                {

                    P = new System.Windows.Vector(b, o.location.Y);

                    if (MovingEntity.distance(P, o.location) <= (double)o.r)
                    {
                        //check if point of crossection of 2 lines belongs to the edge
                        if ((P.Y >= y1 || P.Y >= y2) && (P.Y <= y1 || P.Y <= y2))
                        {
                            return false;
                        }
                    }
                }
            }


            return true;
        }
        int[,] removeFromPath(int from, int to, int[,] path)
        {
            int[,] ret = new int[2, path.Length / 2 - (to - from - 1)];
            int j = 0;

            for (int i = 0; i < ret.Length / 2; i++)
            {
                if (j == from + 1) j = to;

                ret[0, i] = path[0, j];
                ret[1, i] = path[1, j];
                j++;
            }

            return ret;
        }



        internal void drawPaths(int[] path, double v1, double v2, double x1, double x2)
        {

            lastPath = new int[2, path.Length + 2];
            smoothedPath = new int[2, path.Length + 2];

            lastPath[0, 0] = (int)v1;
            lastPath[1, 0] = (int)v2;


            for (int i = 1; i <= path.Length; i++)
            {
                lastPath[0, i] = (int)grid.getCoords(path[i - 1]).X;
                lastPath[1, i] = (int)grid.getCoords(path[i - 1]).Y;
            }

            lastPath[0, lastPath.Length / 2 - 1] = (int)x1;
            lastPath[1, lastPath.Length / 2 - 1] = (int)x2;


            pathSmoothing();

            if (this.path2.StrokeThickness != 0)
            {
                grid.hide(); grid.show();
            }

            drawPath = true;

        }


        //exploring
        public void InitExplorePath()
        {
            explorePath = new List<Vector>();

            Console.WriteLine("ExplorePath: out of " + grid.N + " vertices:");

            int i = 0;
            Random rand = new Random();
            int v;
            while (i<grid.N)
            {
                v = rand.Next(10) + i;
                if (grid.matrix[v, v] != -1)
                {
                    explorePath.Add(new Vector(grid.getX(v), grid.getY(v)));
                //    Console.WriteLine("Vertex: "+v);
                }

                i += grid.N / 40;
            }




        }



    }
}
