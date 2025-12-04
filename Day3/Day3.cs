using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day3
{
    internal class Day3
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string filePath = Path.Combine(appDirectory, "../../../Day3/day3.txt");
        //static string filePath = Path.Combine(appDirectory, "../../../Day3/day3_test.txt");

        public static void Part1()
        {
            var input = File.ReadAllText(filePath);
            var battery_collection = input.Split("\r\n").Select(line => line.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

            long joltage = 0;
            var length = battery_collection[0].Length;

            foreach (var row in battery_collection)
            {
                var largests = FindLargests(row, 2);
                joltage += ConstructNumber(largests, 2);
                Console.WriteLine($"{ConstructNumber(largests, 2)}");

            }

            Console.WriteLine($"Part 1: {joltage}");
        }

        public static void Part2()
        {
            var input = File.ReadAllText(filePath);
            var battery_collection = input.Split("\r\n").Select(line => line.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

            long joltage = 0;
            var length = battery_collection[0].Length;

            foreach (var row in battery_collection)
            {
                var largests = FindLargests(row, 12);
                joltage += ConstructNumber(largests, 12);
                Console.WriteLine($"{ConstructNumber(largests, 12)}");

            }

            Console.WriteLine($"Part 2: {joltage}");
        }

        private static int[] FindLargests(int[] search, int length)
        {
            var result = new int[length];
            var prev_pos = -1;

            for (var i = 0; i < length; i++)
            {
                result[i] = search[search.Length - length + i];
            }

            for (var i = 0; i < length; i++)
            {
                var cur_pos = search.Length - length + i;
                for (var j = search.Length - length + i; j > prev_pos; j--)
                {
                    if (search[j] >= result[i])
                    {
                        result[i] = search[j];
                        cur_pos = j;
                    }
                }
                prev_pos = cur_pos;
            }

            return result;
        }

        private static long ConstructNumber(int[] digits, int length)
        {
            long result = 0;
            for (var i = 0; i < length; i++)
            {
                result = result * 10 + digits[i];
            }
            return result;
        }
    }
}
