using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace day_twelve
{
    internal class Program
    {
        static Dictionary<string, List<string>> lookup = new();
        static List<List<string>> found_paths = new();

        static bool CanVisit(string cave, List<string> path)
        {
            return char.IsUpper(cave[0]) || !path.Contains(cave);
        }

        static bool IsStart(string str)
        {
            return str == "start";
        }

        static bool IsEnd(string str)
        {
            return str == "end";
        }

        struct PathComparer : IEqualityComparer<List<string>>
        {
            public bool Equals(List<string>? x, List<string>? y)
            {
                if (x == null && y == null)
                    return true;

                if (x == null || y == null)
                    return false;

                if (x.Count != y.Count)
                    return false;

                for (int i = 0; i < x.Count; i++)
                {
                    if (x[i] != y[i])
                        return false;
                }

                return true;
            }

            public int GetHashCode([DisallowNull] List<string> obj)
            {
                throw new NotImplementedException();
            }

        }

        static void PathFind(string from, List<string>? path = null, int depth = 0)
        {
            ++depth;

            // Fork 0 is the initial list others need to create new lists
            int fork = 0;
            foreach (var cave in lookup[from])
            {
                if (IsStart(from))
                {
                    path = new() { from };
                }
                else if (fork > 0)
                {
                    path = new(path!);
                    path.RemoveRange(depth, path.Count - depth);
                }
                System.Diagnostics.Debug.Assert(path != null);

                if (CanVisit(cave, path))
                {
                    path.Add(cave);
                    ++fork;

                    if (IsEnd(cave) && !found_paths.Contains(path, new PathComparer()))
                    {
                        found_paths.Add(path);
                    }
                    else
                    {
                        PathFind(cave, path, depth);
                    }

                }

            }
        }

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            foreach (var line in lines)
            {
                var caves = line.Split('-');
                System.Diagnostics.Debug.Assert(caves.Length == 2);
                for (int i = 0; i < 2; i++ )
                {
                    var cave = caves[i];
                    var opposite_cave = caves[1 - i];
                    if (lookup.ContainsKey(cave))
                    {
                        lookup[cave].Add(opposite_cave);
                    }
                    else
                    {
                        lookup[cave] = new List<string>() { opposite_cave };
                    }
                    
                }
            }

            PathFind("start");

            Console.WriteLine($"Part one answer: {found_paths.Count}");
        }
    }
}
