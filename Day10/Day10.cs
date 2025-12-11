using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Day10
{
    internal class Day10
    {
        static string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //static string filePath = Path.Combine(appDirectory, "../../../Day10/day10.txt");
        static string filePath = Path.Combine(appDirectory, "../../../Day10/day10_test.txt");

        public static void Part1()
        {
            var lines = File.ReadAllLines(filePath);
            var n_lines = lines.Length;
            var lights = new List<bool[]>();
            var buttons = new List<bool[][]>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var n_parts = parts.Length;
                var n = parts[0].Length - 2;
                var light_config = new bool[n];
                for (var i = 0; i < n; i++)
                {
                    light_config[i] = parts[0][i + 1] == '#' ? true : false;
                }
                lights.Add(light_config);

                var button_configs = new bool[n_parts - 2][];
                for (var i = 1; i < n_parts - 1; i++)
                {
                    var button_config = new bool[n];
                    var lights_affected = parts[i].Substring(1, parts[i].Length - 2).Split(',').Select(s => int.Parse(s));
                    foreach (var la in lights_affected)
                    {
                        button_config[la] = true;
                    }
                    button_configs[i - 1] = button_config;
                }
                buttons.Add(button_configs);
            }

            var total = 0;

            for (var i = 0; i < n_lines; i++)
            {
                var queue = new Queue<State1>();
                var initial_state = new State1()
                {
                    light_state = new bool[lights[i].Length],
                    button_presses = 0,
                    buttons_pressed = new List<int>()
                };
                queue.Enqueue(initial_state);

                while (queue.Any())
                {
                    var state = queue.Dequeue();
                    if (state.light_state.SequenceEqual(lights[i]))
                    {
                        total += state.button_presses;
                        break;
                    }

                    foreach (var button in buttons[i])
                    {
                        var new_state = new State1()
                        {
                            light_state = (bool[])state.light_state.Clone(),
                            button_presses = state.button_presses + 1,
                            buttons_pressed = new List<int>(state.buttons_pressed)
                        };

                        for (var j = 0; j < button.Length; j++)
                        {
                            if (button[j])
                            {
                                new_state.light_state[j] = !new_state.light_state[j];
                            }
                        }

                        queue.Enqueue(new_state);
                    }
                    
                }
            }

            Console.WriteLine($"Day 10 - Part 1: {total}");
        }

        public static void Part2()
        {
            var lines = File.ReadAllLines(filePath);
            var n_lines = lines.Length;
            var lights = new List<int[]>();
            var buttons = new List<bool[][]>();

            foreach (var line in lines)
            {
                var parts = line.Split(' ');
                var n_parts = parts.Length;
                var n = parts[0].Length - 2;
                var light_config = new int[n];
                light_config = parts[parts.Length - 1].Substring(1, parts[parts.Length - 1].Length - 2).Split(',').Select(s => int.Parse(s)).ToArray();
                lights.Add(light_config);

                var button_configs = new bool[n_parts - 2][];
                for (var i = 1; i < n_parts - 1; i++)
                {
                    var button_config = new bool[n];
                    var lights_affected = parts[i].Substring(1, parts[i].Length - 2).Split(',').Select(s => int.Parse(s));
                    foreach (var la in lights_affected)
                    {
                        button_config[la] = true;
                    }
                    button_configs[i - 1] = button_config;
                }
                buttons.Add(button_configs);
            }

            var total = 0;

            for (var line = 0; line < n_lines; line++)
            {
                var sorted_light_config = lights[line].Select((x, i) => new LightsOrdered { state = x, index = i })
                                                         .OrderBy(l => l.state)
                                                         .ToArray();

                var queue = new Queue<State2>();
                var initial_state = new State2()
                {
                    light_state = new int[lights[line].Length],
                    button_presses = 0,
                    buttons_pressed = new int[buttons[line].Length],
                };
                queue.Enqueue(initial_state);

                foreach (var light in sorted_light_config)
                {
                    var internal_queue = new Queue<State2>();

                    while (queue.Any())
                    {
                        var state = queue.Dequeue();
                        internal_queue.Enqueue(state);
                    }

                    while (internal_queue.Any()) {
                        var state = internal_queue.Dequeue();

                        if (LightsInvalid(state.light_state, lights[line]))
                        {
                            continue;
                        }
                        else if (state.light_state[light.index] == light.state)
                        {
                            queue.Enqueue(state);
                            continue;
                        }

                        for (var btn_id = 0; btn_id < buttons[line].Length; btn_id++)
                        {
                            var button = buttons[line][btn_id];
                            if (!button[light.index])
                            {
                                continue;
                            }

                            var new_state = new State2()
                            {
                                light_state = (int[])state.light_state.Clone(),
                                button_presses = state.button_presses + 1,
                                buttons_pressed = state.buttons_pressed.Append(btn_id).ToArray(),
                            };

                            new_state.buttons_pressed[btn_id] += 1;

                            for (var la = 0; la < button.Length; la++)
                            {
                                if (button[la])
                                {
                                    new_state.light_state[la] += 1;
                                }
                            }

                            internal_queue.Enqueue(new_state);
                        }
                    }
                }

                var final_state = queue.Dequeue();
                var presses = final_state.button_presses;

                while (queue.Any())
                {
                    var next_state = queue.Dequeue();
                    if (next_state.button_presses < presses)
                    {
                        final_state = next_state;
                        presses = next_state.button_presses;
                    }
                }
                total += presses;
            }

            Console.WriteLine($"Day 10 - Part 2: {total}");
        }

        public struct LightsOrdered
        {
            public int state;
            public int index;
        }

        public struct State1
        {
            public bool[] light_state;
            public int button_presses;
            public List<int> buttons_pressed;
        }

        public struct State2
        {
            public int[] light_state;
            public int button_presses;
            public int[] buttons_pressed;
        }

        public static bool LightsMatch(int[] state1, int[] state2)
        {
            if (state1.Length != state2.Length)
            {
                return false;
            }
            for (var i = 0; i < state1.Length; i++)
            {
                if (state1[i] != state2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool LightsInvalid(int[] lights, int[] wanted)
        {
            for (var i = 0; i < lights.Length; i++)
            {
                if (lights[i] > wanted[i])
                {
                    return true;
                }
            }
            return false;
        }
    }
}
