using System;
using System.Linq;

namespace day_seven
{
    internal class Program
    {
        static int GetFuelUse(int moves)
        {
            if (moves > 0)
                return moves + GetFuelUse(moves - 1);
            else
                return 0;
        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            // Line 0 is the bingo caller numbers
            var positions = (from entry in lines[0].Split(',') select int.Parse(entry));

            var min = positions.Min();
            var max = positions.Max();

            var fuel_results_part1 = new int[max - min];
            var fuel_results_part2 = new int[max - min];

            // Recursive function is slow - limited value set so just do it once 108ms this way versus 8s
            var fuel_lookup_part2 = new int[max - min + 1];
            for (int i = 0; i < fuel_lookup_part2.Length; i++)
            {
                fuel_lookup_part2[i] = GetFuelUse(i);
            }

            for (int i = min; i < max; i++)
            {
                var result_index = i - min;
                foreach (var position in positions)
                {
                    var moves = Math.Abs(position - i);
                    fuel_results_part1[result_index] += moves;

                    fuel_results_part2[result_index] += fuel_lookup_part2[moves];
                }
            }

            var part_one_answer = fuel_results_part1.Min();
            var part_two_answer = fuel_results_part2.Min();

            Console.WriteLine($"part one answer: {part_one_answer}");
            Console.WriteLine($"part two answer: {part_two_answer}");
        }
    }
}
