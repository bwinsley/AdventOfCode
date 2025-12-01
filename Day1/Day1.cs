
namespace AdventOfCode.Day1
{
    public static class Day1
    {
        public static void Execute()
        {
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string filePath = Path.Combine(appDirectory, "../../../Day1/day1.txt");

            // Part 1
            var current = 50;
            var answer = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                var positive = line.StartsWith('R');
                var number = line.Substring(1);
                if (int.TryParse(number, out int value))
                {
                    current += positive ? value : -value;
                }

                if (current % 100 == 0)
                {
                    answer++;
                }
            }

            Console.WriteLine($"Answer: {answer}");

            // Part 2
            current = 50;
            var previous = 50;
            answer = 0;
            foreach (string line in File.ReadLines(filePath))
            {
                previous = current;
                var positive = line.StartsWith('R');
                var number = line.Substring(1);
                var change = int.Parse(number);
                var turns = change / 100;
                var clicks = change % 100;

                answer += turns;

                current += positive ? clicks : -clicks;
                if ((previous != 0 && current <= 0) || current >= 100)
                {
                    answer++;
                }

                if (current < 0)
                {
                    current += 100;
                }

                current = current % 100;
            }

            Console.WriteLine($"Answer: {answer}");
        }
    }
}
