using System;
using System.Collections.Generic;

namespace day_fifteen
{
    internal class Program
    {
        static IEnumerable<(int x, int y)> Iterate((int x, int y) start_pos, int[,] grid)
        {
            if (start_pos.x < grid.GetLength(0) - 1)
                yield return start_pos with { x = start_pos.x + 1 };

            if (start_pos.y < grid.GetLength(1) - 1)
                yield return start_pos with { y = start_pos.y + 1 };

            if (start_pos.x > 0)
                yield return start_pos with { x = start_pos.x - 1 };

            if (start_pos.y > 0)
                yield return start_pos with { y = start_pos.y - 1 };

        }

        static int GetLowestRisk(string[] lines, int size_factor)
        {
            var grid = new int[lines.Length * size_factor, lines.Length * size_factor];

            const int max_individual_risk = 9;

            for (int x = 0; x < size_factor; x++)
            {
                for (int y = 0; y < size_factor; y++)
                {
                    int increase_factor_x = x * lines.Length;
                    int increase_factor_y = y * lines.Length;
                    for (int i = 0; i < lines.Length; i++)
                    {
                        for (int j = 0; j < lines.Length; j++)
                        {
                            var num = lines[i][j] - '0' + x + y;

                            while (num > max_individual_risk)
                                num -= max_individual_risk;

                            grid[increase_factor_x + i, increase_factor_y + j] = num;
                        }
                    }
                }
            }


            var start_pos = (x: 0, y: 0);
            var target_pos = (x: grid.GetLength(0) - 1, y: grid.GetLength(1) - 1);

            var risk = new int[grid.GetLength(0), grid.GetLength(1)];

            for (int i = 0; i < risk.GetLength(0); i++)
            {
                for (int j = 0; j < risk.GetLength(1); j++)
                {
                    // No score for the start point
                    if (i == 0 && j == 0)
                        continue;

                    risk[i, j] = int.MaxValue;
                }
            }

            var queue = new PriorityQueue<(int x, int y), int>();
            queue.Enqueue(start_pos, 0);

            do
            {
                var pos = queue.Dequeue();

                foreach (var next_pos in Iterate(pos, grid))
                {
                    int current_risk = risk[next_pos.y, next_pos.x];
                    int new_risk = risk[pos.y, pos.x] + grid[next_pos.y, next_pos.x];

                    if (new_risk < current_risk)
                    {
                        risk[next_pos.y, next_pos.x] = new_risk;
                        queue.Enqueue(next_pos, new_risk);
                    }
                }
            }
            while (queue.Count > 0);

            return risk[target_pos.y, target_pos.x];
        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            var lowest_risk = GetLowestRisk(lines, 1);
            Console.WriteLine($"Part one answer: {lowest_risk}");

            lowest_risk = GetLowestRisk(lines, 5);
            Console.WriteLine($"Part two answer: {lowest_risk}");
        }
    }
}
