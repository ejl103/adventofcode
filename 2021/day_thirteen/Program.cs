using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace day_thirteen
{
    internal class Program
    {
        struct PointComparer : IEqualityComparer<int[]>, IComparer<int[]>
        {
            public int Compare(int[]? one, int[]? two)
            {
                // Sort by low y then low x
                if (one[1] != two[1])
                    return one[1] - two[1];

                return one[0] - two[0];
            }

            public bool Equals(int[]? x, int[]? y)
            {
                if (x.Length != 2 || y.Length != 2)
                    throw new ArgumentException();

                return x[0] == y[0] && x[1] == y[1];
            }

            public int GetHashCode([DisallowNull] int[] obj)
            {
                return obj[0] ^ obj[1];
            }
        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            var coords = new List<int[]>();

            var folds = new List<(char line, int index)>();

            foreach (string line in lines)
            {
                var split = line.Split(',');
                if (split.Length == 2)
                {
                    coords.Add(split.Select(s => int.Parse(s)).ToArray());
                }
                else // the fold lines
                {
                    var stripped = line.Replace("fold along ", "").Split('=');
                    if (stripped.Length == 2)
                        folds.Add((stripped[0][0], int.Parse(stripped[1])));
                }
            }

            bool part1_done = false;
            foreach (var fold in folds)
            {
                bool y_fold = fold.line == 'y';

                // can't predict which side the > group will be
                var grouped = coords.GroupBy(c => y_fold ? c[1] < fold.index : c[0] < fold.index).Select(s => s.Select(v => v).ToList()).ToList();

                int greater_than_group;

                // Nothing to fold??
                if (grouped.Count < 2)
                {
                    //continue;
                    // If only one fold need to figure out which way round it is
                    if ((y_fold && grouped[0][0][1] > fold.index) || (!y_fold && grouped[0][0][0] > fold.index))
                    {
                        grouped.Add(new());
                        greater_than_group = 1;
                    }
                    else
                        continue;
                }
                else
                {
                    greater_than_group = (y_fold && grouped[0][0][1] > fold.index) || (!y_fold && grouped[0][0][0] > fold.index) ? 0 : 1;
                }

                var less_than_group = 1 - greater_than_group;
                int index = y_fold ? 1 : 0;

                // Fold up so higher numbers need adjust to match the the lower "half" coords
                foreach (var pair in grouped[greater_than_group])
                {
                    int new_coord = fold.index - (pair[index] - fold.index);
                    System.Diagnostics.Debug.Assert(new_coord >= 0);

                    var new_coords = new int[2];
                    new_coords[index] = new_coord;
                    new_coords[1 - index] = pair[1 - index];

                    grouped[less_than_group].Add(new_coords);
                }

                coords = grouped[less_than_group].Distinct(new PointComparer()).ToList();

                if (!part1_done)
                {
                    // part 1 just the first fold
                    Console.WriteLine($"Part one answer: {coords.Count}");
                    part1_done = true;
                }


            }
            Console.WriteLine($"Part two answer will be written to grid ");

            coords.Sort(new PointComparer());

            using StreamWriter file = new("part2.txt", append: false);
            
            int prev_y = 0;
            int prev_x = 0;
            for (int i = 0; i < coords.Count; i++)
            {
                int y = coords[i][1];;
                if (y != prev_y)
                {

                    // lines with nothing on them
                    for (int j = prev_y; j < y; j++)
                    {
                        file.Write('\n');
                    }
                    prev_y = y;
                    prev_x = 0;
                }

                int x = coords[i][0];
                for (int j = prev_x; j < x; j++)
                {
                    file.Write(' ');
                }
                prev_x = x+1;

                file.Write('*');
            }

        }
    }
}
