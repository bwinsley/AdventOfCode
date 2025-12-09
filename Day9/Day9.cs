using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day9
{
    internal class Day9
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //static string filePath = Path.Combine(appDirectory, "../../../Day9/day9.txt");
        static string filePath = Path.Combine(appDirectory, "../../../Day9/day9_test.txt");

        public static void Part1()
        {
            var coords = File.ReadAllLines(filePath).Select(l => l.Split(",").Select(s => long.Parse(s)).ToArray()).ToArray();
            long area = 0;

            for (var i = 0; i < coords.Length; i++)
            {
                for (var j = 0; j < coords.Length; j++)
                {
                    var a = Area(coords[i][0], coords[i][1], coords[j][0], coords[j][1]);
                    if (a > area)
                    {
                        area = a;
                    }
                }
            }

            Console.WriteLine($"Day 9 - Part 1: {area}");
        }

        struct Coordinate
        {
            public long X;
            public long Y;
        }

        public static void Part2()
        {
            var coords = File.ReadAllLines(filePath).Select(l => l.Split(",").Select(s => long.Parse(s)).ToArray()).ToArray();

            var x_min = coords[0][0];
            var x_max = coords[0][0];
            var y_min = coords[0][1];
            var y_max = coords[0][1];
            var verticals = new List<(Coordinate D, Coordinate U)>();
            var horizontal = new List<(Coordinate L, Coordinate R)>();
            for (var i = 0; i < coords.Length; i++)
            {
                var j = i == coords.Length - 1 ? 0 : i + 1;

                if (coords[i][0] < x_min) x_min = coords[i][0];
                if (coords[i][0] > x_max) x_max = coords[i][0];
                if (coords[i][1] < y_min) y_min = coords[i][1];
                if (coords[i][1] > y_max) y_max = coords[i][1];

                if (coords[i][0] == coords[j][0])
                {
                    if (coords[i][1] < coords[j][1])
                    {
                        verticals.Add((new Coordinate() { X = coords[i][0], Y = coords[i][1] },
                                        new Coordinate() { X = coords[j][0], Y = coords[j][1] }
                                    ));
                    }
                    else
                    {
                        verticals.Add((new Coordinate() { X = coords[j][0], Y = coords[j][1] },
                                        new Coordinate() { X = coords[i][0], Y = coords[i][1] }
                                    ));
                    }
                }
                else if (coords[i][1] == coords[j][1])
                {
                    if (coords[i][0] < coords[j][0])
                    {
                        horizontal.Add((new Coordinate() { X = coords[i][0], Y = coords[i][1] },
                                        new Coordinate() { X = coords[j][0], Y = coords[j][1] }
                                    ));
                    }
                    else
                    {
                        horizontal.Add((new Coordinate() { X = coords[j][0], Y = coords[j][1] },
                                        new Coordinate() { X = coords[i][0], Y = coords[i][1] }
                                    ));
                    }
                }
            }

            Dictionary<int, List<int>> vertical_intercepts_by_x = new();
            Dictionary<int, List<int>> horizontal_intercepts_by_y = new();

            foreach (var v in verticals)
            {
                var y_coord = (int)v.D.Y;
                
                for (var y = (int)v.D.Y; y <= (int)v.U.Y; y++)
                {
                    if (!horizontal_intercepts_by_y.ContainsKey(y))
                    {
                        horizontal_intercepts_by_y[y] = new List<int>();
                    }
                    horizontal_intercepts_by_y[y].Add((int)v.D.X);
                }
            }

            foreach (var y in horizontal_intercepts_by_y.Keys)
            {
                horizontal_intercepts_by_y[y].Sort();
            }

            foreach (var h in horizontal)
            {
                var x_coord = (int)h.L.X;
                for (var x = (int)h.L.X; x <= (int)h.R.X; x++)
                {
                    if (!vertical_intercepts_by_x.ContainsKey(x))
                    {
                        vertical_intercepts_by_x[x] = new List<int>();
                    }
                    vertical_intercepts_by_x[x].Add((int)h.L.Y);
                }
            }

            foreach (var x in vertical_intercepts_by_x.Keys)
            {
                vertical_intercepts_by_x[x].Sort();
            }


            long area = 0;
            for (var i = 0; i < coords.Length; i++)
            {
                Console.WriteLine($"{i} out of {coords.Length}");
                for (var j = i + 1; j < coords.Length; j++)
                {
                    var a = Area(coords[i][0], coords[i][1], coords[j][0], coords[j][1]);
                    if (a > area)
                    {
                        var x0 = Math.Min(coords[i][0], coords[j][0]);
                        var x1 = Math.Max(coords[i][0], coords[j][0]);
                        var y0 = Math.Min(coords[i][1], coords[j][1]);
                        var y1 = Math.Max(coords[i][1], coords[j][1]);

                        var exit = false;

                        for (var x = x0; x < x1; x++)
                        {
                            if (!IsPointInGrid((int)x, (int)y0, vertical_intercepts_by_x, horizontal_intercepts_by_y))
                            {
                                exit = true;
                                break;
                            }
                            else if (!IsPointInGrid((int)x, (int)y1, vertical_intercepts_by_x, horizontal_intercepts_by_y))
                            {
                                exit = true;
                                break;
                            }
                        }

                        if (exit) continue;

                        for (var y = y0; y <= y1; y++)
                        {
                            if (!IsPointInGrid((int)x0, (int)y, vertical_intercepts_by_x, horizontal_intercepts_by_y))
                            {
                                exit = true;
                                break;
                            }
                            else if (!IsPointInGrid((int)x1, (int)y, vertical_intercepts_by_x, horizontal_intercepts_by_y))
                            {
                                exit = true;
                                break;
                            }
                        }

                        if (exit) continue;

                        area = a;
                    }
                }
            }

            Console.WriteLine($"Day 9 - Part 2: {area}");
        }

        public static long Area(long x1, long y1, long x2, long y2)
        {
            return (Math.Abs(x1 - x2) + 1) * (Math.Abs(y1 - y2) + 1);
        }

        public static bool InGrid(long x, long y, long start_x, long start_y)
        {
            var grid_file = Path.Combine(appDirectory, "../../../Day9/grid.txt");
            using var reader = new StreamReader(grid_file);
            string line = "";
            for (var j = 0; j < y - start_y + 1; j++)
            {
                line = reader.ReadLine()!;
            }
            return int.Parse(line![(int)(x - start_x)].ToString()) >= 4;
        }

        public static bool IsPointInGrid(int x, int y, Dictionary<int, List<int>> vertical_intercepts_by_x, Dictionary<int, List<int>> horizontal_intercepts_by_y)
        {
            var within_y_bounds = false;

            if (y >= vertical_intercepts_by_x[x][0] && y <= vertical_intercepts_by_x[x].Last())
            {
                within_y_bounds = true;
            }

            var within_x_bounds = false;
            if (x >= horizontal_intercepts_by_y[y][0] && x <= horizontal_intercepts_by_y[y].Last())
            {
                within_x_bounds = true;
            }
            return within_x_bounds && within_y_bounds;
        }

        public static void PrintGrid(int[,] grid)
        {
            for (var y = 0; y < grid.GetLength(1); y++)
            {
                for (var x = 0; x < grid.GetLength(0); x++)
                {
                    Console.Write(grid[x, y]);
                }
                Console.WriteLine();
            }

        }
    }
}
