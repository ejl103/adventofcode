using System;
using System.Collections.Generic;
using System.Linq;

namespace day_eight
{
    internal class Program
    {

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            int part_one_count = 0;
            int part_two_count = 0;

            foreach (var line in lines)
            {
                var both_sides = (from side in line.Split('|') select side).ToArray();

                System.Diagnostics.Debug.Assert(both_sides.Length == 2);

                var rhs = both_sides[1].TrimStart().Split(' ');

                // Part one just need second part - 1 uses 2 segments, 7 uses 3, 4 uses 4 and 8 uses all 7 - these are the unique values
                part_one_count += (from entry in rhs let count = entry.Length where count is 2 or 3 or 4 or 7 select entry).Count();

                var lhs = (from entry in both_sides[0].TrimEnd().Split(' ') select entry);

                var value_lookup = new Dictionary<string, int>();
                while (value_lookup.Count < lhs.Count())
                {
                    foreach (var signal_pattern in lhs)
                    {
                        if (value_lookup.ContainsKey(signal_pattern))
                            continue;

                        int? value = null;
                        switch (signal_pattern.Length)
                        {
                            case 2:
                                value = 1;
                                break;

                            case 3:
                                value = 7;
                                break;

                            case 4:
                                value = 4;
                                break;

                            case 5:
                                if (value_lookup.ContainsValue(1) && value_lookup.ContainsValue(4))
                                {
                                    if (signal_pattern.Except(value_lookup.First(p => p.Value == 1).Key).Count() == 3)
                                    {
                                        value = 3;
                                    }
                                    else if (signal_pattern.Except(value_lookup.First(p => p.Value == 4).Key).Count() == 3)
                                    {
                                        value = 2;
                                    }
                                    else
                                    {
                                        value = 5;
                                    }
                                }
                                break;

                            case 6:
                                if (value_lookup.ContainsValue(4) && value_lookup.ContainsValue(5))
                                {
                                    if (signal_pattern.Except(value_lookup.First(p => p.Value == 4).Key).Count() == 2)
                                    {
                                        value = 9;
                                    }
                                    else if (signal_pattern.Except(value_lookup.First(p => p.Value == 5).Key).Count() == 2)
                                    {
                                        value = 0;
                                    }
                                    else
                                    {
                                        value = 6;
                                    }
                                }
                                break;
                            case 7:
                                value = 8;
                                break;

                            default:
                                throw new InvalidOperationException();
                        }

                        if (value.HasValue)
                            value_lookup[signal_pattern] = value.Value;

                    }
                }

                string str = String.Empty;
                foreach (var output in rhs)
                {
                    foreach(var signal_pattern in lhs)
                    {
                        if (signal_pattern.Length == output.Length && signal_pattern.All(output.Contains))
                        {
                            str += value_lookup[signal_pattern].ToString();
                        }
                    }
                }

                part_two_count += int.Parse(str);
            }
         


            Console.WriteLine($"Part One Answer: {part_one_count}");
            Console.WriteLine($"Part Two Answer: {part_two_count}");
        }
    }
}
