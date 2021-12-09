using System;
using System.Collections.Generic;
using System.Linq;

namespace day_nine
{
    internal class Program
    {
        static IEnumerable<int> IterateValuesToCheck(int index, int upper_bound)
        {
            if (index > 0)
                yield return index - 1;

            if (index < upper_bound - 1)
                yield return index + 1;
        }

        static int FindBasinSize(int y, int x, int[][] grid, SortedSet<(int, int)>? used_points = null)
        {
            // include the low point
            const int no_basin = 9;

            // New basin
            if (used_points == null)
            {
                used_points = new SortedSet<(int, int)>() { (y, x) };
            }

            for (int i = y - 1; i >= 0 && grid[i][x] != no_basin && !used_points.Contains((i, x)); --i)
            {
                used_points.Add((i, x));
                FindBasinSize(i, x, grid, used_points);
            }

            for (int i = y + 1; i < grid.Length && grid[i][x] != no_basin && !used_points.Contains((i, x)); ++i)
            {
                used_points.Add((i, x));
                FindBasinSize(i, x, grid, used_points);
            }


            for (int i = x - 1; i >= 0 && grid[y][i] != no_basin && !used_points.Contains((y, i)); --i)
            {
                used_points.Add((y, i));
                FindBasinSize(y, i, grid, used_points);
            }


            for (int i = x + 1; i < grid[y].Length && grid[y][i] != no_basin && !used_points.Contains((y, i)); ++i)
            {
                used_points.Add((y, i));
                FindBasinSize(y, i, grid, used_points);
            }

            return used_points.Count;
        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            var grid = new int[lines.Length][];

            // Need to make a 2D grid
            for (int i = 0; i < lines.Length; i++)
            {
                // convert char to the int value
                grid[i] = (from item in lines[i] select item - '0').ToArray();
            }

            var low_point_risks = new List<int>();
            var basin_sizes = new List<int>();

            for (int y = 0; y < grid.Length; y++)
            {
                for (int x = 0; x < grid[y].Length; x++)
                {
                    int my_val = grid[y][x];
                    bool is_low_point = true;
                    foreach (var check in IterateValuesToCheck(y, grid.Length))
                    {
                        if (my_val >= grid[check][x])
                        {
                            is_low_point = false;
                            break;
                        }
                    }

                    if (!is_low_point)
                        continue;

                    foreach (var check in IterateValuesToCheck(x, grid[y].Length))
                    {
                        if (my_val >= grid[y][check])
                        {
                            is_low_point = false;
                            break;
                        }
                    }

                    if (is_low_point)
                    {
                        low_point_risks.Add(my_val + 1);

                        // workout basins?
                        basin_sizes.Add(FindBasinSize(y, x, grid));
                    }
                }
            }

            Console.WriteLine($"Part One Answer: {low_point_risks.Sum()}");

            // three largest basins and multiply sizes together
            int total_size = basin_sizes.OrderByDescending(x => x).Where((x, i) => i < 3).Aggregate((x, y) => x * y);

            Console.WriteLine($"Part Two Answer: {total_size}");
        }
    }
}
