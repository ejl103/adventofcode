using System;
using System.Collections.Generic;
using System.Linq;

namespace day_eleven
{
    internal class Program
    {
        const int upper_bound = 10;
        const int flash_threshold = 9;

        static IEnumerable<(int y, int x)> IterateValuesToCheck(int y, int x)
        {
            bool can_y_sub = y > 0;
            bool can_y_add = y < upper_bound - 1;
            bool can_x_sub = x > 0;
            bool can_x_add = x < upper_bound - 1;

            if (can_y_sub)
            {
                int dy = y - 1;

                yield return (dy, x);

                if (can_x_sub)
                {
                    yield return (dy, x - 1);
                }

                if (can_x_add)
                {
                    yield return (dy, x + 1);
                }
            }

            if (can_y_add)
            {
                int iy = y + 1;

                yield return (iy, x);

                if (can_x_sub)
                {
                    yield return (iy, x - 1);
                }

                if (can_x_add)
                {
                    yield return (iy, x + 1);
                }
            }

            if (can_x_sub)
            {
                yield return (y, x - 1);
            }

            if (can_x_add)
            {
                yield return (y, x + 1);
            }

        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            // Always 10x10
            var grid = new int[upper_bound][];

            // FIll out friend the grid
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = (from c in lines[i] select c - '0').ToArray();
            }

            int num_flashes = 0;
            int? step_when_all_flashed = null;

            const int all_flashed = upper_bound * upper_bound;

            for (int i = 0; true; i++)
            {
                // Can only flash once per round
                var flashed = new SortedSet<(int y, int x)>();

                for (int y = 0; y < grid.Length; y++)
                {
                    for (int x = 0; x < upper_bound; x++)
                    {
                        ++grid[y][x];
                        CheckFlash(grid, y, x, flashed);

                    }
                }

                foreach (var point in flashed)
                {
                    grid[point.y][point.x] = 0;
                }

                // Simulate for 100 iterations for part 1
                if (i < 100)
                    num_flashes += flashed.Count;

                if (flashed.Count == all_flashed)
                {
                    step_when_all_flashed = i + 1;
                    break;
                }
            }

            Console.WriteLine($"Part One Answer: {num_flashes}");
            Console.WriteLine($"Part Two Answer: {step_when_all_flashed}");
        }

        private static void CheckFlash(int[][] grid, int y, int x, SortedSet<(int y, int x)> flashed)
        {
            if (grid[y][x] > flash_threshold && flashed.Add((y, x)))
            {
                // Increment all adjacent inc diagonal octos
                foreach (var point in IterateValuesToCheck(y, x))
                {
                    if (++grid[point.y][point.x] <= flash_threshold)
                        continue;

                    CheckFlash(grid, point.y, point.x, flashed);
                }

            }

        }
    }
}
