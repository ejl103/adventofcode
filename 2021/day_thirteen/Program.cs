using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace day_thirteen
{
    internal class Program
    {
        struct PointComparer : IEqualityComparer<int[]>
        {
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

            // part 1 just the first fold
            bool y_fold = folds[0].line == 'y';

            // [0] will be > threshold
            // [1] will be < 
            var grouped = coords.GroupBy(c => y_fold ? c[1] < folds[0].index : c[0] < folds[0].index).Select(s => s.Select(v => v).ToList()).ToArray();

            int index = y_fold ? 1 : 0;

            // Fold up so higher numbers need adjust to match the the lower "half" coords
            foreach (var pair in grouped[0])
            {
                int new_coord = folds[0].index - (pair[index] - folds[0].index);
                System.Diagnostics.Debug.Assert(new_coord >= 0);

                var new_coords = new int[2];
                new_coords[index] = new_coord; 
                new_coords[1 - index] = pair[1 - index];

                grouped[1].Add(new_coords);
            }

            var distinct_points = grouped[1].Distinct(new PointComparer()).ToArray();

            Console.WriteLine($"Part one answer: {distinct_points.Length}");
        }
    }
}
