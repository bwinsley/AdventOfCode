using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day7
{
    internal class Day7
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string filePath = Path.Combine(appDirectory, "../../../Day7/day7.txt");
        //static string filePath = Path.Combine(appDirectory, "../../../Day7/day7_test.txt");

        public static void Part1()
        {
            var lines = File.ReadLines(filePath).Select(line => line.ToCharArray()).ToArray();
            var n_lines = lines.Length;
            var n_cols = lines[0].Length;

            var tachs = lines[0].Select(c => c == 'S' ? true : false).ToArray();
            var splits = 0;
            var lower_col_bound = n_cols / 2 - 1;
            var upper_col_bound = n_cols / 2 + 1;

            for (var row = 2; row < n_lines; row += 2)
            {
                for (var col = lower_col_bound; col <= upper_col_bound; col++)
                {
                    if (lines[row][col] == '^' && tachs[col])
                    {
                        tachs[col - 1] = true;
                        tachs[col] = false;
                        tachs[col + 1] = true;
                        splits++;
                    }
                }
                lower_col_bound -= 1;
                upper_col_bound += 1;
            }
            Console.WriteLine($"Part 1: {splits}");
        }

        public static void Part2()
        {
            var lines = File.ReadLines(filePath).Select(line => line.ToCharArray()).ToArray();
            var n_lines = lines.Length;
            var n_cols = lines[0].Length;

            var tachs = lines[0].Select(c => c == 'S' ? true : false).ToArray();
            var paths = lines[0].Select(c => c == 'S' ? (long)1 : (long)0).ToArray();
            var lower_col_bound = n_cols / 2 - 1;
            var upper_col_bound = n_cols / 2 + 1;

            for (var row = 2; row < n_lines; row += 2)
            {
                var new_paths = lines[0].Select(c => (long)0).ToArray();

                for (var col = lower_col_bound; col <= upper_col_bound; col++)
                {
                    if (lines[row][col] == '^' && tachs[col])
                    {
                        tachs[col - 1] = true;
                        tachs[col] = false;
                        tachs[col + 1] = true;

                        new_paths[col - 1] += paths[col];
                        new_paths[col] = 0;
                        new_paths[col + 1] += paths[col];
                    } else
                    {
                        new_paths[col] += paths[col];
                    }
                }
                paths = new_paths;
                lower_col_bound -= 1;
                upper_col_bound += 1;
            }
            Console.WriteLine($"Part 2: {paths.Sum()}");
        }
    }
}
