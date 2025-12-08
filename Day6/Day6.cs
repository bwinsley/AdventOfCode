using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day6
{
    internal class Day6
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string filePath = Path.Combine(appDirectory, "../../../Day6/day6.txt");
        //static string filePath = Path.Combine(appDirectory, "../../../Day6/day6_test.txt");

        public static void Part1()
        {
            var lines = File.ReadLines(filePath).Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries)).ToArray();
            var numbers = lines[0].Count();
            long total = 0;

            for (var column = 0; column < numbers; column++)
            {
                var operation = lines[lines.Length - 1][column];
                long answer = operation == "+" ? 0 : 1;

                for (var line = 0; line < lines.Length - 1; line++)
                {
                    if (operation == "+")
                    {
                        answer += long.Parse(lines[line][column]);
                    }
                    else if (operation == "*")
                    {
                        answer *= long.Parse(lines[line][column]);
                    }
                }
                total += answer;
            }
            Console.WriteLine($"Part 1: {total}");
        }

        public static void Part2()
        {
            var lines = File.ReadLines(filePath).Select(line => line.ToCharArray()).ToArray();
            var line_length = lines[0].Length;
            long total = 0;
            var operation = lines[lines.Length - 1][0];
            long answer = operation == '+' ? 0 : 1;

            for (var column = 0; column < line_length; column++)
            {
                if (lines[lines.Length - 1][column] != ' ')
                {
                    operation = lines[lines.Length - 1][column];
                    answer = operation == '+' ? 0 : 1;
                }

                var number = 0;
                var add_to_total = true;
                for (var line = 0; line < lines.Length - 1; line++)
                {
                    if (column != line_length -1 && lines[line][column + 1] != ' ')
                    {
                        add_to_total = false;
                    }

                    if (lines[line][column] == ' ')
                    {
                        continue;
                    }
                    number = number * 10 + int.Parse(lines[line][column].ToString());
                }

                if (operation == '+')
                {
                    answer += number;
                }
                else if (operation == '*')
                {
                    answer *= number;
                }

                if (add_to_total)
                {
                    total += answer;
                }
            }

            Console.WriteLine($"Part 2: {total}");

        }
    }
}
