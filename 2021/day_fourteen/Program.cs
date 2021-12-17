using System;
using System.Collections.Generic;
using System.Linq;

namespace day_fourteen
{
    internal class Program
    {
        static Dictionary<string, string> rules = new();
        static Dictionary<string, long> pair_count = new();
        static Dictionary<char, long> elem_count = new();

        static void UpdateValue<T>(Dictionary<T, long> map, T key, long value) where T : notnull
        {
            map.TryGetValue(key, out var current_count);
            map[key] = current_count + value;
        }

        static long RunIterations(int times)
        {
            for (int i = 0; i < times; i++)
            {
                var new_pair_count = new Dictionary<string, long>(pair_count);
                foreach (var pair in new_pair_count)
                {
                    if (pair.Value > 0)
                    {
                        var new_pair1 = pair.Key[0].ToString() + rules[pair.Key];
                        var new_pair2 = rules[pair.Key] + pair.Key[1];

                        UpdateValue(pair_count, new_pair1, pair.Value);
                        UpdateValue(elem_count, new_pair1[0], pair.Value);
                        UpdateValue(elem_count, new_pair1[1], pair.Value);

                        UpdateValue(pair_count, new_pair2, pair.Value);
                        // new_pair2[0] overlaps
                        UpdateValue(elem_count, new_pair2[1], pair.Value);

                        UpdateValue(pair_count, pair.Key, -pair.Value);
                        UpdateValue(elem_count, pair.Key[0], -pair.Value);
                        UpdateValue(elem_count, pair.Key[1], -pair.Value);
                    }

                }
            }

            var max = elem_count.Aggregate((l, r) => l.Value > r.Value ? l : r).Value;
            var min = elem_count.Aggregate((l, r) => l.Value < r.Value ? l : r).Value;

            return max - min;
        }
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            string polymer_input = lines[0];

            for (int i = 2; i < lines.Length; i++)
            {
                var split = lines[i].Replace(" ->", "").Split(' ');
                rules[split[0]] = split[1];
                pair_count[split[0]] = 0;
            }

            var starting_pairs = polymer_input.Where((_, i) => i + 1 < polymer_input.Length)
                .Select((c, i) =>
                {
                    return c.ToString() + polymer_input[i + 1];
                });

            foreach (var pair in starting_pairs)
            {
                pair_count[pair]++;
            }

            foreach (var c in polymer_input)
            {
                UpdateValue(elem_count, c, 1);
            }

            // part 1 10 steps
            var answer = RunIterations(10);

            Console.WriteLine($"Part one answer: {answer}");

            // part 2 get to 40, so another 30
            answer = RunIterations(30);

            Console.WriteLine($"Part two answer: {answer}");
        }
    }
}
