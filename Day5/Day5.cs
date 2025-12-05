using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day5
{
    public static class Day5
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string filePath = Path.Combine(appDirectory, "../../../Day5/day5.txt");
        //static string filePath = Path.Combine(appDirectory, "../../../Day5/day5_test.txt");

        public static (List<(long, long)> Ranges, List<long> Ingredients) ReadInput()
        {
            var reading_ranges = true;
            List<(long, long)> ranges = new();
            List<long> ingredients = new();
            foreach (var line in File.ReadLines(filePath))
            {
                if (line.Trim() == "")
                {
                    reading_ranges = false;
                } else if (reading_ranges)
                {
                    var parts = line.Split("-");
                    ranges.Add((long.Parse(parts[0]), long.Parse(parts[1])));
                } else
                {
                    ingredients.Add(long.Parse(line));
                }
            }
            return (ranges, ingredients);
        }

        public static void Part1()
        {
            var (ranges, ingredients) = ReadInput();
            var count = 0;

            foreach (var ingredient in ingredients)
            {
                foreach (var range in ranges)
                {
                    if (ingredient >= range.Item1 && ingredient <= range.Item2)
                    {
                        count++;
                        break;
                    }
                }
            }
            Console.WriteLine($"Part 1: {count}");
        }

        public static void Part2()
        {
            var (ranges, _) = ReadInput();

            var sorted_ranges = ranges.OrderBy(r => r.Item1).ToList();

            long count = 0;

            long prev_upper = -1;
            foreach (var range in sorted_ranges)
            {
                long lower = range.Item1;
                long upper = range.Item2;

                if (lower <= prev_upper && upper <= prev_upper)
                {
                    continue;
                } else if (lower <= prev_upper && upper > prev_upper)
                {
                    lower = prev_upper + 1;
                }

                count += (upper - lower + 1);
                prev_upper = upper;
            }
            Console.WriteLine($"Part 2: {count}");
        }
    }
}
