using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day8
{
    internal class Day8
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        static string filePath = Path.Combine(appDirectory, "../../../Day8/day8.txt");
        //static string filePath = Path.Combine(appDirectory, "../../../Day8/day8_test.txt");

        struct Distance
        {
            public int From;
            public int To;
            public long Dist;
        }

        public static void Part1()
        {
            var coords = File.ReadAllLines(filePath).Select(l => l.Split(",").Select(s => int.Parse(s)).ToArray()).ToArray();
            var n_coords = coords.Length;
            var distances = new List<Distance>();

            Dictionary<int, List<int>> circuits = new();
            Dictionary<int, int> coord_to_circuit = new();

            for (var i = 0; i < n_coords; i++)
            {
                for (var j = i + 1; j < n_coords; j++)
                {
                    var dist = (long)Math.Sqrt(Math.Pow((long)Math.Abs(coords[i][0] - coords[j][0]), 2) + Math.Pow((long)Math.Abs(coords[i][1] - coords[j][1]), 2) + Math.Pow((long)Math.Abs(coords[i][2] - coords[j][2]), 2));
                    distances.Add(new Distance { From = i, To = j, Dist = dist } );
                }
            }

            distances.Sort((a, b) => a.Dist.CompareTo(b.Dist));

            var connections = 0;
            var n_groups = 1;
            foreach (var dist in distances)
            {
                if (connections == 1000)
                {
                    break;
                }

                if (!coord_to_circuit.ContainsKey(dist.From) && !coord_to_circuit.ContainsKey(dist.To))
                { // neither are in a circuit
                    connections++;
                    coord_to_circuit[dist.From] = n_groups;
                    coord_to_circuit[dist.To] = n_groups;
                    circuits[n_groups] = new List<int> { dist.From, dist.To };
                    n_groups++;
                } else if (coord_to_circuit.ContainsKey(dist.From) && coord_to_circuit.ContainsKey(dist.To))
                { // both in a circuit
                    if (coord_to_circuit[dist.From] == coord_to_circuit[dist.To])
                    {
                        connections++;
                        continue; // same circuit
                    } else
                    {
                        connections++;
                        var first_circuit = coord_to_circuit[dist.From];
                        var second_circuit = coord_to_circuit[dist.To];
                        foreach (var coord in circuits[second_circuit])
                        {
                            coord_to_circuit[coord] = first_circuit;
                            circuits[first_circuit].Add(coord);
                        }
                        circuits.Remove(second_circuit);
                    }
                } else if (coord_to_circuit.ContainsKey(dist.From))
                { // from is in a circuit
                    connections++;
                    var circuit = coord_to_circuit[dist.From];
                    coord_to_circuit[dist.To] = circuit;
                    circuits[circuit].Add(dist.To);
                } else if (coord_to_circuit.ContainsKey(dist.To))
                { // to is in a circuit
                    connections++;
                    var circuit = coord_to_circuit[dist.To];
                    coord_to_circuit[dist.From] = circuit;
                    circuits[circuit].Add(dist.From);
                }
            }

            var circuits_by_size = circuits.Values.Select(c => c.Count).OrderByDescending(c => c).ToArray();

            Console.WriteLine($"Part 1: {circuits_by_size[0] * circuits_by_size[1] * circuits_by_size[2]}");
        }

        public static void Part2()
        {
            var coords = File.ReadAllLines(filePath).Select(l => l.Split(",").Select(s => long.Parse(s)).ToArray()).ToArray();
            var n_coords = coords.Length;
            var distances = new List<Distance>();

            Dictionary<int, List<int>> circuits = new();
            Dictionary<int, int> coord_to_circuit = new();

            for (var i = 0; i < n_coords; i++)
            {
                for (var j = i + 1; j < n_coords; j++)
                {
                    var dist = (long)Math.Sqrt(
                                            Math.Pow(Math.Abs((long)coords[i][0] - (long)coords[j][0]), 2)
                                            + Math.Pow(Math.Abs((long)coords[i][1] - (long)coords[j][1]), 2)
                                            + Math.Pow(Math.Abs((long)coords[i][2] - (long)coords[j][2]), 2)
                                        );
                    distances.Add(new Distance { From = i, To = j, Dist = dist });
                }
            }

            distances.Sort((a, b) => a.Dist.CompareTo(b.Dist));

            var n_groups = 1;
            long product = 0;
            var loops = 0;
            foreach (var dist in distances)
            {
                loops++;

                if (coord_to_circuit.Count == n_coords && circuits.Count() == 1)
                {
                    break;
                }

                if (!coord_to_circuit.ContainsKey(dist.From) && !coord_to_circuit.ContainsKey(dist.To))
                { // neither are in a circuit
                    coord_to_circuit[dist.From] = n_groups;
                    coord_to_circuit[dist.To] = n_groups;
                    circuits[n_groups] = new List<int> { dist.From, dist.To };
                    n_groups++;
                }
                else if (coord_to_circuit.ContainsKey(dist.From) && coord_to_circuit.ContainsKey(dist.To))
                { // both in a circuit
                    if (coord_to_circuit[dist.From] == coord_to_circuit[dist.To])
                    {
                        continue; // same circuit
                    }
                    else
                    {
                        var first_circuit = coord_to_circuit[dist.From];
                        var second_circuit = coord_to_circuit[dist.To];
                        foreach (var coord in circuits[second_circuit])
                        {
                            coord_to_circuit[coord] = first_circuit;
                            circuits[first_circuit].Add(coord);
                        }
                        circuits.Remove(second_circuit);
                        product = coords[dist.From][0] * coords[dist.To][0];
                    }
                }
                else if (coord_to_circuit.ContainsKey(dist.From))
                { // from is in a circuit
                    var circuit = coord_to_circuit[dist.From];
                    coord_to_circuit[dist.To] = circuit;
                    circuits[circuit].Add(dist.To);
                    product = coords[dist.From][0] * coords[dist.To][0];
                }
                else if (coord_to_circuit.ContainsKey(dist.To))
                { // to is in a circuit
                    var circuit = coord_to_circuit[dist.To];
                    coord_to_circuit[dist.From] = circuit;
                    circuits[circuit].Add(dist.From);
                    product = coords[dist.From][0] * coords[dist.To][0];
                }
            }

            Console.WriteLine($"Part 2: {product}");
        }
    }
}
