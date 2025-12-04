using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day4
{
    public static class Day4
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string filePath = Path.Combine(appDirectory, "../../../Day4/day4.txt");
        //static string filePath = Path.Combine(appDirectory, "../../../Day4/day4_test.txt");

        public static void Part1()
        {
            var input = File.ReadAllText(filePath);
            var grid = input.Split("\r\n").Select(line => line.Select(c => c == '@' ? 1 : 0).ToArray()).ToArray();

            var row_count = grid.Length;
            var col_count = grid[0].Length;

            var counts = new int[row_count, col_count];
            var total = 0;

            for (var row = 0; row < row_count; row++)
            {
                for (var col = 0; col < col_count; col++)
                {
                    var surroundings = new (int Row, int Col)[]
                        {
                            ( row - 1, col - 1), ( row - 1, col), ( row - 1, col + 1),
                            ( row    , col - 1), ( row    , col), ( row    , col + 1),
                            ( row + 1, col - 1), ( row + 1, col), ( row + 1, col + 1),
                        };

                    if (grid[row][col] == 0)
                    {
                        foreach (var surround in surroundings
                            .Where(pos => pos.Row >= 0 && pos.Row < row_count && pos.Col >= 0 && pos.Col < col_count)
                            .Where(pos => grid[pos.Row][pos.Col] == 1))
                        {
                            counts[surround.Row, surround.Col]++;
                            if (counts[surround.Row, surround.Col] == 5)
                            {
                                total++;
                            }
                        }
                    } else
                    {
                        foreach (var surround in surroundings
                            .Where(pos => pos.Row < 0 || pos.Row >= row_count || pos.Col < 0 || pos.Col >= col_count))
                        {
                            counts[row, col]++;
                            if (counts[row, col] == 5)
                            {
                                total++;
                            }
                        }
                    }
                }
            }

            Console.WriteLine($"Day 4 - Part 1: {total}");
        }

        public static void Part2()
        {
            var input = File.ReadAllText(filePath);
            var grid = input.Split("\r\n").Select(line => line.Select(c => c == '@' ? 1 : 0).ToArray()).ToArray();

            var row_count = grid.Length;
            var col_count = grid[0].Length;

            var counts = new int[row_count, col_count];
            var total = 0;

            var queue = new Queue<(int Row, int Col)>();

            for (var row = 0; row < row_count; row++)
            {
                for (var col = 0; col < col_count; col++)
                {
                    ProcessGridPoint(row, col, ref grid, ref counts, ref total, queue);
                }
            }

            while (queue.Count > 0)
            {
                var (row, col) = queue.Dequeue();
                grid[row][col] = 0;
                ProcessGridPoint(row, col, ref grid, ref counts, ref total, queue);
            }

            Console.WriteLine($"Day 4 - Part 2: {total}");
        }

        public static void ProcessGridPoint(int row, int col, ref int[][] grid, ref int[,] counts, ref int total, Queue<(int Row, int Col)> queue)
        {
            var row_count = grid.Length;
            var col_count = grid[0].Length;
            var surroundings = new (int Row, int Col)[]
                        {
                            ( row - 1, col - 1), ( row - 1, col), ( row - 1, col + 1),
                            ( row    , col - 1), ( row    , col), ( row    , col + 1),
                            ( row + 1, col - 1), ( row + 1, col), ( row + 1, col + 1),
                        };

            if (grid[row][col] == 0)
            {
                foreach (var surround in surroundings
                    .Where(pos => pos.Row >= 0 && pos.Row < row_count && pos.Col >= 0 && pos.Col < col_count))
                {
                    if (grid[surround.Row][surround.Col] == 0)
                    {
                        continue;
                    }
                    counts[surround.Row, surround.Col]++;
                    if (counts[surround.Row, surround.Col] == 5)
                    {
                        total++;
                        queue.Enqueue((surround.Row, surround.Col));
                    }
                }
            }
            else
            {
                foreach (var surround in surroundings
                    .Where(pos => pos.Row < 0 || pos.Row >= row_count || pos.Col < 0 || pos.Col >= col_count))
                {
                    counts[row, col]++;
                    if (counts[row, col] == 5)
                    {
                        total++;
                        queue.Enqueue((row, col));
                    }
                }
            }
        }
    }
}
