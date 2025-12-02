using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day2
{
    internal class Day2
    {
        public static void Part1()
        {
            //var input = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";
            var input = "8284583-8497825,7171599589-7171806875,726-1031,109709-251143,1039-2064,650391-673817,674522-857785,53851-79525,8874170-8908147,4197684-4326484,22095-51217,92761-107689,23127451-23279882,4145708930-4145757240,375283-509798,585093-612147,7921-11457,899998-1044449,3-19,35-64,244-657,5514-7852,9292905274-9292965269,287261640-287314275,70-129,86249864-86269107,5441357-5687039,2493-5147,93835572-94041507,277109-336732,74668271-74836119,616692-643777,521461-548256,3131219357-3131417388";
            var ranges = input.Split(',').Select(s => s.Split('-'));

            long total = 0;
            foreach (var range in ranges)
            {
                Console.WriteLine($"===> {range[0]}, {range[1]}");

                var length = range[0].Length;

                while (length <= range[1].Length)
                {
                    if (length % 2 == 1)
                    {
                        length++;
                        continue;
                    }

                    string start;

                    if (length == range[0].Length)
                    {
                        start = string.Concat(range[0].Take(length / 2));
                        if (long.Parse(start + start) < long.Parse(range[0]))
                        {
                            start = (long.Parse(start) + 1).ToString();
                        }
                    } else
                    {
                        start = "1".PadRight(length / 2, '0');
                    }

                    string end;
                    if (length == range[1].Length)
                    {
                        end = string.Concat(range[1].Take(length / 2));
                        if (long.Parse(end + end) > long.Parse(range[1]))
                        {
                            end = (long.Parse(end) - 1).ToString();
                        }
                    } else
                    {
                        end = "9".PadRight(length / 2, '9');
                    }

                    for (var i = long.Parse(start); i <= long.Parse(end); i++)
                    {
                        Console.WriteLine($"> {i} {i}");
                        total += long.Parse(i.ToString() + i.ToString());
                    }
                    length++;
                }
            }

            Console.WriteLine($"Total: {total}");
        }

        public static void Part2()
        {
            //var input = "11-22,95-115,998-1012,1188511880-1188511890,222220-222224,1698522-1698528,446443-446449,38593856-38593862,565653-565659,824824821-824824827,2121212118-2121212124";
            var input = "8284583-8497825,7171599589-7171806875,726-1031,109709-251143,1039-2064,650391-673817,674522-857785,53851-79525,8874170-8908147,4197684-4326484,22095-51217,92761-107689,23127451-23279882,4145708930-4145757240,375283-509798,585093-612147,7921-11457,899998-1044449,3-19,35-64,244-657,5514-7852,9292905274-9292965269,287261640-287314275,70-129,86249864-86269107,5441357-5687039,2493-5147,93835572-94041507,277109-336732,74668271-74836119,616692-643777,521461-548256,3131219357-3131417388";
            var ranges = input.Split(',').Select(s => s.Split('-'));

            long total = 0;
            foreach (var range in ranges)
            {
                Console.WriteLine($"===> ===> {range[0]}, {range[1]}");

                SortedSet<long> found = new();

                var length = range[0].Length;

                while (length <= range[1].Length)
                {
                    for (var factorial = 1; factorial < length; factorial++)
                    { // too lazy to optimize factorial calculation
                        if (length % factorial != 0)
                        {
                            continue;
                        }

                        Console.WriteLine($">>> {factorial}");

                        var divisor = length / factorial;
                        string start;

                        if (length == range[0].Length)
                        {
                            start = string.Concat(range[0].Take(factorial));
                            if (long.Parse(RepeatString(start, divisor)) < long.Parse(range[0]))
                            {
                                start = (long.Parse(start) + 1).ToString();
                            }
                        }
                        else
                        {
                            start = "1".PadRight(factorial, '0');
                        }

                        string end;
                        if (length == range[1].Length)
                        {
                            end = string.Concat(range[1].Take(factorial));
                            if (long.Parse(RepeatString(end, divisor)) > long.Parse(range[1]))
                            {
                                end = (long.Parse(end) - 1).ToString();
                            }
                        }
                        else
                        {
                            end = "9".PadRight(factorial, '9');
                        }

                        for (var i = long.Parse(start); i <= long.Parse(end); i++)
                        {
                            Console.Write("> ");
                            for (var rep = 0; rep < divisor; rep++)
                            {
                                Console.Write(i.ToString() + " ");
                            }
                            Console.Write("\n");

                            var to_add = long.Parse(RepeatString(i.ToString(), divisor));
                            if (!found.Contains(to_add))
                            {
                                found.Add(to_add);
                                total += to_add;
                            }
                        }
                    }
                    length++;
                }
            }

            Console.WriteLine($"Total: {total}");
        }

        static string RepeatString(string str, int count)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i++)
            {
                sb.Append(str);
            }
            return sb.ToString();
        }
    }
}
