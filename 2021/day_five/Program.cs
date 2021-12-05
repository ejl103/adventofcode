using System;
using System.Collections.Generic;
using System.Linq;

namespace day_five
{
    internal class Program
    {
        private readonly struct Point2D : IComparable<Point2D>, IEquatable<Point2D>
        {
            public readonly int x { get; init; }
            public readonly int y { get; init; }

            int IComparable<Point2D>.CompareTo(Point2D other)
            {
                if (this.x < other.x)
                    return -1;
                if (this.x > other.x) 
                    return 1;

                if (this.y < other.y)
                    return -1;
                if (this.y > other.y)
                    return 1;

                return 0;
            }

            // This isn't strictly necessary but offers better performance than using the default impl
            public bool Equals(Point2D other)
            {
                return this.x == other.x && this.y == other.y;
            }

            // if implementing IEquatable should also override the default as things may not use generics
            public override bool Equals(Object? obj)
            {
                return obj is Point2D other && Equals(other);
            }


            public override int GetHashCode()
            {
                return x ^ y;
            }
        }

        private static int GetAnswer(string[] lines, bool include_diagonals)
        {
            var all_points = new List<Point2D>();

            foreach (var line in lines)
            {
                var ints = line.Replace(" -> ", ",").Split(',').Select(v => int.Parse(v)).ToArray();
                System.Diagnostics.Debug.Assert(ints.Length == 4);

                bool x_equal = ints[0] == ints[2];
                bool y_equal = ints[1] == ints[3];

                // check if x1 == x2 or y1 == y2
                if (x_equal || y_equal)
                {
                    var point_x = new Point2D() { x = ints[0], y = ints[1] };
                    all_points.Add(point_x);

                    var point_y = new Point2D() { x = ints[2], y = ints[3] };
                    all_points.Add(point_y);

                    int start, end;
                    if (y_equal)
                    {
                        start = ints[0];
                        end = ints[2];
                    }
                    else //if (x_equal)
                    {
                        start = ints[1];
                        end = ints[3];
                    }

                    // Checking assumptions
                    System.Diagnostics.Debug.Assert(x_equal != y_equal);
                    System.Diagnostics.Debug.Assert(start != end);

                    if (start > end)
                    {
                        // No swap in C# ??
                        var temp = start;
                        start = end;
                        end = temp;
                    }

                    // Fill in extra points
                    for (int i = start + 1; i < end; i++)
                    {
                        var point = new Point2D() { x = (x_equal ? ints[0] : i), y = (y_equal ? ints[1] : i) };
                        all_points.Add(point);
                    }
                }
                else if (include_diagonals)
                {
                    var diff = ints[0] - ints[2];
                    var x_dir = ints[0] - ints[2];
                    var y_dir = ints[1] - ints[3];
                    for (int xy = 0; xy < Math.Abs(diff) + 1; xy++)
                    {
                        var x = ints[0] + ((x_dir < 0) ? xy : xy * -1);
                        var y = ints[1] + ((y_dir < 0) ? xy : xy * -1);
                        var point = new Point2D() { x = x, y = y };
                        all_points.Add(point);
                    }
              
                }
            }

            // Get then in order to make it easier to count consecutive elemnts
            all_points.Sort();

            Point2D? last_point = null;
            int two_or_more = 0;
            bool in_equal = false;
            foreach (var point in all_points)
            {
                if (last_point.HasValue)
                {
                    // Using .Value to avoid slower call boxed method
                    if (point.Equals(last_point.Value))
                    {
                        if (!in_equal)
                        {
                            ++two_or_more;
                            in_equal = true;
                        }
                    }
                    else
                    {
                        in_equal = false;
                    }
                }

                last_point = point;
            }

            return two_or_more;
        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            Console.WriteLine($"Part one answer: {GetAnswer(lines, include_diagonals: false)}");
            Console.WriteLine($"Part two answer: {GetAnswer(lines, include_diagonals: true)}");
        }
    }
}
