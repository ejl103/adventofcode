using System;
using System.Linq;

namespace day_six
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");
            System.Diagnostics.Debug.Assert(lines.Length == 1);

            var starting_ages = (from entry in lines[0].Split(',') select int.Parse(entry)).ToArray();

            // Ages - ulong has no sum, but long is big enough
            var ages = new long[9];

            foreach (var starting_age in starting_ages)
            {
                ages[starting_age]++;
            }

            // As we modify need to take a copy
            var part1_answer = Simulate(80, (long[])ages.Clone());

            Console.WriteLine($"Part 1 answer: {part1_answer}");

            var part2_answer = Simulate(256, ages); 

            Console.WriteLine($"Part 1 answer: {part2_answer}");
        }

        static long Simulate(int days, long[] ages)
        {
            for (int day = 0; day < days; day++)
            {
                var spawned = ages[0];

                // shuffle up
                for (int i = 0; i < ages.Length - 1; i++)
                {
                    ages[i] = ages[i + 1];
                }

                ages[8] = spawned;
                ages[6] += spawned;
            }

            return ages.Sum();
        }
    }
}
