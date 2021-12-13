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
        static bool allow_one_small_cave_twice = false;

        static bool CanVisit(string cave, List<string> path, bool both_small_caves_used)
        {
            return char.IsUpper(cave[0]) || !path.Contains(cave) || (allow_one_small_cave_twice && !both_small_caves_used && IsSmallCave(cave));
        }

        static bool IsSmallCave(string cave)
        {
            return !IsStart(cave) && !IsEnd(cave) && char.IsLower(cave[0]);
        }

        static bool IsStart(string str)
        {
            return str == "start";
        }

        static bool IsEnd(string str)
        {
            return str == "end";
        }

        static void PathFind(string from, List<string>? path = null, int depth = 0, string? active_small_cave = null)
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
                    if (active_small_cave != null && allow_one_small_cave_twice)
                    {
                        // if we are removing the small cave then no longer active
                        for (int i = depth; i < path.Count; i++)
                        {
                            if (path[i] == active_small_cave)
                            {
                                active_small_cave = null;
                                break;
                            }
                        }
                    }

                    path.RemoveRange(depth, path.Count - depth);

                    //System.Diagnostics.Debug.Assert(active_small_cave == null ||path.Count(c => c == active_small_cave)==2);
                }
                System.Diagnostics.Debug.Assert(path != null);

                if (CanVisit(cave, path, active_small_cave != null))
                {
                    if (allow_one_small_cave_twice && active_small_cave == null && IsSmallCave(cave) && path.Contains(cave))
                    {
                        active_small_cave = cave;
                    }

                    path.Add(cave);
                    ++fork;

                    if (IsEnd(cave))
                    {      
                        found_paths.Add(path);
                    }
                    else
                    {
                        PathFind(cave, path, depth, active_small_cave);
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

            // Reset for run two (dirty hack!)
            found_paths.Clear();
            allow_one_small_cave_twice = true;

            PathFind("start");

            Console.WriteLine($"Part two answer: {found_paths.Count}");

        }
    }
}
