using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day11
{
    internal class Day11
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string filePath = Path.Combine(appDirectory, "../../../Day11/day11.txt");
        //static string filePath = Path.Combine(appDirectory, "../../../Day11/day11_test.txt");

        public static void Part1()
        {
            var dict = new Dictionary<string, Device>();
            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(' ');
                var this_device = parts[0].Substring(0, parts[0].Length - 1);

                if (!dict.ContainsKey(this_device))
                {
                    dict[this_device] = new Device
                    {
                        Name = this_device,
                        Connections = new List<Device>()
                    };
                }

                var device = dict[this_device];

                foreach (var part in parts.Skip(1))
                {
                    if (!dict.ContainsKey(part))
                    {
                        dict[part] = new Device
                        {
                            Name = part,
                            Connections = new List<Device>()
                        };
                    }

                    device.Connections.Add(dict[part]);
                }
            }

            Device you = dict["you"];

            var paths = Iterate(you);


            Console.WriteLine($"Day 11 - Part 1: {paths}");
        }

        static string filePath2 = Path.Combine(appDirectory, "../../../Day11/day11.txt");
        //static string filePath2 = Path.Combine(appDirectory, "../../../Day11/day11_2_test.txt");

        public static void Part2()
        {
            var dict = new Dictionary<string, Device2>();
            foreach (var line in File.ReadAllLines(filePath2))
            {
                var parts = line.Split(' ');
                var this_device = parts[0].Substring(0, parts[0].Length - 1);

                if (!dict.ContainsKey(this_device))
                {
                    dict[this_device] = new Device2
                    {
                        Name = this_device,
                        Connections = new List<Device2>(),
                        BackConnections = new List<Device2>(),
                        DacSeen = this_device == "dac",
                        FftSeen = this_device == "fft"
                    };
                }

                var device = dict[this_device];

                foreach (var part in parts.Skip(1))
                {
                    if (!dict.ContainsKey(part))
                    {
                        dict[part] = new Device2
                        {
                            Name = part,
                            Connections = new List<Device2>(),
                            BackConnections = new List<Device2>(),
                            DacSeen = part == "dac",
                            FftSeen = part == "fft"
                        };
                    }

                    device.Connections.Add(dict[part]);
                    dict[part].BackConnections.Add(device);
                }
            }

            var queue = new Queue<Device2>();
            Device2 last = dict["out"];
            last.Count = 1;
            queue.Enqueue(last);

            while (queue.Any())
            {
                var cur = queue.Dequeue();

                if (cur.Times != cur.Connections.Count)
                {
                    continue;
                }

                cur.Times++;

                var dac_seen = cur.Connections.Any(c => c.DacSeen);
                var fft_seen = cur.Connections.Any(c => c.FftSeen);
                var both_seen = cur.Connections.Any(c => c.DacSeen && c.FftSeen);

                for (var i = 0; i < cur.Connections.Count; i++)
                {
                    var conn = cur.Connections[i];

                    if (both_seen)
                    {
                        if (conn.DacSeen && conn.FftSeen)
                        {
                            cur.Count += conn.Count;
                        }
                    } else if (dac_seen && fft_seen)
                    {
                        // not both seen but separately seen means this is bad node, nothing ahead is good
                        continue;
                    } else if (dac_seen || fft_seen)
                    {
                        if (conn.DacSeen || conn.FftSeen)
                        {
                            cur.Count += conn.Count;
                        }
                    } else
                    {
                        cur.Count += conn.Count;
                    }
                }

                if (cur.Count == 0)
                {
                    continue;
                }

                for (var i = 0; i < cur.BackConnections.Count; i++)
                {
                    var back = cur.BackConnections[i];
                    back.DacSeen |= cur.DacSeen;
                    back.FftSeen |= cur.FftSeen;
                    back.Times += 1;
                    queue.Enqueue(back);
                }
            }

            Console.WriteLine($"Day 11 - Part 2: {dict["svr"].Count}");
        }

        public static long Iterate(Device cur)
        {
            if (cur.Name == "out")
            {
                return 1;
            }

            long paths = 0;
            foreach (var conn in cur.Connections)
            {
                paths += Iterate(conn);
            }
            return paths;
        }

        public struct Device
        {
            public string Name;
            public List<Device> Connections;
            public List<Device> BackConnections;
        }

        public record class Device2
        {
            public string Name;
            public List<Device2> Connections;
            public List<Device2> BackConnections;
            public long Count; // paths
            public int Times = 0; // times visited
            public bool DacSeen = false;
            public bool FftSeen = false;

            public Device2()
            {
            }
        }
    }
}
