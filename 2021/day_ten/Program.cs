using System;
using System.Linq;
using System.Collections.Generic;

namespace day_ten
{
    internal class Program
    {
        static bool IsStartChar(char c) => c is '(' or '[' or '{' or '<';

        static bool IsMirrorChar(char front, char back) => back == front switch
        {
            '(' => ')',
            '[' => ']',
            '{' => '}',
            '<' => '>',
            _ => throw new ArgumentException()
        };

        static void Main(string[] args)
        {
            string[] lines = System.IO.File.ReadAllLines(@"..\..\..\input.txt");

            var lines_to_remove = new List<string>();
            var dodgy_chars = new List<char>();
            foreach (var line in lines)
            {
                var stack = new Stack<char>();
                foreach(var c in line)
                {
                    if (IsStartChar(c))
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        if (!IsMirrorChar(stack.Pop(), c))
                        {
                            dodgy_chars.Add(c);
                            lines_to_remove.Add(line);
                            break;
                        }
                    }
   
                }
            }

            int part_one_total = 0;
            foreach (var c in dodgy_chars)
            {
                part_one_total += c switch
                {
                    ')' => 3,
                    ']' => 57,
                    '}' => 1197,
                    '>' => 25137,
                    _   => throw new ArgumentException()
                };
            }

            Console.WriteLine($"Part One Total: {part_one_total}");

            var line_scores = new List<long>();
            var incomplete_lines = lines.Except(lines_to_remove);
            foreach (var line in incomplete_lines)
            {
                var stack = new Stack<char>();
                foreach (var c in line)
                {
                    if (IsStartChar(c))
                    {
                        stack.Push(c);
                    }
                    else
                    {
                        stack.Pop();
                    }
                }

                long line_score = 0;
                while (stack.TryPop(out char c))
                {
                    line_score *= 5;
                    line_score += c switch
                    {
                        '(' => 1,
                        '[' => 2,
                        '{' => 3,
                        '<' => 4,
                        _ => throw new ArgumentException()
                    };
                }


                line_scores.Add(line_score);
            }

            line_scores.Sort();
            var part_two_total = line_scores[line_scores.Count / 2];

            Console.WriteLine($"Part Two Total: {part_two_total}");
        }
    }
}
